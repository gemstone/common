﻿//******************************************************************************************************
//  ISupportLifecycle.cs - Gbtc
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
//  10/09/2008 - Pinal C. Patel
//       Generated original version of source code.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  11/23/2011 - J. Ritchie Carroll
//       Added required Disposed event for interface to allow deep automated life cycle management.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;

namespace Gemstone;

/// <summary>
/// Specifies that this object provides support for performing tasks during the key stages of object life-cycle.
/// </summary>
/// <remarks>
/// <list type="table">
///     <listheader>
///         <term>Life-cycle Stage</term>
///         <description>Equivalent Member</description>
///     </listheader>
///     <item>
///         <term>Birth</term>
///         <description><see cref="Initialize()"/></description>
///     </item>
///     <item>
///         <term>Life (Work/Sleep)</term>
///         <description><see cref="Enabled"/></description>
///     </item>
///     <item>
///         <term>Death</term>
///         <description><see cref="IDisposable.Dispose()"/></description>
///     </item>
/// </list>
/// </remarks>
public interface ISupportLifecycle : IDisposable
{
    /// <summary>
    /// Raised after the source object has been properly disposed.
    /// </summary>
    event EventHandler? Disposed;

    /// <summary>
    /// Initializes the state of the object.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Typical implementation of <see cref="Initialize()"/> should allow the object state to be initialized only 
    /// once. <see cref="Initialize()"/> should be called automatically from one or more key entry points of the 
    /// object. For example, if the object is a <see cref="System.ComponentModel.Component"/> and it implements 
    /// the <see cref="System.ComponentModel.ISupportInitialize"/> interface then <see cref="Initialize()"/> should 
    /// be called from the <see cref="System.ComponentModel.ISupportInitialize.EndInit()"/> method so that the object 
    /// gets initialized automatically when consumed through the IDE designer surface. In addition to this 
    /// <see cref="Initialize()"/> should also be called from key or mandatory methods of the object, like 'Start()'
    /// or 'Connect()', so that the object gets initialized even when not consumed through the IDE designer surface.
    /// </para>
    /// </remarks>
    void Initialize();

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the object is enabled.
    /// </summary>
    /// <remarks>
    /// Typical implementation of <see cref="Enabled"/> should suspend the internal processing when the object is 
    /// disabled and resume processing when the object is enabled.
    /// </remarks>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets a flag that indicates whether the object has been disposed.
    /// </summary>
    bool IsDisposed { get; }
}