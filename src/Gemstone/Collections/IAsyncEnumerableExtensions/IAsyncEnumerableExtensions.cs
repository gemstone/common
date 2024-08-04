//******************************************************************************************************
//  IAsyncEnumerableExtensions.cs - Gbtc
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
//  08/04/2024 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Gemstone.Collections.IAsyncEnumerableExtensions;

/// <summary>
/// Defines extension functions related to manipulation of <see cref="IAsyncEnumerable{T}"/> implementations.
/// </summary>
public static class IAsyncEnumerableExtensions
{
    /// <summary>
    /// Sets the <see cref="CancellationToken"/> to be passed to <see cref="IAsyncEnumerable{T}.GetAsyncEnumerator(CancellationToken)"/> when
    /// iterating and configures how awaits on the tasks returned from an async iteration will be performed, defaults to not capturing.
    /// </summary>
    /// <typeparam name="T">The type of the objects being iterated.</typeparam>
    /// <param name="source">The source enumerable being iterated.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <param name="continueOnCapturedContext">Whether to capture and marshal back to the current context.</param>
    /// <returns>The configured enumerable.</returns>
    /// <remarks>
    /// The <see cref="TaskAsyncEnumerableExtensions.WithCancellation{T}"/> extension method sets <param name="continueOnCapturedContext"/> to
    /// <c>true</c>, this extension method allows the <param name="continueOnCapturedContext"/> to be set <c>false</c>, the default value for
    /// this extension method.
    /// </remarks>
    public static ConfiguredCancelableAsyncEnumerable<T> WithAwaitConfiguredCancellation<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken, bool continueOnCapturedContext = false)
    {
        return source.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext);
    }
}
