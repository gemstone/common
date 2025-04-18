﻿//******************************************************************************************************
//  OutageLogProcessor.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
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
//  06/27/2014 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Threading;
using Gemstone.Threading.SynchronizedOperations;

namespace Gemstone.IO;

/// <summary>
/// Represents a thread-safe <see cref="OutageLog"/> processor that will operate on each <see cref="Outage"/> with a consumer provided function on independent threads.
/// </summary>
public class OutageLogProcessor : IDisposable
{
    #region [ Members ]

    // Fields
    private readonly OutageLog? m_outageLog;
    private readonly Action<Outage> m_processOutageFunction;
    private readonly Func<Outage, bool> m_canProcessOutageFunction;
    private readonly int m_processInterval;
    private readonly LongSynchronizedOperation m_operation;
    private bool m_enabled;
    private bool m_disposed;

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Creates a <see cref="OutageLogProcessor"/> using a pre-initialized <see cref="OutageLog"/>.
    /// </summary>
    /// <param name="outageLog">Pre-initialized <see cref="OutageLog"/> to process.</param>
    /// <param name="processOutageFunction">A delegate that defines a processing function for an <see cref="Outage"/>.</param>
    /// <param name="canProcessOutageFunction">A delegate that determines if an <see cref="Outage"/> can currently be processed.</param>
    /// <param name="processExceptionHandler">Delegate to handle any exceptions encountered while processing as <see cref="Outage"/>.</param>
    /// <param name="processingInterval">Processing interval, in milliseconds.</param>
    public OutageLogProcessor(OutageLog? outageLog, Action<Outage> processOutageFunction, Func<Outage, bool> canProcessOutageFunction, Action<Exception> processExceptionHandler, int processingInterval)
    {
        ArgumentNullException.ThrowIfNull(outageLog);
        ArgumentNullException.ThrowIfNull(processOutageFunction);
        ArgumentNullException.ThrowIfNull(canProcessOutageFunction);
        ArgumentNullException.ThrowIfNull(processExceptionHandler);

        m_outageLog = outageLog;
        m_outageLog.LogModified += outageLog_CollectionChanged;

        m_processOutageFunction = processOutageFunction;
        m_canProcessOutageFunction = canProcessOutageFunction;
        m_processInterval = processingInterval;
        m_enabled = true;

        m_operation = new LongSynchronizedOperation(ProcessNextItem, processExceptionHandler) { IsBackground = true };
        m_operation.RunAsync();
    }

    /// <summary>
    /// Releases the unmanaged resources before the <see cref="OutageLogProcessor"/> object is reclaimed by <see cref="GC"/>.
    /// </summary>
    ~OutageLogProcessor()
    {
        Dispose(false);
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Enables or disables the <see cref="OutageLogProcessor"/>.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return m_enabled;
        }
        set
        {
            m_enabled = value;

            if (m_enabled)
                m_operation.RunAsync();
        }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Releases all the resources used by the <see cref="OutageLogProcessor"/> object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="OutageLogProcessor"/> object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (m_disposed)
            return;

        try
        {
            if (!disposing)
                return;

            if (m_outageLog is not null)
            {
                m_outageLog.LogModified -= outageLog_CollectionChanged;
                m_outageLog.Dispose();
            }
        }
        finally
        {
            m_disposed = true;  // Prevent duplicate dispose.
        }
    }

    private void outageLog_CollectionChanged(object? sender, EventArgs e)
    {
        // Outage log has changed, kick off processing of next outage
        m_operation.RunAsync();
    }

    private void ProcessNextItem()
    {
        if (m_disposed || !m_enabled || m_outageLog is null)
            return;

        try
        {
            // Get next outage for processing, if any
            Outage? nextOutage = m_outageLog.First();

            if (nextOutage is null)
                return;

            try
            {
                // See if we can process the outage at this time
                if (!m_canProcessOutageFunction(nextOutage))
                    return;

                m_processOutageFunction(nextOutage);

                // Outage processed successfully, attempt to clear it from the log
                m_outageLog.Remove(nextOutage);
            }
            finally
            {
                // Process next item
                m_operation.RunAsync();
            }
        }
        finally
        {
            // Make sure not to process items any faster than the processing interval
            Thread.Sleep(m_processInterval);
        }
    }

    #endregion
}
