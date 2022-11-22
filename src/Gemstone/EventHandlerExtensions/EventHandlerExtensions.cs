//******************************************************************************************************
//  EventExtensions.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Gemstone.Reflection.MethodBaseExtensions;

#pragma warning disable CA1031 // Do not catch general exception types

namespace Gemstone.EventHandlerExtensions
{
    /// <summary>
    /// Defines extension methods related to event handlers.
    /// </summary>
    public static class EventHandlerExtensions
    {
        private enum InvokeMode
        {
            Invoke,
            InvokeParallel,
            InvokeAsync
        }

        /// <summary>
        /// Safely invokes event propagation, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// Any exceptions will be suppressed, see <see cref="LibraryEvents.SuppressedException"/> and other overloads for custom exception handling.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvoke(null, (Action<Exception, Delegate>?)null, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvoke(null, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions which includes parameter for event handler that threw the exception; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, Action<Exception, Delegate>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvoke(null, exceptionHandler, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object? eventLock, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvoke(eventLock, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation with custom event lock and exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions which includes parameter for event handler that threw the exception; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object? eventLock, Action<Exception, Delegate>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs => 
            InvokeEventHandlers(eventHandler, eventLock, exceptionHandler, sender, args, parallel ? InvokeMode.InvokeParallel : InvokeMode.Invoke);

        /// <summary>
        /// Safely invokes event propagation asynchronously, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// Any exceptions will be suppressed, see <see cref="LibraryEvents.SuppressedException"/> and other overloads for custom exception handling.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task SafeInvokeAsync<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvokeAsync(null, (Action<Exception, Delegate>?)null, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation asynchronously with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task SafeInvokeAsync<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvokeAsync(null, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation asynchronously with custom exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions which includes parameter for event handler that threw the exception; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        /// <remarks>
        /// Event handler invocation list access will be locked on <paramref name="eventHandler"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task SafeInvokeAsync<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, Action<Exception, Delegate>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvokeAsync(null, exceptionHandler, sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation asynchronously with custom exception handler, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task SafeInvokeAsync<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object? eventLock, Action<Exception>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs =>
            eventHandler.SafeInvokeAsync(eventLock, (ex, _) => exceptionHandler?.Invoke(ex), sender, args, parallel);

        /// <summary>
        /// Safely invokes event propagation asynchronously with custom event lock and exception handler that accepts user handler delegate, continuing even if an attached user handler throws an exception.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="MulticastDelegate"/> type commonly derived from <see cref="EventHandler"/>.</typeparam>
        /// <typeparam name="TEventArgs">Type derived from <see cref="EventArgs"/>.</typeparam>
        /// <param name="eventHandler">Source <see cref="EventHandler"/> to safely invoke.</param>
        /// <param name="eventLock">Locking object for accessing event handler invocation list; when set to <c>null</c>, lock will be on <paramref name="eventHandler"/>.</param>
        /// <param name="exceptionHandler">Custom delegate to handle encountered exceptions which includes parameter for event handler that threw the exception; when set to <c>null</c>, exception will be suppressed, see <see cref="LibraryEvents.SuppressedException"/>.</param>
        /// <param name="sender">Event source.</param>
        /// <param name="args">Event arguments.</param>
        /// <param name="parallel">Call event handlers in parallel, when attached handlers are greater than one.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task SafeInvokeAsync<TEventHandler, TEventArgs>(this TEventHandler? eventHandler, object? eventLock, Action<Exception, Delegate>? exceptionHandler, object sender, TEventArgs args, bool parallel = false) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs
        {
            if (eventHandler is null)
                return Task.CompletedTask;

            if (!parallel || eventHandler.GetInvocationList().Length < 2)
                return Task.Run(() => InvokeEventHandlers(eventHandler, eventLock, exceptionHandler, sender, args, InvokeMode.Invoke));

            return Task.WhenAll(InvokeEventHandlers(eventHandler, eventLock, exceptionHandler, sender, args, InvokeMode.InvokeAsync));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Task[]? InvokeEventHandlers<TEventHandler, TEventArgs>(TEventHandler? eventHandler, object? eventLock, Action<Exception, Delegate>? exceptionHandler, object sender, TEventArgs args, InvokeMode invokeMode) where TEventHandler : MulticastDelegate where TEventArgs : EventArgs
        {
            if (eventHandler is null)
                return null;

            Delegate[] handlers;

            lock (eventLock ?? eventHandler)
                handlers = eventHandler.GetInvocationList();

            void invokeHandler(Delegate handler)
            {
                switch (handler)
                {
                    case EventHandler simpleHandler:
                        try
                        {
                            simpleHandler.Invoke(sender, args);
                        }
                        catch (Exception ex)
                        {
                            if (exceptionHandler is null)
                                LibraryEvents.OnSuppressedException(typeof(EventHandlerExtensions), new Exception($"SafeInvoke caught exception in {typeof(TEventHandler).FullName} event handler \"{handler.GetHandlerName()}\": {ex.Message}", ex));
                            else
                                exceptionHandler(ex, simpleHandler);
                        }
                        break;
                    case EventHandler<TEventArgs> typedHandler:
                        try
                        {
                            typedHandler.Invoke(sender, args);
                        }
                        catch (Exception ex)
                        {
                            if (exceptionHandler is null)
                                LibraryEvents.OnSuppressedException(typeof(EventHandlerExtensions), new Exception($"SafeInvoke caught exception in {typeof(TEventHandler).FullName} event handler \"{handler.GetHandlerName()}\": {ex.Message}", ex));
                            else
                                exceptionHandler(ex, typedHandler);
                        }
                        break;
                    default:
                        try
                        {
                            handler.DynamicInvoke(sender, args);
                        }
                        catch (Exception ex)
                        {
                            if (exceptionHandler is null)
                                LibraryEvents.OnSuppressedException(typeof(EventHandlerExtensions), new Exception($"SafeInvoke caught exception in {typeof(TEventHandler).FullName} event handler \"{handler.GetHandlerName()}\": {ex.Message}", ex));
                            else
                                exceptionHandler(ex, handler);
                        }
                        break;
                }
            }

            // Safely iterate each attached handler, continuing on possible exception, so no handlers are missed
            switch (invokeMode)
            {
                case InvokeMode.InvokeAsync:
                    return handlers.Select(handler => Task.Run(() => invokeHandler(handler))).ToArray();
                case InvokeMode.InvokeParallel when handlers.Length > 1:
                    Parallel.ForEach(handlers, invokeHandler);
                    break;
                default:
                    foreach (Delegate handler in handlers)
                        invokeHandler(handler);
                    break;
            }

            return null;
        }

        /// <summary>
        /// Gets the method name attached to an event handler.
        /// </summary>
        /// <typeparam name="TEventHandler"><see cref="Delegate"/> type representing single handler of <see cref="EventHandler"/>.</typeparam>
        /// <param name="handler">Attached event handler.</param>
        /// <returns>Name of event handler method including parameters and class name.</returns>
        public static string GetHandlerName<TEventHandler>(this TEventHandler handler) where TEventHandler : Delegate
        {
            try
            {
                return handler.Method.GetFriendlyMethodNameWithClass();
            }
            catch
            {
                return "<undetermined>";
            }
        }
    }
}
