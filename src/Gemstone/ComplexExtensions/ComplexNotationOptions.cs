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

namespace Gemstone.ComplexExtensions
{
    /// <summary>
    /// Provides options for customizing the notation
    /// used for representing complex numbers as text.
    /// </summary>
    [Flags]
    public enum ComplexNotationOptions
    {
        /// <summary>
        /// The default notation if no options are specified.
        /// </summary>
        Default = AllowSubtraction | OperatorWhitespace,

        /// <summary>
        /// Specifies that the symbol on the imaginary term should be prefixed.
        /// </summary>
        PrefixSymbol = (int)Bits.Bit00,

        /// <summary>
        /// Inserts an asterisk between the number and the symbol on
        /// the imaginary term to explicitly indicate multiplication.
        /// </summary>
        InsertAsterisk = (int)Bits.Bit01,

        /// <summary>
        /// Specifies that the imaginary term should come first.
        /// </summary>
        ImaginaryFirst = (int)Bits.Bit02,

        /// <summary>
        /// Specifies that the subtraction operator should
        /// be used if the second term is negative.
        /// </summary>
        AllowSubtraction = (int)Bits.Bit03,

        /// <summary>
        /// Specifies terms should be swapped if doing
        /// so would convert addition into subtraction.
        /// </summary>
        PreferSubtraction = (int)Bits.Bit04,

        /// <summary>
        /// Specifies numbers and terms are always displayed regardless
        /// of whether they can be mathematically simplified.
        /// </summary>
        NoSimplify = (int)Bits.Bit05,

        /// <summary>
        /// Inserts whitespace around the plus/minus operator.
        /// </summary>
        OperatorWhitespace = (int)Bits.Bit06,

        /// <summary>
        /// Inserts whitespace around the asterisk.
        /// </summary>
        AsteriskWhitespace = (int)Bits.Bit07,

        /// <summary>
        /// Inserts whitespace between the symbol and the number.
        /// </summary>
        SymbolWhitespace = (int)Bits.Bit08,

        /// <summary>
        /// Enables all whitespace options.
        /// </summary>
        UseWhitespace = OperatorWhitespace | AsteriskWhitespace | SymbolWhitespace,

        /// <summary>
        /// Provides a verbose output with consistent ordering of terms,
        /// no subtraction, asterisks, and all whitespace options.
        /// </summary>
        Verbose = InsertAsterisk | NoSimplify | UseWhitespace,

        /// <summary>
        /// No options are enabled.
        /// </summary>
        None = 0
    }
}
