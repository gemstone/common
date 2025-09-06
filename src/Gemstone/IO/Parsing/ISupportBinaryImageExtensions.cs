//******************************************************************************************************
//  ISupportBinaryImageExtensions.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  12/04/2008 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Gemstone.IO.Parsing;

/// <summary>
/// Defines extension functions related to <see cref="ISupportBinaryImage"/> implementations.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ISupportBinaryImageExtensions
{
    private static readonly ArrayPool<byte> s_pool = ArrayPool<byte>.Shared;

    /// <summary>
    /// Returns a binary image of an object that implements <see cref="ISupportBinaryImage"/>.
    /// </summary>
    /// <param name="imageSource"><see cref="ISupportBinaryImage"/> source.</param>
    /// <returns>A binary image of an object that implements <see cref="ISupportBinaryImage"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="imageSource"/> cannot be null.</exception>
    /// <remarks>
    /// This is a convenience method. It is often optimal to use <see cref="ISupportBinaryImage.GenerateBinaryImage"/>
    /// directly using a common buffer instead of always allocating new buffers.
    /// </remarks>
    public static byte[] BinaryImage(this ISupportBinaryImage imageSource)
    {
        ArgumentNullException.ThrowIfNull(imageSource);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return [];

        byte[] rented = s_pool.Rent(length);

        try
        {
            int written = GenerateIntoBuffer(imageSource, rented);

            if (written <= 0)
                return [];

            byte[] result = new byte[written];
            
            Buffer.BlockCopy(rented, 0, result, 0, written);
        
            return result;
        }
        finally
        {
            s_pool.Return(rented);
        }
    }

    /// <summary>
    /// Async version of <see cref="BinaryImage(ISupportBinaryImage)"/>.
    /// </summary>
    public static Task<byte[]> BinaryImageAsync(this ISupportBinaryImage imageSource, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(BinaryImage(imageSource));
    }

    /// <summary>
    /// Copies generated image to a stream using a pooled buffer.
    /// </summary>
    public static void CopyBinaryImageToStream(this ISupportBinaryImage imageSource, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return;

        byte[] rented = s_pool.Rent(length);
        
        try
        {
            int writeCount = GenerateIntoBuffer(imageSource, rented);
            
            if (writeCount > 0)
                stream.Write(rented, 0, writeCount);
        }
        finally
        {
            s_pool.Return(rented);
        }
    }

    /// <summary>
    /// Async copy of generated image to a stream using a pooled buffer.
    /// </summary>
    public static async Task CopyBinaryImageToStreamAsync(this ISupportBinaryImage imageSource, Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return;

        byte[] rented = s_pool.Rent(length);

        try
        {
            int writeCount = GenerateIntoBuffer(imageSource, rented);
        
            if (writeCount > 0)
                await stream.WriteAsync(rented.AsMemory(0, writeCount), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            s_pool.Return(rented);
        }
    }

    /// <summary>
    /// Parses up to <see cref="ISupportBinaryImage.BinaryLength"/> bytes from the stream to initialize the object.
    /// Returns the number of bytes parsed.
    /// </summary>
    public static int ParseBinaryImageFromStream(this ISupportBinaryImage imageSource, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return 0;

        byte[] rented = s_pool.Rent(length);
        
        try
        {
            int readCount = stream.Read(rented, 0, length);
            return readCount <= 0 ? 0 : ParseFromBuffer(imageSource, rented, readCount);
        }
        finally
        {
            s_pool.Return(rented);
        }
    }

    /// <summary>
    /// Async parse from the stream. If <paramref name="readExactly"/> is true,
    /// reads exactly <see cref="ISupportBinaryImage.BinaryLength"/> bytes or throws <see cref="EndOfStreamException"/>.
    /// Otherwise, reads up to that many bytes (mirrors sync behavior).
    /// </summary>
    public static async Task<int> ParseBinaryImageFromStreamAsync(this ISupportBinaryImage imageSource, Stream stream, bool readExactly = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;
        
        if (length <= 0)
            return 0;

        byte[] rented = s_pool.Rent(length);
        
        try
        {
            int readCount;

            if (!readExactly)
            {
                readCount = await stream.ReadAsync(rented.AsMemory(0, length), cancellationToken).ConfigureAwait(false);

                if (readCount <= 0)
                    return 0;
            }
            else
            {
                await stream.ReadExactlyAsync(rented.AsMemory(0, length), cancellationToken).ConfigureAwait(false);
                readCount = length;
            }

            return ParseFromBuffer(imageSource, rented, readCount);
        }
        finally
        {
            s_pool.Return(rented);
        }
    }

    // These helper functions pick the fastest path, e.g., span-aware if available

    private static int GenerateIntoBuffer(ISupportBinaryImage imageSource, byte[] rented)
    {
        // Prefer span path (no extra bounds math for startIndex)
        if (imageSource is ISupportBinaryImageSpan spanCapable)
            return spanCapable.GenerateBinaryImage(rented);

        // Legacy byte[] path
        return imageSource.GenerateBinaryImage(rented, 0);
    }

    private static int ParseFromBuffer(ISupportBinaryImage imageSource, byte[] rented, int readCount)
    {
        // Prefer span path
        if (imageSource is ISupportBinaryImageSpan spanCapable)
            return spanCapable.ParseBinaryImage(rented.AsSpan(0, readCount));

        // Legacy byte[] path
        return imageSource.ParseBinaryImage(rented, 0, readCount);
    }
}
