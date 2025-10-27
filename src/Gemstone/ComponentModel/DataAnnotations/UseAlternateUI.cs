//******************************************************************************************************
//  UseAlternateUIAttribute.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/24/2025 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone.ComponentModel.DataAnnotations;

/// <summary>
/// Defines whether to use an alternate UI for the associated property or field.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class UseAlternateUIAttribute(bool useAlternateUI) : Attribute
{
    /// <summary>
    /// Flag that determines whether to use the alternate UI for the associated property or field.
    /// </summary>
    public bool UseAlternateUI { get; init; } = useAlternateUI;
}
