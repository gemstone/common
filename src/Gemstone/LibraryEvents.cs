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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Gemstone.EventHandlerExtensions;

[assembly: InternalsVisibleTo("Gemstone.Data")]
[assembly: InternalsVisibleTo("Gemstone.Expressions")]
[assembly: InternalsVisibleTo("Gemstone.IO")]
[assembly: InternalsVisibleTo("Gemstone.Numeric")]
[assembly: InternalsVisibleTo("Gemstone.Threading")]

#pragma warning disable CA1031 // Do not catch general exception types
// ReSharper disable DelegateSubtraction

namespace Gemstone
{
    /// <summary>
    /// Defines library-level static events.
    /// </summary>
    public static class LibraryEvents
    {
        private static EventHandler<UnhandledExceptionEventArgs> s_suppressedExceptionHandler;
        private static readonly object s_suppressedExceptionLock = new object();

        /// <summary>
        /// Exposes exceptions that were suppressed but otherwise unhandled.  
        /// </summary>
        /// <remarks>
        /// End users should attach to this event so that suppressed exceptions can be exposed to a log.
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

        internal static void OnSuppressedException(object sender, Exception ex) =>
            SafeInvoke(
                s_suppressedExceptionHandler,
                s_suppressedExceptionLock,
                nameof(SuppressedException),
                sender,
                new UnhandledExceptionEventArgs(ex, false));

        private static void SafeInvoke<TEventArgs>(EventHandler<TEventArgs> eventHandler, object eventLock, string eventName, object sender, TEventArgs args)
        {
            void exceptionHandler(Exception ex, EventHandler<TEventArgs> handler) =>
                throw new Exception($"Failed in {eventName} event handler \"{GetHandlerName(handler)}\": {ex.Message}", ex);

            eventHandler.SafeInvoke(eventLock, exceptionHandler, sender, args);
        }

        private static string GetHandlerName<TEventArgs>(EventHandler<TEventArgs> userHandler)
        {
            try
            {
                return userHandler.Method.Name;
            }
            catch
            {
                return "<undetermined>";
            }
        }
    }
}
