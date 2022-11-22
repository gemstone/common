//******************************************************************************************************
//  BufferPool.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  11/17/2016 - Steven E. Chisholm
//       Generated original version of source code. 
//  12/26/2019 - J. Ritchie Carroll
//       Simplified DynamicObjectPool as an internal resource renaming to BufferPool.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gemstone.ActionExtensions;

namespace Gemstone.IO
{
    /// <summary>
    /// Provides a thread safe queue that acts as a buffer pool. 
    /// </summary>
    internal class BufferPool
    {
        private readonly int m_bufferSize;
        private readonly ConcurrentQueue<byte[]> m_buffers;
        private readonly Queue<int> m_countHistory;
        private readonly int m_targetCount;
        private int m_objectsCreated;

        /// <summary>
        /// Creates a new <see cref="BufferPool"/>.
        /// </summary>
        /// <param name="bufferSize">The size of buffers in the pool.</param>
        /// <param name="targetCount">the ideal number of buffers that are always pending on the queue.</param>
        public BufferPool(int bufferSize, int targetCount)
        {
            m_bufferSize = bufferSize;
            m_targetCount = targetCount;
            m_countHistory = new Queue<int>(100);
            m_buffers = new ConcurrentQueue<byte[]>();

            new Action(RunCollection).DelayAndExecute(1000);
        }

        private void RunCollection()
        {
            try
            {
                m_countHistory.Enqueue(m_buffers.Count);

                if (m_countHistory.Count < 60)
                    return;

                int objectsCreated = Interlocked.Exchange(ref m_objectsCreated, 0);

                // If there were ever more than the target items in the queue over the past 60 seconds remove some items.
                // However, don't remove items if the pool ever got to 0 and had objects that had to be created.
                int min = m_countHistory.Min();
                m_countHistory.Clear();

                if (objectsCreated != 0)
                    return;

                while (min > m_targetCount)
                {
                    if (!m_buffers.TryDequeue(out _)) 
                        return;

                    min--;
                }
            }
            finally
            {
                new Action(RunCollection).DelayAndExecute(1000);
            }
        }

        /// <summary>
        /// Removes a buffer from the queue. If one does not exist, one is created.
        /// </summary>
        /// <returns></returns>
        public byte[] Dequeue()
        {
            if (m_buffers.TryDequeue(out byte[]? item))
                return item;

            Interlocked.Increment(ref m_objectsCreated);
            return new byte[m_bufferSize];
        }

        /// <summary>
        /// Adds a buffer back to the queue.
        /// </summary>
        /// <param name="buffer">The buffer to queue.</param>
        public void Enqueue(byte[] buffer) => m_buffers.Enqueue(buffer);
    }
}
