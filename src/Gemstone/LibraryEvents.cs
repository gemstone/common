//******************************************************************************************************
//  LibraryEvents.cs - Gbtc
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
//  12/27/2019 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Gemstone.EventHandlerExtensions;

// TODO: Add new libraries to internals visible list as needed
[assembly: InternalsVisibleTo("Gemstone.Communication")]
[assembly: InternalsVisibleTo("Gemstone.COMTRADE")]
[assembly: InternalsVisibleTo("Gemstone.Configuration")]
[assembly: InternalsVisibleTo("Gemstone.Data")]
[assembly: InternalsVisibleTo("Gemstone.Diagnostics")]
[assembly: InternalsVisibleTo("Gemstone.Expressions")]
[assembly: InternalsVisibleTo("Gemstone.IO")]
[assembly: InternalsVisibleTo("Gemstone.Logging")]
[assembly: InternalsVisibleTo("Gemstone.Numeric")]
[assembly: InternalsVisibleTo("Gemstone.PhasorProtocols")]
[assembly: InternalsVisibleTo("Gemstone.PQDIF")]
[assembly: InternalsVisibleTo("Gemstone.PQDS")]
[assembly: InternalsVisibleTo("Gemstone.Security")]
[assembly: InternalsVisibleTo("Gemstone.Threading")]
[assembly: InternalsVisibleTo("Gemstone.Timeseries")]
[assembly: InternalsVisibleTo("Gemstone.Web")]

// ReSharper disable DelegateSubtraction

namespace Gemstone;

/// <summary>
/// Defines library-level static events.
/// </summary>
/// <remarks>
/// The <see cref="LibraryEvents"/> class automatically attaches to the <see cref="TaskScheduler.UnobservedTaskException"/> event so that
/// any unobserved task exceptions encountered will be marked as observed and exposed via the <see cref="SuppressedException"/> event.<br/>
/// To disable this feature and only use custom <see cref="TaskScheduler.UnobservedTaskException"/> event handling, call the
/// <see cref="DisableUnobservedTaskExceptionHandling"/> method during program initialization.
/// </remarks>
public static class LibraryEvents
{
    private static EventHandler<UnhandledExceptionEventArgs>? s_suppressedExceptionHandler;
    private static readonly object s_suppressedExceptionLock = new();
    private static int s_attached;

    static LibraryEvents()
    {
        EnableUnobservedTaskExceptionHandling();
    }

    /// <summary>
    /// Enables automatic handling of <see cref="TaskScheduler.UnobservedTaskException"/> events. When enabled, any unobserved
    /// task exceptions encountered are marked as observed and exposed via the <see cref="SuppressedException"/> event.
    /// </summary>
    /// <remarks>
    /// This functionality is enabled by default. This method would only ever need to be called to re-enable unobserved
    /// task exception handling after being disabled by a call to <see cref="DisableUnobservedTaskExceptionHandling"/>.
    /// </remarks>
    public static void EnableUnobservedTaskExceptionHandling()
    {
        if (Interlocked.CompareExchange(ref s_attached, 1, 0) == 0)
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    /// <summary>
    /// Disables automatic handling of <see cref="TaskScheduler.UnobservedTaskException"/> events. When disabled, any unobserved
    /// task exceptions encountered will not be marked as observed nor exposed via the <see cref="SuppressedException"/> event.
    /// </summary>
    public static void DisableUnobservedTaskExceptionHandling()
    {
        if (Interlocked.CompareExchange(ref s_attached, 0, 1) == 1)
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
    }

    private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        OnSuppressedException(sender, e.Exception);
    }

    /// <summary>
    /// Exposes exceptions that were suppressed but otherwise unhandled.  
    /// </summary>
    /// <remarks>
    /// <para>
    /// End users should attach to this event so that suppressed exceptions can be exposed to a log.
    /// </para>
    /// <para>
    /// The <see cref="LibraryEvents"/> class automatically attaches to the <see cref="TaskScheduler.UnobservedTaskException"/> event so that
    /// any unobserved task exceptions encountered will be marked as observed and exposed via the <see cref="SuppressedException"/> event.<br/>
    /// To disable this feature and only use custom <see cref="TaskScheduler.UnobservedTaskException"/> event handling, call the
    /// <see cref="DisableUnobservedTaskExceptionHandling"/> method during program initialization.
    /// </para>
    /// <para>
    /// Gemstone libraries only raise this event, no library functions attach to this end user-only event.
    /// </para>
    /// </remarks>
    public static event EventHandler<UnhandledExceptionEventArgs> SuppressedException
    {
        add
        {
            lock (s_suppressedExceptionLock)
                s_suppressedExceptionHandler += value;
        }
        remove
        {
            lock (s_suppressedExceptionLock)
                s_suppressedExceptionHandler -= value;
        }
    }

    // This method is internal to prevent exceptions from being recursively handled. Consequently, Gemstone libraries
    // should not attach to the SuppressedException event to avoid accidentally passing any caught exceptions back to
    // the OnSuppressedException method via an event handler for the SuppressedException event.
    internal static void OnSuppressedException(object? sender, Exception ex)
    {
        if (s_suppressedExceptionHandler is null)
            return;

        // Have to use custom exception handler here, default SafeInvoke handler already calls LibraryEvents.OnSuppressedException
        static void exceptionHandler(Exception ex, Delegate handler)
        {
            throw new Exception($"Failed in {nameof(Gemstone)}.{nameof(LibraryEvents)}.{nameof(SuppressedException)} event handler \"{handler.GetHandlerName()}\": {ex.Message}", ex);
        }

        s_suppressedExceptionHandler.SafeInvoke(s_suppressedExceptionLock, exceptionHandler, sender, new UnhandledExceptionEventArgs(ex, false));
    }
}
