//******************************************************************************************************
//  ByteEncoding.cs - Gbtc
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

// Ignore Spelling: Endian

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
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Gemstone.BitExtensions;

namespace Gemstone
{
    /// <summary>
    /// Defines a set of methods used to convert byte buffers to and from user presentable data formats.
    /// </summary>
    public abstract class ByteEncoding
    {
        #region [ Members ]

        // Nested Types
        #region [ Hexadecimal Encoding Class ]

        /// <summary>
        /// Handles conversion of byte buffers to and from a hexadecimal data format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class HexadecimalEncoding : ByteEncoding
        {
            internal HexadecimalEncoding()
            {
                // This class is meant for internal instantiation only.
            }

            /// <summary>Decodes given string back into a byte buffer.</summary>
            /// <param name="hexData">Encoded hexadecimal data string to decode.</param>
            /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
            /// <returns>Decoded bytes.</returns>
            public override byte[] GetBytes(string hexData, char spacingCharacter)
            {
                if (string.IsNullOrEmpty(hexData))
                    throw new ArgumentNullException(nameof(hexData), "Input string cannot be null or empty");

                // Removes spacing characters, if needed.
                hexData = hexData.Trim();

                if (spacingCharacter != NoSpacing)
                    hexData = hexData.Replace(spacingCharacter.ToString(), "");

                // Processes the string only if it has data in hex format (Example: 48 65 6C 6C 21).
                if (Regex.Matches(hexData, "[^a-fA-F0-9]").Count == 0)
                {
                    // Trims the end of the string to discard any additional characters, if present in the string,
                    // that would prevent the string from being a hex encoded string.
                    // Note: Requires that each character be represented by its 2 character hex value.
                    hexData = hexData[..^(hexData.Length % 2)];

                    byte[] bytes = new byte[hexData.Length / 2];
                    int index = 0;

                    for (int i = 0; i <= hexData.Length - 1; i += 2)
                    {
                        bytes[index] = Convert.ToByte(hexData.Substring(i, 2), 16);
                        index++;
                    }

                    return bytes;
                }

                throw new ArgumentException("Input string is not a valid hex encoded string - invalid characters encountered", nameof(hexData));
            }

            /// <summary>Encodes given buffer into a user presentable representation.</summary>
            /// <param name="bytes">Bytes to encode.</param>
            /// <param name="offset">Offset into buffer to begin encoding.</param>
            /// <param name="length">Length of buffer to encode.</param>
            /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
            /// <returns>String of encoded bytes.</returns>
            public override string GetString(byte[] bytes, int offset, int length, char spacingCharacter) =>
                BytesToString(bytes, offset, length, spacingCharacter, "X2");
        }

        #endregion

        #region [ Decimal Encoding Class ]

        /// <summary>
        /// Handles conversion of byte buffers to and from a decimal data format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class DecimalEncoding : ByteEncoding
        {
            internal DecimalEncoding()
            {
                // This class is meant for internal instantiation only.
            }

            /// <summary>Decodes given string back into a byte buffer.</summary>
            /// <param name="decData">Encoded decimal data string to decode.</param>
            /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
            /// <returns>Decoded bytes.</returns>
            public override byte[] GetBytes(string decData, char spacingCharacter)
            {
                if (string.IsNullOrEmpty(decData))
                    throw new ArgumentNullException(nameof(decData), "Input string cannot be null or empty");

                // Removes spacing characters, if needed.
                decData = decData.Trim();

                if (spacingCharacter != NoSpacing)
                    decData = decData.Replace(spacingCharacter.ToString(), "");

                // Processes the string only if it has data in decimal format (Example: 072 101 108 108 033).
                if (Regex.Matches(decData, "[^0-9]").Count == 0)
                {
                    // Trims the end of the string to discard any additional characters, if present in the
                    // string, that would prevent the string from being an integer encoded string.
                    // Note: Requires that each character be represented by its 3 character decimal value.
                    decData = decData[..^(decData.Length % 3)];

                    byte[] bytes = new byte[decData.Length / 3];
                    int index = 0;

                    for (int i = 0; i <= decData.Length - 1; i += 3)
                    {
                        bytes[index] = Convert.ToByte(decData.Substring(i, 3), 10);
                        index++;
                    }

                    return bytes;
                }

                throw new ArgumentException("Input string is not a valid decimal encoded string - invalid characters encountered", nameof(decData));
            }

            /// <summary>Encodes given buffer into a user presentable representation.</summary>
            /// <param name="bytes">Bytes to encode.</param>
            /// <param name="offset">Offset into buffer to begin encoding.</param>
            /// <param name="length">Length of buffer to encode.</param>
            /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
            /// <returns>String of encoded bytes.</returns>
            public override string GetString(byte[] bytes, int offset, int length, char spacingCharacter) =>
                BytesToString(bytes, offset, length, spacingCharacter, "D3");
        }

