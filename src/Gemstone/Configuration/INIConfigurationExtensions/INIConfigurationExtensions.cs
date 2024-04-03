//******************************************************************************************************
//  INIConfigurationExtensions.cs - Gbtc
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
//  06/14/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Gemstone.Configuration.AppSettings;
using Microsoft.Extensions.Configuration;

namespace Gemstone.Configuration.INIConfigurationExtensions;

/// <summary>
/// Defines extensions for setting up configuration defaults for Gemstone projects.
/// </summary>
public static class INIConfigurationExtensions
{
    /// <summary>
    /// Generates the contents of an INI file based on the configuration settings.
    /// </summary>
    /// <param name="configuration">Source configuration.</param>
    /// <param name="writeValue">Flag that determines whether the actual value (instead of default only) of each setting should be written to the INI file.</param>
    /// <param name="splitDescriptionLines">Flag that determines whether long description lines should be split into multiple lines.</param>
    /// <returns>Generated INI file contents.</returns>
    public static string GenerateINIFileContents(this IConfiguration configuration, bool writeValue, bool splitDescriptionLines)
    {
        static IEnumerable<string> Split(string str, int maxLineLength)
        {
            string[] lines = str.Split(["\r\n", "\n"], StringSplitOptions.None);

            foreach (string line in lines)
            {
                string leftover = line.TrimStart();

                // Lines in the original text that contain
                // only whitespace will be returned as-is
                if (leftover.Length == 0)
                    yield return line;

                while (leftover.Length > 0)
                {
                    char[] chars = leftover
                        .Take(maxLineLength + 1)
                        .Reverse()
                        .SkipWhile(c => !char.IsWhiteSpace(c))
                        .SkipWhile(char.IsWhiteSpace)
                        .Reverse()
                        .ToArray();

                    if (!chars.Any())
                    {
                        // Tokens that are longer than the maximum length will
                        // be returned (in their entirety) on their own line;
                        // maxLineLength is just a suggestion
                        chars = leftover
                            .TakeWhile(c => !char.IsWhiteSpace(c))
                            .ToArray();
                    }

                    string splitLine = new(chars);
                    leftover = leftover.Substring(splitLine.Length).TrimStart();
                    yield return splitLine;
                }
            }
        }

        static bool HasAppSetting(IConfiguration section) =>
            section.GetChildren().Any(HasAppSettingDescription);

        static bool HasAppSettingDescription(IConfigurationSection setting) =>
            setting.GetAppSettingDescription() is not null;

        static string ConvertSettingToINI(IConfigurationSection setting, bool writeValue, bool splitDescriptionLines)
        {
            string key = setting.Key;
            string value = setting.Value ?? "";
            string initialValue = setting.GetAppSettingInitialValue() ?? "";
            string description = setting.GetAppSettingDescription() ?? "";

            // Break up long descriptions to be more readable in the INI file
            IEnumerable<string> descriptionLines = (splitDescriptionLines ?
                    Split(description, 78) :
                    description.Split(["\r\n", "\n"], StringSplitOptions.None))
                .Select(line => $"; {line}");

            string multilineDescription = string.Join(Environment.NewLine, descriptionLines);

            string[] lines;

            if (writeValue && value != initialValue)
            {
                lines =
                [
                    $"{multilineDescription}",
                    $"{key}={value}"
                ];
            }
            else
            {
                lines =
                [
                    $"{multilineDescription}",
                    $";{key}={initialValue}"
                ];
            }

            return string.Join(Environment.NewLine, lines);
        }

        static string ConvertConfigToINI(IConfiguration config, bool writeValue, bool splitDescriptionLines)
        {
            IEnumerable<string> settings = config.GetChildren()
                .Where(HasAppSettingDescription)
                .OrderBy(setting => setting.Key)
                .Select(setting => ConvertSettingToINI(setting, writeValue, splitDescriptionLines));

            string settingSeparator = string.Format("{0}{0}", Environment.NewLine);
            string settingsText = string.Join(settingSeparator, settings);

            // The root section has no heading
            if (config is not ConfigurationSection section)
                return settingsText;

            return string.Join(Environment.NewLine, $"[{section.Key}]", settingsText);
        }

        // Root MUST go before all other sections, so the order is important:
        //     1. Sort by section key
        //     2. Prepend root
        //     3. Filter out sections without any app settings
        IEnumerable<string> appSettingsSections = configuration.AsEnumerable()
            .Select(kvp => configuration.GetSection(kvp.Key))
            .OrderBy(section => section.Key)
            .Prepend(configuration)
            .Where(HasAppSetting)
            .Select(config => ConvertConfigToINI(config, writeValue, splitDescriptionLines));

        string sectionSeparator = string.Format("{0}{0}", Environment.NewLine);
        return string.Join(sectionSeparator, appSettingsSections);
    }
}
