﻿//******************************************************************************************************
//  ShortTime.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/24/2016 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Globalization;

[assembly: InternalsVisibleTo("Gemstone.Diagnostics")]

namespace Gemstone;

internal static class ShortTimeFunctions
{
    //private const long TicksPerMillisecond = 10000;
    //private const long TicksPerSecond = TicksPerMillisecond * 1000;

    private static readonly double s_secondsPerCount;
    private static readonly double s_millisecondsPerCount;
    private static readonly double s_microsecondsPerCount;
    private static readonly double s_ticksPerCount;

    private static readonly double s_countsPerSecond;
    private static readonly double s_countsPerMillisecond;
    private static readonly double s_countsPerMicrosecond;
    private static readonly double s_countsPerTick;

    static ShortTimeFunctions()
    {
        decimal frequency = Stopwatch.Frequency;

        decimal countsPerSecond = frequency;
        s_countsPerSecond = (double)countsPerSecond;
        s_countsPerMillisecond = (double)(countsPerSecond / 1000);
        s_countsPerMicrosecond = (double)(countsPerSecond / 1000000);
        s_countsPerTick = (double)(countsPerSecond / 10000000);

        decimal secondsPerCount = 1 / frequency;
        s_secondsPerCount = (double)secondsPerCount;
        s_millisecondsPerCount = (double)(secondsPerCount * 1000);
        s_microsecondsPerCount = (double)(secondsPerCount * 1000000);
        s_ticksPerCount = (double)(secondsPerCount * 10000000);
    }

    public static long Now()
    {
        return Stopwatch.GetTimestamp();
    }

    public static long AddSeconds(long time, double value)
    {
        return time + (long)(s_countsPerSecond * value);
    }

    public static long AddMilliseconds(long time, double value)
    {
        return time + (long)(s_countsPerMillisecond * value);
    }

    public static long AddMicroseconds(long time, double value)
    {
        return time + (long)(s_countsPerMicrosecond * value);
    }

    public static long AddTicks(long time, double value)
    {
        return time + (long)(s_countsPerTick * value);
    }

    public static double ElapsedSeconds(long a, long b)
    {
        return (b - a) * s_secondsPerCount;
    }

    public static double ElapsedMilliseconds(long a, long b)
    {
        return (b - a) * s_millisecondsPerCount;
    }

    public static double ElapsedMicroseconds(long a, long b)
    {
        return (b - a) * s_microsecondsPerCount;
    }

    public static double ElapsedTicks(long a, long b)
    {
        return (b - a) * s_ticksPerCount;
    }
}

