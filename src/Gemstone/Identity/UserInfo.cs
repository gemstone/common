//******************************************************************************************************
//  UserInfo.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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

// Ignore Spelling: Ldap

using System;
using System.Security;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Gemstone.Identity;

/// <summary>
/// Represents information about a local user or a domain user (e.g., from Active Directory).
/// </summary>
public sealed class UserInfo // TODO: Implement full interface : IUserInfo 
{
    #region [ Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfo"/> class.
    /// </summary>
    /// <param name="loginID">
    /// Login ID in 'domain\accountName' format of the user's account whose information is to be retrieved. Login ID 
    /// can also be specified in 'accountName' format without the domain name, in which case the domain name will be
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
                // Login ID is specified in 'accountName' format.
                accountName = loginID;
            }
            else
            {
                // Login ID is specified in 'accountName@domain' format.
                accountName = accountParts[0];
                Domain = accountParts[1];
            }
        }
        else
        {
            // Login ID is specified in 'domain\accountName' format.
            Domain = accountParts[0];
            accountName = accountParts[1];
        }

        if (string.IsNullOrEmpty(Domain))
            Domain = Environment.MachineName;
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
    public string accountName { get; }

    /// <summary>
    /// Gets the Login ID of the user.
    /// </summary>
    public string LoginID
    {
        get
        {
            return $"{Domain}\\{accountName}";
        }
    }

    /// <summary>
    /// Gets the ID of the user in LDAP format.
    /// </summary>
    public string LdapID
    {
        get
        {
            return $"{accountName}@{Domain}";
        }
    }

    #endregion

    #region [ Static ]

    // Static Fields
    private static string? s_lastUserID ;
    private static UserInfo? s_currentUserInfo;

    /// <summary>
    /// Localized version of Windows "BUILTIN" local permissions group name.
    /// </summary>
    /// <remarks>
    /// For non-English based OS languages, this name may be different. For example, on a German OS this is "VORDEFINIERT".
    /// </remarks>
    public static readonly string BuiltInGroupName = UserInfoWindows.BuiltInGroupName;

    /// <summary>
    /// Localized version of Windows "NT AUTHORITY" local permissions group name.
    /// </summary>
    /// <remarks>
    /// For non-English based OS languages, this name may be different. For example, on a French OS this is "NT-AUTORITÄT".
    /// </remarks>
    public static readonly string NTAuthorityGroupName = UserInfoWindows.NTAuthorityGroupName;

    /// <summary>
    /// Localized version of Windows "NT SERVICE" local Windows services group name.
    /// </summary>
    /// <remarks>
    /// For non-English based OS languages, this name may be different.
    /// </remarks>
    public static readonly string NTServiceGroupName = UserInfoWindows.NTServiceGroupName;

    // Static Properties

    /// <summary>
    /// Gets the ID name of the current user.
    /// </summary>
    /// <remarks>
    /// The ID name returned is that of the user account under which the code is executing.
    /// </remarks>
    public static string CurrentUserID
    {
        get
        {
            if (Common.IsPosixEnvironment)
                return Environment.UserName;

            try
            {
#pragma warning disable CA1416
                return WindowsIdentity.GetCurrent().Name;
#pragma warning restore CA1416
            }
            catch (SecurityException)
            {
                return Environment.UserName;
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
            string currentUserID = CurrentUserID;

            if (!string.IsNullOrEmpty(currentUserID))
            {
                if (string.IsNullOrEmpty(s_lastUserID) || !currentUserID.Equals(s_lastUserID, StringComparison.OrdinalIgnoreCase))
                    s_currentUserInfo = new UserInfo(currentUserID);
            }

            s_lastUserID = currentUserID;

            return s_currentUserInfo;
        }
    }

    // TODO: Implement these methods

    /// <summary>
    /// Converts the given user name to the SID corresponding to that name.
    /// </summary>
    /// <param name="accountName">The user name for which to look up the SID.</param>
    /// <returns>The SID for the given user name, or the user name if no SID can be found.</returns>
    /// <remarks>
    /// If the <paramref name="accountName"/> cannot be converted to a SID, <paramref name="accountName"/>
    /// will be the return value.
    /// </remarks>
    public static string UserNameToSID(string accountName)
    {
        return Common.IsPosixEnvironment ?
            UserInfoUnix.UserNameToSID(accountName) :
            UserInfoWindows.AccountNameToSID(accountName);
    }

    /// <summary>
    /// Determines whether the given security identifier identifies a user account.
    /// </summary>
    /// <param name="sid">The security identifier.</param>
    /// <returns><c>true</c> if the security identifier identifies a user account; <c>false</c> otherwise.</returns>
    public static bool IsUserSID(string sid)
    {
        return Common.IsPosixEnvironment ?
            UserInfoUnix.IsUserSID(sid) :
            UserInfoWindows.IsUserSID(sid);
    }

    /// <summary>
    /// Converts the given group name to the SID corresponding to that name.
    /// </summary>
    /// <param name="groupName">The group name for which to look up the SID.</param>
    /// <returns>The SID for the given group name, or the group name if no SID can be found.</returns>
    /// <remarks>
    /// If the <paramref name="groupName"/> cannot be converted to a SID, <paramref name="groupName"/>
    /// will be the return value.
    /// </remarks>
    public static string GroupNameToSID(string groupName)
    {
        return Common.IsPosixEnvironment ?
            UserInfoUnix.GroupNameToSID(groupName) :
            UserInfoWindows.AccountNameToSID(groupName);
    }

    /// <summary>
    /// Determines whether the given security identifier identifies a group.
    /// </summary>
    /// <param name="sid">The security identifier.</param>
    /// <returns><c>true</c> if the security identifier identifies a group; <c>false</c> otherwise.</returns>
    public static bool IsGroupSID(string sid)
    {
        return Common.IsPosixEnvironment ?
            UserInfoUnix.IsGroupSID(sid) :
            UserInfoWindows.IsGroupSID(sid);
    }

    /// <summary>
    /// Determines if specified <paramref name="domain"/> is the local domain (i.e., local machine).
    /// </summary>
    /// <param name="domain">Domain name to check.</param>
    /// <returns><c>true</c>if specified <paramref name="domain"/> is the local domain (i.e., local machine); otherwise, <c>false</c>.</returns>
    public static bool IsLocalDomain(string domain)
    {
        if (Common.IsPosixEnvironment)
            return
                string.IsNullOrEmpty(domain) ||
                domain.Equals(".", StringComparison.Ordinal) ||
                domain.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase);

        return
            string.IsNullOrEmpty(domain) ||
            domain.Equals(".", StringComparison.Ordinal) ||
            domain.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase) ||
            domain.Equals(NTServiceGroupName, StringComparison.OrdinalIgnoreCase) ||
            domain.Equals(NTAuthorityGroupName, StringComparison.OrdinalIgnoreCase) ||
            domain.Equals("IIS APPPOOL", StringComparison.OrdinalIgnoreCase);
    }

    // DirectoryEntry will only resolve "BUILTIN\" groups with a dot ".\"
    internal static string ValidateGroupName(string groupName)
    {
        if (!Common.IsPosixEnvironment && groupName.StartsWith($@"{BuiltInGroupName}\", StringComparison.OrdinalIgnoreCase))
            return Regex.Replace(groupName, $@"^{BuiltInGroupName}\\", @".\", RegexOptions.IgnoreCase);

        return groupName;
    }

    #endregion
}
