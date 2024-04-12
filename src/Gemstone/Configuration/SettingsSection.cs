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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Gemstone.StringExtensions;
using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration;

/// <summary>
/// Defines a dynamic <see cref="Settings"/> section with typed values.
/// </summary>
public partial class SettingsSection : DynamicObject
{
    private const string ConfigurationAssemblyName = $"{nameof(Gemstone)}.{nameof(Configuration)}";
    private const string EvalTypeName = $"{ConfigurationAssemblyName}.Eval";

    private readonly Settings m_parent;
    private readonly ConcurrentDictionary<string, (object? value, bool isEvalExpr)> m_keyValues = new(StringComparer.OrdinalIgnoreCase);
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
            if (m_keyValues.TryGetValue(key, out (object? cachedValue, bool isEvalExpr) result))
            {
                if (result.cachedValue is null || !result.isEvalExpr)
                    return result.cachedValue;

                // Compilable typed access to 'Gemstone.Configuration.Eval' is not available since
                // type is defined in a subordinate assembly, so we access type dynamically so
                // properties can be accessed at runtime:
                dynamic eval = result.cachedValue;

                // Accessing 'Value' property will automatically evaluate expression as necessary.
                // Initial evaluation result will be cached. To force re-evaluate of expression,
                // set the setting value to 'Eval.Null' before getting property value.
                return eval.Value;
            }

            if (m_configurationSection is null)
                return null;

            (Type _, object? value, bool _, bool isEvalExpr) = FromTypedValue(ConfigurationSection[key]);

            if (value is null)
                return null;
                    