/// <summary>
/// Represents a high resolution time that is very granular but may drift 
/// if trying to accurately measure long time durations (Such as hours). 
/// This time is not adjusted with changes to the system clock.
/// Typical clock drifts by about 2-3 ms per minute as apposed to 0.4ms
/// per minute for standard <see cref="DateTime"/>.
/// </summary>
/// <remarks>
/// Call times are about 40+ million calls per second.
/// </remarks>
public readonly struct ShortTime : IEquatable<ShortTime>
{
    private readonly long m_time;

    private ShortTime(long time)
    {
        m_time = time;
    }

    /// <summary>
    /// Calculates the approximate <see cref="DateTime"/> represented by this time. 
    /// </summary>
    /// <returns></returns>
    public DateTime UtcTime
    {
        get
        {
            return DateTime.UtcNow - Elapsed();
        }
    }

    /// <summary>
    /// The elapsed time in seconds.
    /// </summary>
    /// <returns></returns>
    public double ElapsedSeconds()
    {
        return ElapsedSeconds(Now);
    }

    /// <summary>
    /// The elapsed time in milliseconds.
    /// </summary>
    /// <returns></returns>
    public double ElapsedMilliseconds()
    {
        return ElapsedMilliseconds(Now);
    }

    /// <summary>
    /// The elapsed time in microseconds.
    /// </summary>
    /// <returns></returns>
    public double ElapsedMicroseconds()
    {
        return ElapsedMicroseconds(Now);
    }

    /// <summary>
    /// The elapsed time in ticks.
    /// </summary>
    /// <returns></returns>
    public double ElapsedTicks()
    {
        return ElapsedTicks(Now);
    }

    /// <summary>
    /// Gets the time that has elapsed since the creation of this time.
    /// </summary>
    /// <returns></returns>
    public TimeSpan Elapsed()
    {
        return Elapsed(Now);
    }

    /// <summary>
    /// The elapsed time in seconds
    /// </summary>
    public double ElapsedSeconds(ShortTime futureTime)
    {
        return ShortTimeFunctions.ElapsedSeconds(m_time, futureTime.m_time);
    }

    /// <summary>
    /// The elapsed time in milliseconds
    /// </summary>
    public double ElapsedMilliseconds(ShortTime futureTime)
    {
        return ShortTimeFunctions.ElapsedMilliseconds(m_time, futureTime.m_time);
    }

    /// <summary>
    /// The elapsed time in microseconds
    /// </summary>
    public double ElapsedMicroseconds(ShortTime futureTime)
    {
        return ShortTimeFunctions.ElapsedMicroseconds(m_time, futureTime.m_time);
    }

    /// <summary>
    /// The elapsed time in ticks
    /// </summary>
    public double ElapsedTicks(ShortTime futureTime)
    {
        return ShortTimeFunctions.ElapsedTicks(m_time, futureTime.m_time);
    }

    /// <summary>
    /// The elapsed time
    /// </summary>
    public TimeSpan Elapsed(ShortTime futureTime)
    {
        return new TimeSpan((long)ShortTimeFunctions.ElapsedTicks(m_time, futureTime.m_time));
    }

    /// <summary>
    /// Adds milliseconds to this struct
    /// </summary>
    public ShortTime AddMilliseconds(double duration)
    {
        return new ShortTime(ShortTimeFunctions.AddMilliseconds(m_time, duration));
    }

    /// <summary>
    /// Adds seconds to this struct
    /// </summary>
    public ShortTime AddSeconds(double duration)
    {
        return new ShortTime(ShortTimeFunctions.AddSeconds(m_time, duration));
    }

    /// <summary>
    /// Adds ticks to this struct
    /// </summary>
    public ShortTime AddTicks(double duration)
    {
        return new ShortTime(ShortTimeFunctions.AddTicks(m_time, duration));
    }

    /// <summary>
    /// Adds timespan to this struct
    /// </summary>
    public ShortTime Add(TimeSpan duration)
    {
        return new ShortTime(ShortTimeFunctions.AddTicks(m_time, duration.Ticks));
    }

    /// <summary>
    /// The current time in <see cref="ShortTime"/>
    /// </summary>
    public static ShortTime Now
    {
        get
        {
            return new(ShortTimeFunctions.Now());
        }
    }

    /// <summary>
    /// Tests Less Than
    /// </summary>
    public static bool operator <(ShortTime a, ShortTime b)
    {
        return a.m_time - b.m_time < 0;
        // Accounts for overflows.
    }

    /// <summary>
    /// Tests Greater Than
    /// </summary>
    public static bool operator >(ShortTime a, ShortTime b)
    {
        return a.m_time - b.m_time > 0;
        // Accounts for overflows.
    }

    /// <summary>
    /// Tests Less Than or equal
    /// </summary>
    public static bool operator <=(ShortTime a, ShortTime b)
    {
        return a.m_time - b.m_time <= 0;
        // Accounts for overflows.
    }

    /// <summary>
    /// Tests Greater than or equal Than
    /// </summary>
    public static bool operator >=(ShortTime a, ShortTime b)
    {
        return a.m_time - b.m_time >= 0;
        // Accounts for overflows.
    }

    /// <summary>
    /// Subtracts two times.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static TimeSpan operator -(ShortTime a, ShortTime b)
    {
        return b.Elapsed(a);
    }

    /// <summary>
    /// Are 2 times equal
    /// </summary>
    public static bool operator ==(ShortTime a, ShortTime b)
    {
        return b.m_time == a.m_time;
    }

    /// <summary>
    /// Are 2 times not equal
    /// </summary>
    public static bool operator !=(ShortTime a, ShortTime b)
    {
        return b.m_time != a.m_time;
    }

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
    /// <param name="obj">The object to compare with the current instance. </param>
    /// <filterpriority>2</filterpriority>
    public override bool Equals(object? obj)
    {
        return obj is ShortTime time && Equals(time);
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
    /// <param name="other">An object to compare with this object.</param>
    public bool Equals(ShortTime other)
    {
        return m_time == other.m_time;
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode()
    {
        return m_time.GetHashCode();
    }

    /// <summary>
    /// Shows the UTC time.
    /// </summary>
    public override string ToString()
    {
        return UtcTime.ToString(CultureInfo.InvariantCulture);
    }
}
