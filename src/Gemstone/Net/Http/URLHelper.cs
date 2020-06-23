//******************************************************************************************************
//  URLHelper.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  06/22/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Gemstone.Net.Http
{
    /// <summary>
    /// Defines helper functions for handling URLs.
    /// </summary>
    public static class URLHelper
    {
        private static string[] ValidSchemes { get; } = { "http", "https" };

        /// <summary>
        /// Determines if the string contains a valid URL.
        /// </summary>
        /// <param name="url">The string to check for a valid URL.</param>
        /// <returns>True if the URL is valid; otherwise false.</returns>
        public static bool IsValid(string url) =>
            Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) &&
            ValidSchemes.Contains(uri.Scheme);

        /// <summary>
        /// Opens the user's default browser to the given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to in the user's browser.</param>
        /// <exception cref="SecurityException">The input string is not a valid URL.</exception>
        /// <exception cref="NotSupportedException">Navigation attempted on a platform other than Windows, Linux, or OSX.</exception>
        public static void NavigateInDefaultBrowser(string url)
        {
            if (!IsValid(url))
                throw new SecurityException($"URL invalid - unable to navigate: {url}");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(url);
                startInfo.UseShellExecute = true;
                using (Process.Start(startInfo)) { }
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                using (Process.Start("xdg-open", url)) { }
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                using (Process.Start("open", url)) { }
                return;
            }

            throw new NotSupportedException($"Default browser navigation not supported on this platform");
        }
    }
}
