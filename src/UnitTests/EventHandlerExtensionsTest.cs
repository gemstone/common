//******************************************************************************************************
//  EventHandlerExtensionsTest.cs - Gbtc
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
//  05/28/2020 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using Gemstone.EventHandlerExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemstone.Common.UnitTests
{
    public class EventTest
    {
        public event EventHandler SimpleEvent;

        public event EventHandler<EventArgs<Exception>> OneParamEvent;

        public event EventHandler<EventArgs<byte[], int, int>> ThreeParamEvent;

        public void OnSimpleEvent(bool parallel) => SimpleEvent?.SafeInvoke(this, EventArgs.Empty, parallel);

        public void OnOneParamEvent(Exception ex, bool parallel) => OneParamEvent?.SafeInvoke(this, new EventArgs<Exception>(ex), parallel);

        public void OnThreeParamEvent(byte[] buffer, int offset, int length, bool parallel) => ThreeParamEvent?.SafeInvoke(this, new EventArgs<byte[], int, int>(buffer, offset, length), parallel);
    }

    [TestClass]
    public class EventHandlerExtensionsTest
    {
        [TestMethod]
        public void TestSafeInvoke()
        {
            EventTest eventTest = new EventTest();
        }
    }
}
