//******************************************************************************************************
//  AssemblyInfo.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/29/2005 - Pinal C. Patel
//       Generated original version of source code.
//  12/29/2005 - Pinal C. Patel
//       Migrated 2.0 version of source code from 1.1 source (GSF.Shared.Assembly).
//  12/12/2007 - Darrell Zuercher
//       Edited Code Comments.
//  09/08/2008 - J. Ritchie Carroll
//       Converted to C# as AssemblyInformation.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  10/21/2009 - Pinal C. Patel
//       Added error checking to assembly attribute properties.
//  09/28/2010 - Pinal C. Patel
//       Modified EntryAssembly to perform a reflection only load of the currently executing process 
//       to deal with entry assembly not being available in non-default application domains.
//       Changed GetCustomAttribute() to return CustomAttributeData instead of Object to deal with
//       possible reflection only load being performed in EntryAssembly.
//       Removed debuggable property since it was not very useful and added complexity when extracting.
//  09/21/2011 - J. Ritchie Carroll
//       Added Mono implementation exception regions.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

// Ignore Spelling: Debuggable

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using Gemstone.IO;
using Gemstone.Reflection.AssemblyExtensions;

namespace Gemstone.Reflection;

/// <summary>
/// Represents a common information provider for an assembly.
/// </summary>
public class AssemblyInfo
{
    #region [ Constructors ]

