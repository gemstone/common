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

namespace Gemstone
{
    /// <summary>
    /// Defines functions related to 16-bit words, 32-bit double-words and 64-bit quad-words.
    /// </summary>
    public static class Word
    {
        /// <summary>
        /// Makes an unsigned word (UInt16) from two bytes.
        /// </summary>
        /// <param name="high">High byte.</param>
        /// <param name="low">Low byte.</param>
        /// <returns>An unsigned 16-bit word made from the two specified bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort MakeWord(byte high, byte low) => (ushort)(low + (high << 8));

        /// <summary>
        /// Makes an unsigned double-word (UInt32) from two unsigned words (UInt16).
        /// </summary>
        /// <param name="high">High word.</param>
        /// <param name="low">Low word.</param>
        /// <returns>An unsigned 32-bit double-word made from the two specified unsigned 16-bit words.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint MakeDoubleWord(ushort high, ushort low) => low + ((uint)high << 16);

        /// <summary>
        /// Makes an unsigned quad-word (UInt64) from two unsigned double-words (UInt32).
        /// </summary>
        /// <param name="high">High double-word.</param>
        /// <param name="low">Low double-word.</param>
        /// <returns>An unsigned 64-bit quad-word made from the two specified unsigned 32-bit double-words.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong MakeQuadWord(uint high, uint low) => low + ((ulong)high << 32);
    }
}
