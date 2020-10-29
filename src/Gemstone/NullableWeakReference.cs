//******************************************************************************************************
//  NullableWeakReference.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License a
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/14/2017 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace Gemstone
{
    /// <summary>
    /// A <see cref="WeakReference"/> implementation that can have the <see cref="Target"/> object set to null.
    /// Natively, setting <see cref="WeakReference.Target"/> to null will throw an <see cref="InvalidOperationException"/>,
    /// </summary>
    public class NullableWeakReference : WeakReference
    {
        private bool m_cleared;

        /// <summary>
        /// Creates a <see cref="NullableWeakReference"/>
        /// </summary>
        /// <param name="target">the object to maintain the weak reference to. Cannot be null.</param>
        public NullableWeakReference(object target) : base(target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="NullableWeakReference" /> object has been cleared or garbage collected.
        /// </summary>
        /// <returns>
        /// true if the object referenced by the current <see cref="NullableWeakReference" /> object has not been garbage collected or cleared 
        /// and is still accessible; otherwise, false.
        /// </returns>
        public override bool IsAlive => !m_cleared && base.IsAlive;

        /// <summary>
        /// Gets the object (the target) referenced by the current <see cref="NullableWeakReference" /> object.
        /// Set will only accept null.
        /// </summary>
        /// <returns>
        /// null if the object referenced by the current <see cref="NullableWeakReference" /> object has been garbage collected or cleared;
        /// otherwise, a reference to the object referenced by the current <see cref="NullableWeakReference" /> object.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">If setting this property to anything other than null</exception>
        public override object? Target
        {
            get => m_cleared ? null : base.Target;
            set
            {
                if (value is null)
                    Clear();
                else
                    throw new InvalidOperationException("This target must be set in the constructor.");
            }
        }

        /// <summary>
        /// Sets <see cref="Target"/> to null so subsequent calls to <see cref="Target"/> returns null.
        /// </summary>
        public virtual void Clear() => m_cleared = true;
    }
}
