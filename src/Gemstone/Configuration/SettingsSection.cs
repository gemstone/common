//******************************************************************************************************
//  SettingsSection.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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

using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using Gemstone.TypeExtensions;
using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration;

/// <summary>
/// Defines a dynamic <see cref="Settings"/> section with typed values.
/// </summary>
public class SettingsSection : DynamicObject
{
    private readonly Settings m_parent;
    private readonly ConcurrentDictionary<string, object> m_keyValues = new(StringComparer.OrdinalIgnoreCase);
    private IConfigurationSection? m_configurationSection;

    internal SettingsSection(Settings parent, string sectionName)
    {
        m_parent = parent;
        Name = sectionName;
    }

    internal IConfigurationSection ConfigurationSection
    {
        get => m_configurationSection ??= m_parent.Configuration?.GetSection(Name) ?? throw new InvalidOperationException("Configuration has not been set.");
        set => m_configurationSection = value;
    }

    /// <summary>
    /// Gets the name of the settings section.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the keys for the settings section.
    /// </summary>
    public string[] Keys => m_keyValues.Keys.ToArray();

    /// <summary>
    /// Gets flag that determines if the settings section has been modified.
    /// </summary>
    public bool IsDirty { get; internal set; }

    /// <summary>
    /// Gets the typed value for the specified key.
    /// </summary>
    /// <param name="key">Setting key name.</param>
    public object? this[string key]
    {
        get
        {
            if (m_keyValues.TryGetValue(key, out object? cachedValue))
                return cachedValue;

            if (m_configurationSection is not null)
                return m_keyValues[key] = FromTypedValue(ConfigurationSection[key]).value;

            throw new InvalidOperationException($"Configuration section \"{Name}\" has not been defined.");
        }
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            object? updatedValue;

            if (value is string stringValue)
            {
                (Type valueType, object parsedValue, bool typePrefixed) = FromTypedValue(stringValue);

                if (typePrefixed)
                {
                    updatedValue = parsedValue;
                }
                else
                {
                    if (m_keyValues.TryGetValue(key, out object? initialValue))
                        valueType = initialValue.GetType();

                    if (valueType == typeof(string))
                    {
                        updatedValue = value;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(stringValue))
                            updatedValue = Activator.CreateInstance(valueType) ?? string.Empty;
                        else
                            updatedValue = Common.TypeConvertFromString(stringValue, valueType) ?? Activator.CreateInstance(valueType) ?? string.Empty;
                    }
                }
            }
            else
            {
                updatedValue = value;
            }

            if (!m_keyValues.TryGetValue(key, out object? currentValue) || !currentValue.Equals(updatedValue))
            {
                m_keyValues[key] = updatedValue;
                IsDirty = true;
            }

