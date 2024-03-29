﻿//******************************************************************************************************
//  CommandResponse.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2014 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************


namespace Gemstone.Console;

/// <summary>
/// Represents a response that is returned from <see cref="Command.Execute(string, string, int)"/> with standard output and exit code.
/// </summary>
public class CommandResponse
{
    #region [ Constructors ]

    /// <summary>
    /// Creates a new <see cref="CommandResponse"/>.
    /// </summary>
    /// <param name="standardOutput">Standard output of command process.</param>
    /// <param name="exitCode">Exit code of command process.</param>
    public CommandResponse(string standardOutput, int exitCode)
    {
        StandardOutput = standardOutput;
        ExitCode = exitCode;
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets standard output reported by command process.
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Gets exit code from command process.
    /// </summary>
    public int ExitCode { get; }

    #endregion
}