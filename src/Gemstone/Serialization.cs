//******************************************************************************************************
//  Serialization.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  06/08/2006 - Pinal C. Patel
//       Original version of source code generated.
//  09/09/2008 - J. Ritchie Carroll
//       Converted to C#.
//  09/09/2008 - J. Ritchie Carroll
//       Added TryGetObject overloads.
//  02/16/2009 - Josh L. Patterson
//       Edited Code Comments.
//  08/4/2009 - Josh L. Patterson
//       Edited Code Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  04/06/2011 - Pinal C. Patel
//       Modified GetString() method to not check for the presence of Serializable attribute on the 
//       object being serialized since this is not required by the XmlSerializer.
//  04/08/2011 - Pinal C. Patel
//       Moved Serialize() and Deserialize() methods from GSF.Services.ServiceModel.Serialization class
//       in GSF.Services.dll to consolidate serialization methods.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Gemstone.Reflection;

namespace Gemstone;

/// <summary>
/// Common serialization related functions.
/// </summary>
public static class Serialization
{
    /// <summary>
    /// Serialization binder used to deserialize files that were serialized using the old library frameworks
    /// (TVA Code Library, Time Series Framework, TVA.PhasorProtocols, and PMU Connection Tester) into classes
    /// in the Grid Solutions Framework.
    /// </summary>
    public static readonly SerializationBinder LegacyBinder = new LegacySerializationBinder();

    // Serialization binder used to deserialize files that were serialized using the old library frameworks.
    private class LegacySerializationBinder : SerializationBinder
    {
        /// <summary>
        /// Controls the binding of a serialized object to a type.
        /// </summary>
        /// <param name="assemblyName">Specifies the <see cref="Assembly"/> name of the serialized object.</param>
        /// <param name="typeName">Specifies the <see cref="Type"/> name of the serialized object.</param>
        /// <returns>The type of the object the formatter creates a new instance of.</returns>
        public override Type? BindToType(string assemblyName, string typeName)
        {
            // Perform namespace transformations that occurred when migrating to the Grid Solutions Framework
            // from various older versions of code with different namespaces
            string newTypeName = typeName
                .Replace("TVA.", "Gemstone.")
                .Replace("GSF.TimeSeries.", "Gemstone.Timeseries.")
                .Replace("GSF.", "Gemstone.")
                .Replace("TimeSeriesFramework.", "Gemstone.Timeseries.")
                .Replace("ConnectionTester.", "Gemstone.PhasorProtocols.")  // PMU Connection Tester namespace
                .Replace("TVA.Phasors.", "Gemstone.PhasorProtocols.")       // 2007 TVA Code Library namespace
                .Replace("Tva.Phasors.", "Gemstone.PhasorProtocols.")       // 2008 TVA Code Library namespace
                .Replace("BpaPdcStream", "BPAPDCstream")                    // 2013 GSF uppercase phasor protocol namespace
                .Replace("FNet", "FNET")                                    // 2013 GSF uppercase phasor protocol namespace
                .Replace("Iec61850_90_5", "IEC61850_90_5")                  // 2013 GSF uppercase phasor protocol namespace
                .Replace("Ieee1344", "IEEE1344")                            // 2013 GSF uppercase phasor protocol namespace
                .Replace("IeeeC37_118", "IEEEC37_118");                     // 2013 GSF uppercase phasor protocol namespace

            // Check for 2009 TVA Code Library namespace
            if (newTypeName.StartsWith("PhasorProtocols", StringComparison.Ordinal))
                newTypeName = "Gemstone." + newTypeName;

            // Check for 2014 LineFrequency type
            if (newTypeName.Equals("GSF.PhasorProtocols.LineFrequency", StringComparison.Ordinal))
                newTypeName = "Gemstone.Numeric.EE.LineFrequency";

            // Check for GSF LineFrequency type
            if (newTypeName.Equals("GSF.Units.EE.LineFrequency", StringComparison.Ordinal))
                newTypeName = "Gemstone.Numeric.EE.LineFrequency";

            try
            {
                // Search each assembly in the current application domain for the type with the transformed name
                return AssemblyInfo.FindType(newTypeName);
            }
            catch
            {
                // Fall back on more brute force search when simple search fails
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        Type? newType = assembly.GetType(newTypeName);

                        if (newType is not null)
                            return newType;
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors that occur when attempting to load
                        // types from assemblies as we may still be able to
                        // load the type from a different assembly
                        LibraryEvents.OnSuppressedException(typeof(LegacySerializationBinder), ex);
                    }
                }
            }

            // No type found; return null
            return null;
        }
    }
    
    /// <summary>
    /// Gets <see cref="SerializationInfo"/> value for specified <paramref name="name"/>; otherwise <paramref name="defaultValue"/>.
    /// </summary>
    /// <typeparam name="T">Type of parameter to get from <see cref="SerializationInfo"/>.</typeparam>
    /// <param name="info"><see cref="SerializationInfo"/> object that contains deserialized values.</param>
    /// <param name="name">Name of deserialized parameter to retrieve.</param>
    /// <param name="defaultValue">Default value to return if <paramref name="name"/> does not exist or cannot be deserialized.</param>
    /// <returns>Value for specified <paramref name="name"/>; otherwise <paramref name="defaultValue"/></returns>
    /// <remarks>
    /// <see cref="SerializationInfo"/> do not have a direct way of determining if an item with a specified name exists, so when calling
    /// one of the Get(n) functions you will simply get a <see cref="SerializationException"/> if the parameter does not exist; similarly
    /// you will receive this exception if the parameter fails to properly deserialize. This extension method protects against both of
    /// these failures and returns a default value if the named parameter does not exist or cannot be deserialized.
    /// </remarks>
    public static T? GetOrDefault<T>(this SerializationInfo info, string name, T defaultValue)
    {
        try
        {
            return (T?)info.GetValue(name, typeof(T));
        }
        catch (SerializationException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Gets values of a <see cref="SerializationInfo"/> instance compatible with Linq operations.
    /// </summary>
    /// <param name="info">Target <see cref="SerializationInfo"/> instance.</param>
    /// <returns>Enumeration of <see cref="SerializationEntry"/> objects from <paramref name="info"/> instance.</returns>
    public static IEnumerable<SerializationEntry> GetValues(this SerializationInfo info)
    {
        SerializationInfoEnumerator enumerator = info.GetEnumerator();
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
