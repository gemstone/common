//******************************************************************************************************
//  StackDisposal.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/24/2016 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone.Diagnostics;

/// <summary>
/// A class that will undo a temporary change in the stack variables. Note, this class 
/// will be reused, therefore setting some kind of disposed flag will cause make this 
/// class unusable. The side effect of multiple calls to Dispose is tolerable.
/// </summary>
internal class StackDisposal : IDisposable
{
    private readonly int m_depth;
    private readonly Action<int> m_callback;

    internal StackDisposal(int depth, Action<int> callback)
    {
        m_depth = depth;
        m_callback = callback;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        m_callback(m_depth);
    }
}
