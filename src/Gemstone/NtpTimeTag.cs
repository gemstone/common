//******************************************************************************************************
//  NtpTimeTag.cs - Gbtc
//
//  Copyright � 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright � 2009 - J. Ritchie Carroll
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
using System.Runtime.Serialization;
using Gemstone.WordExtensions;

namespace Gemstone;

/// <summary>
/// Represents a standard Network Time Protocol (NTP) time-tag.
/// </summary>
/// <remarks>
/// As recommended by RFC-2030, all NTP timestamps earlier than 3h 14m 08s UTC on 20 January 1968
/// are reckoned from 6h 28m 16s UTC on 7 February 2036. This gives the <see cref="NtpTimeTag"/>
/// class a functioning range of 1968-01-20 03:14:08 to 2104-02-26 09:42:23.
/// </remarks>
[Serializable]
public class NtpTimeTag : TimeTagBase
{
    #region [ Constructors ]

    /// <summary>
    /// Creates a new <see cref="NtpTimeTag"/>, given number of seconds since 1/1/1900.
    /// </summary>
    /// <param name="seconds">Number of seconds since 1/1/1900.</param>
    public NtpTimeTag(decimal seconds) : base(GetBaseDateOffsetTicks(seconds), seconds)
    {
    }

    /// <summary>
    /// Creates a new <see cref="NtpTimeTag"/>, given number of seconds and fractional seconds since 1/1/1900.
    /// </summary>
    /// <param name="seconds">Number of seconds since 1/1/1900.</param>
    /// <param name="fraction">Number of fractional seconds, in whole picoseconds.</param>
    public NtpTimeTag(uint seconds, uint fraction) : base(GetBaseDateOffsetTicks(seconds), seconds + fraction / (decimal)uint.MaxValue)
    {
    }

    /// <summary>
    /// Creates a new <see cref="NtpTimeTag"/>, given 64-bit NTP timestamp.
    /// </summary>
    /// <param name="timestamp">NTP timestamp containing number of seconds since 1/1/1900 in high-word and fractional seconds in low-word.</param>
    public NtpTimeTag(ulong timestamp) : this(timestamp.HighDoubleWord(), timestamp.LowDoubleWord())
    {
    }

    /// <summary>
    /// Creates a new <see cref="NtpTimeTag"/>, given specified <see cref="Ticks"/>.
    /// </summary>
    /// <param name="timestamp">Timestamp in <see cref="Ticks"/> to create NTP time-tag from (minimum valid date is 1/1/1900).</param>
    /// <remarks>
    /// This constructor will accept a <see cref="DateTime"/> parameter since <see cref="Ticks"/> is implicitly castable to a <see cref="DateTime"/>.
    /// </remarks>
    public NtpTimeTag(Ticks timestamp) : base(GetBaseDateOffsetTicks(timestamp), timestamp)
    {
    }

    /// <summary>
    /// Creates a new <see cref="NtpTimeTag"/> from serialization parameters.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
    /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
    protected NtpTimeTag(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets 64-bit NTP timestamp.
    /// </summary>
    public ulong Timestamp
    {
        get
        {
            return FromTicks(ToDateTime());
        }
    }

    #endregion

    #region [ Static ]

    // Static Fields

    /// <summary>
    /// Number of ticks since 1/1/1900.
    /// </summary>
    /// <remarks>
    /// NTP dates are measured as the number of seconds since 1/1/1900.
    /// </remarks>
    public static readonly Ticks BaseTicks = new DateTime(1900, 1, 1, 0, 0, 0).Ticks;

    /// <summary>
    /// Number of ticks since 2/7/2036 at 6h 28m 16s UTC when MSB is set.
    /// </summary>
    /// <remarks>
    /// According to RFC-2030, NTP dates can also be measured as the number of seconds since 2/7/2036
    /// at 6h 28m 16s UTC if MSB is set.
    /// </remarks>
    public static readonly Ticks AlternateBaseTicks = new DateTime(2036, 2, 7, 6, 28, 16).Ticks;

    // Static Methods

    /// <summary>
    /// Gets proper NTP offset based on <paramref name="seconds"/> value, see RFC-2030.
    /// </summary>
    /// <param name="seconds">Seconds value.</param>
    /// <returns>Proper NTP offset.</returns>
    public static long GetBaseDateOffsetTicks(decimal seconds)
    {
        return GetBaseDateOffsetTicks((Ticks)(seconds * Ticks.PerSecond));
    }

    /// <summary>
    /// Gets proper NTP offset based on <paramref name="timestamp"/> value, see RFC-2030.
    /// </summary>
    /// <param name="timestamp"><see cref="Ticks"/> timestamp value.</param>
    /// <returns>Proper NTP offset.</returns>
    public static long GetBaseDateOffsetTicks(Ticks timestamp)
    {
        return timestamp < AlternateBaseTicks ? BaseTicks : AlternateBaseTicks;
    }

    /// <summary>
    /// Gets proper NTP offset based on most significant byte on <paramref name="seconds"/> value, see RFC-2030.
    /// </summary>
    /// <param name="seconds">NTP seconds timestamp value.</param>
    /// <returns>Proper NTP offset.</returns>
    public static long GetBaseDateOffsetTicks(uint seconds)
    {
        return (seconds & 0x80000000) > 0 ? BaseTicks : AlternateBaseTicks;
    }

    /// <summary>
    /// Gets 64-bit NTP timestamp given <paramref name="timestamp"/> in <see cref="Ticks"/>.
    /// </summary>
    /// <param name="timestamp">Timestamp in <see cref="Ticks"/>.</param>
    /// <returns>Seconds in NTP from given <paramref name="timestamp"/>.</returns>
    public static ulong FromTicks(Ticks timestamp)
    {
        timestamp -= GetBaseDateOffsetTicks(timestamp);

        uint seconds = (uint)Math.Truncate(timestamp.ToSeconds());
        uint fraction = (uint)(timestamp.DistanceBeyondSecond().ToSeconds() * uint.MaxValue);

        return Word.MakeQuadWord(seconds, fraction);
    }

    #endregion
}
