//******************************************************************************************************
//  EventHandlerExtensionsTest.cs - Gbtc
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
//  05/28/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using Gemstone.EventHandlerExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemstone.Common.UnitTests
{
    public class EventTest
    {
        public event EventHandler SimpleEvent;

        public event EventHandler<EventArgs<Exception>> OneParamEvent;

        public event EventHandler<EventArgs<byte[], int, int>> ThreeParamEvent;

        public event FileSystemEventHandler CustomEvent;

        public void OnSimpleEvent(bool parallel) => SimpleEvent?.SafeInvoke(this, EventArgs.Empty, parallel);

        public void OnOneParamEvent(Exception ex, bool parallel) => OneParamEvent?.SafeInvoke(this, new EventArgs<Exception>(ex), parallel);

        public void OnThreeParamEvent(byte[] buffer, int offset, int length, bool parallel) => ThreeParamEvent?.SafeInvoke(this, new EventArgs<byte[], int, int>(buffer, offset, length), parallel);

        public void OnCustomEvent(FileSystemEventArgs fileArgs, bool parallel) => CustomEvent?.SafeInvoke(this, fileArgs, parallel);
    }

    [TestClass]
    public class EventHandlerExtensionsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestSafeInvokeSequential()
        {
            TestSafeInvoke(false);
        }

        [TestMethod]
        public void TestSafeInvokeParallel()
        {
            TestSafeInvoke(true);
        }

        private void TestSafeInvoke(bool parallel)
        {
            EventTest eventTest = new();
            InvalidOperationException exception = new("Test");
            byte[] buffer = { 0x09, 0x08, 0x21 };
            int tests = 0;

            void simpleEventHandler1(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 1 succeeded...");
                tests++;
            }

            void simpleEventHandler2(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 2 succeeded...");
                tests++;
            }

            void simpleEventHandler3(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 3 succeeded...");
                tests++;
            }

            void simpleEventHandler4(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 4 succeeded...");
                tests++;
            }

            void simpleEventHandler5(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 5 succeeded...");
                tests++;
            }

            void simpleEventHandler6(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 6 succeeded...");
                tests++;
            }

            void simpleEventHandler7(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 7 succeeded...");
                tests++;
            }

            void simpleEventHandler8(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 8 succeeded...");
                tests++;
            }

            void simpleEventHandler9(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 9 succeeded...");
                tests++;
            }

            void simpleEventHandler10(object sender, EventArgs e)
            {
                Assert.AreEqual(e, EventArgs.Empty);
                TestContext.WriteLine("Simple event test 10 succeeded...");
                tests++;
            }

            void oneParamEventHandler1(object sender, EventArgs<Exception> e)
            {
                Assert.AreEqual(e.Argument, exception);
                TestContext.WriteLine("One param event test 1 succeeded...");
                tests++;
            }
            
            void oneParamEventHandler2(object sender, EventArgs<Exception> e)
            {
                Assert.AreEqual(e.Argument, exception);
                TestContext.WriteLine("One param event test 2 succeeded...");
                tests++;
            }

            void threeParamEventHandler1(object sender, EventArgs<byte[], int, int> e)
            {
                Assert.AreEqual(e.Argument1, buffer);
                Assert.AreEqual(e.Argument2, 0);
                Assert.AreEqual(e.Argument3, buffer.Length);
                TestContext.WriteLine("Three param event test 1 succeeded...");
                tests++;
            }

            void threeParamEventHandler2(object sender, EventArgs<byte[], int, int> e)
            {
                Assert.AreEqual(e.Argument1, buffer);
                Assert.AreEqual(e.Argument2, 0);
                Assert.AreEqual(e.Argument3, buffer.Length);
                TestContext.WriteLine("Three param event test 2 succeeded...");
                tests++;
            }
            
            void customEventHandler1(object sender, FileSystemEventArgs e)
            {
                Assert.AreEqual(e.ChangeType, WatcherChangeTypes.Changed);
                TestContext.WriteLine("Custom event test 1 succeeded...");
                tests++;
            }

            void customEventHandler2(object sender, FileSystemEventArgs e)
            {
                Assert.AreEqual(e.ChangeType, WatcherChangeTypes.Changed);
                TestContext.WriteLine("Custom event test 2 succeeded...");
                tests++;
            }

            eventTest.SimpleEvent += simpleEventHandler1;
            eventTest.SimpleEvent += simpleEventHandler2;
            eventTest.SimpleEvent += simpleEventHandler3;
            eventTest.SimpleEvent += simpleEventHandler4;
            eventTest.SimpleEvent += simpleEventHandler5;
            eventTest.SimpleEvent += simpleEventHandler6;
            eventTest.SimpleEvent += simpleEventHandler7;
            eventTest.SimpleEvent += simpleEventHandler8;
            eventTest.SimpleEvent += simpleEventHandler9;
            eventTest.SimpleEvent += simpleEventHandler10;
            eventTest.OneParamEvent += oneParamEventHandler1;
            eventTest.OneParamEvent += oneParamEventHandler1;
            eventTest.OneParamEvent += oneParamEventHandler1;
            eventTest.OneParamEvent += oneParamEventHandler1;
            eventTest.OneParamEvent += oneParamEventHandler2;
            eventTest.OneParamEvent += oneParamEventHandler2;
            eventTest.OneParamEvent += oneParamEventHandler2;
            eventTest.OneParamEvent += oneParamEventHandler2;
            eventTest.ThreeParamEvent += threeParamEventHandler1;
            eventTest.ThreeParamEvent += threeParamEventHandler1;
            eventTest.ThreeParamEvent += threeParamEventHandler1;
            eventTest.ThreeParamEvent += threeParamEventHandler1;
            eventTest.ThreeParamEvent += threeParamEventHandler2;
            eventTest.ThreeParamEvent += threeParamEventHandler2;
            eventTest.ThreeParamEvent += threeParamEventHandler2;
            eventTest.ThreeParamEvent += threeParamEventHandler2;
            eventTest.CustomEvent += customEventHandler1;
            eventTest.CustomEvent += customEventHandler1;
            eventTest.CustomEvent += customEventHandler1;
            eventTest.CustomEvent += customEventHandler1;
            eventTest.CustomEvent += customEventHandler2;
            eventTest.CustomEvent += customEventHandler2;
            eventTest.CustomEvent += customEventHandler2;
            eventTest.CustomEvent += customEventHandler2;

            Stopwatch timer = Stopwatch.StartNew();
            eventTest.OnSimpleEvent(parallel);
            eventTest.OnOneParamEvent(exception, parallel);
            eventTest.OnThreeParamEvent(buffer, 0, buffer.Length, parallel);
            eventTest.OnCustomEvent(new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""), parallel);
            timer.Stop();

            Assert.AreEqual(tests, 34);

            TestContext.WriteLine($">> Completed {tests:N0} {(parallel ? "parallel" : "sequential")} tests in {timer.ElapsedTicks / (double)TimeSpan.TicksPerSecond:N4} seconds");
        }
    }
}
