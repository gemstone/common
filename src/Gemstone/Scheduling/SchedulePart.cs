//******************************************************************************************************
//  SchedulePart.cs - Gbtc
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
//  08/01/2006 - Pinal C. Patel
//       Generated original version of source code.
//  09/15/2008 - J. Ritchie Carroll
//       Converted to C#.
//  11/03/2008 - Pinal C. Patel
//       Edited code comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Gemstone.Scheduling;

#region [ Enumerations ]

/// <summary>
/// Indicates the date/time element that a <see cref="SchedulePart"/> represents.
/// </summary>
/// <remarks>This enumeration specifically corresponds to the UNIX crontab date/time elements.</remarks>
public enum DateTimePart
{
    /// <summary>
    /// <see cref="SchedulePart"/> represents minutes. Legal values are 0 through 59, or 60 which maps to 0.
    /// </summary>
    Minute,
    /// <summary>
    /// <see cref="SchedulePart"/> represents hours. Legal values are 0 through 23, or 24 which maps to 0.
    /// </summary>
    Hour,
    /// <summary>
    /// <see cref="SchedulePart"/> represents day of month. Legal values are 1 through 31.
    /// </summary>
    Day,
    /// <summary>
    /// <see cref="SchedulePart"/> represents months. Legal values are 1 through 12.
    /// </summary>
    Month,
    /// <summary>
    /// <see cref="SchedulePart"/> represents day of week. Legal values are 0 through 6, or 7 which maps to 0. Sunday is 0 and Saturday is 6.
    /// </summary>
    DayOfWeek
}

/// <summary>
/// Indicates the syntax used in a <see cref="SchedulePart"/> for specifying its values.
/// </summary>
public enum SchedulePartTextSyntax
{
    /// <summary>
    /// Values for the <see cref="SchedulePart"/> were specified using the '*' text syntax. Included values are 
    /// all legal values for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> represents.
    /// </summary>
    Any,
    /// <summary>
    /// Values for the <see cref="SchedulePart"/> were specified using the '*/n' text syntax. Included values are 
    /// legal values for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> represents that are 
    /// divisible by 'n'. 
    /// </summary>
    EveryN,
    /// <summary>
    /// Values for the <see cref="SchedulePart"/> were specified using the 'n1-nn' text syntax. Included values 
    /// are legal values for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> represents that
    /// are within the specified range.
    /// </summary>
    Range,
    /// <summary>
    /// Values for the <see cref="SchedulePart"/> were specified using the 'n1,n2,nn' text syntax. Included values 
    /// are specific legal values for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> represents.
    /// </summary>
    Specific,
    /// <summary>
    /// Values for the <see cref="SchedulePart"/> were specified using the 'n1-nn/n' text syntax. Included values 
    /// are legal values for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> represents that
    /// are within the specified range and that are divisible by 'n'. 
    /// </summary>
    RangeWithEveryN
}

#endregion

/// <summary>
/// Represents a part of the <see cref="Schedule"/>.
/// </summary>
/// <seealso cref="Schedule"/>
public class SchedulePart
{
    #region [ Members ]

