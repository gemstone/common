//******************************************************************************************************
//  Encoding7Bit.cs - Gbtc
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
//  03/16/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Gemstone;

/// <summary>
/// Defines 7-bit encoding functions.
/// </summary>
public static class Encoding7Bit
{
    /// <summary>
    /// Gets the number of bytes required to write the provided value.
    /// </summary>
    /// <param name="value">Value to measure</param>
    /// <returns>Number of bytes required to write the provided value.</returns>
    public static int GetSizeInt15(short value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot be negative");

        return value < 128 ? 1 : 2;
    }

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source byte stream.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int MeasureInt15(byte* stream) => stream[0] < 128 ? 1 : 2;

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source byte stream.</param>
    /// <param name="position">Start position in stream.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int Measure15(byte* stream, int position) => MeasureInt15(stream + position);

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source byte stream.</param>
    /// <param name="position">Start position in stream.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static int Measure15(byte[] stream, int position) => stream[position] < 128 ? 1 : 2;

    #region [ Write ]

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="value">Value to write. Cannot be negative.</param>
    /// <returns>Number of bytes required to store the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int WriteInt15(byte* stream, short value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot be negative");

        if (value < 128)
        {
            stream[0] = (byte)value;

            return 1;
        }

        stream[0] = (byte)(value | 128);
        stream[1] = (byte)(value >> 7);

        return 2;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static unsafe void WriteInt15(byte* stream, ref int position, short value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot be negative");

        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);
        stream[position + 1] = (byte)(value >> 7);
        position += 2;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static void WriteInt15(byte[] stream, ref int position, short value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot be negative");

        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);
        stream[position + 1] = (byte)(value >> 7);
        position += 2;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Method to write a byte.</param>
    /// <param name="value">Value to write.</param>
    public static void WriteInt15(Action<byte> stream, short value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot be negative");

        if (value < 128)
        {
            stream((byte)value);

            return;
        }

        stream((byte)(value | 128));
        stream((byte)(value >> 7));
    }

    #endregion

    #region [ Read ]

    /// <summary>
    /// Reads a 7-bit encoded short.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded short.</returns>
    public static unsafe short ReadInt15(byte* stream, ref int position)
    {
        stream += position;
        short value = stream[0];

        if (value < 128)
        {
            position += 1;

            return value;
        }

        value ^= (short)(stream[1] << 7);
        position += 2;

        return (short)(value ^ 0x80);
    }

    /// <summary>
    /// Reads a 7-bit encoded short.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded short.</returns>
    public static short ReadInt15(byte[] stream, ref int position)
    {
        short value = stream[position];

        if (value < 128)
        {
            position++;

            return value;
        }

        value ^= (short)(stream[position + 1] << 7);
        position += 2;

        return (short)(value ^ 0x80);
    }

    /// <summary>
    /// Reads a 7-bit encoded short.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>t7-bit encoded short.</returns>
    /// <remarks>
    /// This method will check for the end of the stream.
    /// </remarks>
    /// <exception cref="EndOfStreamException">Occurs if the end of the stream was reached.</exception>
    public static short ReadInt15(Stream stream)
    {
        short value = (short)stream.ReadByte();

        if (value < 128)
            return value;

        value ^= (short)(stream.ReadByte() << 7);

        return (short)(value ^ 0x80);
    }

    /// <summary>
    /// Reads a 7-bit encoded short.
    /// </summary>
    /// <param name="stream">Function used to read next byte.</param>
    /// <returns>7-bit encoded short.</returns>
    public static short ReadInt15(Func<byte> stream)
    {
        short value = stream();

        if (value < 128)
            return value;

        value ^= (short)(stream() << 7);

        return (short)(value ^ 0x80);
    }

    #endregion

    #region [ 32 bit ]

