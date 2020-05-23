//******************************************************************************************************
//  Common.cs - Gbtc
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
//  04/03/2006 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/13/2007 - Darrell Zuercher
//       Edited code comments.
//  09/08/2008 - J. Ritchie Carroll
//       Converted to C#.
//  02/13/2009 - Josh L. Patterson
//       Edited Code Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  09/17/2009 - Pinal C. Patel
//       Modified GetApplicationType() to remove dependency on HttpContext.Current.
//  09/28/2010 - Pinal C. Patel
//       Cached the current ApplicationType returned by GetApplicationType() for better performance.
//  12/05/2010 - Pinal C. Patel
//       Added an overload for TypeConvertToString() that takes CultureInfo as a parameter.
//  12/07/2010 - Pinal C. Patel
//       Updated TypeConvertToString() to return an empty string if the passed in value is null.
//  03/09/2011 - Pinal C. Patel
//       Moved UpdateType enumeration from GSF.Services.ServiceProcess namespace for broader usage.
//  04/07/2011 - J. Ritchie Carroll
//       Added ToNonNullNorEmptyString() and ToNonNullNorWhiteSpace() extensions.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Gemstone.Collections.CollectionExtensions;
using Gemstone.Console;
using Gemstone.IO;
using Gemstone.StringExtensions;
using Microsoft.Win32;

#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable IDE1006 // Naming Styles

namespace Gemstone
{
    /// <summary>
    /// Defines common global functions.
    /// </summary>
    public static class Common
    {
        private static string? s_osPlatformName;
        private static PlatformID s_osPlatformID = PlatformID.Win32S;

        /// <summary>
        /// Determines if the current system is a POSIX style environment.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since a .NET application compiled under Mono can run under both Windows and Unix style platforms,
        /// you can use this property to easily determine the current operating environment.
        /// </para>
        /// <para>
        /// This property will return <c>true</c> for both MacOSX and Unix environments. Use the Platform property
        /// of the <see cref="System.Environment.OSVersion"/> to determine more specific platform type, e.g., 
        /// MacOSX or Unix. Note that all flavors of Linux will show up as <see cref="PlatformID.Unix"/>.
        /// </para>
        /// </remarks>        
        public static readonly bool IsPosixEnvironment = Path.DirectorySeparatorChar == '/';   // This is how Mono source often checks this

        /// <summary>
        /// Determines if the code base is currently running under Mono.
        /// </summary>
        /// <remarks>
        /// This property can be used to make a run-time determination if Mono based .NET is being used. However, it is
        /// recommended to use a MONO compiler directive wherever possible instead of determining this at run-time.
        /// </remarks>
        public static bool IsMono = Type.GetType("Mono.Runtime") != null;

        /// <summary>Returns one of two strongly-typed objects.</summary>
        /// <returns>One of two objects, depending on the evaluation of given expression.</returns>
        /// <param name="expression">The expression you want to evaluate.</param>
        /// <param name="truePart">Returned if expression evaluates to True.</param>
        /// <param name="falsePart">Returned if expression evaluates to False.</param>
        /// <typeparam name="T">Return type used for immediate expression</typeparam>
        /// <remarks>
        /// <para>This function acts as a strongly-typed immediate if (a.k.a. inline if).</para>
        /// <para>
        /// It is expected that this function will only be used in languages that do not support ?: conditional operations, e.g., Visual Basic.NET.
        /// In Visual Basic this function can be used as a strongly-typed IIf replacement by specifying "Imports gemstone.common".
        /// </para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once InconsistentNaming
        public static T IIf<T>(bool expression, T truePart, T falsePart) => expression ? truePart : falsePart;

