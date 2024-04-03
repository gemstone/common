//******************************************************************************************************
//  ReadOnlyConfigurationExtensions.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  06/12/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration.ReadOnly
{
    /// <summary>
    /// Defines extensions for adding read-only configuration providers.
    /// </summary>
    public static class ReadOnlyConfigurationExtensions
    {
        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object? x, object? y) =>
                ReferenceEquals(x, y);

            public int GetHashCode(object obj) =>
                RuntimeHelpers.GetHashCode(obj);
        }

        /// <summary>
        /// Configures an <see cref="IConfigurationBuilder"/> with read-only configuration sources.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="builderAction">The action to set up configuration sources that will be made read-only.</param>
        /// <returns>The configuration builder.</returns>
        /// <remarks>
        /// <para>
        /// This method is intended to encapsulate the builder action that creates a group of read-only providers.
        /// </para>
        /// 
        /// <code>
        /// IConfiguration configuration = new ConfigurationBuilder()
        ///     .ConfigureReadOnly(readOnlyBuilder => readOnlyBuilder
        ///         .AddInMemoryCollection(defaultSettings)
        ///         .AddIniFile("usersettings.ini"))
        ///     .AddSQLite()
        ///     .Build();
        ///     
        /// // This will only update the SQLite configuration provider
        /// configuration["Hello"] = "World";
        /// </code>
        /// </remarks>
        public static IConfigurationBuilder ConfigureReadOnly(this IConfigurationBuilder builder, Action<IConfigurationBuilder> builderAction)
        {
            ReferenceEqualityComparer referenceEqualityComparer = new();
            HashSet<object> originalSources = new(builder.Sources, referenceEqualityComparer);
            builderAction(builder);

            for (int i = 0; i < builder.Sources.Count; i++)
            {
                IConfigurationSource source = builder.Sources[i];

                if (originalSources.Contains(source))
                    continue;

                IConfigurationSource readOnlySource = new ReadOnlyConfigurationSource(source);
                builder.Sources[i] = readOnlySource;
            }

            return builder;
        }

        /// <summary>
        /// Converts the most recently added configuration source into a read-only configuration source.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>The configuration builder.</returns>
        /// <remarks>
        /// <para>
        /// This method is intended to be chained after each source that needs to be made read-only.
        /// </para>
        /// 
        /// <code>
        /// IConfiguration configuration = new ConfigurationBuilder()
        ///     .AddInMemoryCollection(defaultSettings).AsReadOnly()
        ///     .AddIniFile("usersettings.ini").AsReadOnly()
        ///     .AddSQLite()
        ///     .Build();
        ///     
        /// // This will only update the SQLite configuration provider
        /// configuration["Hello"] = "World";
        /// </code>
        /// </remarks>
        /// <seealso cref="ReadOnlyConfigurationSource"/>
        public static IConfigurationBuilder AsReadOnly(this IConfigurationBuilder builder)
        {
            int index = builder.Sources.Count - 1;
            IConfigurationSource source = builder.Sources[index];
            IConfigurationSource readOnlySource = new ReadOnlyConfigurationSource(source);
            builder.Sources[index] = readOnlySource;
            return builder;
        }
    }
}