        #endregion

        #region [ Binary Encoding Class ]

        /// <summary>
        /// Handles conversion of byte buffers to and from a binary (i.e., 0 and 1's) data format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class BinaryEncoding : ByteEncoding
        {
            private string[]? m_byteImages;
            private readonly bool m_reverse;

            // This class is meant for internal instantiation only.
            internal BinaryEncoding(Endianness targetEndianness)
            {
                if (targetEndianness == Endianness.BigEndian)
                {
                    // If OS is little endian and we want big endian, this reverses the bit order.
                    m_reverse = BitConverter.IsLittleEndian;
                }
                else
                {
                    // If OS is little endian and we want little endian, this keeps OS bit order.
                    m_reverse = !BitConverter.IsLittleEndian;
                }
            }

            /// <summary>Decodes given string back into a byte buffer.</summary>
            /// <param name="binaryData">Encoded binary data string to decode.</param>
            /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
            /// <returns>Decoded bytes.</returns>
            public override byte[] GetBytes(string binaryData, char spacingCharacter)
            {
                if (string.IsNullOrEmpty(binaryData))
                    throw new ArgumentNullException(nameof(binaryData), "Input string cannot be null or empty");

                // Removes spacing characters, if needed.
                binaryData = binaryData.Trim();

                if (spacingCharacter != NoSpacing)
                    binaryData = binaryData.Replace(spacingCharacter.ToString(), "");

                // Processes the string only if it has data in binary format (Example: 01010110 1010101).
                if (Regex.Matches(binaryData, "[^0-1]").Count == 0)
                {
                    // Trims the end of the string to discard any additional characters, if present in the
                    // string, that would prevent the string from being a binary encoded string.
                    // Note: Requires each character be represented by its 8 character binary value.
                    binaryData = binaryData[..^(binaryData.Length % 8)];

                    byte[] bytes = new byte[binaryData.Length / 8];
                    int index = 0;

                    for (int i = 0; i <= binaryData.Length - 1; i += 8)
                    {
                        bytes[index] = (byte)Bits.Nil;

                        if (m_reverse)
                        {
                            if (binaryData[i + 7] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit00);

                            if (binaryData[i + 6] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit01);

                            if (binaryData[i + 5] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit02);

                            if (binaryData[i + 4] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit03);

                            if (binaryData[i + 3] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit04);

                            if (binaryData[i + 2] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit05);

                            if (binaryData[i + 1] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit06);

                            if (binaryData[i + 0] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit07);
                        }
                        else
                        {
                            if (binaryData[i + 0] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit00);

                            if (binaryData[i + 1] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit01);

                            if (binaryData[i + 2] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit02);

                            if (binaryData[i + 3] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit03);

                            if (binaryData[i + 4] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit04);

                            if (binaryData[i + 5] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit05);

                            if (binaryData[i + 6] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit06);

                            if (binaryData[i + 7] == '1')
                                bytes[index] = bytes[index].SetBits(Bits.Bit07);
                        }

                        index++;
                    }

                    return bytes;
                }

                throw new ArgumentException("Input string is not a valid binary encoded string - invalid characters encountered", nameof(binaryData));
            }

            /// <summary>Encodes given buffer into a user presentable representation.</summary>
            /// <param name="bytes">Bytes to encode.</param>
            /// <param name="offset">Offset into buffer to begin encoding.</param>
            /// <param name="length">Length of buffer to encode.</param>
            /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
            /// <returns>String of encoded bytes.</returns>
            public override string GetString(byte[] bytes, int offset, int length, char spacingCharacter)
            {
                if (bytes is null)
                    throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

                // Initializes byte image array on first call for speed in future calls.
                if (m_byteImages is null)
                {
                    m_byteImages = new string[256];

                    for (int imageByte = byte.MinValue; imageByte <= byte.MaxValue; imageByte++)
                    {
                        StringBuilder byteImage = new();

                        if (m_reverse)
                        {
                            if (imageByte.CheckBits(Bits.Bit07))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit06))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit05))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit04))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit03))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit02))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit01))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit00))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');
                        }
                        else
                        {
                            if (imageByte.CheckBits(Bits.Bit00))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit01))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit02))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit03))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit04))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit05))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit06))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');

                            if (imageByte.CheckBits(Bits.Bit07))
                                byteImage.Append('1');
                            else
                                byteImage.Append('0');
                        }

                        m_byteImages[imageByte] = byteImage.ToString();
                    }
                }

                StringBuilder binaryImage = new();

                for (int i = 0; i < length; i++)
                {
                    if (spacingCharacter != NoSpacing && i > 0)
                        binaryImage.Append(spacingCharacter);

                    binaryImage.Append(m_byteImages[bytes[offset + i]]);
                }

                return binaryImage.ToString();
            }
        }

        #endregion

        #region [ Base64 Encoding Class ]

        /// <summary>
        /// Handles conversion of byte buffers to and from a base64 data format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class Base64Encoding : ByteEncoding
        {
            internal Base64Encoding()
            {
                // This class is meant for internal instantiation only.
            }

            /// <summary>Decodes given string back into a byte buffer.</summary>
            /// <param name="binaryData">Encoded binary data string to decode.</param>
            /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
            /// <returns>Decoded bytes.</returns>
            public override byte[] GetBytes(string binaryData, char spacingCharacter)
            {
                // Removes spacing characters, if needed.
                binaryData = binaryData.Trim();

                if (spacingCharacter != NoSpacing)
                    binaryData = binaryData.Replace(spacingCharacter.ToString(), "");

                return Convert.FromBase64String(binaryData);
            }

            /// <summary>Encodes given buffer into a user presentable representation.</summary>
            /// <param name="bytes">Bytes to encode.</param>
            /// <param name="offset">Offset into buffer to begin encoding.</param>
            /// <param name="length">Length of buffer to encode.</param>
            /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
            /// <returns>String of encoded bytes.</returns>
            public override string GetString(byte[] bytes, int offset, int length, char spacingCharacter)
            {
                if (bytes is null)
                    throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

                string base64String = Convert.ToBase64String(bytes, offset, length);

                if (spacingCharacter == NoSpacing)
                    return base64String;

                StringBuilder base64Image = new();

                for (int i = 0; i <= base64String.Length - 1; i++)
                {
                    if (i > 0)
                        base64Image.Append(spacingCharacter);

                    base64Image.Append(base64String[i]);
                }

                return base64Image.ToString();
            }
        }

        #endregion

        #region [ ASCII Encoding Class ]

        /// <summary>
        /// Handles conversion of byte buffers to and from a ASCII data format.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class ASCIIEncoding : ByteEncoding
        {
            internal ASCIIEncoding()
            {
                // This class is meant for internal instantiation only.
            }

            /// <summary>Decodes given string back into a byte buffer.</summary>
            /// <param name="binaryData">Encoded binary data string to decode.</param>
            /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
            /// <returns>Decoded bytes.</returns>
            public override byte[] GetBytes(string binaryData, char spacingCharacter)
            {
                // Removes spacing characters, if needed.
                binaryData = binaryData.Trim();

                if (spacingCharacter != NoSpacing)
                    binaryData = binaryData.Replace(spacingCharacter.ToString(), "");

                return Encoding.ASCII.GetBytes(binaryData);
            }

            /// <summary>Encodes given buffer into a user presentable representation.</summary>
            /// <param name="bytes">Bytes to encode.</param>
            /// <param name="offset">Offset into buffer to begin encoding.</param>
            /// <param name="length">Length of buffer to encode.</param>
            /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
            /// <returns>String of encoded bytes.</returns>
            public override string GetString(byte[] bytes, int offset, int length, char spacingCharacter)
            {
                if (bytes is null)
                    throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

                string asciiString = Encoding.ASCII.GetString(bytes, offset, length);

                if (spacingCharacter == NoSpacing)
                    return asciiString;

                StringBuilder asciiImage = new();

                for (int i = 0; i <= asciiString.Length - 1; i++)
                {
                    if (i > 0)
                        asciiImage.Append(spacingCharacter);

                    asciiImage.Append(asciiString[i]);
                }

                return asciiImage.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Constant used to specify that "no spacing" should be used for data conversion.
        /// </summary>
        public const char NoSpacing = char.MinValue;

        #endregion

        #region [ Methods ]

        /// <summary>Encodes given buffer into a user presentable representation.</summary>
        /// <param name="bytes">Bytes to encode.</param>
        /// <returns>String representation of byte array.</returns>
        public virtual string GetString(byte[] bytes) => GetString(bytes, NoSpacing);

        /// <summary>Encodes given buffer into a user presentable representation.</summary>
        /// <param name="bytes">Bytes to encode.</param>
        /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
        /// <returns>String of encoded bytes.</returns>
        public virtual string GetString(byte[] bytes, char spacingCharacter)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

            return GetString(bytes, 0, bytes.Length, spacingCharacter);
        }

        /// <summary>Encodes given buffer into a user presentable representation.</summary>
        /// <param name="bytes">Bytes to encode.</param>
        /// <param name="offset">Offset into buffer to begin encoding.</param>
        /// <param name="length">Length of buffer to encode.</param>
        /// <returns>String of encoded bytes.</returns>
        public virtual string GetString(byte[] bytes, int offset, int length)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

            return GetString(bytes, offset, length, NoSpacing);
        }

        /// <summary>Encodes given buffer into a user presentable representation.</summary>
        /// <param name="bytes">Bytes to encode.</param>
        /// <param name="offset">Offset into buffer to begin encoding.</param>
        /// <param name="length">Length of buffer to encode.</param>
        /// <param name="spacingCharacter">Spacing character to place between encoded bytes.</param>
        /// <returns>String of encoded bytes.</returns>
        public abstract string GetString(byte[] bytes, int offset, int length, char spacingCharacter);

        /// <summary>Decodes given string back into a byte buffer.</summary>
        /// <param name="value">Encoded string to decode.</param>
        /// <returns>Decoded bytes.</returns>
        public virtual byte[] GetBytes(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "Input string cannot be null");

            return GetBytes(value, NoSpacing);
        }

        /// <summary>Decodes given string back into a byte buffer.</summary>
        /// <param name="value">Encoded string to decode.</param>
        /// <param name="spacingCharacter">Original spacing character that was inserted between encoded bytes.</param>
        /// <returns>Decoded bytes</returns>
        public abstract byte[] GetBytes(string value, char spacingCharacter);

        #endregion

        #region [ Static ]

        // Static Fields
        private static ByteEncoding? s_hexadecimalEncoding;
        private static ByteEncoding? s_decimalEncoding;
        private static ByteEncoding? s_bigEndianBinaryEncoding;
        private static ByteEncoding? s_littleEndianBinaryEncoding;
        private static ByteEncoding? s_base64Encoding;
        private static ByteEncoding? s_asciiEncoding;

        /// <summary>Handles encoding and decoding of a byte buffer into a hexadecimal-based presentation format.</summary>
        public static ByteEncoding Hexadecimal => s_hexadecimalEncoding ??= new HexadecimalEncoding();

        /// <summary>Handles encoding and decoding of a byte buffer into an integer-based presentation format.</summary>
        public static ByteEncoding Decimal => s_decimalEncoding ??= new DecimalEncoding();

        /// <summary>
        /// Handles encoding and decoding of a byte buffer into a big-endian binary (i.e., 0 and 1's) based
        /// presentation format.
        /// </summary>
        /// <remarks>
        /// Although endianness is typically used in the context of byte order to handle byte
        /// order swapping), this property allows you visualize "bits" in big-endian order,
        /// right-to-left. Note that bits are normally stored in the same order as their bytes.
        /// </remarks>
        public static ByteEncoding BigEndianBinary => s_bigEndianBinaryEncoding ??= new BinaryEncoding(Endianness.BigEndian);

        /// <summary>
        /// Handles encoding and decoding of a byte buffer into a little-endian binary (i.e., 0 and 1's) based
        /// presentation format.
        /// </summary>
        /// <remarks>
        /// Although endianness is typically used in the context of byte order to handle byte
        /// order swapping), this property allows you visualize "bits" in little-endian order,
        /// left-to-right. Note that bits are normally stored in the same order as their bytes.
        /// </remarks>
        public static ByteEncoding LittleEndianBinary => s_littleEndianBinaryEncoding ??= new BinaryEncoding(Endianness.LittleEndian);

        /// <summary>Handles encoding and decoding of a byte buffer into a base64 presentation format.</summary>
        public static ByteEncoding Base64 => s_base64Encoding ??= new Base64Encoding();

        /// <summary>Handles encoding and decoding of a byte buffer into an ASCII character presentation format.</summary>
        public static ByteEncoding ASCII => s_asciiEncoding ??= new ASCIIEncoding();

        /// <summary>
        /// Handles byte to string conversions for implementations that are available from Byte.ToString.
        /// </summary>
        /// <param name="bytes">Encoded string to decode.</param>
        /// <param name="offset">Offset into byte array to begin decoding string.</param>
        /// <param name="length">Number of bytes to decode starting at <paramref name="offset"/></param>
        /// <param name="spacingCharacter">Character to insert between each byte</param>
        /// <param name="format">String decoding format.</param>
        /// <returns>Decoded string</returns>
        internal static string BytesToString(byte[] bytes, int offset, int length, char spacingCharacter, string format)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes), "Input buffer cannot be null");

            StringBuilder byteString = new();

            for (int i = 0; i <= length - 1; i++)
            {
                if (spacingCharacter != NoSpacing && i > 0)
                    byteString.Append(spacingCharacter);

                byteString.Append(bytes[i + offset].ToString(format));
            }

            return byteString.ToString();
        }

        #endregion
    }
}