            ConfigurationSection[key] = ToTypedValue(updatedValue);
        }
    }

    /// <summary>
    /// Defines a setting for the section.
    /// </summary>
    /// <param name="key">Key name of setting to get, case-insensitive.</param>
    /// <param name="defaultValue">Default value if key does not exist.</param>
    /// <param name="description">Description of the setting.</param>
    /// <param name="switchMappings">Optional array of switch mappings for the setting.</param>
    public void Define(string key, object? defaultValue, string description, string[]? switchMappings = null)
    {
        m_keyValues.TryAdd(key, defaultValue ?? string.Empty);

        string typedValue = ToTypedValue(defaultValue);
        
        if (m_configurationSection is not null)
            ConfigurationSection[key] ??= typedValue;
        
        m_parent.DefineSetting($"{Name}:{key}", typedValue, description, switchMappings);
    }

    /// <summary>
    /// Gets the value of the setting with the specified key, if it exists;
    /// otherwise, the default value for the parameter.
    /// </summary>
    /// <param name="key">Key name of setting to get, case-insensitive.</param>
    /// <param name="defaultValue">Default value if key does not exist.</param>
    /// <param name="description">Description of the setting.</param>
    /// <param name="switchMappings">Optional array of switch mappings for the setting.</param>
    /// <returns>
    /// Value of the setting with the specified key, if it exists; otherwise,
    /// the default value for the parameter.
    /// </returns>
    public object? GetOrAdd(string key, object? defaultValue, string description, string[]? switchMappings = null)
    {
        Define(key, defaultValue, description, switchMappings);
        return this[key];
    }

    /// <inheritdoc/>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = this[binder.Name];
        return true;
    }

    /// <inheritdoc/>
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        return TrySetKeyValue(binder.Name, value);
    }

    /// <inheritdoc/>
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        string key = indexes[0].ToString()!;

        // This allows dynamic access to section values with ability to add default values, descriptions and switch mappings
        // For example:
        //     string hostURLs = settings.Web["HostURLs", "http://localhost:8180", "Defines the URLs the hosted service will listen on.", "-u"]
        //     string hostCertificate = settings.Web["HostCertificate", "", "Defines the certificate used to host the service.", "-s"]
        switch (indexes.Length)
        {
            case 1:
                result = this[key];
                return true;
            case 2:
                result = GetOrAdd(key, indexes[1], "");
                return true;
            case 3:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!);
                return true;
            case 4:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString()]);
                return true;
            case 5:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString(), indexes[4].ToString()]);
                return true;
            case 6:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString(), indexes[4].ToString(), indexes[5].ToString()]);
                return true;
        }

        throw new InvalidOperationException("Invalid number of index parameters.");
    }

    /// <inheritdoc/>
    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        return TrySetKeyValue(indexes[0].ToString()!, value);
    }

    private bool TrySetKeyValue(string key, object? value)
    {
        // This allows dynamic setting definitions with values, descriptions and optional switch mappings
        // For example:
        //     Settings.Default.WebHosting.HostURLs = ("http://localhost:8180", "Defines the URLs the hosted service will listen on.", "-u", "--HostURLs");
        //     Settings.Default.WebHosting.HostCertificate = ("", "Defines the certificate used to host the service.", "-s", "--HostCertificate");
        //
        // Non-tuple values just handle setting value assignment:
        //     Settings.Default.WebHosting.HostURLs = "http://localhost:5000";
        switch (value)
        {
            case ({ } defaultValue, string description, string[] switchMappings):
                Define(key, defaultValue, description, switchMappings);
                return true;
            case ({ } defaultValue, string description, string switchMapping):
                Define(key, defaultValue, description, [switchMapping]);
                return true;
            case ({ } defaultValue, string description, string switchMapping1, string switchMapping2):
                Define(key, defaultValue, description, [switchMapping1, switchMapping2]);
                return true;
            case ({ } defaultValue, string description, string switchMapping1, string switchMapping2, string switchMapping3):
                Define(key, defaultValue, description, [switchMapping1, switchMapping2, switchMapping3]);
                return true;
            case ({ } defaultValue, string description):
                Define(key, defaultValue, description);
                return true;
            default:
                this[key] = value;
                return true;
        }
    }

    /// <summary>
    /// Gets the parsed type and value of a configuration setting value.
    /// </summary>
    /// <param name="setting">Configuration setting value that can be prefixed with a type.</param>
    /// <returns>Tuple containing the parsed type, value and flag determining if setting was type prefixed.</returns>
    /// <exception cref="InvalidOperationException">Failed to load specified type.</exception>
    /// <remarks>
    /// <para>
    /// Type name is parsed from the beginning of the setting value, if it exists, and is enclosed in brackets,
    /// e.g.: <c>[int]:123</c> or <c>[System.Int32]:123</c>. If no type name is specified, the assumed default
    /// type will always be <see cref="string"/>.
    /// </para>
    /// <para>
    /// Common C# names like <c>long</c> and <c>DateTime</c> can be used as type names, these will not be
    /// case-sensitive. Custom type names will be case-sensitive and require a full type name.
    /// </para>
    /// </remarks>
    public static (Type type, object value, bool typePrefixed) FromTypedValue(string? setting)
    {
        if (setting is null)
            return (typeof(string), string.Empty, false);

        string[] parts = setting.Split(':');

        if (parts.Length < 2)
            return (typeof(string), setting, false);

        string typeName = parts[0].Trim();

        // Make sure type name is enclosed in brackets, only this indicates a type name
        if (typeName.StartsWith('[') && typeName.EndsWith(']'))
            typeName = typeName[1..^1].Trim();
        else
            return (typeof(string), setting, false);

        string value = setting[(parts[0].Length + 1)..].Trim();

        // Parse common C# type names
        return typeName.ToLowerInvariant() switch
        {
            "string" => (typeof(string), value, true),
            "bool" => (typeof(bool), Convert.ToBoolean(value), true),
            "byte" => (typeof(byte), Convert.ToByte(value), true),
            "sbyte" => (typeof(sbyte), Convert.ToSByte(value), true),
            "short" => (typeof(short), Convert.ToInt16(value), true),
            "ushort" => (typeof(ushort), Convert.ToUInt16(value), true),
            "int" => (typeof(int), Convert.ToInt32(value), true),
            "uint" => (typeof(uint), Convert.ToUInt32(value), true),
            "long" => (typeof(long), Convert.ToInt64(value), true),
            "ulong" => (typeof(ulong), Convert.ToUInt64(value), true),
            "float" => (typeof(float), Convert.ToSingle(value), true),
            "double" => (typeof(double), Convert.ToDouble(value), true),
            "decimal" => (typeof(decimal), Convert.ToDecimal(value), true),
            "char" => (typeof(char), Convert.ToChar(value), true),
            "datetime" => (typeof(DateTime), Convert.ToDateTime(value), true),
            "timespan" => (typeof(TimeSpan), TimeSpan.Parse(value), true),
            "guid" => (typeof(Guid), Guid.Parse(value), true),
            "uri" => (typeof(Uri), new Uri(value), true),
            "version" => (typeof(Version), new Version(value), true),
            "type" => (typeof(Type), Type.GetType(value, true) ?? throw new InvalidOperationException($"Failed to load type \"{value}\"."), true),
            _ => getParsedTypeAndValue()
        };

        // Parse custom type names - custom names will be case-sensitive and require full type name
        (Type, object, bool) getParsedTypeAndValue()
        {
            Type parsedType = Type.GetType(typeName, true) ?? throw new InvalidOperationException($"Failed to load type \"{typeName}\".");
            object parsedValue = Common.TypeConvertFromString(value, parsedType) ?? Activator.CreateInstance(parsedType) ?? string.Empty;

            return (parsedType, parsedValue, true);
        }
    }

    /// <summary>
    /// Converts a value to a typed string representation.
    /// </summary>
    /// <param name="value">Value to convert to a typed representation.</param>
    /// <returns>String formatted as a type-prefixed value, e.g.: <c>[int]:123</c>.</returns>
    public static string ToTypedValue(object? value)
    {
        if (value is null)
            return string.Empty;

        Type valueType = value.GetType();

        // Handle common C# type names
        if (valueType == typeof(string))
            return value.ToString()!;

        if (valueType == typeof(bool))
            return $"[bool]:{value}";

        if (valueType == typeof(byte))
            return $"[byte]:{value}";

        if (valueType == typeof(sbyte))
            return $"[sbyte]:{value}";

        if (valueType == typeof(short))
            return $"[short]:{value}";

        if (valueType == typeof(ushort))
            return $"[ushort]:{value}";

        if (valueType == typeof(int))
            return $"[int]:{value}";

        if (valueType == typeof(uint))
            return $"[uint]:{value}";

        if (valueType == typeof(long))
            return $"[long]:{value}";

        if (valueType == typeof(ulong))
            return $"[ulong]:{value}";

        if (valueType == typeof(float))
            return $"[float]:{value}";

        if (valueType == typeof(double))
            return $"[double]:{value}";

        if (valueType == typeof(decimal))
            return $"[decimal]:{value}";

        if (valueType == typeof(char))
            return $"[char]:{value}";

        if (valueType == typeof(DateTime))
            return $"[DateTime]:{value}";

        if (valueType == typeof(TimeSpan))
            return $"[TimeSpan]:{value}";

        if (valueType == typeof(Guid))
            return $"[Guid]:{value}";

        if (valueType == typeof(Uri))
            return $"[Uri]:{value}";

        if (valueType == typeof(Version))
            return $"[Version]:{value}";

        if (valueType == typeof(Type))
            return $"[Type]:{value}";

        // Handle custom type names
        return $"[{valueType.GetReflectedTypeName()}]:{value}";
    }
}
