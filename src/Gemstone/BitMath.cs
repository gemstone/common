﻿//******************************************************************************************************
//  BitMath.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  06/08/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//******************************************************************************************************

using System;
using System.Collections.Generic;

namespace Gemstone;

/// <summary>
/// Contains some random and useful functions.
/// </summary>
public static class BitMath
{
    #region [ Is Power Of Two ]

    /// <summary>
    /// Determines if the number is a power of 2.
    /// </summary>
    /// <param name="value">The value to check power of two properties.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is a power of 2; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <see pref="value"/> is less than zero</exception>
    public static bool IsPowerOfTwo(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Must be greater than or equal to zero");

        return IsPowerOfTwo((uint)value);
    }

    /// <summary>
    /// Determines if the number is a power of 2.
    /// </summary>
    /// <param name="value">The value to check power of two properties.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is a power of 2; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <see pref="value"/> is less than zero</exception>
    public static bool IsPowerOfTwo(long value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Must be greater than or equal to zero");

        return IsPowerOfTwo((ulong)value);
    }

    /// <summary>
    /// Determines if the number is a power of 2.
    /// </summary>
    /// <param name="value">The value to check power of two properties.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is a power of 2; otherwise, <c>false</c>.</returns>
    public static bool IsPowerOfTwo(uint value)
    {
        return value != 0 && (value & (value - 1)) == 0;
    }

    /// <summary>
    /// Determines if the number is a power of 2.
    /// </summary>
    /// <param name="value">The value to check power of two properties.</param>
    /// <returns><c>true</c> if <paramref name="value"/> is a power of 2; otherwise, <c>false</c>.</returns>
    public static bool IsPowerOfTwo(ulong value)
    {
        return value != 0 && (value & (value - 1)) == 0;
    }

    #endregion

    #region [ Count Bits ]

    /// <summary>
    /// Counts the number of bits that are set
    /// </summary>
    /// <param name="value">Value with bits to count.</param>
    /// <returns>Count of the number of bits set in <paramref name="value"/>.</returns>
    public static int CountBitsSet(uint value)
    {
        uint count;

        for (count = 0; value > 0; value >>= 1)
            count += value & 1;

        return (int)count;
    }

    /// <summary>
    /// Counts the number of bits that are set
    /// </summary>
    /// <param name="value">Value with bits to count.</param>
    /// <returns>Count of the number of bits set in <paramref name="value"/>.</returns>
    public static int CountBitsSet(ulong value)
    {
        ulong count;

        for (count = 0; value > 0; value >>= 1)
            count += value & 1;

        return (int)count;
    }

    /// <summary>
    /// Counts the number of bits that are not set
    /// </summary>
    /// <param name="value">Value with bits to count.</param>
    /// <returns>Count of the number of bits set in <paramref name="value"/>.</returns>
    public static int CountBitsCleared(uint value)
    {
        return CountBitsSet(~value);
    }

    /// <summary>
    /// Counts the number of bits that are not set
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Count of the number of bits set in <paramref name="value"/>.</returns>
    public static int CountBitsCleared(ulong value)
    {
        return CountBitsSet(~value);
    }

    #endregion

    #region [ Round To Power Of Two ]

    /// <summary>
    /// Rounds a number up to the nearest power of 2.
    /// If the value is a power of two, the same value is returned.
    /// If the value is larger than the largest power of 2. It is rounded down.
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <returns><paramref name="value"/> rounded to the nearest power of 2.</returns>
    /// <remarks>
    /// Method based on a method found at: http://graphics.stanford.edu/~seander/bithacks.htm
    /// Subtitle: Round up to the next highest power of 2 
    /// </remarks>
    public static ulong RoundUpToNearestPowerOfTwo(ulong value)
    {
        if (value == 0)
            return 1;

        if (value > 1ul << 62)
            return 1ul << 63;

        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value |= value >> 32;
        value++;

        return value;
    }

    /// <summary>
    /// Rounds a number up to the nearest power of 2.
    /// If the value is a power of two, the same value is returned.
    /// If the value is larger than the largest power of 2. It is rounded down.
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <returns><paramref name="value"/> rounded to the nearest power of 2.</returns>
    /// <remarks>
    /// Method based on a method found at: http://graphics.stanford.edu/~seander/bithacks.htm
    /// Subtitle: Round up to the next highest power of 2 
    /// </remarks>
    public static uint RoundUpToNearestPowerOfTwo(uint value)
    {
        if (value == 0)
            return 1;

        if (value > 1u << 30)
            return 1u << 31;

        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;

        return value;
    }

