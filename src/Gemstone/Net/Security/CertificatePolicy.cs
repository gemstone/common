﻿//******************************************************************************************************
//  CertificatePolicy.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/07/2012 - Stephen C. Wills
//       Generated original version of source code.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gemstone.Net.Security;

/// <summary>
/// Represents a set of flags to be checked when validating remote certificates.
/// </summary>
public class CertificatePolicy
{
    #region [ Constructors ]

    /// <summary>
    /// Creates a new instance of the <see cref="CertificatePolicy"/> class.
    /// </summary>
    public CertificatePolicy()
    {
        ValidPolicyErrors = SslPolicyErrors.None;
        ValidChainFlags = X509ChainStatusFlags.NoError;
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets or sets the set of valid policy errors used when
    /// validating remote certificates using this policy.
    /// </summary>
    public SslPolicyErrors ValidPolicyErrors { get; set; }

    /// <summary>
    /// Gets or sets the set of valid chain flags used when
    /// validating remote certificates using this policy.
    /// </summary>
    public X509ChainStatusFlags ValidChainFlags { get; set; }

    #endregion
}