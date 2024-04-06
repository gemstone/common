//******************************************************************************************************
//  INIConfigurationHelpers.cs - Gbtc
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
//  06/14/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;

namespace Gemstone.Configuration;

/// <summary>
/// Defines helper functions for working with INI configuration files.
/// </summary>
internal static class INIConfigurationHelpers
{
    /// <summary>
    /// Gets file path for INI configuration file.
    /// </summary>
    /// <param name="fileName">Target file INI file name.</param>
    /// <returns>INI file path.</returns>
    public static string GetINIFilePath(string fileName)
    {
        Environment.SpecialFolder specialFolder = Environment.SpecialFolder.CommonApplicationData;
        string appDataPath = Environment.GetFolderPath(specialFolder);
        return Path.Combine(appDataPath, Common.ApplicationName, fileName);
    }

    /// <summary>
    /// Gets an INI file writer for the specified path.
    /// </summary>
    /// <param name="path">Path for INI file.</param>
    /// <returns>INI file write at specified path.</returns>
    public static TextWriter GetINIFileWriter(string path)
    {
        if (File.Exists(path))
            return File.CreateText(path);

        string directoryPath = Path.GetDirectoryName(path) ?? string.Empty;
        Directory.CreateDirectory(directoryPath);

        return File.CreateText(path);
    }

    /// <summary>
    /// Gets an INI file reader for the specified path.
    /// </summary>
    /// <param name="path">Path for INI file.</param>
    /// <returns>INI file reader at specified path.</returns>
    public static TextReader GetINIFileReader(string path)
    {
        return File.Exists(path) ? File.OpenText(path) : TextReader.Null;
    }
}