            m_keyValues[key] = (value, isEvalExpr);
            return value;
        }
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            object updatedValue;
            bool isEvalExpr;

            if (value is string stringValue)
            {
                (Type valueType, object? parsedValue, bool typePrefixed, isEvalExpr) = FromTypedValue(stringValue);

                if (typePrefixed)
                {
                    updatedValue = parsedValue!;
                }
                else
                {
                    if (m_keyValues.TryGetValue(key, out (object? cachedValue, bool _) result))
                        valueType = result.cachedValue?.GetType() ?? typeof(string);

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
                isEvalExpr = value.GetType().FullName?.Equals(EvalTypeName) ?? false;

                // Check if current assigned value is an 'Eval' type and cached value is 'Eval' type
                if (isEvalExpr && m_keyValues.TryGetValue(key, out (object? cachedValue, bool isEvalExpr) result) && result is { cachedValue: not null, isEvalExpr: true })
                {
                    // Compilable typed access to 'Gemstone.Configuration.Eval' is not available since
                    // type is defined in a subordinate assembly, so we access type dynamically so
                    // properties can be accessed at runtime:
                    dynamic eval = value;
                    string expression = eval.Expression;

                    // Check if request was made to reset cached 'Eval' value so expression can be re-evaluated,
                    // i.e., current setting value was assigned a value of 'Eval.Null':
                    if (expression.Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        eval = result.cachedValue;
                        eval.Value = null!;
                        updatedValue = result.cachedValue;
                    }
                }
            }

            if (!m_keyValues.TryGetValue(key, out (object? value, bool _) current) || !(current.value?.Equals(updatedValue) ?? false))
            {
                m_keyValues[key] = (updatedValue, isEvalExpr);
                IsDirty = true;
            }

            ConfigurationSection[key] = ToTypedValue(updatedValue, isEvalExpr);
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
        bool isEvalExpr = defaultValue?.GetType().FullName?.Equals(EvalTypeName) ?? false;

        m_keyValues.TryAdd(key, (defaultValue ?? string.Empty, isEvalExpr));

        string typedValue = ToTypedValue(defaultValue, isEvalExpr);
        
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
    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return m_keyValues.Keys;
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
        //     string hostURLs = settings.Web["HostURLs", "http://localhost:8180", "Defines the URLs the hosted service will listen on.", "-u", "--HostURLs"]
        //     string hostCertificate = settings.Web["HostCertificate", "", "Defines the certificate used to host the service.", "-s", "--HostCertificate"]
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
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString()!]);
                return true;
            case 5:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString()!, indexes[4].ToString()!]);
                return true;
            case 6:
                result = GetOrAdd(key, indexes[1], indexes[2].ToString()!, [indexes[3].ToString()!, indexes[4].ToString()!, indexes[5].ToString()!]);
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
    /// <returns>Tuple containing the parsed type and, value.</returns>
    /// <exception cref="InvalidOperationException">Failed to load specified type.</exception>
    /// <remarks>
    /// <para>
    /// Type name is parsed from the beginning of the setting value, if it exists, and is enclosed in brackets,
    /// e.g.: <c>[int]:123</c> or <c>[System.Int32]:123</c>. If no type name is specified, the assumed default
    /// type will always be <see cref="string"/>.
    /// </para>
    /// <para>
    /// Common C# names like <c>long</c> and <c>DateTime</c> can be used as type names as well as custom type names.
    /// Custom type names require the full type name with a namespace, e.g.: <c>[MyNamespace.MyType]:TypeValue</c>.
    /// Type name lookups are not case-sensitive.
    /// </para>
    /// </remarks>
    /// <exception cref="TypeLoadException">Failed to load type.</exception>
    public static (Type type, object? value) ParseTypedPrefixedValue(string? setting)
    {
        (Type type, object? value, bool _, bool _) = FromTypedValue(setting);
        return (type, value);
    }

    private static (Type type, object? value, bool typePrefixed, bool isEvalExpr) FromTypedValue(string? setting)
    {
        if (setting is null)
            return (typeof(object), null, false, false);

        string[] parts = setting.Split(':');

        if (parts.Length < 2)
            return (typeof(string), setting, false, false);

        string typeName = parts[0].Trim();

        // Make sure type name is enclosed in brackets, only this indicates a type name
        if (typeName.StartsWith('[') && typeName.EndsWith(']'))
            typeName = typeName[1..^1].Trim();
        else
            return (typeof(string), setting, false, false);

        string value = setting[(parts[0].Length + 1)..].Trim();

        // Parse common C# type names
        return typeName.ToLowerInvariant() switch
        {
            "string" => (typeof(string), value, true, false),
            "bool" => (typeof(bool), Convert.ToBoolean(value), true, false),
            "byte" => (typeof(byte), Convert.ToByte(value), true, false),
            "sbyte" => (typeof(sbyte), Convert.ToSByte(value), true, false),
            "short" => (typeof(short), Convert.ToInt16(value), true, false),
            "ushort" => (typeof(ushort), Convert.ToUInt16(value), true, false),
            "int" => (typeof(int), Convert.ToInt32(value), true, false),
            "uint" => (typeof(uint), Convert.ToUInt32(value), true, false),
            "long" => (typeof(long), Convert.ToInt64(value), true, false),
            "ulong" => (typeof(ulong), Convert.ToUInt64(value), true, false),
            "float" => (typeof(float), Convert.ToSingle(value), true, false),
            "double" => (typeof(double), Convert.ToDouble(value), true, false),
            "decimal" => (typeof(decimal), Convert.ToDecimal(value), true, false),
            "char" => (typeof(char), Convert.ToChar(value), true, false),
            "datetime" => (typeof(DateTime), Convert.ToDateTime(value), true, false),
            "timespan" => (typeof(TimeSpan), TimeSpan.Parse(value), true, false),
            "guid" => (typeof(Guid), Guid.Parse(value), true, false),
            "uri" => (typeof(Uri), new Uri(value), true, false),
            "version" => (typeof(Version), new Version(value), true, false),
            "type" => (typeof(Type), Type.GetType(value, true, true) ?? throw new TypeLoadException($"Failed to load type \"{value}\"."), true, false),
            "eval" => parseEvalExpr(), // Evaluate expression, e.g., "[eval]:1 + 2 * 3 or [eval]:{Section.Key}"
            _ => parseCustomTypeAndValueExpr()
        };

        (Type, object, bool, bool) parseEvalExpr()
        {
            // Eval type is defined in the subordinate Gemstone.Configuration assembly, so we can't reference it directly
            Type parsedType = Type.GetType($"{EvalTypeName},{ConfigurationAssemblyName}", true) ?? throw new TypeLoadException($"Failed to load type \"{typeName}\".");
            object evalInstance;

            try
            {
                // Returns instance of Gemstone.Configuration.Eval class
                evalInstance = Common.TypeConvertFromString(value, parsedType, null, false) ?? throw new NullReferenceException("Result of evaluation was null");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse expression \"{value}\": {ex.Message}", ex);
            }

            // Since checking for 'Eval' type is handled by name, type determination is cached by 'isEvalExpr'
            return (parsedType, evalInstance, true, true);
        }

        // Parse custom type names
        (Type, object, bool, bool) parseCustomTypeAndValueExpr()
        {
            // Handle common C# native array types as a special case,
            // this allows for a more concise config representation:
            typeName = typeName.ToLowerInvariant() switch
            {
                "string[]" => "System.String[]",
                "bool[]" => "System.Boolean[]",
                "byte[]" => "System.Byte[]",
                "sbyte[]" => "System.SByte[]",
                "short[]" => "System.Int16[]",
                "ushort[]" => "System.UInt16[]",
                "int[]" => "System.Int32[]",
                "uint[]" => "System.UInt32[]",
                "long[]" => "System.Int64[]",
                "ulong[]" => "System.UInt64[]",
                "float[]" => "System.Single[]",
                "double[]" => "System.Double[]",
                "decimal[]" => "System.Decimal[]",
                "char[]" => "System.Char[]",
                "datetime[]" => "System.DateTime[]",
                "timespan[]" => "System.TimeSpan[]",
                "guid[]" => "System.Guid[]",
                "uri[]" => "System.Uri[]",
                "version[]" => "System.Version[]",
                "type[]" => "System.Type[]",
                _ => typeName
            };

            // Add assumed assembly name to type name if only a type name is provided
            if (!typeName.Contains(',') && typeName.CharCount('.') > 1 && !typeName.StartsWith("System.")) 
                typeName = $"{typeName}, {typeName[..typeName.IndexOf('.', typeName.IndexOf('.') + 1)]}";

            Type parsedType = Type.GetType(typeName, true, true) ?? throw new TypeLoadException($"Failed to load type \"{typeName}\".");
            object parsedValue = Common.TypeConvertFromString(value, parsedType) ?? Activator.CreateInstance(parsedType) ?? string.Empty;

            return (parsedType, parsedValue, true, false);
        }
    }

    /// <summary>
    /// Converts a value to a typed string representation.
    /// </summary>
    /// <param name="value">Value to convert to a typed representation.</param>
    /// <returns>String formatted as a type-prefixed value, e.g.: <c>[int]:123</c>.</returns>
    public static string GenerateTypedPrefixedValue(object? value)
    {
        return ToTypedValue(value, null);
    }

    private static string ToTypedValue(object? value, bool? isEvalExpr)
    {
        if (value is null)
            return string.Empty;

        Type valueType = value.GetType();

        if (isEvalExpr ?? valueType.FullName?.Equals(EvalTypeName) ?? false)
        {
            // Always store unevaluated / un-transpiled expression for Eval types
            dynamic evalInstance = value;
            return $"[eval]:{evalInstance.Expression}";
        }

        return 0 switch
        {
            // Handle common C# type names
            0 when valueType == typeof(string) => value.ToString()!,
            0 when valueType == typeof(bool) => $"[bool]:{value}",
            0 when valueType == typeof(byte) => $"[byte]:{value}",
            0 when valueType == typeof(sbyte) => $"[sbyte]:{value}",
            0 when valueType == typeof(short) => $"[short]:{value}",
            0 when valueType == typeof(ushort) => $"[ushort]:{value}",
            0 when valueType == typeof(int) => $"[int]:{value}",
            0 when valueType == typeof(uint) => $"[uint]:{value}",
            0 when valueType == typeof(long) => $"[long]:{value}",
            0 when valueType == typeof(ulong) => $"[ulong]:{value}",
            0 when valueType == typeof(float) => $"[float]:{value}",
            0 when valueType == typeof(double) => $"[double]:{value}",
            0 when valueType == typeof(decimal) => $"[decimal]:{value}",
            0 when valueType == typeof(char) => $"[char]:{value}",
            0 when valueType == typeof(DateTime) => $"[DateTime]:{value}",
            0 when valueType == typeof(TimeSpan) => $"[TimeSpan]:{value}",
            0 when valueType == typeof(Guid) => $"[Guid]:{value}",
            0 when valueType == typeof(Uri) => $"[Uri]:{value}",
            0 when valueType == typeof(Version) => $"[Version]:{value}",
            0 when valueType == typeof(Type) => $"[Type]:{value}",

            // Handle common C# native array types as a special case,
            // this allows for a more concise config representation:
            0 when valueType == typeof(string[]) => $"[string[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(bool[]) => $"[bool[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(byte[]) => $"[byte[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(sbyte[]) => $"[sbyte[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(short[]) => $"[short[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(ushort[]) => $"[ushort[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(int[]) => $"[int[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(uint[]) => $"[uint[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(long[]) => $"[long[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(ulong[]) => $"[ulong[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(float[]) => $"[float[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(double[]) => $"[double[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(decimal[]) => $"[decimal[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(char[]) => $"[char[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(DateTime[]) => $"[DateTime[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(TimeSpan[]) => $"[TimeSpan[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(Guid[]) => $"[Guid[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(Uri[]) => $"[Uri[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(Version[]) => $"[Version[]]:{Common.TypeConvertToString(value)}",
            0 when valueType == typeof(Type[]) => $"[Type[]]:{Common.TypeConvertToString(value)}",

            // Handle custom type names
            _ => generateCustomTypeAndValueExpr()
        };
        
        string generateCustomTypeAndValueExpr()
        {
            string typeNamespace = valueType.Namespace ?? string.Empty;
            AssemblyName assemblyName = valueType.Assembly.GetName();
            string assemblyShortName = assemblyName.Name ?? assemblyName.FullName;
            string typeFullName = valueType.FullName ?? string.Empty;

            // Only include assembly name if it's not the core library or the type is in a different namespace
            if (assemblyShortName.Equals(s_coreLibAssembly, StringComparison.OrdinalIgnoreCase) || typeNamespace.Length > 0 && assemblyShortName.StartsWith(typeNamespace, StringComparison.OrdinalIgnoreCase))
                assemblyShortName = string.Empty;
            else
                assemblyShortName = $",{assemblyShortName}";

            // Remove version, culture and public key token from assembly name for cleaner generic strings:
            assemblyShortName = s_typeNameCleaner.Replace(assemblyShortName, string.Empty);
            typeFullName = s_typeNameCleaner.Replace(typeFullName, string.Empty);

            // At least for built-in types, this will produce a cleaner generic string like:
            //      System.Collections.Generic.List`1[[System.String]]
            // instead of:
            //      System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]]
            assemblyShortName = assemblyShortName.Replace(s_prefixedCoreLibAssembly, string.Empty);
            typeFullName = typeFullName.Replace(s_prefixedCoreLibAssembly, string.Empty);

            return $"[{typeFullName}{assemblyShortName}]:{Common.TypeConvertToString(value)}";
        }
    }

    private const string TypeNameCleanerPattern = @",\s*Version\s*=\s*[^,]+,\s*Culture\s*=\s*[^,]+,\s*PublicKeyToken\s*=\s*[^,\]]+";
    private static readonly Regex s_typeNameCleaner;
    private static readonly string s_coreLibAssembly;
    private static readonly string s_prefixedCoreLibAssembly;

    static SettingsSection()
    {
#if NET
        s_typeNameCleaner = GenerateTypeNameCleanerRegex();
#else
        s_typeNameCleaner = new Regex(TypeNameCleanerPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
#endif

        // Derive core library assembly name, e.g., "System.Private.CoreLib"
        string assemblyQualifiedName = typeof(string).AssemblyQualifiedName!;
        s_coreLibAssembly = s_typeNameCleaner.Replace(assemblyQualifiedName[(assemblyQualifiedName.IndexOf(',') + 1)..].Trim(), string.Empty);
        s_prefixedCoreLibAssembly = $", {s_coreLibAssembly}";
    }

#if NET
    [GeneratedRegex(TypeNameCleanerPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex GenerateTypeNameCleanerRegex();
#endif
}
