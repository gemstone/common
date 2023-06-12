//******************************************************************************************************
//  Bits.cs - Gbtc
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
//  01/25/2008 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

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

namespace Gemstone;

/// <summary>
/// Represents bits in a signed or unsigned integer value.
/// </summary>
[Flags]
public enum Bits : ulong
{
    /// <summary>No bits set (0x0000000000000000)</summary>
    Nil = 0x00000000,

    // Byte 0, Bits 0-7

    /// <summary>Bit 00 (0x0000000000000001)</summary>
    Bit00 = 0x00000001, // 00000001 = 1

    /// <summary>Bit 01 (0x0000000000000002)</summary>
    Bit01 = Bit00 << 1, // 00000010 = 2

    /// <summary>Bit 02 (0x0000000000000004)</summary>
    Bit02 = Bit01 << 1, // 00000100 = 4

    /// <summary>Bit 03 (0x0000000000000008)</summary>
    Bit03 = Bit02 << 1, // 00001000 = 8

    /// <summary>Bit 04 (0x0000000000000010)</summary>
    Bit04 = Bit03 << 1, // 00010000 = 16

    /// <summary>Bit 05 (0x0000000000000020)</summary>
    Bit05 = Bit04 << 1, // 00100000 = 32

    /// <summary>Bit 06 (0x0000000000000040)</summary>
    Bit06 = Bit05 << 1, // 01000000 = 64

    /// <summary>Bit 07 (0x0000000000000080)</summary>
    Bit07 = Bit06 << 1, // 10000000 = 128

    // Byte 1, Bits 8-15

    /// <summary>Bit 08 (0x0000000000000100)</summary>
    Bit08 = Bit07 << 1,

    /// <summary>Bit 09 (0x0000000000000200)</summary>
    Bit09 = Bit08 << 1,

    /// <summary>Bit 10 (0x0000000000000400)</summary>
    Bit10 = Bit09 << 1,

    /// <summary>Bit 11 (0x0000000000000800)</summary>
    Bit11 = Bit10 << 1,

    /// <summary>Bit 12 (0x0000000000001000)</summary>
    Bit12 = Bit11 << 1,

    /// <summary>Bit 13 (0x0000000000002000)</summary>
    Bit13 = Bit12 << 1,

    /// <summary>Bit 14 (0x0000000000004000)</summary>
    Bit14 = Bit13 << 1,

    /// <summary>Bit 15 (0x0000000000008000)</summary>
    Bit15 = Bit14 << 1,

    // Byte 2, Bits 16-23

    /// <summary>Bit 16 (0x0000000000010000)</summary>
    Bit16 = Bit15 << 1,

    /// <summary>Bit 17 (0x0000000000020000)</summary>
    Bit17 = Bit16 << 1,

    /// <summary>Bit 18 (0x0000000000040000)</summary>
    Bit18 = Bit17 << 1,

    /// <summary>Bit 19 (0x0000000000080000)</summary>
    Bit19 = Bit18 << 1,

    /// <summary>Bit 20 (0x0000000000100000)</summary>
    Bit20 = Bit19 << 1,

    /// <summary>Bit 21 (0x0000000000200000)</summary>
    Bit21 = Bit20 << 1,

    /// <summary>Bit 22 (0x0000000000400000)</summary>
    Bit22 = Bit21 << 1,

    /// <summary>Bit 23 (0x0000000000800000)</summary>
    Bit23 = Bit22 << 1,

    // Byte 3, Bits 24-31

    /// <summary>Bit 24 (0x0000000001000000)</summary>
    Bit24 = Bit23 << 1,

    /// <summary>Bit 25 (0x0000000002000000)</summary>
    Bit25 = Bit24 << 1,

    /// <summary>Bit 26 (0x0000000004000000)</summary>
    Bit26 = Bit25 << 1,

    /// <summary>Bit 27 (0x0000000008000000)</summary>
    Bit27 = Bit26 << 1,

    /// <summary>Bit 28 (0x0000000010000000)</summary>
    Bit28 = Bit27 << 1,

