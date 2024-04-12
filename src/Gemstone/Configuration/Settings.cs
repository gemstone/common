//******************************************************************************************************
//  Settings.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/14/2024 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************
// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gemstone.Configuration.AppSettings;
using Gemstone.Configuration.INIConfigurationExtensions;
using Gemstone.Configuration.ReadOnly;
using Gemstone.StringExtensions;
using Gemstone.Threading.SynchronizedOperations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Memory;
using static Gemstone.Configuration.INIConfigurationHelpers;

namespace Gemstone.Configuration;

/// <summary>
/// Defines the operation mode for configuration settings.
/// </summary>
public enum ConfigurationOperation
{
    /// <summary>
    /// Configuration settings are disabled.
    /// </summary>
    Disabled,

    /// <summary>
    /// Configuration settings are read-only.
    /// </summary>
    ReadOnly,
    
    /// <summary>
    /// Configuration settings are read-write.
    /// </summary>
    ReadWrite
}

/// <summary>
/// Defines system settings for an application.
/// </summary>
/// <remarks>
/// Configuration settings are loaded from common sources for a Gemstone project. The properties
/// <see cref="INIFile"/>, <see cref="SQLite"/>, and <see cref="EnvironmentalVariables"/>
/// control the configuration sources that are available and how they are managed. Handling of
/// available settings are defined in a hierarchy where the settings are loaded are in the
/// following priority order, from lowest to highest:
/// <list type="bullet">
///   <item>INI file (defaults.ini) - Machine Level, %programdata% folder</item>
///   <item>INI file (settings.ini) - Machine Level, %programdata% folder</item>
///   <item>SQLite database (settings.db) - User Level, %appdata% folder</item>
///   <item>Environment variables - Machine Level</item>
///   <item>Environment variables - User Level</item>
/// </list>
/// Command line arguments can also be added to the hierarchy to override settings.
/// For example:
/// <code>
///     Settings settings = new()
///     {
///         INIFile = ConfigurationOperation.ReadWrite,
///         SQLite = ConfigurationOperation.Disabled         
///     };
/// 
///     // Bind settings to configuration sources
///     settings.Bind(new ConfigurationBuilder()
///         .ConfigureGemstoneDefaults(settings)
///         .AddCommandLine(args, settings.SwitchMappings));
/// </code>
/// See the <see cref="SwitchMappings"/> property for defining command line switches.
/// </remarks>
public partial class Settings : DynamicObject
{
    /// <summary>
    /// Defines the configuration section name for system settings.
    /// </summary>
    public const string SystemSettingsCategory = nameof(System);

    private readonly ConcurrentDictionary<string, SettingsSection> m_sections = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(string key, object? defaultValue, string description, string[]? switchMappings)> m_definedSettings = [];
    private readonly List<IConfigurationProvider> m_configurationProviders = [];
    private readonly ShortSynchronizedOperation m_saveOperation;
    private readonly ConfigurationOperation m_environmentalVariables;
    
    /// <summary>
    /// Creates a new <see cref="Settings"/> instance.
    /// </summary>
    public Settings()
    {
        Instance ??= this;
        m_saveOperation = new ShortSynchronizedOperation(SaveSections);
    }

    /// <summary>
    /// Gets the source <see cref="IConfiguration"/> for settings.
    /// </summary>
    public IConfiguration? Configuration { get; private set; }

    /// <summary>
    /// Gets or sets configuration operation mode for INI file settings.
    /// </summary>
    public ConfigurationOperation INIFile { get; init; } = ConfigurationOperation.ReadOnly;

    /// <summary>
    /// Gets or sets configuration operation mode for SQLite settings.
    /// </summary>
    public ConfigurationOperation SQLite { get; init; } = ConfigurationOperation.ReadWrite;

    /// <summary>
    /// Gets or sets configuration operation mode for environmental variables.
    /// </summary>
    /// <exception cref="InvalidOperationException">Environmental variables cannot be used for read-write configuration operations.</exception>
    public ConfigurationOperation EnvironmentalVariables
    {
        get => m_environmentalVariables;
        init
        {
            if (value == ConfigurationOperation.ReadWrite)
                throw new InvalidOperationException("Environmental variables cannot be used for read-write configuration operations.");

            m_environmentalVariables = value;
        }
    }

    /// <summary>
    /// Gets or sets flag that determines if description lines, e.g., those encoded into an INI file,
    /// should be split into multiple lines.
    /// </summary>
    public bool SplitDescriptionLines { get; init; }

    /// <summary>
    /// Gets the names for the settings sections.
    /// </summary>
    public string[] SectionNames => m_sections.Keys.ToArray();

    /// <summary>
    /// Gets the sections count for the settings.
    /// </summary>
    public int Count => m_sections.Count;

