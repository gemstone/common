//******************************************************************************************************
//  ProcessProgress.cs - Gbtc
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
//  05/23/2007 - Pinal C. Patel
//       Generated original version of source code.
//  09/09/2008 - J. Ritchie Carroll
//       Converted to C#.
//  09/26/2008 - J. Ritchie Carroll
//       Added a ProcessProgress.Handler class to allow functions with progress delegate
//       to update progress information using the ProcessProgress class.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;

namespace Gemstone;

/// <summary>
/// Represents current process progress for an operation.
/// </summary>
/// <remarks>
/// Used to track total progress of an identified operation.
/// </remarks>
/// <typeparam name="TUnit">Unit of progress used (long, double, int, etc.)</typeparam>
/// <remarks>
/// Constructs a new instance of the <see cref="ProcessProgress{TUnit}"/> class using specified process name.
/// </remarks>
/// <param name="processName">Name of process for which progress is being monitored.</param>
[Serializable]
public class ProcessProgress<TUnit>(string processName) where TUnit : struct
{
    #region [ Properties ]

    /// <summary>
    /// Gets or sets name of process for which progress is being monitored.
    /// </summary>
    public string ProcessName { get; set; } = processName;

    /// <summary>
    /// Gets or sets current progress message (e.g., current file being copied, etc.)
    /// </summary>
    public string ProgressMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets total number of units to be processed.
    /// </summary>
    public TUnit Total { get; set; }

    /// <summary>
    /// Gets or sets number of units completed processing so far.
    /// </summary>
    public TUnit Complete { get; set; }

    #endregion
}
