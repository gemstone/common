//******************************************************************************************************
//  AppSettingsExtensions.cs - Gbtc
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration.AppSettings
{
    /// <summary>
    /// Defines extensions for managing app settings.
    /// </summary>
    public static class AppSettingsExtensions
    {
        private const string InitialValueKey = "__AppSettings:InitialValue";
        private const string DescriptionKey = "__AppSettings:Description";

        /// <summary>
        /// Adds an <see cref="IConfigurationSource"/> for app settings to the given <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <param name="buildAction">The action to build app settings.</param>
        /// <returns>The configuration builder.</returns>
        /// <remarks>
        /// This extension provides a simple way to add default values as well as descriptions for app settings
        /// directly into an application. The source for these is a simple in-memory collection, and additional
        /// key/value pairs are added so that the initial value and descriptions of these settings can still be
        /// retrieved even if the settings themselves get overridden by another configuration source.
        /// </remarks>
        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configurationBuilder, Action<IAppSettingsBuilder> buildAction)
        {
            IAppSettingsBuilder appSettingsBuilder = new AppSettingsBuilder();
            buildAction(appSettingsBuilder);

            IEnumerable<KeyValuePair<string, string>> appSettings = appSettingsBuilder.Build();
            configurationBuilder.AddInMemoryCollection(appSettings);
            return configurationBuilder;
        }

        /// <summary>
        /// Gets the initial value of the app setting with the given name.
        /// </summary>
        /// <param name="configuration">The configuration that contains the app setting.</param>
        /// <param name="name">The name of the app setting.</param>
        /// <returns>The initial value of the app setting.</returns>
        public static string? GetAppSettingInitialValue(this IConfiguration configuration, string name)
        {
            return configuration[name.ToInitialValueKey()];
        }

        /// <summary>
        /// Gets the description of the app setting with the given name.
        /// </summary>
        /// <param name="configuration">The configuration that contains the app setting.</param>
        /// <param name="name">The name of the app setting.</param>
        /// <returns>The initial value of the app setting.</returns>
        public static string? GetAppSettingDescription(this IConfiguration configuration, string name)
        {
            return configuration[name.ToDescriptionKey()];
        }

        /// <summary>
        /// Gets the initial value of the given app setting.
        /// </summary>
        /// <param name="setting">The app setting.</param>
        /// <returns>The initial value of the app setting.</returns>
        public static string? GetAppSettingInitialValue(this IConfigurationSection setting) =>
            setting[InitialValueKey];

        /// <summary>
        /// Gets the description of the given app setting.
        /// </summary>
        /// <param name="setting">The app setting.</param>
        /// <returns>The description of the app setting.</returns>
        public static string? GetAppSettingDescription(this IConfigurationSection setting) =>
            setting[DescriptionKey];

        internal static string ToInitialValueKey(this string appSettingName) =>
            $"{appSettingName}:{InitialValueKey}";

        internal static string ToDescriptionKey(this string appSettingName) =>
            $"{appSettingName}:{DescriptionKey}";

        // Implementation of IAppSettingsBuilder that works
        // with the extension methods defined in this class.
        private class AppSettingsBuilder : IAppSettingsBuilder
        {
            private class AppSetting
            {
                public string Name { get; }
                public string Value { get; }
                public string Description { get; }

                public AppSetting(string name, string value, string description)
                {
                    Name = name;
                    Value = value;
                    Description = description;
                }

                public KeyValuePair<string, string> ToKeyValuePair() => new(Name, Value);

                public KeyValuePair<string, string> ToInitialValuePair()
                {
                    string key = Name.ToInitialValueKey();
                    return new KeyValuePair<string, string>(key, Value);
                }

                public KeyValuePair<string, string> ToDescriptionPair()
                {
                    string key = Name.ToDescriptionKey();
                    return new KeyValuePair<string, string>(key, Description);
                }
            }

            private Dictionary<string, AppSetting> AppSettingLookup { get; } = new(StringComparer.OrdinalIgnoreCase);

            public IAppSettingsBuilder Add(string name, string value, string description)
            {
                if (AppSettingLookup.ContainsKey(name))
                {
                    //throw new ArgumentException($"Unable to add duplicate app setting: {name}", nameof(name));
                    Debug.WriteLine($"Duplicate app setting encountered: {name}\" - this can be normal.");
                    return this;
                }

                AppSetting appSetting = new(name, value, description);
                AppSettingLookup.Add(name, appSetting);
                return this;
            }

            public IEnumerable<KeyValuePair<string, string>> Build()
            {
                return AppSettingLookup.Values.SelectMany(appSetting => new[]
                {
                    appSetting.ToKeyValuePair(),
                    appSetting.ToInitialValuePair(),
                    appSetting.ToDescriptionPair()
                });
            }
        }
    }
}
