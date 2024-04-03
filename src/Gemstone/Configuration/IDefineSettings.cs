//******************************************************************************************************
//  IDefineSettings.cs - Gbtc
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
//  03/28/2024 - Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

namespace Gemstone.Configuration;

/// <summary>
/// Defines as interface that specifies that this object can define settings for a config file.
/// </summary>
public interface IDefineSettings
{
    /// <summary>
    /// Establishes default settings for the config file.
    /// </summary>
    /// <param name="settings">Settings instance used to hold configuration.</param>
    /// <param name="settingsCategory">The config file settings category under which the settings are defined.</param>
#if NET
    static abstract void DefineSettings(Settings settings, string settingsCategory);
#else
    static void DefineSettings(Settings settings, string settingsCategory) { }
#endif    
}
