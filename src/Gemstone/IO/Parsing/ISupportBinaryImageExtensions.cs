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
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <returns>A binary image of an object that implements <see cref="ISupportBinaryImage"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="imageSource"/> cannot be null.</exception>
    /// <remarks>
    /// This is a convenience method. It is often optimal to use <see cref="ISupportBinaryImage.GenerateBinaryImage"/>
    /// directly using a common buffer instead of always allocating new buffers.
    /// </remarks>
    public static byte[] BinaryImage(this ISupportBinaryImage imageSource, bool clearArray = false)
    {
        ArgumentNullException.ThrowIfNull(imageSource);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return [];

        byte[] buffer = s_pool.Rent(length);

        try
        {
            int written = imageSource.GenerateIntoBuffer(buffer);

            if (written <= 0)
                return [];

            byte[] result = new byte[written];
            
            Buffer.BlockCopy(buffer, 0, result, 0, written);
        
            return result;
        }
        finally
        {
            s_pool.Return(buffer, clearArray);
        }
    }

    /// <summary>
    /// Asynchronously returns a binary image of an object that implements <see cref="ISupportBinaryImage"/>.
    /// </summary>
    /// <param name="imageSource">The <see cref="ISupportBinaryImage"/> source from which the binary image is generated.</param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the binary image of the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="imageSource"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method provides an asynchronous alternative to <see cref="BinaryImage"/>. It is often optimal to use
    /// <see cref="ISupportBinaryImage.GenerateBinaryImage"/> directly with a common buffer instead of always
    /// allocating new buffers.
    /// </remarks>
    public static Task<byte[]> BinaryImageAsync(this ISupportBinaryImage imageSource, bool clearArray = false, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(imageSource.BinaryImage(clearArray));
    }

    /// <summary>
    /// Copies the binary image of the specified <see cref="ISupportBinaryImage"/> to the provided <see cref="Stream"/>.
    /// </summary>
    /// <param name="imageSource">The source object implementing <see cref="ISupportBinaryImage"/> whose binary image will be copied.</param>
    /// <param name="stream">The <see cref="Stream"/> to which the binary image will be written.</param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageSource"/> or <paramref name="stream"/> is <c>null</c>.</exception>
    public static void CopyBinaryImageToStream(this ISupportBinaryImage imageSource, Stream stream, bool clearArray = false)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return;

        byte[] buffer = s_pool.Rent(length);
        
        try
        {
            int writeCount = imageSource.GenerateIntoBuffer(buffer);
            
            if (writeCount > 0)
                stream.Write(buffer, 0, writeCount);
        }
        finally
        {
            s_pool.Return(buffer, clearArray);
        }
    }

    /// <summary>
    /// Asynchronously copies the binary image of an object that implements <see cref="ISupportBinaryImage"/> to the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="imageSource">The <see cref="ISupportBinaryImage"/> source whose binary image will be copied.</param>
    /// <param name="stream">The <see cref="Stream"/> to which the binary image will be written.</param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageSource"/> or <paramref name="stream"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method uses a buffer rented from a shared <see cref="ArrayPool{T}"/> to optimize memory usage.
    /// The buffer is returned to the pool after the operation completes.
    /// </remarks>
    public static async Task CopyBinaryImageToStreamAsync(this ISupportBinaryImage imageSource, Stream stream, bool clearArray = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return;

        byte[] buffer = s_pool.Rent(length);

        try
        {
            int writeCount = imageSource.GenerateIntoBuffer(buffer);

            if (writeCount > 0)
                await stream.WriteAsync(buffer.AsMemory(0, writeCount), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            s_pool.Return(buffer, clearArray);
        }
    }

    /// <summary>
    /// Parses a binary image from the specified <see cref="Stream"/> into an object that implements <see cref="ISupportBinaryImage"/>.
    /// </summary>
    /// <param name="imageSource">The <see cref="ISupportBinaryImage"/> instance into which the binary image will be parsed.</param>
    /// <param name="stream">The <see cref="Stream"/> from which the binary image will be read.</param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <returns>The number of bytes successfully read and parsed from the stream.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageSource"/> or <paramref name="stream"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method reads a binary image from the provided <see cref="Stream"/> and parses it into the specified
    /// <see cref="ISupportBinaryImage"/> instance. It uses a shared buffer pool to minimize memory allocations.
    /// </remarks>
    public static int ParseBinaryImageFromStream(this ISupportBinaryImage imageSource, Stream stream, bool clearArray = false)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;

        if (length <= 0)
            return 0;

        byte[] buffer = s_pool.Rent(length);
        
        try
        {
            int readCount = stream.Read(buffer, 0, length);
            return readCount <= 0 ? 0 : imageSource.ParseFromBuffer(buffer, readCount);
        }
        finally
        {
            s_pool.Return(buffer, clearArray);
        }
    }

    /// <summary>
    /// Asynchronously parses a binary image from the specified <see cref="Stream"/> into an object that implements <see cref="ISupportBinaryImage"/>.
    /// </summary>
    /// <param name="imageSource">The <see cref="ISupportBinaryImage"/> instance into which the binary image will be parsed.</param>
    /// <param name="stream">The <see cref="Stream"/> from which the binary image will be read.</param>
    /// <param name="readExactly">
    /// Specifies whether the method should read exactly the number of bytes required to
    /// fill the binary image. If <c>true</c>, the method will ensure the exact number of
    /// bytes is read; otherwise, it reads as many bytes as are available.
    /// </param>
    /// <param name="clearArray">
    /// Clears any rented buffers of their contents so that a subsequent consumer will not
    /// see the previous consumer's content. If <c>false</c>, default, the array's contents
    /// are left unchanged.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of bytes successfully parsed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageSource"/> or <paramref name="stream"/> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    /// <remarks>
    /// This method uses a buffer rented from a shared <see cref="ArrayPool{T}"/> to read the binary image.
    /// The buffer is returned to the pool after the operation completes.
    /// </remarks>
    public static async Task<int> ParseBinaryImageFromStreamAsync(this ISupportBinaryImage imageSource, Stream stream, bool readExactly = false, bool clearArray = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageSource);
        ArgumentNullException.ThrowIfNull(stream);

        int length = imageSource.BinaryLength;
        
        if (length <= 0)
            return 0;

        byte[] buffer = s_pool.Rent(length);
        
        try
        {
            int readCount;

            if (!readExactly)
            {
                readCount = await stream.ReadAsync(buffer.AsMemory(0, length), cancellationToken).ConfigureAwait(false);

                if (readCount <= 0)
                    return 0;
            }
            else
            {
                await stream.ReadExactlyAsync(buffer.AsMemory(0, length), cancellationToken).ConfigureAwait(false);
                readCount = length;
            }

            return imageSource.ParseFromBuffer(buffer, readCount);
        }
        finally
        {
            s_pool.Return(buffer, clearArray);
        }
    }

    // These helper functions pick the fastest path, e.g., span-aware if available

    private static int GenerateIntoBuffer(this ISupportBinaryImage imageSource, byte[] buffer)
    {
        // Prefer span path (no extra bounds math for startIndex)
        if (imageSource is ISupportBinaryImageSpan spanCapable)
            return spanCapable.GenerateBinaryImage(buffer);

        // Legacy byte[] path
        return imageSource.GenerateBinaryImage(buffer, 0);
    }

    private static int ParseFromBuffer(this ISupportBinaryImage imageSource, byte[] buffer, int readCount)
    {
        // Prefer span path
        if (imageSource is ISupportBinaryImageSpan spanCapable)
            return spanCapable.ParseBinaryImage(buffer.AsSpan(0, readCount));

        // Legacy byte[] path
        return imageSource.ParseBinaryImage(buffer, 0, readCount);
    }
}
