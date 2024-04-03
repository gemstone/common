//******************************************************************************************************
//  ReadOnlyConfigurationProvider.cs - Gbtc
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

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Gemstone.Configuration.ReadOnly
{
    /// <summary>
    /// Wrapper for <see cref="IConfigurationProvider"/> to block calls
    /// to <see cref="IConfigurationProvider.Set(string, string)"/>.
    /// </summary>
    /// <seealso cref="ReadOnlyConfigurationSource"/>
    public class ReadOnlyConfigurationProvider : IConfigurationProvider
    {
        internal IConfigurationProvider Provider { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyConfigurationProvider"/> class.
        /// </summary>
        /// <param name="provider"></param>
        public ReadOnlyConfigurationProvider(IConfigurationProvider provider) =>
            Provider = provider;

        /// <summary>
        /// Returns the immediate descendant configuration keys for a given parent path based
        /// on this <see cref="IConfigurationProvider"/>s data and the set of keys returned by
        /// all the preceding <see cref="IConfigurationProvider"/>s.
        /// </summary>
        /// <param name="earlierKeys">The child keys returned by the preceding providers for the same parent path.</param>
        /// <param name="parentPath">The parent path.</param>
        /// <returns>The child keys.</returns>
        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath) =>
            Provider.GetChildKeys(earlierKeys, parentPath);

        /// <summary>
        /// Returns a change token if this provider supports change tracking, null otherwise.
        /// </summary>
        /// <returns>The change token.</returns>
        public IChangeToken GetReloadToken() =>
            Provider.GetReloadToken();

        /// <summary>
        /// Loads configuration values from the source represented by this <see cref="IConfigurationProvider"/>.
        /// </summary>
        public void Load() =>
            Provider.Load();

        /// <summary>
        /// Sets a configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, string value) { }

        /// <summary>
        /// Tries to get a configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if a value for the specified key was found, otherwise false.</returns>
        public bool TryGet(string key, out string value) =>
            Provider.TryGet(key, out value);
    }
}
