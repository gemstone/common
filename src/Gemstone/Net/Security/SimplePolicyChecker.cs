//******************************************************************************************************
//  SimplePolicyChecker.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
//  05/09/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gemstone.Net.Security;

/// <summary>
/// Simple implementation of <see cref="ICertificateChecker"/>.
/// </summary>
public class SimplePolicyChecker : ICertificateChecker
{
    #region [ Constructors ]

    /// <summary>
    /// Creates a new instance of the <see cref="SimplePolicyChecker"/> class.
    /// </summary>
    public SimplePolicyChecker()
    {
        ValidPolicyErrors = SslPolicyErrors.None;
        ValidChainFlags = X509ChainStatusFlags.NoError;
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets or sets the set of invalid policy errors.
    /// </summary>
    public SslPolicyErrors ValidPolicyErrors { get; set; }

    /// <summary>
    /// Gets or sets the set of invalid chain flags.
    /// </summary>
    public X509ChainStatusFlags ValidChainFlags { get; set; }

    /// <summary>
    /// Gets the reason why the remote certificate validation
    /// failed, or null if certificate validation did not fail.
    /// </summary>
    public string? ReasonForFailure { get; private set; }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Verifies the remote certificate used for authentication.
    /// </summary>
    /// <param name="sender">An object that contains state information for this validation.</param>
    /// <param name="remoteCertificate">The certificate used to authenticate the remote party.</param>
    /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
    /// <param name="errors">One or more errors associated with the remote certificate.</param>
    /// <returns>A flag that determines whether the specified certificate is accepted for authentication.</returns>
    public bool ValidateRemoteCertificate(object? sender, X509Certificate remoteCertificate, X509Chain chain, SslPolicyErrors errors)
    {
        ReasonForFailure = null;

        if ((errors & ~ValidPolicyErrors) != SslPolicyErrors.None)
        {
            ReasonForFailure = $"Policy errors encountered during validation: {errors & ~ValidPolicyErrors}";
            return false;
        }

        X509ChainStatusFlags chainFlags = chain.ChainStatus.Aggregate(X509ChainStatusFlags.NoError, (flags, status) => flags | (status.Status & ~ValidChainFlags));

        if (chainFlags == X509ChainStatusFlags.NoError)
            return true;

        ReasonForFailure = $"Invalid chain flags found during validation: {chainFlags}";
        return false;
    }

    #endregion
}