//******************************************************************************************************
//  Word.cs - Gbtc
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
//  04/10/2009 - James R. Carroll
//       Generated original version of source code.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System.Runtime.CompilerServices;

namespace Gemstone.WordExtensions
{
    /// <summary>
    /// Defines extension functions related to 16-bit words, 32-bit double-words and 64-bit quad-words.
    /// </summary>
    public static class WordExtensions
    {
        /// <summary>
        /// Aligns word length value to a 16-bit boundary.
        /// </summary>
        /// <param name="word">Word value to align.</param>
        /// <returns>Word value aligned to next 16-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short AlignWord(this short word) => (short)(word + 1 - (word - 1) % 2);

        /// <summary>
        /// Aligns word length value to a 16-bit boundary.
        /// </summary>
        /// <param name="word">Word value to align.</param>
        /// <returns>Word value aligned to next 16-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort AlignWord(this ushort word) => (ushort)(word + 1 - (word - 1) % 2);

        /// <summary>
        /// Aligns double-word length value to a 32-bit boundary.
        /// </summary>
        /// <param name="doubleWord">Double-word value to align.</param>
        /// <returns>Double-word value aligned to next 32-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AlignDoubleWord(this int doubleWord) => doubleWord + 3 - (doubleWord - 1) % 4;

        /// <summary>
        /// Aligns double-word length value to a 32-bit boundary.
        /// </summary>
        /// <param name="doubleWord">Double-word value to align.</param>
        /// <returns>Double-word value aligned to next 32-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint AlignDoubleWord(this uint doubleWord) => doubleWord + 3 - (doubleWord - 1) % 4;

        /// <summary>
        /// Aligns quad-word length value to a 64-bit boundary.
        /// </summary>
        /// <param name="quadWord">Quad-word value to align.</param>
        /// <returns>Quad-word value aligned to next 64-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long AlignQuadWord(this long quadWord) => quadWord + 7 - (quadWord - 1) % 8;

        /// <summary>
        /// Aligns quad-word length value to a 64-bit boundary.
        /// </summary>
        /// <param name="quadWord">Quad-word value to align.</param>
        /// <returns>Quad-word value aligned to next 64-bit boundary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong AlignQuadWord(this ulong quadWord) => quadWord + 7 - (quadWord - 1) % 8;

        /// <summary>
        /// Returns the high-nibble (high 4-bits) from a byte.
        /// </summary>
        /// <param name="value">Byte value.</param>
        /// <returns>The high-nibble of the specified byte value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HighNibble(this byte value) => (byte)((value & 0xF0) >> 4);

        /// <summary>
        /// Returns the high-byte from an unsigned word (UInt16).
        /// </summary>
        /// <param name="word">2-byte, 16-bit unsigned integer value.</param>
        /// <returns>The high-order byte of the specified 16-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the byte value whose in-memory representation
        /// is the same as the right-most, most-significant-byte of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HighByte(this ushort word) => (byte)((word & 0xFF00) >> 8);

        /// <summary>
        /// Returns the unsigned high-word (UInt16) from an unsigned double-word (UInt32).
        /// </summary>
        /// <param name="doubleWord">4-byte, 32-bit unsigned integer value.</param>
        /// <returns>The unsigned high-order word of the specified 32-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the word value
        /// whose in-memory representation is the same as the right-most, most-significant-word
        /// of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HighWord(this uint doubleWord) => (ushort)((doubleWord & 0xFFFF0000U) >> 16);

        /// <summary>
        /// Returns the unsigned high-double-word (UInt32) from an unsigned quad-word (UInt64).
        /// </summary>
        /// <param name="quadWord">8-byte, 64-bit unsigned integer value.</param>
        /// <returns>The high-order double-word of the specified 64-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the word value
        /// whose in-memory representation is the same as the right-most, most-significant-word
        /// of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint HighDoubleWord(this ulong quadWord) => (uint)((quadWord & 0xFFFFFFFF00000000UL) >> 32);

        /// <summary>
        /// Returns the low-nibble (low 4-bits) from a byte.
        /// </summary>
        /// <param name="value">Byte value.</param>
        /// <returns>The low-nibble of the specified byte value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LowNibble(this byte value) => (byte)(value & 0x0F);

        /// <summary>
        /// Returns the low-byte from an unsigned word (UInt16).
        /// </summary>
        /// <param name="word">2-byte, 16-bit unsigned integer value.</param>
        /// <returns>The low-order byte of the specified 16-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the byte value
        /// whose in-memory representation is the same as the left-most, least-significant-byte
        /// of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LowByte(this ushort word) => (byte)(word & 0x00FF);

        /// <summary>
        /// Returns the unsigned low-word (UInt16) from an unsigned double-word (UInt32).
        /// </summary>
        /// <param name="doubleWord">4-byte, 32-bit unsigned integer value.</param>
        /// <returns>The unsigned low-order word of the specified 32-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the word value
        /// whose in-memory representation is the same as the left-most, least-significant-word
        /// of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort LowWord(this uint doubleWord) => (ushort)(doubleWord & 0x0000FFFFU);

        /// <summary>
        /// Returns the unsigned low-double-word (UInt32) from an unsigned quad-word (UInt64).
        /// </summary>
        /// <param name="quadWord">8-byte, 64-bit unsigned integer value.</param>
        /// <returns>The low-order double-word of the specified 64-bit unsigned integer value.</returns>
        /// <remarks>
        /// On little-endian architectures (e.g., Intel platforms), this will be the word value
        /// whose in-memory representation is the same as the left-most, least-significant-word
        /// of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint LowDoubleWord(this ulong quadWord) => (uint)(quadWord & 0x00000000FFFFFFFFUL);
    }
}
