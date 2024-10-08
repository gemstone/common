﻿//******************************************************************************************************
//  Power.cs - Gbtc
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
//  01/25/2008 - J. Ritchie Carroll
//       Initial version of source generated.
//  09/11/2008 - J. Ritchie Carroll
//       Converted to C#.
//  08/10/2009 - Josh L. Patterson
//       Edited Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//  10/03/2017 - J. Ritchie Carroll
//       Added units enumeration with associated Convert method.
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll
   All rights reserved.
  
   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:
  
      * Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
       
      * Redistributions in binary form must reproduce the above
        copyright notice, this list of conditions and the following
        disclaimer in the documentation and/or other materials provided
        with the distribution.
  
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER "AS IS" AND ANY
   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
   PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
   OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  
\**************************************************************************/

#endregion

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Gemstone.Units;

#region [ Enumerations ]

/// <summary>
/// Represents the units available for a <see cref="Power"/> value.
/// </summary>
public enum PowerUnit
{
    /// <summary>
    /// Watt power units.
    /// </summary>
    Watts,
    /// <summary>
    /// Horsepower units.
    /// </summary>
    Horsepower,
    /// <summary>
    /// Metric horsepower units.
    /// </summary>
    MetricHorsepower,
    /// <summary>
    /// Boiler horsepower units.
    /// </summary>
    BoilerHorsepower,
    /// <summary>
    /// BTU per second power units.
    /// </summary>
    BTUPerSecond,
    /// <summary>
    /// Calories per second power units.
    /// </summary>
    CaloriesPerSecond,
    /// <summary>
    /// Liters atmosphere per second power units.
    /// </summary>
    LitersAtmospherePerSecond
}

#endregion

/// <summary>
/// Provides a type converter to convert <see cref="Power"/> values to and from various other representations.
/// </summary>
/// <remarks>
/// Since <see cref="Power"/> reports a type code of <see cref="TypeCode.Double"/>, the converter will convert
/// to and from <c>double</c> values as well as other types supported by <see cref="DoubleConverter"/>.
/// </remarks>
public class PowerConverter : DoubleConverter
{
    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(double) || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(double) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(double) && value is Power power)
            return (double)power;

        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is double angle)
            return new Power(angle);

        return base.ConvertFrom(context, culture, value);
    }
}

/// <summary>
/// Represents a power measurement, in watts, as a double-precision floating-point number.
/// </summary>
/// <remarks>
/// This class behaves just like a <see cref="double"/> representing a power in watts; it is implicitly
/// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
/// has the advantage of handling conversions to and from other power representations, specifically
/// horsepower, metric horsepower, boiler horsepower, BTU per second, calorie per second, and liter-atmosphere
/// per second. Metric conversions are handled simply by applying the needed <see cref="SI"/> conversion factor,
/// for example:
/// <example>
/// Convert power in watts to megawatts:
/// <code>
/// public double GetMegawatts(Power watts)
/// {
///     return watts / SI.Mega;
/// }
/// </code>
/// This example converts megawatts to mechanical horsepower:
/// <code>
/// public double GetHorsepower(double megawatts)
/// {
///     return (new Power(megawatts * SI.Mega)).ToHorsepower();
/// }
/// </code>
/// </example>
/// </remarks>
[Serializable]
[TypeConverter(typeof(PowerConverter))]
public struct Power : IComparable, IFormattable, IConvertible, IComparable<Power>, IComparable<double>, IEquatable<Power>, IEquatable<double>
{
    #region [ Members ]

    // Constants
    private const double HorsepowerFactor = 745.69987158227022D;

    private const double MetricHorsepowerFactor = 735.49875D;

    private const double BoilerHorsepowerFactor = 9.810657E+3D;

    private const double BTUPerSecondFactor = 1.05505585262E+3D;

    private const double CaloriesPerSecondFactor = 4.1868D;

    private const double LitersAtmospherePerSecondFactor = 101.325D;