    /// <summary>
    /// Gets the command line switch mappings for <see cref="Settings"/>.
    /// </summary>
    public Dictionary<string, string> SwitchMappings { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the <see cref="SettingsSection"/> for the specified key.
    /// </summary>
    /// <param name="key">Section key.</param>
    public SettingsSection this[string key] => m_sections.GetOrAdd(key, _ => new SettingsSection(this, key));

    /// <summary>
    /// Gets flag that determines if any settings have been changed.
    /// </summary>
    public bool IsDirty
    {
        get => m_sections.Values.Any(section => section.IsDirty);
        private set
        {
            foreach (SettingsSection section in m_sections.Values)
                section.IsDirty = value;
        }
    }

    /// <summary>
    /// Attempts to bind the <see cref="Settings"/> instance to configuration values by matching property
    /// names against configuration keys recursively.
    /// </summary>
    /// <param name="builder">Configuration builder used to bind settings.</param>
    public void Bind(IConfigurationBuilder builder)
    {
        MemoryConfigurationProvider? memoryProvider = null;
        bool iniProviderExists = false;

        // Build a new configuration with keys and values from the set of providers
        // registered in builder sources - we call this instead of directly using
        // the 'Build()' method on the config builder so providers can be cached
        foreach (IConfigurationSource source in builder.Sources)
        {
            IConfigurationProvider provider = source.Build(builder);

            if (memoryProvider is null && provider is ReadOnlyConfigurationProvider { Provider: MemoryConfigurationProvider configProvider })
                memoryProvider = configProvider;

            if (!iniProviderExists && provider is ReadOnlyConfigurationProvider { Provider: IniConfigurationProvider })
                iniProviderExists = true;

            m_configurationProviders.Add(provider);
        }

        // Cache configuration root
        Configuration = new ConfigurationRoot(m_configurationProviders);

        // Attempt to load current descriptions from settings.ini file
        Dictionary<string, Dictionary<string, string>> settingDescriptions = new(StringComparer.OrdinalIgnoreCase);

        if (iniProviderExists)
        {
            string iniPath = GetINIFilePath("settings.ini");
            using TextReader reader = GetINIFileReader(iniPath);
            string[] iniFileContents = reader.ReadToEnd().Split(["\n", "\r", "\r\n"], StringSplitOptions.RemoveEmptyEntries);
            string currentSection = "";
            StringBuilder currentDescription = new();

            foreach (string line in iniFileContents)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.Length == 0)
                    continue;

                // Check if this is a section header
                if (trimmedLine.StartsWith('[') && trimmedLine.EndsWith(']'))
                {
                    currentSection = trimmedLine[1..^1];
                    settingDescriptions[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    currentDescription.Clear();
                    continue;
                }

                // Check if this is a key/value pair, commented or not
                Match match = s_iniKeyValuePair.Match(trimmedLine);

                if (match.Success)
                {
                    string key = match.Groups["key"].Value;
                    string description = currentDescription.ToString().Trim();

                    if (!string.IsNullOrEmpty(description))
                        settingDescriptions[currentSection][key] = currentDescription.ToString();

                    currentDescription.Clear();
                    continue;
                }

                if (!trimmedLine.StartsWith(";"))
                    continue;

                // Track description lines
                if (currentDescription.Length > 0)
                    currentDescription.Append(' ');

                currentDescription.Append(trimmedLine[1..].Trim());
            }
        }

        // Load settings from configuration sources hierarchy
        foreach (IConfigurationSection configSection in Configuration.GetChildren())
        {
            SettingsSection section = this[configSection.Key];

            foreach (IConfigurationSection entry in configSection.GetChildren())
            {
                section[entry.Key] = entry.Value;

                if (string.IsNullOrEmpty(entry.Value) || memoryProvider is null)
                    continue;

                // For entries that may exist in INI file but were never officially defined, set initial value as empty string,
                // this will ensure that the value persists through save operations, ignoring default value
                if (string.IsNullOrEmpty(entry.GetAppSettingInitialValue()))
                    memoryProvider.Add($"{configSection.Key}:{entry.Key.ToInitialValueKey()}", "");

                if (!string.IsNullOrEmpty(entry.GetAppSettingDescription()))
                    continue;

                // Maintain descriptions from INI file, if one is defined (could have been added manually)
                if (settingDescriptions.TryGetValue(configSection.Key, out Dictionary<string, string>? descriptions) && descriptions.TryGetValue(entry.Key, out string? description) && !string.IsNullOrWhiteSpace(description))
                    memoryProvider.Add($"{configSection.Key}:{entry.Key.ToDescriptionKey()}", description);
                else
                    memoryProvider.Add($"{configSection.Key}:{entry.Key.ToDescriptionKey()}", entry.Key.ToSpacedLabel());
            }

            section.ConfigurationSection = configSection;
            section.IsDirty = false;
        }
    }