        /// <summary>Creates a strongly-typed Array.</summary>
        /// <returns>New array of specified type.</returns>
        /// <param name="length">Desired length of new array.</param>
        /// <typeparam name="T">Return type for new array.</typeparam>
        /// <remarks>
        /// <para>It is expected that this function will only be used in Visual Basic.NET.</para>
        /// <para>
        /// The Array.CreateInstance provides better performance and more direct CLR access for array creation (not to
        /// mention less confusion on the matter of array lengths) in VB.NET, however the returned System.Array is not
        /// typed properly. This function properly casts the return array based on the type specification helping
        /// when Option Strict is enabled.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="VB">
        ///     Dim buffer As Byte() = CreateArray(Of Byte)(12)
        ///     Dim matrix As Integer()() = CreateArray(Of Integer())(10)
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] CreateArray<T>(int length) => new T[length];

        /// <summary>Creates a strongly-typed Array with an initial value parameter.</summary>
        /// <returns>New array of specified type.</returns>
        /// <param name="length">Desired length of new array.</param>
        /// <param name="initialValue">Value used to initialize all array elements.</param>
        /// <typeparam name="T">Return type for new array.</typeparam>
        /// <remarks>
        /// It is expected that this function will only be used in Visual Basic.NET.
        /// </remarks>
        /// <example>
        /// <code language="VB">
        ///     Dim elements As Integer() = CreateArray(12, -1)
        ///     Dim names As String() = CreateArray(100, "undefined")
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] CreateArray<T>(int length, T initialValue)
        {
            T[] typedArray = CreateArray<T>(length);

            // Initializes all elements with the default value.
            for (int x = 0; x < typedArray.Length; x++)
                typedArray[x] = initialValue;

            return typedArray;
        }

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="string"/> using an appropriate <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="value">Value to convert to a <see cref="string"/>.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="string"/>.</returns>
        /// <remarks>
        /// <para>
        /// If <see cref="TypeConverter"/> fails, the value's <c>ToString()</c> value will be returned.
        /// Returned value will never be null, if no value exists an empty string ("") will be returned.
        /// </para>
        /// <para>
        /// You can use the <see cref="Gemstone.StringExtensions.StringExtensions.ConvertToType{T}(string)"/> string extension
        /// method or <see cref="TypeConvertFromString(string, Type)"/> to convert the string back to its
        /// original <see cref="Type"/>.
        /// </para>
        /// </remarks>
        public static string TypeConvertToString(object value) => TypeConvertToString(value, null);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="string"/> using an appropriate <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="value">Value to convert to a <see cref="string"/>.</param>
        /// <param name="culture"><see cref="CultureInfo"/> to use for the conversion.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="string"/>.</returns>
        /// <remarks>
        /// <para>
        /// If <see cref="TypeConverter"/> fails, the value's <c>ToString()</c> value will be returned.
        /// Returned value will never be null, if no value exists an empty string ("") will be returned.
        /// </para>
        /// <para>
        /// You can use the <see cref="Gemstone.StringExtensions.StringExtensions.ConvertToType{T}(string, CultureInfo)"/> string
        /// extension method or <see cref="TypeConvertFromString(string, Type, CultureInfo)"/> to convert
        /// the string back to its original <see cref="Type"/>.
        /// </para>
        /// </remarks>
        public static string TypeConvertToString(object value, CultureInfo? culture)
        {
            // Don't proceed further if value is null.
            if (value == null)
                return string.Empty;

            // If value is already a string, no need to attempt conversion
            if (value is string stringVal)
                return stringVal;

            // Initialize culture info if not specified.
            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            try
            {
                // Attempt to use type converter to set field value
                TypeConverter converter = TypeDescriptor.GetConverter(value);

                // ReSharper disable once AssignNullToNotNullAttribute
                return converter.ConvertToString(null, culture, value).ToNonNullString();
            }
            catch
            {
                // Otherwise just call object's ToString method
                return value.ToNonNullString();
            }
        }

        /// <summary>
        /// Converts this string into the specified type.
        /// </summary>
        /// <param name="value">Source string to convert to type.</param>
        /// <param name="type"><see cref="Type"/> to convert string to.</param>
        /// <returns>
        /// <see cref="string"/> converted to specified <see cref="Type"/>; default value of
        /// specified type if conversion fails.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This function makes use of a <see cref="TypeConverter"/> to convert <paramref name="value"/>
        /// to the specified <paramref name="type"/>, the best way to make sure <paramref name="value"/>
        /// can be converted back to its original type is to use the same <see cref="TypeConverter"/> to
        /// convert the original object to a <see cref="string"/>; see the
        /// <see cref="TypeConvertToString(object)"/> method for an easy way to do this.
        /// </para>
        /// <para>
        /// This function varies from <see cref="Gemstone.StringExtensions.StringExtensions.ConvertToType{T}(string)"/>  in that it
        /// will use the default value for the <paramref name="type"/> parameter if <paramref name="value"/>
        /// is empty or <c>null</c>.
        /// </para>
        /// </remarks>
        public static object? TypeConvertFromString(string value, Type type) => TypeConvertFromString(value, type, null);

        /// <summary>
        /// Converts this string into the specified type.
        /// </summary>
        /// <param name="value">Source string to convert to type.</param>
        /// <param name="type"><see cref="Type"/> to convert string to.</param>
        /// <param name="culture"><see cref="CultureInfo"/> to use for the conversion.</param>
        /// <returns>
        /// <see cref="string"/> converted to specified <see cref="Type"/>; default value of
        /// specified type if conversion fails.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This function makes use of a <see cref="TypeConverter"/> to convert <paramref name="value"/>
        /// to the specified <paramref name="type"/>, the best way to make sure <paramref name="value"/>
        /// can be converted back to its original type is to use the same <see cref="TypeConverter"/> to
        /// convert the original object to a <see cref="string"/>; see the
        /// <see cref="TypeConvertToString(object)"/> method for an easy way to do this.
        /// </para>
        /// <para>
        /// This function varies from <see cref="Gemstone.StringExtensions.StringExtensions.ConvertToType{T}(string, CultureInfo)"/>
        /// in that it will use the default value for the <paramref name="type"/> parameter if
        /// <paramref name="value"/> is empty or <c>null</c>.
        /// </para>
        /// </remarks>
        public static object? TypeConvertFromString(string value, Type type, CultureInfo? culture)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = Activator.CreateInstance(type).ToString();

            return value.ConvertToType(type, culture);
        }

        /// <summary>Determines if given item is equal to its default value (e.g., null or 0.0).</summary>
        /// <param name="item">Object to evaluate.</param>
        /// <returns>Result of evaluation as a <see cref="bool"/>.</returns>
        /// <remarks>
        /// Native types default to zero, not null, therefore this can be used to evaluate if an item is its default (i.e., uninitialized) value.
        /// </remarks>
        public static bool IsDefaultValue(object item)
        {
            // Only reference types can be null, therefore null is its default value
            if (item == null)
                return true;

            Type itemType = item.GetType();

            if (!itemType.IsValueType)
                return false;

            // Handle common types
            if (item is IConvertible convertible)
            {
                try
                {
                    switch (convertible.GetTypeCode())
                    {
                        case TypeCode.Boolean:
                            return (bool)item == default;
                        case TypeCode.SByte:
                            return (sbyte)item == default(sbyte);
                        case TypeCode.Byte:
                            return (byte)item == default(byte);
                        case TypeCode.Int16:
                            return (short)item == default(short);
                        case TypeCode.UInt16:
                            return (ushort)item == default(ushort);
                        case TypeCode.Int32:
                            return (int)item == default;
                        case TypeCode.UInt32:
                            return (uint)item == default;
                        case TypeCode.Int64:
                            return (long)item == default;
                        case TypeCode.UInt64:
                            return (ulong)item == default;
                        case TypeCode.Single:
                            return (float)item == default;
                        case TypeCode.Double:
                            return (double)item == default;
                        case TypeCode.Decimal:
                            return (decimal)item == default;
                        case TypeCode.Char:
                            return (char)item == default(char);
                        case TypeCode.DateTime:
                            return (DateTime)item == default;
                    }
                }
                catch (InvalidCastException)
                {
                    // An exception here indicates that the item is a custom type that
                    // lied about its type code. The type should still be instantiable,
                    // so we can ignore this exception
                }
            }

            // Handle custom value types
            return ((ValueType)item).Equals(Activator.CreateInstance(itemType));
        }

        /// <summary>Determines if given item is a reference type.</summary>
        /// <param name="item">Object to evaluate.</param>
        /// <returns>Result of evaluation as a <see cref="bool"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReference(object item) => !(item is ValueType);

        /// <summary>Determines if given item is a reference type but not a string.</summary>
        /// <param name="item">Object to evaluate.</param>
        /// <returns>Result of evaluation as a <see cref="bool"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNonStringReference(object item) => IsReference(item) && !(item is string);

        /// <summary>
        /// Determines if <paramref name="typeCode"/> is a numeric type, i.e., one of:
        /// <see cref="TypeCode.Boolean"/>, <see cref="TypeCode.SByte"/>, <see cref="TypeCode.Byte"/>,
        /// <see cref="TypeCode.Int16"/>, <see cref="TypeCode.UInt16"/>, <see cref="TypeCode.Int32"/>,
        /// <see cref="TypeCode.UInt32"/>, <see cref="TypeCode.Int64"/>, <see cref="TypeCode.UInt64"/>
        /// <see cref="TypeCode.Single"/>, <see cref="TypeCode.Double"/> or <see cref="TypeCode.Decimal"/>.
        /// </summary>
        /// <param name="typeCode"><see cref="TypeCode"/> value to check.</param>
        /// <returns><c>true</c> if <paramref name="typeCode"/> is a numeric type; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumericType(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Boolean => true,
                TypeCode.SByte => true,
                TypeCode.Byte => true,
                TypeCode.Int16 => true,
                TypeCode.UInt16 => true,
                TypeCode.Int32 => true,
                TypeCode.UInt32 => true,
                TypeCode.Int64 => true,
                TypeCode.UInt64 => true,
                TypeCode.Single => true,
                TypeCode.Double => true,
                TypeCode.Decimal => true,
                _ => false
            };
        }

        /// <summary>
        /// Determines if <paramref name="type"/> is a numeric type, i.e., has a <see cref="TypeCode"/> that is one of:
        /// <see cref="TypeCode.Boolean"/>, <see cref="TypeCode.SByte"/>, <see cref="TypeCode.Byte"/>,
        /// <see cref="TypeCode.Int16"/>, <see cref="TypeCode.UInt16"/>, <see cref="TypeCode.Int32"/>,
        /// <see cref="TypeCode.UInt32"/>, <see cref="TypeCode.Int64"/>, <see cref="TypeCode.UInt64"/>
        /// <see cref="TypeCode.Single"/>, <see cref="TypeCode.Double"/> or <see cref="TypeCode.Decimal"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="type"/> is a numeric type; otherwise, <c>false</c>.</returns>
        public static bool IsNumericType(Type type) => IsNumericType(Type.GetTypeCode(type));

        /// <summary>
        /// Determines if <typeparamref name="T"/> is a numeric type, i.e., has a <see cref="TypeCode"/> that is one of:
        /// <see cref="TypeCode.Boolean"/>, <see cref="TypeCode.SByte"/>, <see cref="TypeCode.Byte"/>,
        /// <see cref="TypeCode.Int16"/>, <see cref="TypeCode.UInt16"/>, <see cref="TypeCode.Int32"/>,
        /// <see cref="TypeCode.UInt32"/>, <see cref="TypeCode.Int64"/>, <see cref="TypeCode.UInt64"/>
        /// <see cref="TypeCode.Single"/>, <see cref="TypeCode.Double"/> or <see cref="TypeCode.Decimal"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
        /// <returns><c>true</c> if <typeparamref name="T"/> is a numeric type; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsNumericType<T>() => IsNumericType(Type.GetTypeCode(typeof(T)));

        /// <summary>
        /// Determines if <see cref="Type"/> of <paramref name="item"/> is a numeric type, i.e., <paramref name="item"/>
        /// is <see cref="IConvertible"/> and has a <see cref="TypeCode"/> that is one of:
        /// <see cref="TypeCode.Boolean"/>, <see cref="TypeCode.SByte"/>, <see cref="TypeCode.Byte"/>,
        /// <see cref="TypeCode.Int16"/>, <see cref="TypeCode.UInt16"/>, <see cref="TypeCode.Int32"/>,
        /// <see cref="TypeCode.UInt32"/>, <see cref="TypeCode.Int64"/>, <see cref="TypeCode.UInt64"/>
        /// <see cref="TypeCode.Single"/>, <see cref="TypeCode.Double"/> or <see cref="TypeCode.Decimal"/>.
        /// </summary>
        /// <param name="item">Object to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is a numeric type; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumericType(object item) => item is IConvertible convertible && IsNumericType(convertible.GetTypeCode());

        /// <summary>
        /// Determines if given <paramref name="item"/> is or can be interpreted as numeric.
        /// </summary>
        /// <param name="item">Object to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is or can be interpreted as numeric; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// If type of <paramref name="item"/> is a <see cref="char"/> or a <see cref="string"/>, then if value can be parsed as a numeric
        /// value, result will be <c>true</c>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumeric(object item) => IsNumericType(item) || (item is char || item is string) && decimal.TryParse(item.ToString(), out _);

        /// <summary>Returns the smallest item from a list of parameters.</summary>
        /// <typeparam name="T">Return type <see cref="Type"/> that is the minimum value in the <paramref name="itemList"/>.</typeparam>
        /// <param name="itemList">A variable number of parameters of the specified type.</param>
        /// <returns>Result is the minimum value of type <see cref="Type"/> in the <paramref name="itemList"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(params T[] itemList) => itemList.Min();

        /// <summary>Returns the largest item from a list of parameters.</summary>
        /// <typeparam name="T">Return type <see cref="Type"/> that is the maximum value in the <paramref name="itemList"/>.</typeparam>
        /// <param name="itemList">A variable number of parameters of the specified type .</param>
        /// <returns>Result is the maximum value of type <see cref="Type"/> in the <paramref name="itemList"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(params T[] itemList) => itemList.Max();

        /// <summary>Returns the value that is neither the largest nor the smallest.</summary>
        /// <typeparam name="T"><see cref="Type"/> of the objects passed to and returned from this method.</typeparam>
        /// <param name="value1">Value 1.</param>
        /// <param name="value2">Value 2.</param>
        /// <param name="value3">Value 3.</param>
        /// <returns>Result is the value that is neither the largest nor the smallest.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Mid<T>(T value1, T value2, T value3) where T : IComparable<T>
        {
            if (value1 == null)
                throw new ArgumentNullException(nameof(value1));

            if (value2 == null)
                throw new ArgumentNullException(nameof(value2));

            if (value3 == null)
                throw new ArgumentNullException(nameof(value3));

            int comp1To2 = value1.CompareTo(value2);
            int comp1To3 = value1.CompareTo(value3);
            int comp2To3 = value2.CompareTo(value3);

            // If 3 is the smallest, pick the smaller of 1 and 2
            if (comp1To3 >= 0 && comp2To3 >= 0)
                return comp1To2 <= 0 ? value1 : value2;

            // If 2 is the smallest, pick the smaller of 1 and 3
            if (comp1To2 >= 0 && comp2To3 <= 0)
                return comp1To3 <= 0 ? value1 : value3;

            // 1 is the smallest so pick the smaller of 2 and 3
            return comp2To3 <= 0 ? value2 : value3;
        }

        /// <summary>
        /// Gets the operating system <see cref="PlatformID"/>
        /// </summary>
        /// <returns>The operating system <see cref="PlatformID"/>.</returns>
        /// <remarks>
        /// This function will properly detect the platform ID, even if running on Mac.
        /// </remarks>
        // ReSharper disable once InconsistentNaming
        public static PlatformID GetOSPlatformID()
        {
            if (s_osPlatformID != PlatformID.Win32S)
                return s_osPlatformID;

            s_osPlatformID = Environment.OSVersion.Platform;

            if (s_osPlatformID == PlatformID.Unix)
            {
                // Environment.OSVersion.Platform can report Unix when running on Mac OS X
                try
                {
                    s_osPlatformID = Command.Execute("uname").StandardOutput.StartsWith("Darwin", StringComparison.OrdinalIgnoreCase) ? PlatformID.MacOSX : PlatformID.Unix;
                }
                catch
                {
                    // Fall back on looking for Mac specific root folders:
                    if (Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/Users") && Directory.Exists("/Volumes"))
                        s_osPlatformID = PlatformID.MacOSX;
                }
            }

            return s_osPlatformID;
        }


        /// <summary>
        /// Gets the operating system product name.
        /// </summary>
        /// <returns>Operating system product name.</returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        // ReSharper disable once InconsistentNaming
        public static string GetOSProductName()
        {
            if (s_osPlatformName != null)
                return s_osPlatformName;

            switch (GetOSPlatformID())
            {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    // Call sw_vers on Mac to get product name and version information, Linux could have this
                    try
                    {
                        string output = Command.Execute("sw_vers").StandardOutput;
                        Dictionary<string, string> kvps = output.ParseKeyValuePairs('\n', ':');
                        if (kvps.Count > 0)
                            s_osPlatformName = kvps.Values.Select(val => val.Trim()).ToDelimitedString(" ");
                    }
                    catch
                    {
                        s_osPlatformName = null;
                    }

                    if (string.IsNullOrEmpty(s_osPlatformName))
                    {
                        // Try some common ways to get product name on Linux, some might work on Mac
                        try
                        {
                            foreach (string fileName in FilePath.GetFileList("/etc/*release*"))
                            {
                                using (StreamReader reader = new StreamReader(fileName))
                                {
                                    string line = reader.ReadLine();

                                    while (line != null)
                                    {
                                        if (line.StartsWith("PRETTY_NAME", StringComparison.OrdinalIgnoreCase) && !line.Contains('#'))
                                        {
                                            string[] parts = line.Split('=');

                                            if (parts.Length == 2)
                                            {
                                                s_osPlatformName = parts[1].Replace("\"", "");
                                                break;
                                            }
                                        }

                                        line = reader.ReadLine();
                                    }
                                }

                                if (!string.IsNullOrEmpty(s_osPlatformName))
                                    break;
                            }
                        }
                        catch
                        {
                            try
                            {
                                string output = Command.Execute("lsb_release", "-a").StandardOutput;
                                Dictionary<string, string> kvps = output.ParseKeyValuePairs('\n', ':');
                                if (kvps.TryGetValue("Description", out s_osPlatformName) && !string.IsNullOrEmpty(s_osPlatformName))
                                    s_osPlatformName = s_osPlatformName.Trim();

                            }
                            catch
                            {
                                s_osPlatformName = null;
                            }
                        }
                    }
                    break;
                default:
                    // Get Windows product name
                    try
                    {
                        s_osPlatformName = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", null).ToString();
                    }
                    catch
                    {
                        s_osPlatformName = null;
                    }
                    break;
            }

            if (string.IsNullOrWhiteSpace(s_osPlatformName))
                s_osPlatformName = GetOSPlatformID().ToString();

            if (IsMono)
                s_osPlatformName += " using Mono";

            return s_osPlatformName ?? "Undetermined";
        }
    }
}
