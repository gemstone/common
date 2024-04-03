//******************************************************************************************************
//  IAppSettingsBuilder.cs - Gbtc
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
//  06/13/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;

namespace Gemstone.Configuration.AppSettings
{
    /// <summary>
    /// Builder for app settings with descriptions.
    /// </summary>
    public interface IAppSettingsBuilder
    {
        /// <summary>
        /// Adds an app setting to the builder.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        /// <param name="description">A description of the setting.</param>
        /// <returns>The app settings builder.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="name"/> is a duplicate of a previously added app setting</exception>
        public IAppSettingsBuilder Add(string name, string value, string description);

        /// <summary>
        /// Converts the app settings into a collection of key/value pairs.
        /// </summary>
        /// <returns>The collection of key/value pairs.</returns>
        public IEnumerable<KeyValuePair<string, string>> Build();
    }
}