    /// <summary>
    /// Gets the number of bytes required to write the provided value.
    /// </summary>
    /// <param name="value">Value to measure.</param>
    /// <returns>Number of bytes required to write the provided value.</returns>
    public static int GetSize(uint value)
    {
        if (value < 128)
            return 1;

        if (value < 128 * 128)
            return 2;

        if (value < 128 * 128 * 128)
            return 3;

        if (value < 128 * 128 * 128 * 128)
            return 4;

        return 5;
    }

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int MeasureUInt32(byte* stream)
    {
        if (stream[0] < 128)
            return 1;

        if (stream[1] < 128)
            return 2;

        if (stream[2] < 128)
            return 3;

        if (stream[3] < 128)
            return 4;

        return 5;
    }

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int MeasureUInt32(byte* stream, int position) => MeasureUInt32(stream + position);

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static int MeasureUInt32(byte[] stream, int position)
    {
        if (stream[position + 0] < 128)
            return 1;

        if (stream[position + 1] < 128)
            return 2;

        if (stream[position + 2] < 128)
            return 3;

        if (stream[position + 3] < 128)
            return 4;

        return 5;
    }

    #region [ Write ]

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="value">Value to write.</param>
    /// <returns>Number of bytes required to store the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int Write(byte* stream, uint value)
    {
        if (value < 128)
        {
            stream[0] = (byte)value;

            return 1;
        }

        stream[0] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[1] = (byte)(value >> 7);

            return 2;
        }

        stream[1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[2] = (byte)(value >> 14);

            return 3;
        }

        stream[2] = (byte)((value >> 14) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[3] = (byte)(value >> 21);

            return 4;
        }

        stream[3] = (byte)((value >> 21) | 128);
        stream[4] = (byte)(value >> 28);

        return 5;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static unsafe void Write(byte* stream, ref int position, uint value)
    {
        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[position + 1] = (byte)(value >> 7);
            position += 2;

            return;
        }

        stream[position + 1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[position + 2] = (byte)(value >> 14);
            position += 3;

            return;
        }

        stream[position + 2] = (byte)((value >> 14) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[position + 3] = (byte)(value >> 21);
            position += 4;

            return;
        }

        stream[position + 3] = (byte)((value >> 21) | 128);
        stream[position + 4] = (byte)(value >> 28);
        position += 5;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static void Write(byte[] stream, ref int position, uint value)
    {
        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[position + 1] = (byte)(value >> 7);
            position += 2;

            return;
        }

        stream[position + 1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[position + 2] = (byte)(value >> 14);
            position += 3;

            return;
        }

        stream[position + 2] = (byte)((value >> 14) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[position + 3] = (byte)(value >> 21);
            position += 4;

            return;
        }

        stream[position + 3] = (byte)((value >> 21) | 128);
        stream[position + 4] = (byte)(value >> 28);
        position += 5;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Method to write a byte.</param>
    /// <param name="value">Value to write.</param>
    public static void Write(Action<byte> stream, uint value)
    {
        if (value < 128)
        {
            stream((byte)value);

            return;
        }

        stream((byte)(value | 128));

        if (value < 128 * 128)
        {
            stream((byte)(value >> 7));

            return;
        }

        stream((byte)((value >> 7) | 128));

        if (value < 128 * 128 * 128)
        {
            stream((byte)(value >> 14));

            return;
        }

        stream((byte)((value >> 14) | 128));

        if (value < 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> 21));