    /// <summary>Bit 29 (0x0000000020000000)</summary>
    Bit29 = Bit28 << 1,

    /// <summary>Bit 30 (0x0000000040000000)</summary>
    Bit30 = Bit29 << 1,

    /// <summary>Bit 31 (0x0000000080000000)</summary>
    Bit31 = Bit30 << 1,

    // Byte 4, Bits 32-39

    /// <summary>Bit 32 (0x0000000100000000)</summary>
    Bit32 = Bit31 << 1,

    /// <summary>Bit 33 (0x0000000200000000)</summary>
    Bit33 = Bit32 << 1,

    /// <summary>Bit 34 (0x0000000400000000)</summary>
    Bit34 = Bit33 << 1,

    /// <summary>Bit 35 (0x0000000800000000)</summary>
    Bit35 = Bit34 << 1,

    /// <summary>Bit 36 (0x0000001000000000)</summary>
    Bit36 = Bit35 << 1,

    /// <summary>Bit 37 (0x0000002000000000)</summary>
    Bit37 = Bit36 << 1,

    /// <summary>Bit 38 (0x0000004000000000)</summary>
    Bit38 = Bit37 << 1,

    /// <summary>Bit 39 (0x0000008000000000)</summary>
    Bit39 = Bit38 << 1,

    // Byte 5, Bits 40-47

    /// <summary>Bit 40 (0x0000010000000000)</summary>
    Bit40 = Bit39 << 1,

    /// <summary>Bit 41 (0x0000020000000000)</summary>
    Bit41 = Bit40 << 1,

    /// <summary>Bit 42 (0x0000040000000000)</summary>
    Bit42 = Bit41 << 1,

    /// <summary>Bit 43 (0x0000080000000000)</summary>
    Bit43 = Bit42 << 1,

    /// <summary>Bit 44 (0x0000100000000000)</summary>
    Bit44 = Bit43 << 1,

    /// <summary>Bit 45 (0x0000200000000000)</summary>
    Bit45 = Bit44 << 1,

    /// <summary>Bit 46 (0x0000400000000000)</summary>
    Bit46 = Bit45 << 1,

    /// <summary>Bit 47 (0x0000800000000000)</summary>
    Bit47 = Bit46 << 1,

    // Byte 6, Bits 48-55

    /// <summary>Bit 48 (0x0001000000000000)</summary>
    Bit48 = Bit47 << 1,

    /// <summary>Bit 49 (0x0002000000000000)</summary>
    Bit49 = Bit48 << 1,

    /// <summary>Bit 50 (0x0004000000000000)</summary>
    Bit50 = Bit49 << 1,

    /// <summary>Bit 51 (0x0008000000000000)</summary>
    Bit51 = Bit50 << 1,

    /// <summary>Bit 52 (0x0010000000000000)</summary>
    Bit52 = Bit51 << 1,

    /// <summary>Bit 53 (0x0020000000000000)</summary>
    Bit53 = Bit52 << 1,

    /// <summary>Bit 54 (0x0040000000000000)</summary>
    Bit54 = Bit53 << 1,

    /// <summary>Bit 55 (0x0080000000000000)</summary>
    Bit55 = Bit54 << 1,

    // Byte 7, Bits 56-63

    /// <summary>Bit 56 (0x0100000000000000)</summary>
    Bit56 = Bit55 << 1,

    /// <summary>Bit 57 (0x0200000000000000)</summary>
    Bit57 = Bit56 << 1,

    /// <summary>Bit 58 (0x0400000000000000)</summary>
    Bit58 = Bit57 << 1,

    /// <summary>Bit 59 (0x0800000000000000)</summary>
    Bit59 = Bit58 << 1,

    /// <summary>Bit 60 (0x1000000000000000)</summary>
    Bit60 = Bit59 << 1,

    /// <summary>Bit 61 (0x2000000000000000)</summary>
    Bit61 = Bit60 << 1,

    /// <summary>Bit 62 (0x4000000000000000)</summary>
    Bit62 = Bit61 << 1,

    /// <summary>Bit 63 (0x8000000000000000)</summary>
    Bit63 = Bit62 << 1
}