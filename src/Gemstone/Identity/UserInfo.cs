//******************************************************************************************************
//  UserInfo.cs - Gbtc
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
//  01/03/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.Principal;

namespace Gemstone.Identity
{
    /// <summary>
    /// Represents information about a local user or a domain user (e.g., from Active Directory).
    /// </summary>
    public sealed class UserInfo
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="loginID">
        /// Login ID in 'domain\username' format of the user's account whose information is to be retrieved. Login ID 
        /// can also be specified in 'username' format without the domain name, in which case the domain name will be
        /// approximated based on the privileged user domain if specified, default login domain of the host machine 
        /// if available, or the domain of the identity that owns the host process.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="loginID"/> is a null or empty string.</exception>
        public UserInfo(string loginID)        
        {
            if (string.IsNullOrEmpty(loginID))
                throw new ArgumentNullException(nameof(loginID));

            string[] accountParts = loginID.Split('\\');

            if (accountParts.Length != 2)
            {
                accountParts = loginID.Split('@');

                if (accountParts.Length != 2)
                {
                    // Login ID is specified in 'username' format.
                    UserName = loginID;
                }
                else
                {
                    // Login ID is specified in 'username@domain' format.
                    UserName = accountParts[0];
                    Domain = accountParts[1];
                }
            }
            else
            {
                // Login ID is specified in 'domain\username' format.
                Domain = accountParts[0];
                UserName = accountParts[1];
            }

            if (string.IsNullOrEmpty(Domain))
                Domain = Environment.MachineName ?? "";
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the domain for the user.
        /// </summary>
        public string Domain { get; } = default!;

        /// <summary>
        /// Gets the user name of the user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the Login ID of the user.
        /// </summary>
        public string LoginID => $"{Domain}\\{UserName}";

        /// <summary>
        /// Gets the ID of the user in LDAP format.
        /// </summary>
        public string LdapID => $"{UserName}@{Domain}";

        #endregion

        #region [ Static ]

        // Static Fields
        private static string s_lastUserID = default!;
        private static UserInfo s_currentUserInfo = default!;


        // Static Properties

        /// <summary>
        /// Gets the ID name of the current user.
        /// </summary>
        /// <remarks>
        /// The ID name returned is that of the user account under which the code is executing.
        /// </remarks>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static string? CurrentUserID
        {
            get
            {
                try
                {
                    return WindowsIdentity.GetCurrent().Name;
                }
                catch (SecurityException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="UserInfo"/> object for the <see cref="CurrentUserID"/>.
        /// </summary>
        public static UserInfo? CurrentUserInfo
        {
            get
            {
                string currentUserID = CurrentUserID!;

                if (!string.IsNullOrEmpty(currentUserID))
                {
                    if (s_currentUserInfo == null || string.IsNullOrEmpty(s_lastUserID) || !currentUserID.Equals(s_lastUserID, StringComparison.OrdinalIgnoreCase))
                        s_currentUserInfo = new UserInfo(currentUserID);
                }

                s_lastUserID = currentUserID;

                return s_currentUserInfo;
            }
        }

        #endregion
    }
}