    /// <inheritdoc/>
    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return m_sections.Keys;
    }

    /// <inheritdoc/>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        string key = binder.Name;

        // If you try to get a value of a property that is
        // not defined in the class, this method is called.
        result = m_sections.GetOrAdd(key, _ => new SettingsSection(this, key));

        return true;
    }

    /// <inheritdoc/>
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        if (value is not SettingsSection section)
            return false;

        m_sections[binder.Name] = section;

        return true;
    }

    /// <inheritdoc/>
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
        if (indexes.Length != 1)
            throw new ArgumentException($"{nameof(Settings)} indexer requires a single index representing string name of settings section.");

        string key = indexes[0].ToString()!;

        result = m_sections.GetOrAdd(key, _ => new SettingsSection(this, key));

        return true;
    }

    /// <inheritdoc/>
    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        if (indexes.Length != 1)
            throw new ArgumentException($"{nameof(Settings)} indexer requires a single index representing string name of settings section.");

        if (value is not SettingsSection section)
            return false;

        m_sections[indexes[0].ToString()!] = section;

        return true;
    }

    /// <summary>
    /// Configures the <see cref="IAppSettingsBuilder"/> for <see cref="Settings"/>.
    /// </summary>
    /// <param name="builder">Builder used to configure settings.</param>
    public void ConfigureAppSettings(IAppSettingsBuilder builder)
    {
        foreach ((string key, object? defaultValue, string description, string[]? switchMappings) in m_definedSettings)
        {
            builder.Add(key, defaultValue?.ToString() ?? "", description);

            if (switchMappings is null || switchMappings.Length == 0)
                continue;

            foreach (string switchMapping in switchMappings)
                SwitchMappings[switchMapping] = key;
        }
    }

    // Defines application settings for the specified section key
    internal void DefineSetting(string key, string defaultValue, string description, string[]? switchMappings)
    {
        m_definedSettings.Add((key, defaultValue, description, switchMappings));
    }

    /// <summary>
    /// Saves any changed settings.
    /// </summary>
    /// <param name="waitForSave">Determines if save operation should wait for completion.</param>
    /// <param name="forceSave">Determines if save operation should be forced, i.e., whether settings are dirty or not.</param>
    public void Save(bool waitForSave, bool forceSave = false)
    {
        if (!IsDirty && !forceSave)
            return;

        if (waitForSave)
            m_saveOperation.Run(true);
        else
            m_saveOperation.RunAsync();

        IsDirty = false;
    }

    private void SaveSections()
    {
        try
        {
            foreach (IConfigurationProvider provider in m_configurationProviders)
            {
                if (provider is ReadOnlyConfigurationProvider readOnlyProvider)
                {
                    if (readOnlyProvider.Provider is not IniConfigurationProvider || INIFile != ConfigurationOperation.ReadWrite)
                        continue;

                    // Handle INI file as a special case, writing entire file contents on save
                    string contents = Configuration!.GenerateINIFileContents(splitDescriptionLines: SplitDescriptionLines);
                    string iniFilePath = GetINIFilePath("settings.ini");
                    using TextWriter writer = GetINIFileWriter(iniFilePath);
                    writer.Write(contents);
                }
                else
                {
                    // SQLite configuration provider is in a subordinate assembly, so we check for it by name
                    if (provider.GetType().Name.Equals("SQLiteConfigurationProvider") && SQLite != ConfigurationOperation.ReadWrite)
                        continue;

                    foreach (SettingsSection section in m_sections.Values)
                    {
                        if (!section.IsDirty)
                            continue;

                        // Update configuration provider with each setting - in the case of
                        // SQLite, this will update the configuration database contents
                        foreach (string key in section.Keys)
                            provider.Set($"{section.Name}:{key}", section.ConfigurationSection[key]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed while trying to save configuration: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Saves any changed settings.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="forceSave">Determines if save operation should be forced, i.e., whether settings are dirty or not.</param>
    /// <remarks>
    /// This method will not return until the save operation has completed.
    /// </remarks>
    public static void Save(Settings? settings = null, bool forceSave = false)
    {
        (settings ?? Instance).Save(true, forceSave);
    }

    /// <summary>
    /// Gets the default instance of <see cref="Settings"/>.
    /// </summary>
    public static Settings Instance { get; private set; } = default!;

    /// <summary>
    /// Gets the default instance of <see cref="Settings"/> as a dynamic object.
    /// </summary>
    /// <returns>Default instance of <see cref="Settings"/> as a dynamic object.</returns>
    /// <exception cref="InvalidOperationException">Settings have not been initialized.</exception>
    public static dynamic Default => Instance ?? throw new InvalidOperationException("Settings have not been initialized.");

    /// <summary>
    /// Updates the default instance of <see cref="Settings"/>.
    /// </summary>
    /// <param name="settings">New default instance of <see cref="Settings"/>.</param>
    /// <remarks>
    /// This changes the default singleton instance of <see cref="Settings"/> to the specified instance.
    /// Use this method with caution as it can lead to unexpected behavior if the default instance is changed.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void UpdateInstance(Settings settings)
    {
        Instance = settings;
    }
    
    private const string INIKeyValuePairPattern = @";?\s*(?<key>\w+)\s*=.*";
    private static readonly Regex s_iniKeyValuePair;

    static Settings()
    {
#if NET
        s_iniKeyValuePair = GenerateINIKeyValuePairRegex();
#else
        s_iniKeyValuePair = new Regex(INIKeyValuePairPattern, RegexOptions.Compiled);
#endif
    }

#if NET
    [GeneratedRegex(INIKeyValuePairPattern, RegexOptions.Compiled)]
    private static partial Regex GenerateINIKeyValuePairRegex();
#endif
}
