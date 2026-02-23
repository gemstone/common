//******************************************************************************************************
//  ToggleVisibilityAttribute.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  02/18/2026 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone.ComponentModel.DataAnnotations;

/// <summary>
/// Specifies that a property should render a toggle switch in the UI that allows the user to disable and/or reset the properties value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ToggleVisibilityAttribute(string label, bool triggerValue = false) : Attribute
{
    /// <summary>
    /// Gets the label to display on the toggle switch.
    /// </summary>
    public string Label { get; } = label;

    /// <summary>
    /// Gets the boolean value of the toggle that triggers the hide/reset behavior.
    /// </summary>
    public bool TriggerValue { get; } = triggerValue;
}
