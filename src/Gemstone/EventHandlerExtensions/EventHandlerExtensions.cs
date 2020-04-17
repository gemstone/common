//******************************************************************************************************
//  EventExtensions.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  01/06/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#pragma warning disable CA1031 // Do not catch general exception types

namespace Gemstone.EventHandlerExtensions
{
    /// <summary>
    /// Defines extension methods related to event handlers.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Safely invokes event propagation, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel.</param>
        /// <remarks>
        /// Accessing event handler invocation list will be locked on <paramref name="eventHandler"/>.
        /// Any exceptions will be suppressed, see other overloads for custom exception handling.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler eventHandler, object sender, TEventArgs args, bool parallel = true) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            SafeInvoke(eventHandler, null, (Action<Exception, EventHandler>?)null, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Exception handler; when set to <c>null</c>, exception will be suppressed.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel.</param>
        /// <remarks>
        /// Accessing event handler invocation list will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler eventHandler, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = true) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            SafeInvoke(eventHandler, null, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Exception handler; when set to <c>null</c>, exception will be suppressed.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel.</param>
        /// <remarks>
        /// Accessing event handler invocation list will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler eventHandler, Action<Exception, EventHandler>? exceptionHandler, object sender, TEventArgs args, bool parallel = true) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            SafeInvoke(eventHandler, null, exceptionHandler, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Exception handler; when set to <c>null</c>, exception will be suppressed.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler eventHandler, object? eventLock, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = true) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            SafeInvoke(eventHandler, eventLock, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom event lock and exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Exception handler; when set to <c>null</c>, exception will be suppressed.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler eventHandler, object? eventLock, Action<Exception, EventHandler>? exceptionHandler, object sender, TEventArgs args, bool parallel = true) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs
        {
            if (eventHandler == null)
                return;

            Delegate[] handlers;

            lock (eventLock ?? eventHandler)
                handlers = eventHandler.GetInvocationList();

            void invokeHandler(Delegate handler)
            {
                if (!(handler is EventHandler userHandler))
                    return;

                try
                {
                    userHandler(sender, args);
                }
                catch (Exception ex)
                {
                    if (exceptionHandler == null)
                        LibraryEvents.OnSuppressedException(typeof(EventHandlerExtensions), new Exception($"Safe invoke user event handler exception: {ex.Message}", ex));
                    else
                        exceptionHandler(ex, userHandler);
                }
            }

            // Safely iterate each attached handler, continuing on possible exception, so no handlers are missed
            if (parallel)
            {
                Parallel.ForEach(handlers, invokeHandler);
            }
            else
            {
                foreach (Delegate handler in handlers)
                    invokeHandler(handler);
            }
        }
    }
}