    /// <summary>
    /// Rounds a number down to the nearest power of 2.
    /// If the value is a power of two, the same value is returned.
    /// If value is zero, one is returned.
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <returns><paramref name="value"/> rounded to the nearest power of 2.</returns>
    public static ulong RoundDownToNearestPowerOfTwo(ulong value)
    {
        if (value == 0ul)
            return 1;

        return (1ul << 63) >> CountLeadingZeros(value);
    }

    /// <summary>
    /// Rounds a number down to the nearest power of 2.
    /// If the value is a power of two, the same value is returned.
    /// If value is zero, 1 is returned.
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <returns><paramref name="value"/> rounded to the nearest power of 2.</returns>
    public static uint RoundDownToNearestPowerOfTwo(uint value)
    {
        if (value == 0u)
            return 1;

        return (1u << 31) >> CountLeadingZeros(value);
    }

    #endregion

    /// <summary>
    /// Creates a bit mask for a number with the given number of bits.
    /// </summary>
    /// <param name="bitCount">Bit count.</param>
    /// <returns>Bit mask for a number with the given number of bits.</returns>
    public static ulong CreateBitMask(int bitCount)
    {
        if (bitCount == 0)
            return 0;

        return ulong.MaxValue >> (64 - bitCount);
    }

    #region [ Count Leading/Trailing Zeros ]

    /// <summary>
    /// Counts the number of consecutive 0's starting from the lowest bit working up.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 0's starting from the lowest bit working up.</returns>
    /// <remarks>
    /// Unfortunately, c# cannot call the cpu instruction ctz
    /// Example from http://en.wikipedia.org/wiki/Find_first_set
    /// </remarks>
    public static int CountTrailingZeros(uint value)
    {
        if (value == 0)
            return 32;

        int position = 0;

        if ((value & 0xffffu) == 0u)
        {
            value >>= 16;
            position += 16;
        }

        if ((value & 0xffu) == 0u)
        {
            value >>= 8;
            position += 8;
        }

        if ((value & 0xfu) == 0u)
        {
            value >>= 4;
            position += 4;
        }

        if ((value & 0x3u) == 0u)
        {
            value >>= 2;
            position += 2;
        }

        if ((value & 0x1u) == 0u)
            position += 1;

        return position;
    }

    /// <summary>
    /// Counts the number of consecutive 0's starting from the lowest bit working up.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 0's starting from the lowest bit working up.</returns>
    /// <remarks>
    /// Unfortunately, c# cannot call the cpu instruction ctz
    /// Example from http://en.wikipedia.org/wiki/Find_first_set
    /// </remarks>
    public static int CountTrailingZeros(ulong value)
    {
        if (value == 0)
            return 64;

        int position = 0;

        if ((value & 0xfffffffful) == 0ul)
        {
            value >>= 32;
            position += 32;
        }

        if ((value & 0xfffful) == 0ul)
        {
            value >>= 16;
            position += 16;
        }

        if ((value & 0xfful) == 0ul)
        {
            value >>= 8;
            position += 8;
        }

        if ((value & 0xful) == 0ul)
        {
            value >>= 4;
            position += 4;
        }

        if ((value & 0x3ul) == 0ul)
        {
            value >>= 2;
            position += 2;
        }

        if ((value & 0x1ul) == 0ul)
            position += 1;

        return position;
    }

    /// <summary>
    /// Counts the number of consecutive 0's starting from the highest bit working down.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 0's starting from the highest bit working down.</returns>
    /// <remarks>
    /// Unfortunately, c# cannot call the cpu instruction clz
    /// Example from http://en.wikipedia.org/wiki/Find_first_set
    /// </remarks>
    public static int CountLeadingZeros(uint value)
    {
        if (value == 0ul)
            return 32;

        int position = 0;

        if ((value & 0xFFFF0000u) == 0u)
        {
            value <<= 16;
            position += 16;
        }

        if ((value & 0xFF000000u) == 0u)
        {
            value <<= 8;
            position += 8;
        }

        if ((value & 0xF0000000) == 0u)
        {
            value <<= 4;
            position += 4;
        }

        if ((value & 0xC0000000) == 0u)
        {
            value <<= 2;
            position += 2;
        }

        if ((value & 0x80000000) == 0u)
            position += 1;

        return position;
    }