    // Fields
    private readonly List<int> m_values;

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulePart"/> class.
    /// </summary>
    /// <param name="valueText">The text that specifies the values for the <see cref="SchedulePart"/> object.</param>
    /// <param name="dateTimePart">The <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> object represents.</param>
    public SchedulePart(string valueText, DateTimePart dateTimePart)
    {
        m_values = new List<int>();

        if (ValidateAndPopulate(valueText, dateTimePart))
        {
            // The text provided for populating the values is valid according to the specified date-time part.
            ValueText = valueText;
            DateTimePart = dateTimePart;
        }
        else
        {
            throw new ArgumentException($"Text is not valid for {dateTimePart} schedule part");
        }
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets the text used to specify the values for the <see cref="SchedulePart"/> object.
    /// </summary>
    public string ValueText { get; }

    /// <summary>
    /// Gets the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> object represents.
    /// </summary>
    public DateTimePart DateTimePart { get; }

    /// <summary>
    /// Gets the <see cref="SchedulePartTextSyntax"/> used in the <see cref="ValueText"/> for specifying the 
    /// values of the <see cref="SchedulePart"/> object.
    /// </summary>
    public SchedulePartTextSyntax ValueTextSyntax { get; private set; }

    /// <summary>
    /// Gets a meaningful description of the <see cref="SchedulePart"/> object.
    /// </summary>
    public string Description
    {
        get
        {
            string[] range;

            switch (ValueTextSyntax)
            {
                case SchedulePartTextSyntax.Any:
                    return $"Any {DateTimePart}";
                case SchedulePartTextSyntax.EveryN:
                    return $"Every {ValueText.Split('/')[1]} {DateTimePart}(s)";
                case SchedulePartTextSyntax.Range:
                    range = ValueText.Split('-');
                    return $"{DateTimePart} {range[0]} to {range[1]}";
                case SchedulePartTextSyntax.Specific:
                    return $"{DateTimePart} {ValueText}";
                case SchedulePartTextSyntax.RangeWithEveryN:
                    range = ValueText.Split('-');
                    string[] interval = range[1].Split('/');
                    return $"{range[0]} to {interval[0]} every {interval[1]} {DateTimePart}(s)";
                default:
                    return string.Empty;
            }
        }
    }

    /// <summary>
    /// Gets the list of values for the <see cref="SchedulePart"/> object specified using <see cref="ValueText"/>.
    /// </summary>
    public ReadOnlyCollection<int> Values => m_values.AsReadOnly();

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Determines if the <see cref="Values"/> for the <see cref="DateTimePart"/> that the <see cref="SchedulePart"/> 
    /// object represents matches the specified <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> against which the <see cref="Values"/> are to be matches.</param>
    /// <returns><c>true</c> if one of the <see cref="Values"/> matches the <paramref name="dateTime"/>; otherwise, <c>false</c>.</returns>
    public bool Matches(DateTime dateTime)
    {
        return DateTimePart switch
        {
            DateTimePart.Minute => m_values.Contains(dateTime.Minute),
            DateTimePart.Hour => m_values.Contains(dateTime.Hour),
            DateTimePart.Day => m_values.Contains(dateTime.Day),
            DateTimePart.Month => m_values.Contains(dateTime.Month),
            DateTimePart.DayOfWeek => m_values.Contains((int)dateTime.DayOfWeek),
            _ => false
        };
    }

    private bool ValidateAndPopulate(string schedulePart, DateTimePart dateTimePart)
    {
        int minValue = 0;
        int maxValue = 0;

        switch (dateTimePart)
        {
            case DateTimePart.Minute:
                maxValue = 59;
                break;
            case DateTimePart.Hour:
                maxValue = 23;
                break;
            case DateTimePart.Day:
                minValue = 1;
                maxValue = 31;
                break;
            case DateTimePart.Month:
                minValue = 1;
                maxValue = 12;
                break;
            case DateTimePart.DayOfWeek:
                maxValue = 6;
                break;
        }

        // Match literal asterisk
        if (Regex.Match(schedulePart, "^(\\*){1}$").Success)
        {
            // ^(\*){1}$             Matches: *
            ValueTextSyntax = SchedulePartTextSyntax.Any;
            PopulateValues(minValue, maxValue, 1);

            return true;
        }

        if (Regex.Match(schedulePart, "^(\\d+\\-\\d+/\\d+){1}$").Success)
        {
            // ^(\d+\-\d+/\d+){1}$   Matches: [any digit]-[any digit]/[any digit]
            string[] range = schedulePart.Split('-');
            string[] parts = range[1].Split('/');
            int lowRange = Convert.ToInt32(range[0]);
            int highRange = Convert.ToInt32(parts[0]) % maxValue;
            int interval = Convert.ToInt32(parts[1]);

            if (lowRange < highRange && lowRange >= minValue && highRange <= maxValue && interval > 0 && interval <= maxValue)
            {
                ValueTextSyntax = SchedulePartTextSyntax.RangeWithEveryN;
                PopulateValues(lowRange, highRange, interval);
                return true;
            }
        }
        else if (Regex.Match(schedulePart, "^(\\*/\\d+){1}$").Success)
        {
            // ^(\*/\d+){1}$         Matches: */[any digit]
            int interval = Convert.ToInt32(schedulePart.Split('/')[1]) % maxValue;

            if (interval > 0 && interval <= maxValue)
            {
                ValueTextSyntax = SchedulePartTextSyntax.EveryN;
                PopulateValues(minValue, maxValue, interval);
                return true;
            }
        }
        else if (Regex.Match(schedulePart, "^(\\d+\\-\\d+){1}$").Success)
        {
            // ^(\d+\-\d+){1}$       Matches: [any digit]-[any digit]
            string[] range = schedulePart.Split('-');
            int lowRange = Convert.ToInt32(range[0]);
            int highRange = Convert.ToInt32(range[1]) % maxValue;

            if (lowRange < highRange && lowRange >= minValue && highRange <= maxValue)
            {
                ValueTextSyntax = SchedulePartTextSyntax.Range;
                PopulateValues(lowRange, highRange, 1);
                return true;
            }
        }
        else if (Regex.Match(schedulePart, "^((\\d+,?)+){1}$").Success)
        {
            // ^((\d+,?)+){1}$       Matches: [any digit] AND [any digit], ..., [any digit]
            ValueTextSyntax = SchedulePartTextSyntax.Specific;

            foreach (string part in schedulePart.Split(','))
            {
                if (int.TryParse(part, out int value))
                {
                    value %= maxValue;

                    if (!(value >= minValue && value <= maxValue))
                        return false;

                    if (!m_values.Contains(value))
                        m_values.Add(value);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    private void PopulateValues(int fromValue, int toValue, int stepValue)
    {
        for (int i = fromValue; i <= toValue; i += stepValue)
            m_values.Add(i);
    }

    #endregion
}