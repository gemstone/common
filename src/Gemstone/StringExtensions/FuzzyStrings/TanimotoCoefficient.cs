﻿//******************************************************************************************************
//  TanimotoCoefficient.cs - Gbtc
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
//  04/14/2013 - Kevin D. Jones
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Linq;

// TODO: Please add comments to these classes
#pragma warning disable 1591

namespace Gemstone.StringExtensions.FuzzyStrings;

public static partial class ComparisonMetrics
{
    public static double TanimotoCoefficient(this string source, string target)
    {
        double Na = source.Length;
        double Nb = target.Length;
        double Nc = source.Intersect(target).Count();

        return Nc / (Na + Nb - Nc);
    }
}
