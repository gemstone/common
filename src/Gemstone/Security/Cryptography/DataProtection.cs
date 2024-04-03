//******************************************************************************************************
//  DataProtection.cs - Gbtc
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
//  02/26/2023 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using Gemstone.Caching;
using Gemstone.Configuration;
using Microsoft.AspNetCore.DataProtection;

namespace Gemstone.Security.Cryptography
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data.
    /// </summary>
    /// <remarks>
    /// This is a safety wrapper around the <see cref="IDataProtector"/> class such that it can be used with
    /// <c>LocalMachine</c> scope regardless of current user. This is especially important for applications
    /// that may be running as user account that has no association to the current user, e.g., an Azure AD
    /// user or database account when authenticated using <c>AdoSecurityProvider</c>.
    /// </remarks>
    public static class DataProtection
    {
        /// <summary>
        /// Folder name for data protection keys.
        /// </summary>
        public const string DataProtectionKeysFolder = "DataProtectionKeys";

        /// <summary>
        /// Default settings category for cryptography services.
        /// </summary>
        public const string DefaultSettingsCategory = "CryptographyServices";

        /// <summary>
        /// Default timeout, in minutes, for user-specific data protection provider.
        /// </summary>
        public const double DefaultUserDataProtectionTimeout = 5.0D;

        private static IDataProtector? s_localMachineDataProtector;

        /// <summary>
        /// Encrypts the data in a specified byte array and returns a byte array that contains the encrypted data.
        /// </summary>
        /// <returns>A byte array representing the encrypted data.</returns>
        /// <param name="unencryptedData">A byte array that contains data to encrypt.</param>
        /// <param name="optionalEntropy">An optional additional byte array used to increase the complexity of the encryption, or null for no additional complexity.</param>
        /// <param name="protectToLocalMachine">Set to <c>true</c> to protect data to the local machine; otherwise, set to <c>false</c> to protect data to the current user.</param>
        /// <param name="settingsCategory">The config file settings category under which the settings are defined.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="unencryptedData"/> parameter is null.</exception>
        /// <exception cref="CryptographicException">The encryption failed.</exception>
        public static byte[] Protect(byte[] unencryptedData, byte[]? optionalEntropy = null, bool protectToLocalMachine = true, string settingsCategory = DefaultSettingsCategory)
        {
            if (unencryptedData is null)
                throw new ArgumentNullException(nameof(unencryptedData));

            if (!protectToLocalMachine)
                return ProtectData(GetDataProtector(false, settingsCategory), unencryptedData, optionalEntropy);

            IPrincipal? principal = Thread.CurrentPrincipal;
            byte[] protectedBytes;

            try
            {
                Thread.CurrentPrincipal = null;
                protectedBytes = ProtectData(GetDataProtector(true, settingsCategory), unencryptedData, optionalEntropy);
            }
            finally
            {
                Thread.CurrentPrincipal = principal;
            }

            return protectedBytes;
        }

        /// <summary>
        /// Decrypts the data in a specified byte array and returns a byte array that contains the decrypted data.
        /// </summary>
        /// <returns>A byte array representing the decrypted data.</returns>
        /// <param name="encryptedData">A byte array containing data encrypted using the <see cref="Protect"/> method.</param>
        /// <param name="optionalEntropy">An optional additional byte array that was used to encrypt the data, or null if the additional byte array was not used.</param>
        /// <param name="protectToLocalMachine">Set to <c>true</c> to protect data to the local machine; otherwise, set to <c>false</c> to protect data to the current user.</param>
        /// <param name="settingsCategory">The config file settings category under which the settings are defined.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="encryptedData"/> parameter is null.</exception>
        /// <exception cref="CryptographicException">The decryption failed.</exception>
        public static byte[] Unprotect(byte[] encryptedData, byte[]? optionalEntropy = null, bool protectToLocalMachine = true, string settingsCategory = DefaultSettingsCategory)
        {
            if (encryptedData is null)
                throw new ArgumentNullException(nameof(encryptedData));

            if (!protectToLocalMachine)
                return UnprotectData(GetDataProtector(false, settingsCategory), encryptedData, optionalEntropy);

            IPrincipal? principal = Thread.CurrentPrincipal;
            byte[] unprotectedBytes;

            try
            {
                Thread.CurrentPrincipal = null;
                unprotectedBytes = UnprotectData(GetDataProtector(true, settingsCategory), encryptedData, optionalEntropy);
            }
            finally
            {
                Thread.CurrentPrincipal = principal;
            }

            return unprotectedBytes;
        }

        private static byte[] ProtectData(IDataProtector dataProtector, byte[] data, byte[]? optionalEntropy)
        {
            if (optionalEntropy is null || optionalEntropy.Length == 0)
                return dataProtector.Protect(data);

            data = [..optionalEntropy, ..data];

            return dataProtector.Protect(data);
        }

        private static byte[] UnprotectData(IDataProtector dataProtector, byte[] data, byte[]? optionalEntropy)
        {
            if (optionalEntropy is null || optionalEntropy.Length == 0)
                return dataProtector.Unprotect(data);

            byte[] unprotectedData = dataProtector.Unprotect(data);

            if (unprotectedData.Length < optionalEntropy.Length || !optionalEntropy.SequenceEqual(unprotectedData[..optionalEntropy.Length]))
                throw new CryptographicException("Data was not protected using the specified optional entropy.");

            return unprotectedData[optionalEntropy.Length..];
        }

        private static IDataProtector GetDataProtector(bool protectToLocalMachine, string settingsCategory)
        {
            Lazy<string> applicationName = new(() => Settings.Default[settingsCategory].ApplicationName ?? Common.ApplicationName);

            if (protectToLocalMachine)
                return s_localMachineDataProtector ??= CreateDataProtector(true, applicationName.Value, "Machine");

            ClaimsPrincipal? currentPrincipal = ClaimsPrincipal.Current;

            currentPrincipal ??= Thread.CurrentPrincipal as ClaimsPrincipal;

            string userName = currentPrincipal?.Identity?.Name ?? Environment.UserName;
            double cacheTimeout = Settings.Default[settingsCategory].UserDataProtectionTimeout ?? DefaultUserDataProtectionTimeout;

            return MemoryCache<IDataProtector>.GetOrAdd(userName, cacheTimeout, () => CreateDataProtector(false, applicationName.Value, $"User:{userName.ToLowerInvariant()}"));
        }

        private static IDataProtector CreateDataProtector(bool protectToLocalMachine, string applicationName, string target)
        {
            string keyFilePath = GetKeyFilePath(DataProtectionKeysFolder, applicationName, protectToLocalMachine);

            IDataProtectionProvider dataProtectionProvider = DataProtectionProvider.Create(
                new DirectoryInfo(keyFilePath),
                configuration =>
                {
                    configuration.SetApplicationName(applicationName);

                #pragma warning disable CA1416
                    if (!Common.IsPosixEnvironment)
                        configuration.ProtectKeysWithDpapi(protectToLocalMachine);
                #pragma warning restore CA1416
                });

            return dataProtectionProvider.CreateProtector($"{nameof(Gemstone)}:{applicationName}:{target}");
        }

        private static string GetKeyFilePath(string keyFolder, string applicationName, bool protectToLocalMachine)
        {
            string appDataPath = Environment.GetFolderPath(protectToLocalMachine ? 
                Environment.SpecialFolder.CommonApplicationData : 
                Environment.SpecialFolder.LocalApplicationData);

            return Path.Combine(appDataPath, applicationName, keyFolder);
        }

        /// <inheritdoc cref="IDefineSettings.DefineSettings" />
        public static void DefineSettings(Settings settings, string settingsCategory = DefaultSettingsCategory)
        {
            dynamic section = settings[settingsCategory];

            section.ApplicationName = (Common.ApplicationName, "Name of the application using the data protection provider used for key discrimination.");
            section.UserDataProtectionTimeout = (DefaultUserDataProtectionTimeout, "Timeout, in minutes, for cached user-specific data protection provider.");
        }
    }
}