    // Fields
    private readonly double m_value; // Power value stored in watts

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Creates a new <see cref="Power"/>.
    /// </summary>
    /// <param name="value">New power value in watts.</param>
    public Power(double value)
    {
        m_value = value;
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Gets the <see cref="Power"/> value in mechanical horsepower (Imperial).
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in mechanical horsepower.</returns>
    public double ToHorsepower()
    {
        return m_value / HorsepowerFactor;
    }

    /// <summary>
    /// Gets the <see cref="Power"/> value in metric horsepower.
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in metric horsepower.</returns>
    public double ToMetricHorsepower()
    {
        return m_value / MetricHorsepowerFactor;
    }

    /// <summary>
    /// Gets the <see cref="Power"/> value in boiler horsepower.
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in boiler horsepower.</returns>
    public double ToBoilerHorsepower()
    {
        return m_value / BoilerHorsepowerFactor;
    }

    /// <summary>
    /// Gets the <see cref="Power"/> value in BTU (International Table) per second.
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in BTU per second.</returns>
    public double ToBTUPerSecond()
    {
        return m_value / BTUPerSecondFactor;
    }

    /// <summary>
    /// Gets the <see cref="Power"/> value in calories (International Table) per second.
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in calories per second.</returns>
    public double ToCaloriesPerSecond()
    {
        return m_value / CaloriesPerSecondFactor;
    }

    /// <summary>
    /// Gets the <see cref="Power"/> value in liters-atmosphere per second.
    /// </summary>
    /// <returns>Value of <see cref="Power"/> in liters-atmosphere per second.</returns>
    public double ToLitersAtmospherePerSecond()
    {
        return m_value / LitersAtmospherePerSecondFactor;
    }

    /// <summary>
    /// Converts the <see cref="Power"/> to the specified <paramref name="targetUnit"/>.
    /// </summary>
    /// <param name="targetUnit">Target units.</param>
    /// <returns><see cref="Power"/> converted to <paramref name="targetUnit"/>.</returns>
    public double ConvertTo(PowerUnit targetUnit)
    {
        return targetUnit switch
        {
            PowerUnit.Watts => m_value,
            PowerUnit.Horsepower => ToHorsepower(),
            PowerUnit.MetricHorsepower => ToMetricHorsepower(),
            PowerUnit.BoilerHorsepower => ToBoilerHorsepower(),
            PowerUnit.BTUPerSecond => ToBTUPerSecond(),
            PowerUnit.CaloriesPerSecond => ToCaloriesPerSecond(),
            PowerUnit.LitersAtmospherePerSecond => ToLitersAtmospherePerSecond(),
            _ => throw new ArgumentOutOfRangeException(nameof(targetUnit), targetUnit, null)
        };
    }

    #region [ Numeric Interface Implementations ]

    /// <summary>
    /// Compares this instance to a specified object and returns an indication of their relative values.
    /// </summary>
    /// <param name="value">An object to compare, or null.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and value. Returns less than zero
    /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
    /// if this instance is greater than value.
    /// </returns>
    /// <exception cref="ArgumentException">value is not a <see cref="double"/> or <see cref="Power"/>.</exception>
    public int CompareTo(object? value)
    {
        if (value is null)
            return 1;

        double num;

        switch (value)
        {
            case double dbl:
                num = dbl;

                break;
            case Power power:
                num = power;

                break;
            default:
                try
                {
                    num = Convert.ToDouble(value);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Argument must be a Double or a Power", ex);
                }
                break;
        }

        return m_value < num ? -1 : m_value > num ? 1 : 0;
    }

    /// <summary>
    /// Compares this instance to a specified <see cref="Power"/> and returns an indication of their
    /// relative values.
    /// </summary>
    /// <param name="value">A <see cref="Power"/> to compare.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and value. Returns less than zero
    /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
    /// if this instance is greater than value.
    /// </returns>
    public int CompareTo(Power value)
    {
        return CompareTo((double)value);
    }

    /// <summary>
    /// Compares this instance to a specified <see cref="double"/> and returns an indication of their
    /// relative values.
    /// </summary>
    /// <param name="value">A <see cref="double"/> to compare.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and value. Returns less than zero
    /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
    /// if this instance is greater than value.
    /// </returns>
    public int CompareTo(double value)
    {
        return m_value < value ? -1 : m_value > value ? 1 : 0;
    }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An object to compare, or null.</param>
    /// <returns>
    /// True if obj is an instance of <see cref="double"/> or <see cref="Power"/> and equals the value of this instance;
    /// otherwise, False.
    /// </returns>
    public override bool Equals(object? obj)
    {
        double num;

        switch (obj)
        {
            case double dbl:
                num = dbl;

                break;
            case Power power:
                num = power;

                break;
            default:
                try
                {
                    num = Convert.ToDouble(obj);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Argument must be a Double or a Power", ex);
                }
                break;
        }

        return Equals(num);
    }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="Power"/> value.
    /// </summary>
    /// <param name="obj">A <see cref="Power"/> value to compare to this instance.</param>
    /// <returns>
    /// True if obj has the same value as this instance; otherwise, False.
    /// </returns>
    public bool Equals(Power obj)
    {
        return Equals((double)obj);
    }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="double"/> value.
    /// </summary>
    /// <param name="obj">A <see cref="double"/> value to compare to this instance.</param>
    /// <returns>
    /// True if obj has the same value as this instance; otherwise, False.
    /// </returns>
    public bool Equals(double obj)
    {
        return m_value == obj;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer hash code.
    /// </returns>
    public override int GetHashCode()
    {
        return m_value.GetHashCode();
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>
    /// The string representation of the value of this instance, consisting of a minus sign if
    /// the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeros.
    /// </returns>
    public override string ToString()
    {
        return m_value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation, using
    /// the specified format.
    /// </summary>
    /// <param name="format">A format string.</param>
    /// <returns>
    /// The string representation of the value of this instance as specified by format.
    /// </returns>
    public string ToString(string? format)
    {
        return m_value.ToString(format);
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation using the
    /// specified culture-specific format information.
    /// </summary>
    /// <param name="provider">
    /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The string representation of the value of this instance as specified by provider.
    /// </returns>
    public string ToString(IFormatProvider? provider)
    {
        return m_value.ToString(provider);
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation using the
    /// specified format and culture-specific format information.
    /// </summary>
    /// <param name="format">A format specification.</param>
    /// <param name="provider">
    /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The string representation of the value of this instance as specified by format and provider.
    /// </returns>
    public string ToString(string? format, IFormatProvider? provider)
    {
        return m_value.ToString(format, provider);
    }

    /// <summary>
    /// Converts the string representation of a number to its <see cref="Power"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <returns>
    /// A <see cref="Power"/> equivalent to the number contained in s.
    /// </returns>
    /// <exception cref="ArgumentNullException">s is null.</exception>
    /// <exception cref="OverflowException">
    /// s represents a number less than <see cref="Power.MinValue"/> or greater than <see cref="Power.MaxValue"/>.
    /// </exception>
    /// <exception cref="FormatException">s is not in the correct format.</exception>
    public static Power Parse(string s)
    {
        return double.Parse(s);
    }

    /// <summary>
    /// Converts the string representation of a number in a specified style to its <see cref="Power"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
    /// </param>
    /// <returns>
    /// A <see cref="Power"/> equivalent to the number contained in s.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
    /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
    /// </exception>
    /// <exception cref="ArgumentNullException">s is null.</exception>
    /// <exception cref="OverflowException">
    /// s represents a number less than <see cref="Power.MinValue"/> or greater than <see cref="Power.MaxValue"/>.
    /// </exception>
    /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
    public static Power Parse(string s, NumberStyles style)
    {
        return double.Parse(s, style);
    }

    /// <summary>
    /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Power"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">
    /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
    /// </param>
    /// <returns>
    /// A <see cref="Power"/> equivalent to the number contained in s.
    /// </returns>
    /// <exception cref="ArgumentNullException">s is null.</exception>
    /// <exception cref="OverflowException">
    /// s represents a number less than <see cref="Power.MinValue"/> or greater than <see cref="Power.MaxValue"/>.
    /// </exception>
    /// <exception cref="FormatException">s is not in the correct format.</exception>
    public static Power Parse(string s, IFormatProvider? provider)
    {
        return double.Parse(s, provider);
    }

    /// <summary>
    /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Power"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
    /// </param>
    /// <param name="provider">
    /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
    /// </param>
    /// <returns>
    /// A <see cref="Power"/> equivalent to the number contained in s.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
    /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
    /// </exception>
    /// <exception cref="ArgumentNullException">s is null.</exception>
    /// <exception cref="OverflowException">
    /// s represents a number less than <see cref="Power.MinValue"/> or greater than <see cref="Power.MaxValue"/>.
    /// </exception>
    /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
    public static Power Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return double.Parse(s, style, provider);
    }

    /// <summary>
    /// Converts the string representation of a number to its <see cref="Power"/> equivalent. A return value
    /// indicates whether the conversion succeeded or failed.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="Power"/> value equivalent to the number contained in s,
    /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
    /// is not of the correct format, or represents a number less than <see cref="Power.MinValue"/> or greater than <see cref="Power.MaxValue"/>.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>true if s was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string s, out Power result)
    {
        bool parseResponse = double.TryParse(s, out double parseResult);
        result = parseResult;

        return parseResponse;
    }

    /// <summary>
    /// Converts the string representation of a number in a specified style and culture-specific format to its
    /// <see cref="Power"/> equivalent. A return value indicates whether the conversion succeeded or failed.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="Power"/> value equivalent to the number contained in s,
    /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
    /// is not in a format compliant with style, or represents a number less than <see cref="Power.MinValue"/> or
    /// greater than <see cref="Power.MaxValue"/>. This parameter is passed uninitialized.
    /// </param>
    /// <param name="provider">
    /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
    /// </param>
    /// <returns>true if s was converted successfully; otherwise, false.</returns>
    /// <exception cref="ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
    /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
    /// </exception>
    public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Power result)
    {
        bool parseResponse = double.TryParse(s, style, provider, out double parseResult);
        result = parseResult;

        return parseResponse;
    }

    /// <summary>
    /// Returns the <see cref="TypeCode"/> for value type <see cref="double"/>.
    /// </summary>
    /// <returns>The enumerated constant, <see cref="TypeCode.Double"/>.</returns>
    public TypeCode GetTypeCode()
    {
        return TypeCode.Double;
    }

    #region [ Explicit IConvertible Implementation ]

    // These are explicitly implemented on the native System.Double implementations, so we do the same...

    bool IConvertible.ToBoolean(IFormatProvider? provider)
    {
        return Convert.ToBoolean(m_value, provider);
    }

    char IConvertible.ToChar(IFormatProvider? provider)
    {
        return Convert.ToChar(m_value, provider);
    }

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
    {
        return Convert.ToSByte(m_value, provider);
    }

    byte IConvertible.ToByte(IFormatProvider? provider)
    {
        return Convert.ToByte(m_value, provider);
    }

    short IConvertible.ToInt16(IFormatProvider? provider)
    {
        return Convert.ToInt16(m_value, provider);
    }

    ushort IConvertible.ToUInt16(IFormatProvider? provider)
    {
        return Convert.ToUInt16(m_value, provider);
    }

    int IConvertible.ToInt32(IFormatProvider? provider)
    {
        return Convert.ToInt32(m_value, provider);
    }

    uint IConvertible.ToUInt32(IFormatProvider? provider)
    {
        return Convert.ToUInt32(m_value, provider);
    }

    long IConvertible.ToInt64(IFormatProvider? provider)
    {
        return Convert.ToInt64(m_value, provider);
    }

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
    {
        return Convert.ToUInt64(m_value, provider);
    }

    float IConvertible.ToSingle(IFormatProvider? provider)
    {
        return Convert.ToSingle(m_value, provider);
    }

    double IConvertible.ToDouble(IFormatProvider? provider)
    {
        return m_value;
    }

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
    {
        return Convert.ToDecimal(m_value, provider);
    }

    DateTime IConvertible.ToDateTime(IFormatProvider? provider)
    {
        return Convert.ToDateTime(m_value, provider);
    }

    object IConvertible.ToType(Type type, IFormatProvider? provider)
    {
        return Convert.ChangeType(m_value, type, provider) ?? Activator.CreateInstance(type)!;
    }

    #endregion

    #endregion

    #endregion

    #region [ Operators ]

    #region [ Comparison Operators ]

    /// <summary>
    /// Compares the two values for equality.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator ==(Power value1, Power value2)
    {
        return value1.Equals(value2);
    }

    /// <summary>
    /// Compares the two values for inequality.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator !=(Power value1, Power value2)
    {
        return !value1.Equals(value2);
    }

    /// <summary>
    /// Returns true if left value is less than right value.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator <(Power value1, Power value2)
    {
        return value1.CompareTo(value2) < 0;
    }

    /// <summary>
    /// Returns true if left value is less or equal to than right value.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator <=(Power value1, Power value2)
    {
        return value1.CompareTo(value2) <= 0;
    }

    /// <summary>
    /// Returns true if left value is greater than right value.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator >(Power value1, Power value2)
    {
        return value1.CompareTo(value2) > 0;
    }

    /// <summary>
    /// Returns true if left value is greater than or equal to right value.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> object as the left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> object as the right hand operand.</param>
    /// <returns>A <see cref="bool"/> as the result of the operation.</returns>
    public static bool operator >=(Power value1, Power value2)
    {
        return value1.CompareTo(value2) >= 0;
    }

    #endregion

    #region [ Type Conversion Operators ]

    /// <summary>
    /// Implicitly converts value, represented in watts, to a <see cref="Power"/>.
    /// </summary>
    /// <param name="value">A <see cref="double"/> value.</param>
    /// <returns>A <see cref="Power"/> object.</returns>
    public static implicit operator Power(double value)
    {
        return new Power(value);
    }

    /// <summary>
    /// Implicitly converts <see cref="Power"/>, represented in watts, to a <see cref="double"/>.
    /// </summary>
    /// <param name="value">A <see cref="Power"/> object.</param>
    /// <returns>A <see cref="double"/> value.</returns>
    public static implicit operator double(Power value)
    {
        return value.m_value;
    }

    #endregion

    #region [ Arithmetic Operators ]

    /// <summary>
    /// Returns computed remainder after dividing first value by the second.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="Power"/> object as the result of the operation.</returns>
    public static Power operator %(Power value1, Power value2)
    {
        return value1.m_value % value2.m_value;
    }

    /// <summary>
    /// Returns computed sum of values.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="Power"/> object as the result of the operation.</returns>
    public static Power operator +(Power value1, Power value2)
    {
        return value1.m_value + value2.m_value;
    }

    /// <summary>
    /// Returns computed difference of values.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="Power"/> object as the result of the operation.</returns>
    public static Power operator -(Power value1, Power value2)
    {
        return value1.m_value - value2.m_value;
    }

    /// <summary>
    /// Returns computed product of values.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="Power"/> object as the result of the operation.</returns>
    public static Power operator *(Power value1, Power value2)
    {
        return value1.m_value * value2.m_value;
    }

    /// <summary>
    /// Returns computed division of values.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="Power"/> object as the result of the operation.</returns>
    public static Power operator /(Power value1, Power value2)
    {
        return value1.m_value / value2.m_value;
    }

    // C# doesn't expose an exponent operator but some other .NET languages do,
    // so we expose the operator via its native special IL function name

    /// <summary>
    /// Returns result of first value raised to power of second value.
    /// </summary>
    /// <param name="value1">A <see cref="Power"/> left hand operand.</param>
    /// <param name="value2">A <see cref="Power"/> right hand operand.</param>
    /// <returns>A <see cref="double"/> value as the result of the operation.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
    public static double op_Exponent(Power value1, Power value2)
    {
        return Math.Pow(value1.m_value, value2.m_value);
    }

    #endregion

    #endregion

    #region [ Static ]

    // Static Fields

    /// <summary>Represents the largest possible value of an <see cref="Power"/>. This field is constant.</summary>
    public static readonly Power MaxValue = double.MaxValue;

    /// <summary>Represents the smallest possible value of an <see cref="Power"/>. This field is constant.</summary>
    public static readonly Power MinValue = double.MinValue;

    // Static Methods

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in mechanical horsepower (Imperial).
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in mechanical horsepower.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in mechanical horsepower.</returns>
    public static Power FromHorsepower(double value)
    {
        return new Power(value * HorsepowerFactor);
    }

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in metric horsepower.
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in metric horsepower.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in metric horsepower.</returns>
    public static Power FromMetricHorsepower(double value)
    {
        return new Power(value * MetricHorsepowerFactor);
    }

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in boiler horsepower.
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in boiler horsepower.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in boiler horsepower.</returns>
    public static Power FromBoilerHorsepower(double value)
    {
        return new Power(value * BoilerHorsepowerFactor);
    }

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in BTU (International Table) per second.
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in BTU per second.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in BTU per second.</returns>
    public static Power FromBTUPerSecond(double value)
    {
        return new Power(value * BTUPerSecondFactor);
    }

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in calories (International Table) per second.
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in calories per second.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in calories per second.</returns>
    public static Power FromCaloriesPerSecond(double value)
    {
        return new Power(value * CaloriesPerSecondFactor);
    }

    /// <summary>
    /// Creates a new <see cref="Power"/> value from the specified <paramref name="value"/> in liters-atmosphere per second.
    /// </summary>
    /// <param name="value">New <see cref="Power"/> value in liters-atmosphere per second.</param>
    /// <returns>New <see cref="Power"/> object from the specified <paramref name="value"/> in liters-atmosphere per second.</returns>
    public static Power FromLitersAtmospherePerSecond(double value)
    {
        return new Power(value * LitersAtmospherePerSecondFactor);
    }

    /// <summary>
    /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Power"/> in watts.
    /// </summary>
    /// <param name="value">Source value.</param>
    /// <param name="sourceUnit">Source value units.</param>
    /// <returns>New <see cref="Power"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
    public static Power ConvertFrom(double value, PowerUnit sourceUnit)
    {
        return sourceUnit switch
        {
            PowerUnit.Watts => value,
            PowerUnit.Horsepower => FromHorsepower(value),
            PowerUnit.MetricHorsepower => FromMetricHorsepower(value),
            PowerUnit.BoilerHorsepower => FromBoilerHorsepower(value),
            PowerUnit.BTUPerSecond => FromBTUPerSecond(value),
            PowerUnit.CaloriesPerSecond => FromCaloriesPerSecond(value),
            PowerUnit.LitersAtmospherePerSecond => FromLitersAtmospherePerSecond(value),
            _ => throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null)
        };
    }

    #endregion
}
