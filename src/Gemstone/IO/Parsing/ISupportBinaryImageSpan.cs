//******************************************************************************************************
//  ISupportBinaryImageSpan.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  09/05/2025 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Buffers;
using Gemstone.ArrayExtensions;

namespace Gemstone.IO.Parsing;

/// <summary>
/// Span-optimized binary image production/consumption with buffered I/O helpers.
/// Optional companion to <see cref="ISupportBinaryImage"/>; most callers only need to
/// implement the two span methods. Everything else has sensible defaults.
/// </summary>
public interface ISupportBinaryImageSpan : ISupportBinaryImage
{
    /// <summary>
    /// Writes this object's binary image to <paramref name="destination"/>.
    /// </summary>
    /// <returns>The number of bytes written.</returns>
    int GenerateBinaryImage(Span<byte> destination);

    /// <summary>
    /// Parses this object's state from <paramref name="source"/>.
    /// </summary>
    /// <returns>The number of bytes consumed.</returns>
    int ParseBinaryImage(ReadOnlySpan<byte> source);

    int ISupportBinaryImage.GenerateBinaryImage(byte[] buffer, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        int binaryLength = BinaryLength;

        buffer.ValidateParameters(startIndex, binaryLength);

        return GenerateBinaryImage(buffer.AsSpan(startIndex, binaryLength));
    }

    int ISupportBinaryImage.ParseBinaryImage(byte[] buffer, int startIndex, int length)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        buffer.ValidateParameters(startIndex, length);

        return ParseBinaryImage(buffer.AsSpan(startIndex, length));
    }

    /// <summary>
    /// Determines how many bytes are required to parse this instance from the given sequence.
    /// Default assumes fixed-size and returns <see cref="ISupportBinaryImage.BinaryLength"/>.
    /// Override for variable-length formats (peek a header, etc.).
    /// </summary>
    int GetRequiredLength(ReadOnlySequence<byte> source)
    {
        return BinaryLength;
    }

    /// <summary>
    /// Attempts to write this object's binary image to the specified <paramref name="destination"/> span.
    /// </summary>
    /// <param name="destination">The span to which the binary image will be written.</param>
    /// <param name="written">When this method returns, contains the number of bytes written to <paramref name="destination"/>.</param>
    /// <returns>
    /// <c>true</c> if the binary image was successfully written to <paramref name="destination"/>; 
    /// otherwise, <c>false</c> if the <paramref name="destination"/> span is too small.
    /// </returns>
    bool TryGenerateBinaryImage(Span<byte> destination, out int written)
    {
        written = 0;
        
        if (destination.Length < BinaryLength)
            return false;

        written = GenerateBinaryImage(destination[..BinaryLength]);

        return true;
    }

    /// <summary>
    /// Attempts to parse this object's state from the specified <paramref name="source"/> span.
    /// </summary>
    /// <param name="source">The span from which the binary image will be parsed.</param>
    /// <param name="consumed">
    /// When this method returns, contains the number of bytes consumed from <paramref name="source"/> 
    /// if the parsing was successful; otherwise, <c>0</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the binary image was successfully parsed from <paramref name="source"/>; 
    /// otherwise, <c>false</c> if the <paramref name="source"/> span is too small.
    /// </returns>
    bool TryParseBinaryImage(ReadOnlySpan<byte> source, out int consumed)
    {
        consumed = 0;
    
        if (source.Length < BinaryLength)
            return false;

        consumed = ParseBinaryImage(source[..BinaryLength]);

        return true;
    }

    /// <summary>
    /// Writes this object's binary image to the specified <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The <see cref="IBufferWriter{T}"/> to which the binary image will be written.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of bytes written exceeds the available buffer space.</exception>
    void WriteBinaryImage(IBufferWriter<byte> writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        int need = BinaryLength;

        if (need <= 0)
            return;

        Span<byte> dest = writer.GetSpan(need);
        int written = GenerateBinaryImage(dest);

        if ((uint)written > (uint)dest.Length)
            throw new InvalidOperationException($"Generated {written} bytes, buffer had {dest.Length}.");

        writer.Advance(written);
    }

    /// <summary>
    /// Parses from a <see cref="ReadOnlySequence{T}"/> and returns bytes consumed.
    /// </summary>
    /// <param name="source">The source sequence from which to parse.</param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <remarks>
    /// Default: fixed-size fast paths, pooled copy only when necessary.
    /// Override for variable-length formats to avoid copies entirely.
    /// </remarks>
    int ParseBinaryImage(ReadOnlySequence<byte> source, bool clearArray = false)
    {
        int need = GetRequiredLength(source);

        if (need <= 0)
            return 0;

        if (source.Length < need)
            return 0; // not enough data yet

        // Single segment? Parse straight from the span.
        if (source.IsSingleSegment)
            return ParseBinaryImage(source.FirstSpan[..need]);

        // Multi-segment; if first span is enough, avoid a copy.
        ReadOnlySpan<byte> first = source.FirstSpan;
        
        if (first.Length >= need)
            return ParseBinaryImage(first[..need]);

        // Otherwise copy exactly what we need into a pooled buffer.
        byte[] rented = ArrayPool<byte>.Shared.Rent(need);
        
        try
        {
            source.Slice(0, need).CopyTo(rented);
            return ParseBinaryImage(rented.AsSpan(0, need));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented, clearArray);
        }
    }

    /// <summary>
    /// Parses from a streaming reader; advances by bytes consumed.
    /// Implemented in terms of <see cref="ParseBinaryImage(ReadOnlySequence{byte}, bool)"/>.
    /// </summary>
    bool TryParse(ref SequenceReader<byte> reader)
    {
        int need = GetRequiredLength(reader.Sequence.Slice(reader.Position));
        
        if (need < 0)
            return false;

        if (need == 0)
            return true; // nothing to consume

        if (reader.Remaining < need)
            return false;

        // Slice exactly the bytes we need from the current cursor.
        ReadOnlySequence<byte> slice = reader.Sequence.Slice(reader.Position, need);

        int consumed = ParseBinaryImage(slice);

        if (consumed <= 0 || consumed > need)
            return false;

        reader.Advance(consumed);

        return true;
    }
}
