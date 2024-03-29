﻿//******************************************************************************************************
//  ChargeTest.cs - Gbtc
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
//  02/01/2011 - Denis Kholine
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

#region [ University of Illinois/NCSA Open Source License ]
/*
Copyright © <2012> <University of Illinois>
All rights reserved.

Developed by: <ITI>
<University of Illinois>
<http://www.iti.illinois.edu/>
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal with the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
• Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimers.
• Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimers in the documentation and/or other materials provided with the distribution.
• Neither the names of <Name of Development Group, Name of Institution>, nor the names of its contributors may be used to endorse or promote products derived from this Software without specific prior written permission.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS WITH THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using Gemstone.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemstone.Common.UnitTests.Units
{

    /// <summary>
    ///This is a test class for ChargeTest and is intended
    ///to contain all ChargeTest Unit Tests
    ///</summary>
    [TestClass]
    public class ChargeTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Charge Constructor
        ///</summary>
        [TestMethod]
        public void ChargeConstructorTest()
        {
            List<double> values = new();

            //Initialization
            values.Add(0);

            foreach (double value in values)
            {
                Charge target = new(value);
                Assert.IsInstanceOfType(target, typeof(Charge));
                Assert.IsNotNull(target);
            }

            values.Clear();

        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="double"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="double"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToDoubleEqualTest()
        {
            //equal
            Charge target = new(10F);
            double value = 10F;
            int expected = 0;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="double"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="double"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToDoubleLessTest()
        {
            Charge target = new(10F);
            double value = 9F;
            int expected = 1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="double"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="double"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToDoubleGreaterTest()
        {
            Charge target = new(10F);
            double value = 11F;
            int expected = -1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="Charge"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Charge"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToChargeEqualTest()
        {
            Charge target = new(10F);
            Charge value = new(10F);
            int expected = 0;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="Charge"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Charge"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToChargeLessTest()
        {
            Charge target = new(10F);
            Charge value = new(9F);
            int expected = 1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified <see cref="Charge"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Charge"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        [TestMethod]
        public void CompareToChargeGreaterTest()
        {
            Charge target = new(10F);
            Charge value = new(11F);
            int expected = -1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Charge"/>.</exception>
        [TestMethod]
        public void CompareToObjectEqualTest()
        {
            Charge target = new(10F);
            object value = new Charge(10F);
            int expected = 0;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Charge"/>.</exception>
        [TestMethod]
        public void CompareToObjectLessTest()
        {
            Charge target = new(10F);
            object value = new Charge(9F);
            int expected = 1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// A test for CompareTo
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Charge"/>.</exception>
        [TestMethod]
        public void CompareToObjectGreaterTest()
        {
            Charge target = new(10F);
            object value = new Charge(11F);
            int expected = -1;
            int actual = target.CompareTo(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for Equals
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare, or null.</param>
        /// <returns>
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Charge"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        [TestMethod]
        public void EqualsObjectTest()
        {
            Charge target = new(10F);
            object obj = 11F;
            bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for Equals
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Charge"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Charge"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        [TestMethod]
        public void EqualsChargeTest()
        {
            Charge target = new(10F);
            Charge obj = new(9F);
            bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for Equals
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Double"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Double"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        [TestMethod]
        public void EqualsDoubleTest()
        {
            Charge target = new(10F);
            double obj = 9F;
            bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        ///// <summary>
        ///// A test for FromAbcoulombs
        ///// Creates a new <see cref="Charge"/> value from the specified <paramref name="value"/> in abcoulombs.
        ///// </summary>
        ///// <param name="value">New <see cref="Charge"/> value in abcoulombs.</param>
        ///// <returns>New <see cref="Charge"/> object from the specified <paramref name="value"/> in abcoulombs.</returns>
        //[TestMethod]
        //public void FromAbcoulombsTest()
        //{
        //    double value = 10F;
        //    Charge expected = new Charge(10F);
        //    Charge actual = Charge.FromAbcoulombs(value);
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        ///// A test for FromAmpereHours
        ///// Creates a new <see cref="Charge"/> value from the specified <paramref name="value"/> in ampere-hours.
        ///// </summary>
        ///// <param name="value">New <see cref="Charge"/> value in ampere-hours.</param>
        ///// <returns>New <see cref="Charge"/> object from the specified <paramref name="value"/> in ampere-hours.</returns>
        //[TestMethod]
        //public void FromAmpereHoursTest()
        //{
        //    double value = 10F;
        //    Charge expected = new Charge(10);
        //    Charge actual = Charge.FromAmpereHours(value);
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        ///// A test for FromAtomicUnitsOfCharge
        ///// Creates a new <see cref="Charge"/> value from the specified <paramref name="value"/> in atomic units of charge.
        ///// </summary>
        ///// <param name="value">New <see cref="Charge"/> value in atomic units of charge.</param>
        ///// <returns>New <see cref="Charge"/> object from the specified <paramref name="value"/> in atomic units of charge.</returns>
        //[TestMethod]
        //public void FromAtomicUnitsOfChargeTest()
        //{
        //    double value = 10F;
        //    Charge expected = new Charge(10F);
        //    Charge actual = Charge.FromAtomicUnitsOfCharge(value);
        //    Assert.AreEqual(expected, actual);
        //    ///Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        ///// A test for FromFaraday
        ///// Creates a new <see cref="Charge"/> value from the specified <paramref name="value"/> in faraday.
        ///// </summary>
        ///// <param name="value">New <see cref="Charge"/> value in faraday.</param>
        ///// <returns>New <see cref="Charge"/> object from the specified <paramref name="value"/> in faraday.</returns>
        //[TestMethod]
        //public void FromFaradayTest()
        //{
        //    double value = 10F;
        //    Charge expected = new Charge(10F);
        //    Charge actual = Charge.FromFaraday(value);
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        ///// A test for FromStatcoulombs
        ///// Creates a new <see cref="Charge"/> value from the specified <paramref name="value"/> in statcoulombs.
        ///// </summary>
        ///// <param name="value">New <see cref="Charge"/> value in statcoulombs.</param>
        ///// <returns>New <see cref="Charge"/> object from the specified <paramref name="value"/> in statcoulombs.</returns>
        //[TestMethod]
        //public void FromStatcoulombsTest()
        //{
        //    double value = 10F;
        //    Charge expected = new Charge(10F);
        //    Charge actual = Charge.FromStatcoulombs(value);
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        ///// A test for GetHashCode
        ///// Returns the hash code for this instance.
        ///// </summary>
        ///// <returns>
        ///// A 32-bit signed integer hash code.
        ///// </returns>
        //[TestMethod]
        //public void GetHashCodeTest()
        //{
        //    Charge target = new Charge(10F);
        //    int expected = 10;
        //    int actual = target.GetHashCode();
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        /// A test for GetTypeCode
        /// Returns the <see cref="TypeCode"/> for value type <see cref="Double"/>.
        /// </summary>
        /// <returns>The enumerated constant, <see cref="TypeCode.Double"/>.</returns>
        [TestMethod]
        public void GetTypeCodeTest()
        {
            Charge target = new(10F);
            TypeCode expected = new();
            expected = TypeCode.Double;
            TypeCode actual = target.GetTypeCode();
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Charge"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// A <see cref="Charge"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Charge.MinValue"/> or greater than <see cref="Charge.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        [TestMethod]
        public void ParseStyleTest()
        {
            double value = 10F;
            string s = value.ToString();
            NumberStyles style = new();
            style = NumberStyles.Any;
            Charge expected = new(value);
            Charge actual = Charge.Parse(s, style);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Charge"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Charge"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Charge.MinValue"/> or greater than <see cref="Charge.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        [TestMethod]
        public void ParseStyleProviderTest()
        {
            double value = 10F;
            string s = value.ToString();
            NumberStyles style = new();
            IFormatProvider provider = null; // TODO: Initialize to an appropriate value
            Charge expected = new(10F);
            Charge actual = Charge.Parse(s, style, provider);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Charge"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Charge"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Charge.MinValue"/> or greater than <see cref="Charge.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        [TestMethod]
        public void ParseProviderTest()
        {
            double value = 10F;
            string s = value.ToString();
            IFormatProvider provider = null; // TODO: Initialize to an appropriate value
            Charge expected = new(10F);
            Charge actual = Charge.Parse(s, provider);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Charge"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// A <see cref="Charge"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Charge.MinValue"/> or greater than <see cref="Charge.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        [TestMethod]
        public void ParseStringTest()
        {
            double value = 10F;
            string s = value.ToString();
            Charge expected = new(10F);
            Charge actual = Charge.Parse(s);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for System.IConvertible.ToBoolean
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToBooleanTest()
        {
            IConvertible target = new Charge(20F);
            IFormatProvider provider = null;
            bool expected = true;
            bool actual = target.ToBoolean(provider);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for System.IConvertible.ToByte
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToByteTest()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            byte expected = 10;
            byte actual = target.ToByte(provider);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Convertion to byte from Charge is limited use with caution");
        }

//        /// <summary>
//        ///A test for System.IConvertible.ToChar
//        ///</summary>
//        [TestMethod]
//        [DeploymentItem("Gemstone.Common.dll")]
//        public void ToCharTest()
//        {
//            /*
//            IConvertible target = new Charge(1F);
//            IFormatProvider provider = null;
//            char expected = '\0';
//            char actual;
//            actual = target.ToChar(provider);
//            Assert.AreEqual(expected, actual);
//            */
//            Assert.Inconclusive("Invalid cast from 'Double' to 'Char'.");
//        }