    /// <summary>
    /// Counts the number of consecutive 0's starting from the highest bit working down.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 0's starting from the highest bit working down.</returns>
    /// <remarks>
    /// Unfortunately, c# cannot call the cpu instruction clz
    /// Example from http://en.wikipedia.org/wiki/Find_first_set
    /// </remarks>
    public static int CountLeadingZeros(ulong value)
    {
        if (value == 0ul)
            return 64;

        int position = 0;

        if ((value & 0xFFFFFFFF00000000ul) == 0ul)
        {
            value <<= 32;
            position += 32;
        }

        if ((value & 0xFFFF000000000000ul) == 0ul)
        {
            value <<= 16;
            position += 16;
        }

        if ((value & 0xFF00000000000000ul) == 0ul)
        {
            value <<= 8;
            position += 8;
        }

        if ((value & 0xF000000000000000ul) == 0ul)
        {
            value <<= 4;
            position += 4;
        }

        if ((value & 0xC000000000000000ul) == 0ul)
        {
            value <<= 2;
            position += 2;
        }

        if ((value & 0x8000000000000000ul) == 0ul)
            position += 1;

        return position;
    }

    #endregion

    #region [ Count Leading/Trailing Ones ]

    /// <summary>
    /// Counts the number of consecutive 1's starting from the lowest bit working up.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 1's starting from the lowest bit working up.</returns>
    public static int CountTrailingOnes(uint value)
    {
        return CountTrailingZeros(~value);
    }

    /// <summary>
    /// Counts the number of consecutive 1's starting from the lowest bit working up.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 1's starting from the lowest bit working up.</returns>
    public static int CountTrailingOnes(ulong value)
    {
        return CountTrailingZeros(~value);
    }

    /// <summary>
    /// Counts the number of consecutive 1's starting from the highest bit working down.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 1's starting from the highest bit working down.</returns>
    public static int CountLeadingOnes(uint value)
    {
        return CountLeadingZeros(~value);
    }

    /// <summary>
    /// Counts the number of consecutive 1's starting from the highest bit working down.
    /// </summary>
    /// <param name="value">Target value for bit counting.</param>
    /// <returns>Number of consecutive 1's starting from the highest bit working down.</returns>
    public static int CountLeadingOnes(ulong value)
    {
        return CountLeadingZeros(~value);
    }

    #endregion

    /// <summary>
    /// Returns the bit position for every bit that is set in the provided value.
    /// Bit positions are defined as 0-63;
    /// </summary>
    /// <param name="value">Target value for bit check enumeration.</param>
    /// <returns>Enumeration of all set bit positions.</returns>
    public static IEnumerable<int> GetSetBitPositions(ulong value)
    {
        // Once value becomes zero, the remainder of the loop can be short-cut
        for (int x = 0; value != 0; x++, value >>= 1)
        {
            if ((value & 1) == 1)
                yield return x;
        }
    }

    /// <summary>
    /// Returns the bit position for every bit that is set in the provided value.
    /// Bit positions are defined as 0-31;
    /// </summary>
    /// <param name="value">Target value for bit check enumeration.</param>
    /// <returns>Enumeration of all set bit positions.</returns>
    public static IEnumerable<int> GetSetBitPositions(uint value)
    {
        // Once value becomes zero, the remainder of the loop can be short-cut
        for (int x = 0; value != 0; x++, value >>= 1)
        {
            if ((value & 1) == 1)
                yield return x;
        }
    }

    /// <summary>
    /// Returns the bit position for every bit that is cleared in the provided value.
    /// Bit positions are defined as 0-31;
    /// </summary>
    /// <param name="value">Target value for bit check enumeration.</param>
    /// <returns>Enumeration of all cleared bit positions.</returns>
    public static IEnumerable<int> GetClearedBitPositions(uint value)
    {
        return GetSetBitPositions(~value);
    }

    /// <summary>
    /// Returns the bit position for every bit that is cleared in the provided value.
    /// Bit positions are defined as 0-63;
    /// </summary>
    /// <param name="value">Target value for bit check enumeration.</param>
    /// <returns>Enumeration of all cleared bit positions.</returns>
    public static IEnumerable<int> GetClearedBitPositions(ulong value)
    {
        return GetSetBitPositions(~value);
    }
}