    /// <summary>Initializes a new instance of the <see cref="AssemblyInfo"/> class.</summary>
    /// <param name="assemblyInstance">An <see cref="Assembly"/> object.</param>
    public AssemblyInfo(Assembly assemblyInstance) => Assembly = assemblyInstance;

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets the underlying <see cref="Assembly"/> being represented by this <see cref="AssemblyInfo"/> object.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the title information of the <see cref="Assembly"/>.
    /// </summary>
    public string Title
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyTitleAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the description information of the <see cref="Assembly"/>.
    /// </summary>
    public string Description
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyDescriptionAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the company name information of the <see cref="Assembly"/>.
    /// </summary>
    public string Company
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyCompanyAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the product name information of the <see cref="Assembly"/>.
    /// </summary>
    public string Product
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyProductAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the copyright information of the <see cref="Assembly"/>.
    /// </summary>
    public string Copyright
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyCopyrightAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the trademark information of the <see cref="Assembly"/>.
    /// </summary>
    public string Trademark
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyTrademarkAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the configuration information of the <see cref="Assembly"/>.
    /// </summary>
    public string Configuration
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyConfigurationAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets a boolean value indicating if the <see cref="Assembly"/> has been built as delay-signed.
    /// </summary>
    public bool DelaySign
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyDelaySignAttribute));

            if (attribute is null)
                return false;

            return (bool?)attribute.ConstructorArguments[0].Value ?? false;
        }
    }

    /// <summary>
    /// Gets the version information of the <see cref="Assembly"/>.
    /// </summary>
    public string InformationalVersion
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the name of the file containing the key pair used to generate a strong name for the attributed <see cref="Assembly"/>.
    /// </summary>
    public string KeyFile
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(AssemblyKeyFileAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the culture name of the <see cref="Assembly"/>.
    /// </summary>
    public string CultureName
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(NeutralResourcesLanguageAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the assembly version used to instruct the System.Resources.ResourceManager to ask for a particular
    /// version of a satellite assembly to simplify updates of the main assembly of an application.
    /// </summary>
    public string SatelliteContractVersion
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(SatelliteContractVersionAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the string representing the assembly version used to indicate to a COM client that all classes
    /// in the current version of the assembly are compatible with classes in an earlier version of the assembly.
    /// </summary>
    public string ComCompatibleVersion
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(ComCompatibleVersionAttribute));

            if (attribute is null)
                return string.Empty;

            return $"{attribute.ConstructorArguments[0].Value}.{attribute.ConstructorArguments[1].Value}.{attribute.ConstructorArguments[2].Value}.{attribute.ConstructorArguments[3].Value}";
        }
    }

    /// <summary>
    /// Gets a boolean value indicating if the <see cref="Assembly"/> is exposed to COM.
    /// </summary>
    public bool ComVisible
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(ComVisibleAttribute));

            if (attribute is null)
                return false;

            return (bool?)attribute.ConstructorArguments[0].Value ?? false;
        }
    }

    /// <summary>
    /// Gets a boolean value indicating if the <see cref="Assembly"/> was built in debug mode.
    /// </summary>
    public bool Debuggable
    {
        get
        {
            DebuggableAttribute? attribute = Assembly.GetCustomAttributes<DebuggableAttribute>().FirstOrDefault();
            return attribute?.IsJITOptimizerDisabled ?? false;
        }
    }

    /// <summary>
    /// Gets the GUID that is used as an ID if the <see cref="Assembly"/> is exposed to COM.
    /// </summary>
    public string Guid
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(GuidAttribute));

            if (attribute is null)
                return string.Empty;

            return attribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets a boolean value indicating whether the <see cref="Assembly"/> is CLS-compliant.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public bool CLSCompliant
    {
        get
        {
            CustomAttributeData? attribute = GetCustomAttribute(typeof(CLSCompliantAttribute));

            if (attribute is null)
                return false;

            return (bool?)attribute.ConstructorArguments[0].Value ?? false;
        }
    }

    /// <summary>
    /// Gets the path or UNC location of the loaded file that contains the manifest.
    /// </summary>
    public string Location => Assembly.Location;

    /// <summary>
    /// Gets the display name of the <see cref="Assembly"/>.
    /// </summary>
    public string? FullName => Assembly.FullName;

    /// <summary>
    /// Gets the simple, unencrypted name of the <see cref="Assembly"/>.
    /// </summary>
    public string? Name => Assembly.GetName().Name;

    /// <summary>
    /// Gets the major, minor, revision, and build numbers of the <see cref="Assembly"/>.
    /// </summary>
    public Version? Version => Assembly.GetName().Version;

    /// <summary>
    /// Gets the string representing the version of the common language runtime (CLR) saved in the file
    /// containing the manifest.
    /// </summary>
    public string ImageRuntimeVersion => Assembly.ImageRuntimeVersion;

    /// <summary>
    /// Gets the date and time when the <see cref="Assembly"/> was built.
    /// </summary>
    public DateTime BuildDate => File.GetLastWriteTime(Assembly.Location);

    private string? m_rootNamespace;

    /// <summary>
    /// Gets the root namespace of the <see cref="Assembly"/>.
    /// </summary>
    public string RootNamespace
    {
        get
        {
            string getRootNamespace()
            {
                Type[] exportedTypes = Assembly.GetExportedTypes();
                return exportedTypes.Length == 0 ? string.Empty : exportedTypes[0].Namespace ?? string.Empty;
            }

            return m_rootNamespace ??= getRootNamespace();
        }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Gets a collection of assembly attributes exposed by the assembly.
    /// </summary>
    /// <returns>A System.Specialized.KeyValueCollection of assembly attributes.</returns>
    public NameValueCollection GetAttributes()
    {
        NameValueCollection assemblyAttributes = new()
        {
            { "Full Name", FullName },
            { "Name", Name },
            { "Version", Version?.ToString() ?? "<undefined>" },
            { "Image Runtime Version", ImageRuntimeVersion },
            { "Build Date", BuildDate.ToString(CultureInfo.InvariantCulture) },
            { "Location", Location },
            //{ "Code Base", CodeBase },
            //{ "GAC Loaded", GACLoaded.ToString() },
            { "Title", Title },
            { "Description", Description },
            { "Company", Company },
            { "Product", Product },
            { "Copyright", Copyright },
            { "Trademark", Trademark },
            { "Configuration", Configuration },
            { "Delay Sign", DelaySign.ToString() },
            { "Informational Version", InformationalVersion },
            { "Key File", KeyFile },
            { "Culture Name", CultureName },
            { "Satellite Contract Version", SatelliteContractVersion },
            { "Com Compatible Version", ComCompatibleVersion },
            { "Com Visible", ComVisible.ToString() },
            { "Guid", Guid },
            { "CLS Compliant", CLSCompliant.ToString() }
        };

        return assemblyAttributes;
    }

    /// <summary>
    /// Gets the specified assembly attribute if it is exposed by the assembly.
    /// </summary>
    /// <param name="attributeType">Type of the attribute to get.</param>
    /// <returns>The requested assembly attribute if it exists; otherwise null.</returns>
    /// <remarks>
    /// This method always returns <c>null</c> under Mono deployments.
    /// </remarks>
    public CustomAttributeData? GetCustomAttribute(Type attributeType) => // Returns the requested assembly attribute
        Assembly.GetCustomAttributesData().FirstOrDefault(assemblyAttribute => assemblyAttribute.Constructor.DeclaringType == attributeType);

    /// <summary>
    /// Gets the specified embedded resource from the assembly.
    /// </summary>
    /// <param name="resourceName">The full name (including the namespace) of the embedded resource to get.</param>
    /// <returns>The embedded resource.</returns>
    public Stream? GetEmbeddedResource(string resourceName) =>
        Assembly.GetEmbeddedResource(resourceName);

    #endregion

    #region [ Static ]

    // Static Fields
    private static AssemblyInfo? s_callingAssembly;
    private static AssemblyInfo? s_entryAssembly;
    private static AssemblyInfo? s_executingAssembly;
    private static Dictionary<string, Assembly>? s_assemblyCache;
    private static bool s_addedResolver;
    private static readonly Dictionary<string, Type> s_typeCache = new();
    private static readonly AppDomainTypeLookup s_typeLookup = new();

    // Static Properties

    /// <summary>
    /// Finds the specified <paramref name="typeName"/> searching through all loaded assemblies.
    /// </summary>
    /// <param name="typeName">Fully qualified type name.</param>
    /// <returns>The <see cref="Type"/> found; otherwise <c>null</c>.</returns>
    public static Type? FindType(string typeName)
    {
        lock (s_typeCache)
        {
            if (s_typeLookup.HasChanged)
            {
                foreach (Type type in s_typeLookup.FindTypes())
                    s_typeCache[type.FullName ?? type.Name] = type;
            }

            s_typeCache.TryGetValue(typeName, out Type? result);

            return result;
        }
    }

    /// <summary>
    /// Gets the <see cref="AssemblyInfo"/> object of the assembly that invoked the currently executing method.
    /// </summary>
    public static AssemblyInfo? CallingAssembly
    {
        get
        {
            if (s_callingAssembly is not null)
                return s_callingAssembly;

            // We have to find the calling assembly of the caller
            StackTrace trace = new();
            Assembly caller = Assembly.GetCallingAssembly();
            Assembly current = Assembly.GetExecutingAssembly();

            StackFrame[] stackFrames = trace.GetFrames();

            //if (stackFrames is null)
            //    return s_callingAssembly;

            foreach (StackFrame frame in stackFrames)
            {
                MethodBase? method = frame.GetMethod();

                if (method?.DeclaringType is null)
                    continue;

                Assembly? assembly = Assembly.GetAssembly(method.DeclaringType);

                if (assembly == caller || assembly == current || assembly is null)
                    continue;

                // Assembly is neither the current assembly or the calling assembly
                s_callingAssembly = new AssemblyInfo(assembly);
                break;
            }

            return s_callingAssembly;
        }
    }

    /// <summary>
    /// Gets the <see cref="AssemblyInfo"/> object of the process executable in the default application domain.
    /// </summary>
    public static AssemblyInfo? EntryAssembly
    {
        get
        {
            if (s_entryAssembly is not null)
                return s_entryAssembly;

            Assembly? entryAssembly = Assembly.GetEntryAssembly();

            //if (entryAssembly is null)
            //{
            //    string? mainModuleFileName = Process.GetCurrentProcess().MainModule?.FileName;

            //    if (!string.IsNullOrWhiteSpace(mainModuleFileName))
            //        entryAssembly = Assembly.ReflectionOnlyLoadFrom(mainModuleFileName);
            //}

            if (entryAssembly is null)
                return null;
                    
            s_entryAssembly = new AssemblyInfo(entryAssembly);

            return s_entryAssembly;
        }
    }

    /// <summary>
    /// Gets the <see cref="AssemblyInfo"/> object of the assembly that contains the code that is currently executing.
    /// </summary>
    public static AssemblyInfo ExecutingAssembly => s_executingAssembly ??= new AssemblyInfo(Assembly.GetCallingAssembly());

    // Static Methods

    /// <summary>
    /// Loads the specified assembly that is embedded as a resource in the assembly.
    /// </summary>
    /// <param name="assemblyName">Name of the assembly to load.</param>
    /// <remarks>Note that this function cannot be used to load Gemstone.Core itself, since this is where function resides.</remarks>
    [SecurityCritical]
    public static void LoadAssemblyFromResource(string assemblyName)
    {
        // Hooks into assembly resolve event for current domain so it can load assembly from embedded resource
        if (!s_addedResolver)
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyFromResource;
            s_addedResolver = true;
        }

        // Loads the assembly (this will invoke event that will resolve assembly from resource)
        AppDomain.CurrentDomain.Load(assemblyName);
    }

    private static Assembly? ResolveAssemblyFromResource(object? sender, ResolveEventArgs e)
    {
        string shortName = e.Name.Split(',')[0];

        s_assemblyCache ??= new Dictionary<string, Assembly>();

        if (s_assemblyCache.TryGetValue(shortName, out Assembly? resourceAssembly))
            return resourceAssembly;

        // Loops through all of the resources in the executing assembly
        foreach (string name in Assembly.GetEntryAssembly()?.GetManifestResourceNames() ?? Array.Empty<string>())
        {
            // Sees if the embedded resource name matches the assembly it is trying to load.
            if (string.Compare(FilePath.GetFileNameWithoutExtension(name), $"{EntryAssembly?.RootNamespace ?? nameof(Gemstone)}.{shortName}", StringComparison.OrdinalIgnoreCase) != 0)
                continue;

            // If so, loads embedded resource assembly into a binary buffer
            Stream? resourceStream = Assembly.GetEntryAssembly()?.GetManifestResourceStream(name);

            if (resourceStream is not null)
            {
                byte[] buffer = new byte[resourceStream.Length];

                resourceStream.Read(buffer, 0, (int)resourceStream.Length);
                resourceStream.Close();

                // Loads assembly from binary buffer
                resourceAssembly = Assembly.Load(buffer);

                // Add assembly to the cache
                s_assemblyCache.Add(shortName, resourceAssembly);
            }

            break;
        }

        return resourceAssembly;
    }

    #endregion
}
