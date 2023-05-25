//******************************************************************************************************
//  BitExtensions.cs - Gbtc
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
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

// Ignore Spelling: bitmask

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll, Pinal C. Patel
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

using System;

namespace Gemstone.BitExtensions
{
    /// <summary>
    /// Defines extension methods related to bit operations.
    /// </summary>
    public static class BitExtensions
    {
        /// <summary>
        /// Gets the bit value for the specified bit index (0 - 63).
        /// </summary>
        /// <param name="bit">Bit index (0 - 63)</param>
        /// <returns>Value of the specified <paramref name="bit"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Parameter must be between 0 and 63.</exception>
        public static Bits BitVal(int bit)
        {
            return bit switch
            {
                #region [ Bit Cases (0 - 63) ]
                00 => Bits.Bit00,
                01 => Bits.Bit01,
                02 => Bits.Bit02,
                03 => Bits.Bit03,
                04 => Bits.Bit04,
                05 => Bits.Bit05,
                06 => Bits.Bit06,
                07 => Bits.Bit07,
                08 => Bits.Bit08,
                09 => Bits.Bit09,
                10 => Bits.Bit10,
                11 => Bits.Bit11,
                12 => Bits.Bit12,
                13 => Bits.Bit13,
                14 => Bits.Bit14,
                15 => Bits.Bit15,
                16 => Bits.Bit16,
                17 => Bits.Bit17,
                18 => Bits.Bit18,
                19 => Bits.Bit19,
                20 => Bits.Bit20,
                21 => Bits.Bit21,
                22 => Bits.Bit22,
                23 => Bits.Bit23,
                24 => Bits.Bit24,
                25 => Bits.Bit25,
                26 => Bits.Bit26,
                27 => Bits.Bit27,
                28 => Bits.Bit28,
                29 => Bits.Bit29,
                30 => Bits.Bit30,
                31 => Bits.Bit31,
                32 => Bits.Bit32,
                33 => Bits.Bit33,
                34 => Bits.Bit34,
                35 => Bits.Bit35,
                36 => Bits.Bit36,
                37 => Bits.Bit37,
                38 => Bits.Bit38,
                39 => Bits.Bit39,
                40 => Bits.Bit40,
                41 => Bits.Bit41,
                42 => Bits.Bit42,
                43 => Bits.Bit43,
                44 => Bits.Bit44,
                45 => Bits.Bit45,
                46 => Bits.Bit46,
                47 => Bits.Bit47,
                48 => Bits.Bit48,
                49 => Bits.Bit49,
                50 => Bits.Bit50,
                51 => Bits.Bit51,
                52 => Bits.Bit52,
                53 => Bits.Bit53,
                54 => Bits.Bit54,
                55 => Bits.Bit55,
                56 => Bits.Bit56,
                57 => Bits.Bit57,
                58 => Bits.Bit58,
                59 => Bits.Bit59,
                60 => Bits.Bit60,
                61 => Bits.Bit61,
                62 => Bits.Bit62,
                63 => Bits.Bit63,
                _ => throw new ArgumentOutOfRangeException(nameof(bit), "Parameter must be between 0 and 63")
                #endregion
            };
        }

