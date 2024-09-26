//******************************************************************************************************
//  ApproximatelyEquals.cs - Gbtc
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

using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Please add comments to these classes
#pragma warning disable 1591

namespace Gemstone.StringExtensions.FuzzyStrings;

public static partial class ComparisonMetrics
{
    public static bool ApproximatelyEquals(this string source, string target, FuzzyStringComparisonOptions options, FuzzyStringComparisonTolerance tolerance)
    {
        List<double> comparisonResults = [];

        if (!options.HasFlag(FuzzyStringComparisonOptions.CaseSensitive))
        {
            source = source.Capitalize();
            target = target.Capitalize();
        }

        // Min: 0    Max: source.Length = target.Length
        if (options.HasFlag(FuzzyStringComparisonOptions.UseHammingDistance))
        {
            if (source.Length == target.Length)
            {
                comparisonResults.Add(source.HammingDistance(target) / (double)target.Length);
            }
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseJaccardDistance))
        {
            comparisonResults.Add(source.JaccardDistance(target));
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseJaroDistance))
        {
            comparisonResults.Add(source.JaroDistance(target));
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseJaroWinklerDistance))
        {
            comparisonResults.Add(source.JaroWinklerDistance(target));
        }

        // Min: 0    Max: LevenshteinDistanceUpperBounds - LevenshteinDistanceLowerBounds
        // Min: LevenshteinDistanceLowerBounds    Max: LevenshteinDistanceUpperBounds
        if (options.HasFlag(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance))
        {
            comparisonResults.Add(Convert.ToDouble(source.NormalizedLevenshteinDistance(target)) / Convert.ToDouble(Math.Max(source.Length, target.Length) - source.LevenshteinDistanceLowerBounds(target)));
        }
        else if (options.HasFlag(FuzzyStringComparisonOptions.UseLevenshteinDistance))
        {
            comparisonResults.Add(Convert.ToDouble(source.LevenshteinDistance(target)) / Convert.ToDouble(source.LevenshteinDistanceUpperBounds(target)));
        }

        if (options.HasFlag(FuzzyStringComparisonOptions.UseLongestCommonSubsequence))
        {
            comparisonResults.Add(1 - Convert.ToDouble(source.LongestCommonSubsequence(target).Length / Convert.ToDouble(Math.Min(source.Length, target.Length))));
        }

        if (options.HasFlag(FuzzyStringComparisonOptions.UseLongestCommonSubstring))
        {
            comparisonResults.Add(1 - Convert.ToDouble(source.LongestCommonSubstring(target).Length / Convert.ToDouble(Math.Min(source.Length, target.Length))));
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseSorensenDiceDistance))
        {
            comparisonResults.Add(source.SorensenDiceDistance(target));
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseOverlapCoefficient))
        {
            comparisonResults.Add(1 - source.OverlapCoefficient(target));
        }

        // Min: 0    Max: 1
        if (options.HasFlag(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity))
        {
            comparisonResults.Add(1 - source.RatcliffObershelpSimilarity(target));
        }

        if (tolerance == FuzzyStringComparisonTolerance.Strong)
        {
            return comparisonResults.Average() < 0.25;
        }

        if (tolerance == FuzzyStringComparisonTolerance.Normal)
        {
            return comparisonResults.Average() < 0.5;
        }
        
        if (tolerance == FuzzyStringComparisonTolerance.Weak)
        {
            return comparisonResults.Average() < 0.75;
        }
        
        if (tolerance == FuzzyStringComparisonTolerance.Manual)
        {
            return comparisonResults.Average() > 0.6;
        }

        return false;
    }
}
