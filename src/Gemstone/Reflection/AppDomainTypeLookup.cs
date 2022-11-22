//******************************************************************************************************
//  AppDomainTypeLookup.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  11/19/2016 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

#pragma warning disable CA1031 // Do not catch general exception types

namespace Gemstone.Reflection
{
    /// <summary>
    /// Defines a lookup class that searches all assemblies in the current <see cref="AppDomain"/> for all <see cref="Type"/>.
    /// </summary>
    public class AppDomainTypeLookup
    {
        #region [ Members ]

        // Fields
        private readonly object m_syncRoot;
        private readonly HashSet<Assembly> m_loadedAssemblies;
        private int m_assemblyVersionNumber;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a AppDomainTypeLookup
        /// </summary>
        public AppDomainTypeLookup()
        {
            m_assemblyVersionNumber = -1;
            m_syncRoot = new object();
            m_loadedAssemblies = new HashSet<Assembly>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets flag that determines if there is a possibility that a new assembly has been loaded and new types are available.
        /// </summary>
        public bool HasChanged => m_assemblyVersionNumber != AssemblyLoadedVersionNumber.VersionNumber;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Searches all assemblies of this <see cref="AppDomain"/> for all <see cref="Type"/>s.
        /// </summary>
        /// <returns>List of found types.</returns>
        public List<Type> FindTypes()
        {
            if (!HasChanged)
                return new List<Type>();

            lock (m_syncRoot)
            {
                m_assemblyVersionNumber = AssemblyLoadedVersionNumber.VersionNumber;

                return LoadNewAssemblies();
            }
        }

        private List<Type> LoadNewAssemblies()
        {
            List<Type> types = new();

            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly assembly in assemblies)
                {
                    if (m_loadedAssemblies.Contains(assembly))
                        continue;

                    m_loadedAssemblies.Add(assembly);

                    if (!assembly.IsDynamic)
                        FindAllModules(types, assembly);
                }
            }
            catch (Exception ex)
            {
                LibraryEvents.OnSuppressedException(this, new Exception($"Static constructor exception: {ex.Message}", ex));
            }

            return types;
        }

        private void FindAllModules(List<Type> types, Assembly assembly)
        {
            Module[] modules = assembly.GetModules(false);

            foreach (Module module in modules)
            {
                try
                {
                    FindAllTypes(types, module);
                }
                catch (Exception ex)
                {
                    LibraryEvents.OnSuppressedException(this, new Exception($"Static constructor exception: {ex.Message}", ex));
                }
            }
        }

        private void FindAllTypes(List<Type> newlyFoundObjects, Module module)
        {
            Type?[] types;

            try
            {
                types = module.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Since its possible that during enumeration, the GetTypes method can error, this will allow us 
                // to enumerate the types that did not error.
                LibraryEvents.OnSuppressedException(this, new Exception($"Reflection load exception: {ex.Message}", ex));
                types = ex.Types;
            }

            foreach (Type? assemblyType in types)
            {
                try
                {
                    if (assemblyType is not null)
                        newlyFoundObjects.Add(assemblyType);
                }
                catch (Exception ex)
                {
                    LibraryEvents.OnSuppressedException(this, new Exception($"Static constructor exception: {ex.Message}", ex));
                }
            }
        }

        #endregion
    }
}