//        /// <summary>
//        ///A test for System.IConvertible.ToDateTime
//        ///</summary>
//        [TestMethod]
//        [DeploymentItem("Gemstone.Common.dll")]
//        public void ToDateTimeTest()
//        {/*
//IConvertible target = new Charge(10F); // TODO: Initialize to an appropriate value
//IFormatProvider provider = null; // TODO: Initialize to an appropriate value
//DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
//DateTime actual;
//actual = target.ToDateTime(provider);
//Assert.AreEqual(expected, actual);
//*/
//            Assert.Inconclusive("Invalid cast from 'Double' to 'DateTime'.");
//        }

        /// <summary>
        ///A test for System.IConvertible.ToDecimal
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToDecimalTest()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            decimal expected = new(10F);
            decimal actual = target.ToDecimal(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToDouble
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToDoubleTest()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            double expected = 10F;
            double actual = target.ToDouble(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToInt16
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToInt16Test()
        {
            IConvertible target = new Charge(10.16568F);
            IFormatProvider provider = null;
            short expected = 10;
            short actual = target.ToInt16(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToInt32
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToInt32Test()
        {
            IConvertible target = new Charge(10.2324F);
            IFormatProvider provider = null;
            int expected = 10;
            int actual = target.ToInt32(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToInt64
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToInt64Test()
        {
            IConvertible target = new Charge(10.3423F);
            IFormatProvider provider = null;
            long expected = 10;
            long actual = target.ToInt64(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToSByte
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToSByteTest()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            sbyte expected = 10;
            sbyte actual = target.ToSByte(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToSingle
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToSingleTest()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            float expected = 10F;
            float actual = target.ToSingle(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToType
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToTypeTest()
        {
            IConvertible target = new Charge(10F);
            Type conversionType = typeof(double);
            IFormatProvider provider = null;
            object expected = (double)10;
            object actual = target.ToType(conversionType, provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToUInt16
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToUInt16Test()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            ushort expected = 10;
            ushort actual = target.ToUInt16(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToUInt32
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToUInt32Test()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            uint expected = 10;
            uint actual = target.ToUInt32(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.IConvertible.ToUInt64
        ///</summary>
        [TestMethod]
        [DeploymentItem("Gemstone.Common.dll")]
        public void ToUInt64Test()
        {
            IConvertible target = new Charge(10F);
            IFormatProvider provider = null;
            ulong expected = 10;
            ulong actual = target.ToUInt64(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToAbcoulombs
        ///</summary>
        [TestMethod]
        public void ToAbcoulombsTest()
        {
            Charge target = new(10F);
            double expected = 1F;
            double actual = target.ToAbcoulombs();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToAmpereHours
        ///</summary>
        [TestMethod]
        public void ToAmpereHoursTest()
        {
            Charge target = new(10F);
            double expected = 0.0027777777777777779;
            double actual = target.ToAmpereHours();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToAtomicUnitsOfCharge
        ///</summary>
        [TestMethod]
        public void ToAtomicUnitsOfChargeTest()
        {
            Charge target = new(1F);
            double expected = 6.2415097445115249E+18;
            double actual = target.ToAtomicUnitsOfCharge();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToFaraday
        ///</summary>
        [TestMethod]
        public void ToFaradayTest()
        {
            Charge target = new(10F);
            double expected = 0.00010364268992774003;
            double actual = target.ToFaraday();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToStatcoulombs
        ///</summary>
        [TestMethod]
        public void ToStatcoulombsTest()
        {
            Charge target = new(10F);
            double expected = 29979245368.431435;
            double actual = target.ToStatcoulombs();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ToString
        /// Converts the numeric value of this instance to its equivalent string representation using the
        /// specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A format specification.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// The string representation of the value of this instance as specified by format and provider.
        /// </returns>
        [TestMethod]
        public void ToStringFormatProviderTest()
        {
            double value = 10;
            Charge target = new(10F);
            string format = string.Empty;
            IFormatProvider provider = null;
            string expected = value.ToString();
            string actual = target.ToString(format, provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ToString
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the value of this instance, consisting of a minus sign if
        /// the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.
        /// </returns>
        [TestMethod]
        public void ToStringTest()
        {
            double value = 10F;
            Charge target = new(10F);
            string expected = value.ToString();
            string actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ToString
        /// Converts the numeric value of this instance to its equivalent string representation using the
        /// specified culture-specific format information.
        /// </summary>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// The string representation of the value of this instance as specified by provider.
        /// </returns>
        [TestMethod]
        public void ToStringProviderTest()
        {
            double value = 10F;
            Charge target = new(10F);
            IFormatProvider provider = null;
            string expected = value.ToString();
            string actual = target.ToString(provider);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for ToString
        /// Converts the numeric value of this instance to its equivalent string representation, using
        /// the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>
        /// The string representation of the value of this instance as specified by format.
        /// </returns>
        [TestMethod]
        public void ToStringFormatTest()
        {
            double value = 10F;
            Charge target = new(10F);
            string format = string.Empty;
            string expected = value.ToString();
            string actual = target.ToString(format);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for TryParse
        /// Converts the string representation of a number to its <see cref="Charge"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Charge"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s paracoulomb is null,
        /// is not of the correct format, or represents a number less than <see cref="Charge.MinValue"/> or greater than <see cref="Charge.MaxValue"/>.
        /// This paracoulomb is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        [TestMethod]
        public void TryParseTest()
        {
            string s = "10";
            Charge result = new(10F);
            Charge resultExpected = new(10F);
            bool expected = true;
            bool actual = Charge.TryParse(s, out result);
            Assert.AreEqual(resultExpected, result);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for TryParse
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Charge"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Charge"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s paracoulomb is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Charge.MinValue"/> or
        /// greater than <see cref="Charge.MaxValue"/>. This paracoulomb is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        [TestMethod]
        public void TryParseStyleProviderTest()
        {
            string s = "10";
            NumberStyles style = new();
            IFormatProvider provider = null;
            Charge result = new(10F);
            Charge resultExpected = new(10F);
            bool expected = true;
            bool actual = Charge.TryParse(s, style, provider, out result);
            Assert.AreEqual(resultExpected, result);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod]
        public void op_AdditionTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(10F);
            Charge expected = new(20F);
            Charge actual = value1 + value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Division
        ///</summary>
        [TestMethod]
        public void op_DivisionTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(10F);
            Charge expected = new(1F);
            Charge actual = value1 / value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod]
        public void op_EqualityTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(10F);
            bool expected = true;
            bool actual = value1 == value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Exponent
        ///</summary>
        [TestMethod]
        public void op_ExponentTest()
        {
            Charge value1 = new(2F);
            Charge value2 = new(3F);
            double expected = 8F;
            double actual = Charge.op_Exponent(value1, value2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_GreaterThan
        ///</summary>
        [TestMethod]
        public void op_GreaterThanTest()
        {
            Charge value1 = new(11F);
            Charge value2 = new(10F);
            bool expected = true;
            bool actual = value1 > value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_GreaterThanOrEqual
        ///</summary>
        [TestMethod]
        public void op_GreaterThanOrEqualTest()
        {
            Charge value1 = new(10);
            Charge value2 = new(10);
            bool expected = true;
            bool actual = value1 >= value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod]
        public void op_ImplicitTest()
        {
            double value = 10F;
            Charge expected = new(10F);
            Charge actual = value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod]
        public void op_ImplicitTest1()
        {
            Charge value = new(10);
            double expected = 10F;
            double actual = value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod]
        public void op_InequalityTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(12F);
            bool expected = true;
            bool actual = value1 != value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_LessThan
        ///</summary>
        [TestMethod]
        public void op_LessThanTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(12F);
            bool expected = true;
            bool actual = value1 < value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_LessThanOrEqual
        ///</summary>
        [TestMethod]
        public void op_LessThanOrEqualTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(11F);
            bool expected = true;
            bool actual = value1 <= value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Modulus
        ///</summary>
        [TestMethod]
        public void op_ModulusTest()
        {
            Charge value1 = new(10D);
            Charge value2 = new(10D);
            Charge expected = new(0D);
            Charge actual = value1 % value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Multiply
        ///</summary>
        [TestMethod]
        public void op_MultiplyTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(10F);
            Charge expected = new(100F);
            Charge actual = value1 * value2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Subtraction
        ///</summary>
        [TestMethod]
        public void op_SubtractionTest()
        {
            Charge value1 = new(10F);
            Charge value2 = new(10F);
            Charge expected = new(0F);
            Charge actual = value1 - value2;
            Assert.AreEqual(expected, actual);
        }
    }
}
