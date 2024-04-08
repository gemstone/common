//******************************************************************************************************
//  LogSuppression.cs - Gbtc
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
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

using System;
using System.Collections.Generic;

namespace Gemstone.Diagnostics;

/// <summary>
/// Manages the suppression of log messages
/// </summary>
public static class LogSuppression
{
    private enum SuppressionMode
    {
        None = 0,
        FirstChanceExceptionOnly = 1,
        AllMessages = 2,
    }

    private static class ThreadLocalThreadStack
    {
        [ThreadStatic]
        private static ThreadStack? s_localValue;

        /// <summary>
        /// Gets the <see cref="ThreadStack"/> item for the current thread.
        /// Note: No exchange compare is needed since <see cref="s_localValue"/>
        /// is local only to the current thread.
        /// </summary>
        public static ThreadStack Value => s_localValue ??= new ThreadStack();
    }

    /// <summary>
    /// This information is maintained in a ThreadLocal variable and is about 
    /// messages and log suppression applied at higher levels of the calling stack.
    /// </summary>
    private class ThreadStack
    {
        private readonly List<SuppressionMode> m_logMessageSuppressionStack = new();

        public bool ShouldSuppressLogMessages => m_logMessageSuppressionStack.Count > 0 && m_logMessageSuppressionStack[^1] >= SuppressionMode.AllMessages;

        public bool ShouldSuppressFirstChanceLogMessages => m_logMessageSuppressionStack.Count > 0 && m_logMessageSuppressionStack[^1] >= SuppressionMode.FirstChanceExceptionOnly;


        public StackDisposal SuppressLogMessages(SuppressionMode suppressionMode)
        {
            m_logMessageSuppressionStack.Add(suppressionMode);
            int depth = m_logMessageSuppressionStack.Count;

            if (depth >= s_stackDisposalSuppressionFlags!.Length) 
                GrowStackDisposal(depth + 1);
            
            return s_stackDisposalSuppressionFlags[depth];
        }

        public void RemoveSuppression(int depth)
        {
            while (m_logMessageSuppressionStack.Count >= depth) 
                m_logMessageSuppressionStack.RemoveAt(m_logMessageSuppressionStack.Count - 1);
        }
    }

    private static StackDisposal[]? s_stackDisposalSuppressionFlags;
    internal static readonly object SyncRoot = new();

    static LogSuppression()
    {
        GrowStackDisposal(1);
    }

    /// <summary>
    /// Gets if Log Messages should be suppressed.
    /// </summary>
    public static bool ShouldSuppressLogMessages => ThreadLocalThreadStack.Value.ShouldSuppressLogMessages;

    /// <summary>
    /// Gets if First Chance Exception Log Messages should be suppressed.
    /// </summary>
    public static bool ShouldSuppressFirstChanceLogMessages => ThreadLocalThreadStack.Value.ShouldSuppressFirstChanceLogMessages;

    /// <summary>
    /// Sets a flag that will prevent log messages from being raised on this thread.
    /// Remember to dispose of the callback to remove this suppression.
    /// </summary>
    /// <returns></returns>
    public static IDisposable SuppressLogMessages()
    {
        return ThreadLocalThreadStack.Value.SuppressLogMessages(SuppressionMode.AllMessages);
    }

    /// <summary>
    /// Sets a flag that will prevent First Chance Exception log messages from being raised on this thread.
    /// Remember to dispose of the callback to remove this suppression.
    /// </summary>
    /// <returns></returns>
    public static IDisposable SuppressFirstChanceExceptionLogMessages()
    {
        return ThreadLocalThreadStack.Value.SuppressLogMessages(SuppressionMode.FirstChanceExceptionOnly);
    }

    /// <summary>
    /// Sets a flag that will allow log messages to be raised again.
    /// Remember to dispose of the callback to remove this override.
    /// </summary>
    /// <returns></returns>
    public static IDisposable OverrideSuppressLogMessages()
    {
        return ThreadLocalThreadStack.Value.SuppressLogMessages(SuppressionMode.None);
    }

    private static void GrowStackDisposal(int desiredSize)
    {
        lock (SyncRoot)
        {
            while (s_stackDisposalSuppressionFlags is null || s_stackDisposalSuppressionFlags.Length < desiredSize)
            {
                int lastSize = s_stackDisposalSuppressionFlags?.Length ?? 2;
                
                StackDisposal[] suppressionFlags = new StackDisposal[lastSize * 2];
                
                for (int x = 0; x < suppressionFlags.Length; x++) 
                    suppressionFlags[x] = new StackDisposal(x, DisposeSuppressionFlags);

                s_stackDisposalSuppressionFlags = suppressionFlags;
            }
        }
    }

    private static void DisposeSuppressionFlags(int depth)
    {
        ThreadLocalThreadStack.Value.RemoveSuppression(depth);
    }
}