            return;
        }

        stream((byte)((value >> 21) | 128));
        stream((byte)(value >> 28));
    }

    #endregion

    #region [ Read ]

    /// <summary>
    /// Reads a 7-bit encoded uint.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded uint.</returns>
    public static unsafe uint ReadUInt32(byte* stream, ref int position)
    {
        stream += position;
        uint value = stream[0];

        if (value < 128)
        {
            position += 1;

            return value;
        }

        value ^= (uint)stream[1] << 7;

        if (value < 128 * 128)
        {
            position += 2;

            return value ^ 0x80;
        }

        value ^= (uint)stream[2] << 14;

        if (value < 128 * 128 * 128)
        {
            position += 3;

            return value ^ 0x4080;
        }

        value ^= (uint)stream[3] << 21;

        if (value < 128 * 128 * 128 * 128)
        {
            position += 4;

            return value ^ 0x204080;
        }

        value ^= ((uint)stream[4] << 28) ^ 0x10204080;
        position += 5;

        return value;
    }

    /// <summary>
    /// Reads a 7-bit encoded uint.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded uint.</returns>
    public static uint ReadUInt32(byte[] stream, ref int position)
    {
        int pos = position;
        uint value = stream[pos];

        if (value < 128)
        {
            position = pos + 1;

            return value;
        }

        value ^= (uint)stream[pos + 1] << 7;

        if (value < 128 * 128)
        {
            position = pos + 2;

            return value ^ 0x80;
        }

        value ^= (uint)stream[pos + 2] << 14;

        if (value < 128 * 128 * 128)
        {
            position = pos + 3;

            return value ^ 0x4080;
        }

        value ^= (uint)stream[pos + 3] << 21;

        if (value < 128 * 128 * 128 * 128)
        {
            position = pos + 4;

            return value ^ 0x204080;
        }

        value ^= ((uint)stream[pos + 4] << 28) ^ 0x10204080;
        position = pos + 5;

        return value;
    }

    /// <summary>
    /// Reads a 7-bit encoded uint.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>7-bit encoded uint.</returns>
    /// <remarks>
    /// This method will check for the end of the stream.
    /// </remarks>
    /// <exception cref="EndOfStreamException">Occurs if the end of the stream was reached.</exception>
    public static uint ReadUInt32(Stream stream)
    {
        uint value = (uint)stream.ReadByte();

        if (value < 128)
            return value;

        value ^= (uint)stream.ReadByte() << 7;

        if (value < 128 * 128)
            return value ^ 0x80;

        value ^= (uint)stream.ReadByte() << 14;

        if (value < 128 * 128 * 128)
            return value ^ 0x4080;

        value ^= (uint)stream.ReadByte() << 21;

        if (value < 128 * 128 * 128 * 128)
            return value ^ 0x204080;

        value ^= ((uint)stream.ReadByte() << 28) ^ 0x10204080;

        return value;
    }

    /// <summary>
    /// Reads a 7-bit encoded uint.
    /// </summary>
    /// <param name="stream">Function used to read next byte.</param>
    /// <returns>7-bit encoded uint.</returns>
    public static uint ReadUInt32(Func<byte> stream)
    {
        uint value = stream();

        if (value < 128)
            return value;

        value ^= (uint)stream() << 7;

        if (value < 128 * 128)
            return value ^ 0x80;

        value ^= (uint)stream() << 14;

        if (value < 128 * 128 * 128)
            return value ^ 0x4080;

        value ^= (uint)stream() << 21;

        if (value < 128 * 128 * 128 * 128)
            return value ^ 0x204080;

        value ^= ((uint)stream() << 28) ^ 0x10204080;

        return value;
    }

    #endregion

    #endregion

    #region [ 64 bit ]

    /// <summary>
    /// Gets the number of bytes required to write the provided value.
    /// </summary>
    /// <param name="value">Value to measure.</param>
    /// <returns>The number of bytes needed to store the provided value.</returns>
    public static int GetSize(ulong value)
    {
        if (value < 128)
            return 1;

        if (value < 128 * 128)
            return 2;

        if (value < 128 * 128 * 128)
            return 3;

        if (value < 128 * 128 * 128 * 128)
            return 4;

        if (value < 128L * 128 * 128 * 128 * 128)
            return 5;

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
            return 6;

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
            return 7;

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
            return 8;

        return 9;
    }

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int MeasureUInt64(byte* stream)
    {
        if (stream[0] < 128)
            return 1;

        if (stream[1] < 128)
            return 2;

        if (stream[2] < 128)
            return 3;

        if (stream[3] < 128)
            return 4;

        if (stream[4] < 128)
            return 5;

        if (stream[5] < 128)
            return 6;

        if (stream[6] < 128)
            return 7;

        if (stream[7] < 128)
            return 8;

        return 9;
    }

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static unsafe int MeasureUInt64(byte* stream, int position) => MeasureUInt64(stream + position);

    /// <summary>
    /// Gets the number of bytes for the supplied value in the stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>Number of bytes for the supplied value in the stream.</returns>
    public static int MeasureUInt64(byte[] stream, int position)
    {
        if (stream[position + 0] < 128)
            return 1;

        if (stream[position + 1] < 128)
            return 2;

        if (stream[position + 2] < 128)
            return 3;

        if (stream[position + 3] < 128)
            return 4;

        if (stream[position + 4] < 128)
            return 5;

        if (stream[position + 5] < 128)
            return 6;

        if (stream[position + 6] < 128)
            return 7;

        if (stream[position + 7] < 128)
            return 8;

        return 9;
    }

    #region [ Write ]

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="value">Value to write.</param>
    /// <returns>Number of bytes required to store the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int Write(byte* stream, ulong value)
    {
        if (value < 128)
        {
            stream[0] = (byte)value;

            return 1;
        }

        stream[0] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[1] = (byte)(value >> 7);

            return 2;
        }

        stream[1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[2] = (byte)(value >> (7 + 7));

            return 3;
        }

        stream[2] = (byte)((value >> (7 + 7)) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[3] = (byte)(value >> (7 + 7 + 7));

            return 4;
        }

        stream[3] = (byte)((value >> (7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            stream[4] = (byte)(value >> (7 + 7 + 7 + 7));

            return 5;
        }

        stream[4] = (byte)((value >> (7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            stream[5] = (byte)(value >> (7 + 7 + 7 + 7 + 7));

            return 6;
        }

        stream[5] = (byte)((value >> (7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[6] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7));

            return 7;
        }

        stream[6] = (byte)((value >> (7 + 7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7));

            return 8;
        }

        stream[7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7) | 128);
        stream[8] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7));

        return 9;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static unsafe void Write(byte* stream, ref int position, ulong value)
    {
        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[position + 1] = (byte)(value >> 7);
            position += 2;

            return;
        }

        stream[position + 1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[position + 2] = (byte)(value >> (7 + 7));
            position += 3;

            return;
        }

        stream[position + 2] = (byte)((value >> (7 + 7)) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[position + 3] = (byte)(value >> (7 + 7 + 7));
            position += 4;

            return;
        }

        stream[position + 3] = (byte)((value >> (7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            stream[position + 4] = (byte)(value >> (7 + 7 + 7 + 7));
            position += 5;

            return;
        }

        stream[position + 4] = (byte)((value >> (7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 5] = (byte)(value >> (7 + 7 + 7 + 7 + 7));
            position += 6;

            return;
        }

        stream[position + 5] = (byte)((value >> (7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 6] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7));
            position += 7;

            return;
        }

        stream[position + 6] = (byte)((value >> (7 + 7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7));
            position += 8;

            return;
        }

        stream[position + 7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7) | 128);
        stream[position + 8] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7));
        position += 9;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.
    /// This field will be updated when the function returns.</param>
    /// <param name="value">Value to write.</param>
    public static void Write(byte[] stream, ref int position, ulong value)
    {
        if (value < 128)
        {
            stream[position] = (byte)value;
            position += 1;

            return;
        }

        stream[position] = (byte)(value | 128);

        if (value < 128 * 128)
        {
            stream[position + 1] = (byte)(value >> 7);
            position += 2;

            return;
        }

        stream[position + 1] = (byte)((value >> 7) | 128);

        if (value < 128 * 128 * 128)
        {
            stream[position + 2] = (byte)(value >> (7 + 7));
            position += 3;

            return;
        }

        stream[position + 2] = (byte)((value >> (7 + 7)) | 128);

        if (value < 128 * 128 * 128 * 128)
        {
            stream[position + 3] = (byte)(value >> (7 + 7 + 7));
            position += 4;

            return;
        }

        stream[position + 3] = (byte)((value >> (7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            stream[position + 4] = (byte)(value >> (7 + 7 + 7 + 7));
            position += 5;

            return;
        }

        stream[position + 4] = (byte)((value >> (7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 5] = (byte)(value >> (7 + 7 + 7 + 7 + 7));
            position += 6;

            return;
        }

        stream[position + 5] = (byte)((value >> (7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 6] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7));
            position += 7;

            return;
        }

        stream[position + 6] = (byte)((value >> (7 + 7 + 7 + 7 + 7 + 7)) | 128);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream[position + 7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7));
            position += 8;

            return;
        }

        stream[position + 7] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7) | 128);
        stream[position + 8] = (byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7));
        position += 9;
    }

    /// <summary>
    /// Writes the 7-bit encoded value to the provided stream.
    /// </summary>
    /// <param name="stream">Method to write a byte.</param>
    /// <param name="value">Value to write.</param>
    public static void Write(Action<byte> stream, ulong value)
    {
        if (value < 128)
        {
            stream((byte)value);

            return;
        }

        stream((byte)(value | 128));

        if (value < 128 * 128)
        {
            stream((byte)(value >> 7));

            return;
        }

        stream((byte)((value >> 7) | 128));

        if (value < 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7)));

            return;
        }

        stream((byte)((value >> (7 + 7)) | 128));

        if (value < 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7 + 7)));

            return;
        }

        stream((byte)((value >> (7 + 7 + 7)) | 128));

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7 + 7 + 7)));

            return;
        }

        stream((byte)((value >> (7 + 7 + 7 + 7)) | 128));

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7 + 7 + 7 + 7)));

            return;
        }

        stream((byte)((value >> (7 + 7 + 7 + 7 + 7)) | 128));

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7 + 7 + 7 + 7 + 7)));

            return;
        }

        stream((byte)((value >> (7 + 7 + 7 + 7 + 7 + 7)) | 128));

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            stream((byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7)));

            return;
        }

        stream((byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7) | 128));
        stream((byte)(value >> (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7)));
    }

    #endregion

    #region [ Read ]

    /// <summary>
    /// Reads a 7-bit encoded ulong.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded ulong.</returns>
    public static unsafe ulong ReadUInt64(byte* stream, ref int position)
    {
        stream += position;
        ulong value = stream[0];

        if (value < 128)
        {
            position += 1;

            return value;
        }

        value ^= (ulong)stream[1] << 7;

        if (value < 128 * 128)
        {
            position += 2;

            return value ^ 0x80;
        }

        value ^= (ulong)stream[2] << (7 + 7);

        if (value < 128 * 128 * 128)
        {
            position += 3;

            return value ^ 0x4080;
        }

        value ^= (ulong)stream[3] << (7 + 7 + 7);

        if (value < 128 * 128 * 128 * 128)
        {
            position += 4;

            return value ^ 0x204080;
        }

        value ^= (ulong)stream[4] << (7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            position += 5;

            return value ^ 0x10204080L;
        }

        value ^= (ulong)stream[5] << (7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            position += 6;

            return value ^ 0x810204080L;
        }

        value ^= (ulong)stream[6] << (7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            position += 7;

            return value ^ 0x40810204080L;
        }

        value ^= (ulong)stream[7] << (7 + 7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            position += 8;

            return value ^ 0x2040810204080L;
        }

        value ^= (ulong)stream[8] << (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7);
        position += 9;

        return value ^ 0x102040810204080L;
    }

    /// <summary>
    /// Reads a 7-bit encoded ulong.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="position">Reference position in stream.  Position will be updated after reading.</param>
    /// <returns>7-bit encoded ulong.</returns>
    public static ulong ReadUInt64(byte[] stream, ref int position)
    {
        int pos = position;
        ulong value = stream[pos];

        if (value < 128)
        {
            position += 1;

            return value;
        }

        value ^= (ulong)stream[pos + 1] << 7;

        if (value < 128 * 128)
        {
            position += 2;

            return value ^ 0x80;
        }

        value ^= (ulong)stream[pos + 2] << (7 + 7);

        if (value < 128 * 128 * 128)
        {
            position += 3;

            return value ^ 0x4080;
        }

        value ^= (ulong)stream[pos + 3] << (7 + 7 + 7);

        if (value < 128 * 128 * 128 * 128)
        {
            position += 4;

            return value ^ 0x204080;
        }

        value ^= (ulong)stream[pos + 4] << (7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128)
        {
            position += 5;

            return value ^ 0x10204080L;
        }

        value ^= (ulong)stream[pos + 5] << (7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
        {
            position += 6;

            return value ^ 0x810204080L;
        }

        value ^= (ulong)stream[pos + 6] << (7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
        {
            position += 7;

            return value ^ 0x40810204080L;
        }

        value ^= (ulong)stream[pos + 7] << (7 + 7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
        {
            position += 8;

            return value ^ 0x2040810204080L;
        }

        value ^= (ulong)stream[pos + 8] << (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7);
        position += 9;

        return value ^ 0x102040810204080L;
    }

    /// <summary>
    /// Reads a 7-bit encoded ulong.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <returns>7-bit encoded ulong.</returns>
    /// <remarks>
    /// This method will check for the end of the stream.
    /// </remarks>
    /// <exception cref="EndOfStreamException">Occurs if the end of the stream was reached.</exception>
    public static ulong ReadUInt64(Stream stream)
    {
        ulong value = (ulong)stream.ReadByte();

        if (value < 128)
            return value;

        value ^= (ulong)stream.ReadByte() << 7;

        if (value < 128 * 128)
            return value ^ 0x80;

        value ^= (ulong)stream.ReadByte() << (7 + 7);

        if (value < 128 * 128 * 128)
            return value ^ 0x4080;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7);

        if (value < 128 * 128 * 128 * 128)
            return value ^ 0x204080;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128)
            return value ^ 0x10204080L;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x810204080L;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x40810204080L;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x2040810204080L;

        value ^= (ulong)stream.ReadByte() << (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7);

        return value ^ 0x102040810204080L;
    }

    /// <summary>
    /// Reads a 7-bit encoded ulong.
    /// </summary>
    /// <param name="stream">Function used to read next byte.</param>
    /// <returns>7-bit encoded ulong.</returns>
    public static ulong ReadUInt64(Func<byte> stream)
    {
        ulong value = stream();

        if (value < 128)
            return value;

        value ^= (ulong)stream() << 7;

        if (value < 128 * 128)
            return value ^ 0x80;

        value ^= (ulong)stream() << (7 + 7);

        if (value < 128 * 128 * 128)
            return value ^ 0x4080;

        value ^= (ulong)stream() << (7 + 7 + 7);

        if (value < 128 * 128 * 128 * 128)
            return value ^ 0x204080;

        value ^= (ulong)stream() << (7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128)
            return value ^ 0x10204080L;

        value ^= (ulong)stream() << (7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x810204080L;

        value ^= (ulong)stream() << (7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x40810204080L;

        value ^= (ulong)stream() << (7 + 7 + 7 + 7 + 7 + 7 + 7);

        if (value < 128L * 128 * 128 * 128 * 128 * 128 * 128 * 128)
            return value ^ 0x2040810204080L;

        value ^= (ulong)stream() << (7 + 7 + 7 + 7 + 7 + 7 + 7 + 7);

        return value ^ 0x102040810204080L;
    }

    #endregion

    #endregion
}