        #region [ SetBits Extensions ]

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> set.</returns>
        public static sbyte SetBits(this sbyte source, Bits bits)
        {
            checked
            {
                return SetBits(source, (sbyte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> set.</returns>
        public static sbyte SetBits(this sbyte source, sbyte bits) => (sbyte)(source | bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> set.</returns>
        public static byte SetBits(this byte source, Bits bits)
        {
            checked
            {
                return SetBits(source, (byte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> set.</returns>
        public static byte SetBits(this byte source, byte bits) => (byte)(source | bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> set.</returns>
        public static short SetBits(this short source, Bits bits)
        {
            checked
            {
                return SetBits(source, (short)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> set.</returns>
        public static short SetBits(this short source, short bits) => (short)(source | bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> set.</returns>
        public static ushort SetBits(this ushort source, Bits bits)
        {
            checked
            {
                return SetBits(source, (ushort)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> set.</returns>
        public static ushort SetBits(this ushort source, ushort bits) => (ushort)(source | bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> set.</returns>
        public static int SetBits(this int source, Bits bits)
        {
            checked
            {
                return SetBits(source, (int)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> set.</returns>
        public static int SetBits(this int source, int bits) => source | bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> set.</returns>
        public static uint SetBits(this uint source, Bits bits)
        {
            checked
            {
                return SetBits(source, (uint)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> set.</returns>
        public static uint SetBits(this uint source, uint bits) => source | bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> set.</returns>
        public static long SetBits(this long source, Bits bits)
        {
            checked
            {
                return SetBits(source, (long)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> set.</returns>
        public static long SetBits(this long source, long bits) => source | bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to set.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> set.</returns>
        public static ulong SetBits(this ulong source, Bits bits) => SetBits(source, (ulong)bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to set.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> set.</returns>
        public static ulong SetBits(this ulong source, ulong bits) => source | bits;

        #endregion

        #region [ ClearBits Extensions ]

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static sbyte ClearBits(this sbyte source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (sbyte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static sbyte ClearBits(this sbyte source, sbyte bits) => (sbyte)(source & ~bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static byte ClearBits(this byte source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (byte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static byte ClearBits(this byte source, byte bits) => (byte)(source & ~bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static short ClearBits(this short source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (short)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static short ClearBits(this short source, short bits) => (short)(source & ~bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static ushort ClearBits(this ushort source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (ushort)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static ushort ClearBits(this ushort source, ushort bits) => (ushort)(source & ~bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static int ClearBits(this int source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (int)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static int ClearBits(this int source, int bits) => source & ~bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static uint ClearBits(this uint source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (uint)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static uint ClearBits(this uint source, uint bits) => source & ~bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static long ClearBits(this long source, Bits bits)
        {
            checked
            {
                return ClearBits(source, (long)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static long ClearBits(this long source, long bits) => source & ~bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to clear.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static ulong ClearBits(this ulong source, Bits bits) => ClearBits(source, (ulong)bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> cleared.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to clear.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> cleared.</returns>
        public static ulong ClearBits(this ulong source, ulong bits) => source & ~bits;

        #endregion

        #region [ CheckBits Extensions ]

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this sbyte source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (sbyte)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this sbyte source, sbyte bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this sbyte source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (sbyte)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this sbyte source, sbyte bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this byte source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (byte)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this byte source, byte bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this byte source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (byte)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this byte source, byte bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this short source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (short)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this short source, short bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this short source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (short)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this short source, short bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ushort source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (ushort)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ushort source, ushort bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ushort source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (ushort)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ushort source, ushort bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this int source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (int)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this int source, int bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this int source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (int)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this int source, int bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this uint source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (uint)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this uint source, uint bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this uint source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (uint)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this uint source, uint bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this long source, Bits bits)
        {
            checked
            {
                return CheckBits(source, (long)bits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this long source, long bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this long source, Bits bits, bool allBits)
        {
            checked
            {
                return CheckBits(source, (long)bits, allBits);
            }
        }

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this long source, long bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ulong source, Bits bits) => CheckBits(source, (ulong)bits);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ulong source, ulong bits) => CheckBits(source, bits, true);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ulong source, Bits bits, bool allBits) => CheckBits(source, (ulong)bits, allBits);

        /// <summary>
        /// Determines if specified <paramref name="bits"/> are set.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to check.</param>
        /// <param name="allBits">true to check if all <paramref name="bits"/> are set; otherwise false.</param>
        /// <returns>true if specified <paramref name="bits"/> are set in <paramref name="source"/> value; otherwise false.</returns>
        public static bool CheckBits(this ulong source, ulong bits, bool allBits) => allBits ? (source & bits) == bits : (source & bits) != 0;

        #endregion

        #region [ ToggleBits Extensions ]

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static sbyte ToggleBits(this sbyte source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (sbyte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="sbyte"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static sbyte ToggleBits(this sbyte source, sbyte bits) => (sbyte)(source ^ bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static byte ToggleBits(this byte source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (byte)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="byte"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static byte ToggleBits(this byte source, byte bits) => (byte)(source ^ bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static short ToggleBits(this short source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (short)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="short"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static short ToggleBits(this short source, short bits) => (short)(source ^ bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static ushort ToggleBits(this ushort source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (ushort)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="ushort"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static ushort ToggleBits(this ushort source, ushort bits) => (ushort)(source ^ bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static int ToggleBits(this int source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (int)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="int"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static int ToggleBits(this int source, int bits) => source ^ bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static uint ToggleBits(this uint source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (uint)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="uint"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static uint ToggleBits(this uint source, uint bits) => source ^ bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static long ToggleBits(this long source, Bits bits)
        {
            checked
            {
                return ToggleBits(source, (long)bits);
            }
        }

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="long"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static long ToggleBits(this long source, long bits) => source ^ bits;

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits"><see cref="Bits"/> to toggle.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static ulong ToggleBits(this ulong source, Bits bits) => ToggleBits(source, (ulong)bits);

        /// <summary>
        /// Returns value with specified <paramref name="bits"/> toggled.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bits">Bit-mask of the bits to toggle.</param>
        /// <returns><see cref="ulong"/> value with specified <paramref name="bits"/> toggled.</returns>
        public static ulong ToggleBits(this ulong source, ulong bits) => source ^ bits;

        #endregion

        #region [ GetMaskedValue Extensions ]

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="sbyte"/> value.</returns>
        public static sbyte GetMaskedValue(this sbyte source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (sbyte)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="sbyte"/> value.</returns>
        public static sbyte GetMaskedValue(this sbyte source, sbyte bitmask) => (sbyte)(source & bitmask);

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="byte"/> value.</returns>
        public static byte GetMaskedValue(this byte source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (byte)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="byte"/> value.</returns>
        public static byte GetMaskedValue(this byte source, byte bitmask) => (byte)(source & bitmask);

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="short"/> value.</returns>
        public static short GetMaskedValue(this short source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (short)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="short"/> value.</returns>
        public static short GetMaskedValue(this short source, short bitmask) => (short)(source & bitmask);

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="ushort"/> value.</returns>
        public static ushort GetMaskedValue(this ushort source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (ushort)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="ushort"/> value.</returns>
        public static ushort GetMaskedValue(this ushort source, ushort bitmask) => (ushort)(source & bitmask);

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="int"/> value.</returns>
        public static int GetMaskedValue(this int source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (int)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="int"/> value.</returns>
        public static int GetMaskedValue(this int source, int bitmask) => source & bitmask;

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="uint"/> value.</returns>
        public static uint GetMaskedValue(this uint source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (uint)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="uint"/> value.</returns>
        public static uint GetMaskedValue(this uint source, uint bitmask) => source & bitmask;

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="long"/> value.</returns>
        public static long GetMaskedValue(this long source, Bits bitmask)
        {
            checked
            {
                return GetMaskedValue(source, (long)bitmask);
            }
        }

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="long"/> value.</returns>
        public static long GetMaskedValue(this long source, long bitmask) => source & bitmask;

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <returns><see cref="ulong"/> value.</returns>
        public static ulong GetMaskedValue(this ulong source, Bits bitmask) => GetMaskedValue(source, (ulong)bitmask);

        /// <summary>
        /// Returns value stored in the bits represented by the specified <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <returns><see cref="ulong"/> value.</returns>
        public static ulong GetMaskedValue(this ulong source, ulong bitmask) => source & bitmask;

        #endregion

        #region [ SetMaskedValue Extensions ]

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="sbyte"/> value.</returns>
        public static sbyte SetMaskedValue(this sbyte source, Bits bitmask, sbyte value)
        {
            checked
            {
                return SetMaskedValue(source, (sbyte)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="sbyte"/> value.</returns>
        public static sbyte SetMaskedValue(this sbyte source, sbyte bitmask, sbyte value) => (sbyte)((sbyte)(source & ~bitmask) | value);

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="byte"/> value.</returns>
        public static byte SetMaskedValue(this byte source, Bits bitmask, byte value)
        {
            checked
            {
                return SetMaskedValue(source, (byte)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="byte"/> value.</returns>
        public static byte SetMaskedValue(this byte source, byte bitmask, byte value) => (byte)((source & ~bitmask) | value);

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="short"/> value.</returns>
        public static short SetMaskedValue(this short source, Bits bitmask, short value)
        {
            checked
            {
                return SetMaskedValue(source, (short)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="short"/> value.</returns>
        public static short SetMaskedValue(this short source, short bitmask, short value) => (short)((short)(source & ~bitmask) | value);

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="ushort"/> value.</returns>
        public static ushort SetMaskedValue(this ushort source, Bits bitmask, ushort value)
        {
            checked
            {
                return SetMaskedValue(source, (ushort)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="ushort"/> value.</returns>
        public static ushort SetMaskedValue(this ushort source, ushort bitmask, ushort value) => (ushort)((source & ~bitmask) | value);

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="int"/> value.</returns>
        public static int SetMaskedValue(this int source, Bits bitmask, int value)
        {
            checked
            {
                return SetMaskedValue(source, (int)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="int"/> value.</returns>
        public static int SetMaskedValue(this int source, int bitmask, int value) => (source & ~bitmask) | value;

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="uint"/> value.</returns>
        public static uint SetMaskedValue(this uint source, Bits bitmask, uint value)
        {
            checked
            {
                return SetMaskedValue(source, (uint)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="uint"/> value.</returns>
        public static uint SetMaskedValue(this uint source, uint bitmask, uint value) => (source & ~bitmask) | value;

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="long"/> value.</returns>
        public static long SetMaskedValue(this long source, Bits bitmask, long value)
        {
            checked
            {
                return SetMaskedValue(source, (long)bitmask, value);
            }
        }

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="long"/> value.</returns>
        public static long SetMaskedValue(this long source, long bitmask, long value) => (source & ~bitmask) | value;

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask"><see cref="Bits"/> that make-up the bit-mask.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="ulong"/> value.</returns>
        public static ulong SetMaskedValue(this ulong source, Bits bitmask, ulong value) => SetMaskedValue(source, (ulong)bitmask, value);

        /// <summary>
        /// Returns value after setting a new <paramref name="value"/> for the bits specified by the <paramref name="bitmask"/>.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="bitmask">Bit-mask of the bits involved.</param>
        /// <param name="value">New value.</param>
        /// <returns><see cref="ulong"/> value.</returns>
        public static ulong SetMaskedValue(this ulong source, ulong bitmask, ulong value) => (source & ~bitmask) | value;

        #endregion

        #region [ Bit Rotation Extensions ]

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static byte BitRotL(this byte value, int rotations)
        {
            for (int x = 1; x <= rotations % 8; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit07);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static sbyte BitRotL(this sbyte value, int rotations)
        {
            for (int x = 1; x <= rotations % 8; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit07);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static short BitRotL(this short value, int rotations)
        {
            for (int x = 1; x <= rotations % 16; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit15);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static ushort BitRotL(this ushort value, int rotations)
        {
            for (int x = 1; x <= rotations % 16; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit15);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static int BitRotL(this int value, int rotations)
        {
            for (int x = 1; x <= rotations % 32; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit31);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static uint BitRotL(this uint value, int rotations)
        {
            for (int x = 1; x <= rotations % 32; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit31);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static long BitRotL(this long value, int rotations)
        {
            for (int x = 1; x <= rotations % 64; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit63);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate right on little-endian
        /// architectures.
        /// </remarks>
        public static ulong BitRotL(this ulong value, int rotations)
        {
            for (int x = 1; x <= rotations % 64; x++)
            {
                bool hiBitSet = value.CheckBits(Bits.Bit63);

                value <<= 1;

                if (hiBitSet)
                    value = value.SetBits(Bits.Bit00);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static byte BitRotR(this byte value, int rotations)
        {
            for (int x = 1; x <= rotations % 8; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit07) : value.ClearBits(Bits.Bit07);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static sbyte BitRotR(this sbyte value, int rotations)
        {
            for (int x = 1; x <= rotations % 8; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit07) : value.ClearBits(Bits.Bit07);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static short BitRotR(this short value, int rotations)
        {
            for (int x = 1; x <= rotations % 16; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit15) : value.ClearBits(Bits.Bit15);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static ushort BitRotR(this ushort value, int rotations)
        {
            for (int x = 1; x <= rotations % 16; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit15) : value.ClearBits(Bits.Bit15);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static int BitRotR(this int value, int rotations)
        {
            for (int x = 1; x <= rotations % 32; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit31) : value.ClearBits(Bits.Bit31);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static uint BitRotR(this uint value, int rotations)
        {
            for (int x = 1; x <= rotations % 32; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit31) : value.ClearBits(Bits.Bit31);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static long BitRotR(this long value, int rotations)
        {
            for (int x = 1; x <= rotations % 64; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit63) : value.ClearBits(Bits.Bit63);
            }

            return value;
        }

        /// <summary>
        /// Performs rightwise bit-rotation for the specified number of rotations.
        /// </summary>
        /// <param name="value">Value used for bit-rotation.</param>
        /// <param name="rotations">Number of rotations to perform.</param>
        /// <returns>Value that has its bits rotated to the right the specified number of times.</returns>
        /// <remarks>
        /// Actual rotation direction is from a big-endian perspective - this is an artifact of the native
        /// .NET bit shift operators. As a result bits may actually appear to rotate left on little-endian
        /// architectures.
        /// </remarks>
        public static ulong BitRotR(this ulong value, int rotations)
        {
            for (int x = 1; x <= rotations % 64; x++)
            {
                bool loBitSet = value.CheckBits(Bits.Bit00);

                value >>= 1;

                value = loBitSet ? value.SetBits(Bits.Bit63) : value.ClearBits(Bits.Bit63);
            }

            return value;
        }

        #endregion
    }
}
