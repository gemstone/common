//******************************************************************************************************
//  ReadOnlyConfigurationSource.cs - Gbtc
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

using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration.ReadOnly
{
    /// <summary>
    /// Wrapper for <see cref="IConfigurationSource"/> to block calls
    /// to <see cref="IConfigurationProvider.Set(string, string)"/>.
    /// </summary>
    /// <remarks>
    /// Configuration providers are typically designed to load configuration into an in-memory
    /// dictionary from their configuration source. Subsequently, the in-memory dictionary can be
    /// modified programmatically via the <see cref="P:IConfiguration.Item(int)"/> indexer.
    /// This class blocks calls to <see cref="IConfigurationProvider.Set(string, string)"/>
    /// on the underlying configuration source's provider so that static defaults won't be
    /// modified when updating configuration.
    /// </remarks>
    public class ReadOnlyConfigurationSource : IConfigurationSource
    {
        private IConfigurationSource Source { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyConfigurationSource"/> class.
        /// </summary>
        /// <param name="source">The source to be made read-only.</param>
        public ReadOnlyConfigurationSource(IConfigurationSource source) =>
            Source = source;

        /// <summary>
        /// Builds the <see cref="IConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The configuration builder</param>
        /// <returns>The read-only configuration provider.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            IConfigurationProvider provider = Source.Build(builder);
            return new ReadOnlyConfigurationProvider(provider);
        }
    }
}
