//******************************************************************************************************
//  ParameterAttribute.cs - Gbtc
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
//  07/10/2025 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone.ComponentModel.DataAnnotations;

/// <summary>
/// Represents metadata for a method parameter, including its name, a UI label, and an optional description.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ParameterAttribute : Attribute
{
    /// <summary>
    /// Name of the method parameter this metadata applies to.
    /// </summary>
    public string ParamName { get; }

    /// <summary>
    /// Label to show in the UI.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; }

    public ParameterAttribute(string paramName, string label)
    {
        ParamName = paramName;
        Label = label;
    }

    public ParameterAttribute(string paramName, string label, string description) : this(paramName, label)
    {
        Description = description;
    }
}
