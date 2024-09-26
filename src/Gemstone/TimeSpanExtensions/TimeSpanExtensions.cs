//******************************************************************************************************
//  TimeSpanExtensions.cs - Gbtc
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
//  -----------------------------------------------------------------------------------------------------
//  12/14/2008 - F. Russell Robertson
//       Generated original version of source code.
//  05/30/2008 - J. Ritchie Carroll
//       Updated to use existing elapsed time string function of GSF.Units.Time.
//
//*******************************************************************************************************

using System;
using Gemstone.Units;

namespace Gemstone.TimeSpanExtensions;

/// <summary>
/// Extends the TimeSpan Class
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Converts the <see cref="TimeSpan"/> value into a textual representation of years, days, hours,
    /// minutes and seconds with the specified number of fractional digits given string array of
    /// time names.
    /// </summary>
    /// <param name="value">The <see cref="TimeSpan"/> to process.</param>
    /// <param name="secondPrecision">Number of fractional digits to display for seconds.</param>
    /// <param name="timeNames">Time names array to use during textual conversion.</param>
    /// <param name="minimumSubSecondResolution">
    ///     Minimum sub-second resolution to display. Defaults to <see cref="SI.Milli"/>.
    /// </param>
    /// <remarks>
    /// <para>
    /// Set <paramref name="secondPrecision"/> to -1 to suppress seconds display, this will
    /// force minimum resolution of time display to minutes.
    /// </para>
    /// <para>
    /// <paramref name="timeNames"/> array needs one string entry for each of the following names:<br/>
    /// " year", " years", " day", " days", " hour", " hours", " minute", " minutes", " second", " seconds", "less than ".
    /// </para>
    /// <example>
    /// <code>
    ///   DateTime g_start = DateTime.UtcNow;
    ///   DateTime EndTime = DateTime.UtcNow;
    ///   TimeSpan duration = EndTime.Subtract(g_start);
    ///   Console.WriteLine("Elapsed Time = " + duration.ToElapsedTimeString());
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>
    /// The string representation of the value of this instance, consisting of the number of
    /// years, days, hours, minutes and seconds represented by this value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="minimumSubSecondResolution"/> is not less than or equal to <see cref="SI.Milli"/> or
    /// <paramref name="minimumSubSecondResolution"/> is not defined in <see cref="SI.Factors"/> array.
    /// </exception>
    public static string ToElapsedTimeString(this TimeSpan value, int secondPrecision = 2, string[]? timeNames = null, double minimumSubSecondResolution = SI.Milli)
    {
        return Time.ToElapsedTimeString(value.TotalSeconds, secondPrecision, timeNames, minimumSubSecondResolution);
    }
}
