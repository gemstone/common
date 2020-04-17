//******************************************************************************************************
//  EndianOrder.cs - Gbtc
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
//  11/12/2004 - J. Ritchie Carroll
//       Initial version of source generated.
//  01/14/2005 - J. Ritchie Carroll
//       Added GetByte overloads, and To<Type> functions - changes reviewed by John Shugart.
//  08/03/2009 - Josh L. Patterson
//       Updated comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  10/27/2011 - J. Ritchie Carroll
//       Added a GetBytes<T> overload.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//   2/12/2014 - Steven E. Chisholm
//       Approximately a 6x improvement by simplifying primitive type access and using unsafe code.
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

using System;
using System.Runtime.CompilerServices;
using LE = Gemstone.LittleEndian;
using BE = Gemstone.BigEndian;

namespace Gemstone
{
    #region [ Enumerations ]

    /// <summary>
    /// Endian Byte Order Enumeration.
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        /// Big-endian byte order.
        /// </summary>
        BigEndian,
        /// <summary>
        /// Little-endian byte order.
        /// </summary>
        LittleEndian
    }

    #endregion

    /// <summary>
    /// Represents a big-endian byte order interoperability class.
    /// </summary>
    public class BigEndianOrder : EndianOrder
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="BigEndianOrder"/> class.
        /// </summary>
        public BigEndianOrder() : base(Endianness.BigEndian) { }

        /// <summary>
        /// Returns the default instance of the <see cref="BigEndianOrder"/> class.
        /// </summary>
        public static BigEndianOrder Default { get; } = new BigEndianOrder();
    }

    /// <summary>
    /// Represents a little-endian byte order interoperability class.
    /// </summary>
    public class LittleEndianOrder : EndianOrder
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="LittleEndianOrder"/> class.
        /// </summary>
        public LittleEndianOrder() : base(Endianness.LittleEndian) { }

        /// <summary>
        /// Returns the default instance of the <see cref="LittleEndianOrder"/> class.
        /// </summary>
        public static LittleEndianOrder Default { get; } = new LittleEndianOrder();
    }

    /// <summary>
    /// Represents a native-endian byte order interoperability class.
    /// </summary>
    public class NativeEndianOrder : EndianOrder
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NativeEndianOrder"/> class.
        /// </summary>
        public NativeEndianOrder() : base(NativeEndianness) { }

        /// <summary>
        /// Returns the default instance of the <see cref="NativeEndianOrder"/> class.
        /// </summary>
        public static NativeEndianOrder Default { get; } = new NativeEndianOrder();
    }

    /// <summary>
    /// Represents an endian byte order interoperability class.
    /// </summary>
    /// <remarks>
    /// Intel systems use little-endian byte order, other systems, such as Unix, use big-endian byte ordering.
    /// Little-endian ordering means bits are ordered such that the bit whose in-memory representation is right-most is the most-significant-bit in a byte.
    /// Big-endian ordering means bits are ordered such that the bit whose in-memory representation is left-most is the most-significant-bit in a byte.
    /// </remarks>
    public class EndianOrder
    {
        #region [ Constructors ]

        /// <summary>
        /// Constructs a new instance of the <see cref="EndianOrder"/> class.
        /// </summary>
        /// <param name="targetEndianness">Endianness parameter.</param>
        protected EndianOrder(Endianness targetEndianness) => TargetEndianness = targetEndianness;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Returns the target endian-order of this <see cref="EndianOrder"/> representation.
        /// </summary>
        public readonly Endianness TargetEndianness;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a <see cref="bool"/> value converted from one byte at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>true if the byte at startIndex in value is nonzero; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToBoolean(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToBoolean(value, startIndex) :
            BE.ToBoolean(value, startIndex);

        /// <summary>
        /// Returns a Unicode character converted from two bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A character formed by two bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ToChar(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToChar(value, startIndex) :
            BE.ToChar(value, startIndex);

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A double-precision floating point number formed by eight bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ToDouble(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToDouble(value, startIndex) :
            BE.ToDouble(value, startIndex);

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ToInt16(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToInt16(value, startIndex) :
            BE.ToInt16(value, startIndex);

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToInt32(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToInt32(value, startIndex) :
            BE.ToInt32(value, startIndex);

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ToInt64(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToInt64(value, startIndex) :
            BE.ToInt64(value, startIndex);

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A single-precision floating point number formed by four bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ToSingle(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToSingle(value, startIndex) :
            BE.ToSingle(value, startIndex);

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ToUInt16(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToUInt16(value, startIndex) :
            BE.ToUInt16(value, startIndex);

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ToUInt32(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToUInt32(value, startIndex) :
            BE.ToUInt32(value, startIndex);

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes, accounting for target endian-order, at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes (i.e., buffer containing binary image of value).</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToUInt64(byte[] value, int startIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.ToUInt64(value, startIndex) :
            BE.ToUInt64(value, startIndex);

        /// <summary>
        /// Returns the specified value as an array of bytes in the target endian-order.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An array of bytes with length 1.</returns>
        /// <typeparam name="T">Native value type to get bytes for.</typeparam>
        /// <exception cref="ArgumentException"><paramref name="value"/> type is not primitive.</exception>
        /// <exception cref="InvalidOperationException">Cannot get bytes for <paramref name="value"/> type.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes<T>(T value) where T : struct, IConvertible => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified <see cref="bool"/> value as an array of bytes in the target endian-order.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert.</param>
        /// <returns>An array of bytes with length 1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(bool value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified Unicode character value as an array of bytes in the target endian-order.
        /// </summary>
        /// <param name="value">The Unicode character value to convert.</param>
        /// <returns>An array of bytes with length 2.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(char value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes in the target endian-order.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 8.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(double value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 2.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(short value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(int value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 8.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(long value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes in the target endian-order.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(float value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 2.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(ushort value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(uint value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 8.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(ulong value) => TargetEndianness == Endianness.LittleEndian ?
            LE.GetBytes(value) :
            BE.GetBytes(value);

        /// <summary>
        /// Copies the specified primitive type value as an array of bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <typeparam name="T">Native value type to get bytes for.</typeparam>
        /// <exception cref="ArgumentException"><paramref name="value"/> type is not primitive.</exception>
        /// <exception cref="InvalidOperationException">Cannot get bytes for <paramref name="value"/> type.</exception>
        /// <returns>Length of bytes copied into array based on size of <typeparamref name="T"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes<T>(T value, byte[] destinationArray, int destinationIndex) where T : struct, IConvertible => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified <see cref="bool"/> value as an array of 1 byte in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(bool value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified Unicode character value as an array of 2 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The Unicode character value to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(char value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified double-precision floating point value as an array of 8 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(double value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 16-bit signed integer value as an array of 2 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(short value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 32-bit signed integer value as an array of 4 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(int value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 64-bit signed integer value as an array of 8 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(long value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified single-precision floating point value as an array of 4 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(float value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value as an array of 2 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(ushort value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value as an array of 4 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(uint value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value as an array of 8 bytes in the target endian-order to the destination array.
        /// </summary>
        /// <param name="value">The number to convert and copy.</param>
        /// <param name="destinationArray">The destination buffer.</param>
        /// <param name="destinationIndex">The byte offset into <paramref name="destinationArray"/>.</param>
        /// <returns>Length of bytes copied into array based on size of <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CopyBytes(ulong value, byte[] destinationArray, int destinationIndex) => TargetEndianness == Endianness.LittleEndian ?
            LE.CopyBytes(value, destinationArray, destinationIndex) :
            BE.CopyBytes(value, destinationArray, destinationIndex);

        #endregion

        #region [ Static ]

        /// <summary>
        /// Gets the native <see cref="Endianness"/> of the executing architecture.
        /// </summary>
        public readonly static Endianness NativeEndianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        /// <summary>
        /// Default instance of the <see cref="BigEndianOrder"/> conversion class.
        /// </summary>
        public readonly static EndianOrder BigEndian = BigEndianOrder.Default;

        /// <summary>
        /// Default instance of the <see cref="LittleEndianOrder"/> conversion class.
        /// </summary>
        public readonly static EndianOrder LittleEndian = LittleEndianOrder.Default;

        /// <summary>
        /// Default instance of the <see cref="NativeEndianOrder"/> conversion class.
        /// </summary>
        public readonly static EndianOrder NativeEndian = NativeEndianOrder.Default;

        #endregion
    }
}
