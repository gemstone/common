//******************************************************************************************************
//  ComplexExtensions.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  06/24/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Gemstone.ComplexExtensions
{
    /// <summary>
    /// Defines extension methods for complex numbers.
    /// </summary>
    public static class ComplexExtensions
    {
        /// <summary>
        /// Converts the given string to a complex number.
        /// </summary>
        /// <param name="notation">The string using complex notation</param>
        /// <param name="imaginarySymbol">The symbol used for the square root of -1</param>
        /// <returns>The complex number</returns>
        public static Complex FromComplexNotation(this string notation, char? imaginarySymbol = null)
        {
            if (string.IsNullOrEmpty(notation))
                throw new FormatException("Empty string cannot be parsed as a complex number");

            MatchCollection tokens = Regex.Matches(notation, @"(?<Operators>[-+ ]*)(?<Number>(?:\* *-|[^-+])+)");

            static Func<string, T> ToDelegate<T>(Func<string, T> func) => func;

            // Operator validation
            var getOperatorInfo = ToDelegate(operators => new
            {
                Multiplier = -2 * (operators.Count(c => c == '-') % 2) + 1,
                IsPaddedRight = operators.EndsWith(" "),
                NumOperators = operators.Count(c => c != ' ')
            });

            var operatorInfo = tokens
                .Cast<Match>()
                .Select(token => token.Groups["Operators"].Value)
                .Select(getOperatorInfo)
                .ToList();

            bool tooManyOperators = operatorInfo
                .Select(info => info.NumOperators > 1)
                .First();

            tooManyOperators = tooManyOperators || operatorInfo
                .Any(info => info.NumOperators > 2);

            if (tooManyOperators)
                throw new FormatException($"Too many operators in complex number: {notation}");

            bool invalidSpace = operatorInfo
                .Select(validator => validator.IsPaddedRight)
                .First();

            invalidSpace = invalidSpace || operatorInfo
                .Skip(1)
                .Where(validator => validator.IsPaddedRight)
                .Any(validator => validator.NumOperators > 1);

            if (invalidSpace)
                throw new FormatException($"Invalid space between unary operator and number: {notation}");

            // Number validation
            string symbols = imaginarySymbol?.ToString() ?? "ij";

            char[] trimChars = $" *{symbols}"
                .ToArray();

            var getNumberInfo = ToDelegate(number =>
            {
                int numSymbols = Regex.Matches(number, $"[{symbols}]").Count;
                int numAsterisks = number.Count(c => c == '*');
                bool invalidAsterisk = Regex.IsMatch(number.Trim(), @"^\*|\*$");
                string text = number.Trim(trimChars);

                if (string.IsNullOrEmpty(text) && numSymbols > 0)
                    text = "1";

                return new
                {
                    NumSymbols = numSymbols,
                    NumAsterisks = numAsterisks,
                    InvalidAsterisk = invalidAsterisk,
                    Text = text
                };
            });

            var numberInfo = tokens
                .Cast<Match>()
                .Select(token => token.Groups["Number"].Value)
                .Select(getNumberInfo)
                .ToList();

            if (numberInfo.Count(info => info.NumSymbols == 0) > 1)
                throw new FormatException($"Too many real terms detected: {notation}");

            if (numberInfo.Sum(info => info.NumSymbols) > 1)
                throw new FormatException($"Too many imaginary symbols detected: {notation}");

            if (numberInfo.Sum(info => info.NumAsterisks) > 1 || numberInfo.Any(info => info.InvalidAsterisk))
                throw new FormatException($"Invalid asterisk detected: {notation}");

            List<int> multipliers = operatorInfo
                .Select(info => info.Multiplier)
                .ToList();

            List<double> nums = numberInfo
                .Select(info => info.Text)
                .Select(double.Parse)
                .Select((num, i) => multipliers[i] * num)
                .ToList();

            double real = Enumerable
                .Range(0, nums.Count)
                .Where(i => numberInfo[i].NumSymbols == 0)
                .Select(i => nums[i])
                .DefaultIfEmpty(0.0D)
                .First();

            double imaginary = Enumerable
                .Range(0, nums.Count)
                .Where(i => numberInfo[i].NumSymbols > 0)
                .Select(i => nums[i])
                .DefaultIfEmpty(0.0D)
                .First();

            return new Complex(real, imaginary);
        }

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex) =>
            complex.ToComplexNotation("", 'i', ComplexNotationOptions.Default);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="numericFormat">The format string to use when converting numbers to their string representation</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, string numericFormat) =>
            complex.ToComplexNotation(numericFormat, 'i', ComplexNotationOptions.Default);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="imaginarySymbol">The symbol used for the square root of -1</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, char imaginarySymbol) =>
            complex.ToComplexNotation("", imaginarySymbol, ComplexNotationOptions.Default);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="options">The options that define various aspects of the notation</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, ComplexNotationOptions options) =>
            complex.ToComplexNotation("", 'i', options);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="numericFormat">The format string to use when converting numbers to their string representation</param>
        /// <param name="imaginarySymbol">The symbol used for the square root of -1</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, string numericFormat, char imaginarySymbol) =>
            complex.ToComplexNotation(numericFormat, imaginarySymbol, ComplexNotationOptions.Default);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="numericFormat">The format string to use when converting numbers to their string representation</param>
        /// <param name="options">The options that define various aspects of the notation</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, string numericFormat, ComplexNotationOptions options) =>
            complex.ToComplexNotation(numericFormat, 'i', options);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="imaginarySymbol">The symbol used for the square root of -1</param>
        /// <param name="options">The options that define various aspects of the notation</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, char imaginarySymbol, ComplexNotationOptions options) =>
            complex.ToComplexNotation(string.Empty, imaginarySymbol, options);

        /// <summary>
        /// Converts the given complex number to a string using complex notation.
        /// </summary>
        /// <param name="complex">The complex number</param>
        /// <param name="numericFormat">The format string to use when converting numbers to their string representation</param>
        /// <param name="imaginarySymbol">The symbol used for the square root of -1</param>
        /// <param name="options">The options that define various aspects of the notation</param>
        /// <returns>A string representation of the complex number.</returns>
        public static string ToComplexNotation(this Complex complex, string numericFormat, char imaginarySymbol, ComplexNotationOptions options)
        {
            string formatString = !string.IsNullOrEmpty(numericFormat)
                ? $"{{0:{numericFormat}}}"
                : "{0}";

            string FormatNumber(double number) => string.Format(formatString, number);

            double real = Math.Abs(complex.Real);
            double imaginary = Math.Abs(complex.Imaginary);

            var flags = new
            {
                PrefixSymbol = options.HasFlag(ComplexNotationOptions.PrefixSymbol),
                InsertAsterisk = options.HasFlag(ComplexNotationOptions.InsertAsterisk),
                ImaginaryFirst = options.HasFlag(ComplexNotationOptions.ImaginaryFirst),
                AllowSubtraction = options.HasFlag(ComplexNotationOptions.AllowSubtraction),
                PreferSubtraction = options.HasFlag(ComplexNotationOptions.PreferSubtraction),
                NoSimplify = options.HasFlag(ComplexNotationOptions.NoSimplify),
                OperatorWhitespace = options.HasFlag(ComplexNotationOptions.OperatorWhitespace),
                AsteriskWhitespace = options.HasFlag(ComplexNotationOptions.AsteriskWhitespace),
                SymbolWhitespace = options.HasFlag(ComplexNotationOptions.SymbolWhitespace)
            };

            bool displayRealTerm = (flags.NoSimplify || real != 0.0D);
            bool displayImaginaryTerm = (flags.NoSimplify || imaginary != 0.0D);
            bool displayImaginaryNumber = (flags.NoSimplify || imaginary != 1.0D);

            // This is by far the easiest case
            // so just get it out of the way
            if (!displayImaginaryTerm)
                return FormatNumber(complex.Real);

            // Subtraction can only be preferred if it's allowed in the first place
            bool preferSubtraction = flags.AllowSubtraction && flags.PreferSubtraction;

            bool realFirst =
                (!flags.ImaginaryFirst && !(preferSubtraction && complex.Imaginary >= 0.0D && complex.Real < 0.0D)) ||
                (flags.ImaginaryFirst && preferSubtraction && complex.Real >= 0.0D && complex.Imaginary < 0.0D);

            string GetImaginaryTerm()
            {
                bool displayUnary =
                    (complex.Imaginary < 0.0D) &&
                    (!realFirst || !displayRealTerm || !flags.AllowSubtraction);

                string unary = displayUnary ? "-" : "";

                if (displayImaginaryNumber && flags.PrefixSymbol && flags.InsertAsterisk && flags.AsteriskWhitespace)
                    return $"{imaginarySymbol} * {unary}{FormatNumber(imaginary)}";

                if (displayImaginaryNumber && flags.InsertAsterisk && flags.AsteriskWhitespace)
                    return $"{unary}{FormatNumber(imaginary)} * {imaginarySymbol}";

                if (displayImaginaryNumber && flags.PrefixSymbol && flags.InsertAsterisk)
                    return $"{imaginarySymbol}*{unary}{FormatNumber(imaginary)}";

                if (displayImaginaryNumber && flags.InsertAsterisk)
                    return $"{unary}{FormatNumber(imaginary)}*{imaginarySymbol}";

                if (displayImaginaryNumber && flags.PrefixSymbol && flags.SymbolWhitespace)
                    return $"{unary}{imaginarySymbol} {FormatNumber(imaginary)}";

                if (displayImaginaryNumber && flags.SymbolWhitespace)
                    return $"{unary}{FormatNumber(imaginary)} {imaginarySymbol}";

                if (displayImaginaryNumber && flags.PrefixSymbol)
                    return $"{unary}{imaginarySymbol}{FormatNumber(imaginary)}";

                if (displayImaginaryNumber)
                    return $"{unary}{FormatNumber(imaginary)}{imaginarySymbol}";

                return $"{unary}{imaginarySymbol}";
            }

            string imaginaryTerm = GetImaginaryTerm();

            if (!displayRealTerm)
                return imaginaryTerm;

            string GetRealTerm()
            {
                // No need to check displayImaginaryTerm because if it
                // was false, a value would have been returned already
                bool displayUnary =
                    (complex.Real < 0.0D) &&
                    (realFirst || !flags.AllowSubtraction);

                string unary = displayUnary ? "-" : "";
                return $"{unary}{FormatNumber(real)}";
            }

            string realTerm = GetRealTerm();

            int sign = Math.Sign(!realFirst ? complex.Real : complex.Imaginary);
            string op = (sign >= 0 || !flags.AllowSubtraction) ? "+" : "-";

            if (flags.OperatorWhitespace && realFirst)
                return $"{realTerm} {op} {imaginaryTerm}";

            if (flags.OperatorWhitespace)
                return $"{imaginaryTerm} {op} {realTerm}";

            return realFirst
                ? $"{realTerm}{op}{imaginaryTerm}"
                : $"{imaginaryTerm}{op}{realTerm}";
        }
    }
}
