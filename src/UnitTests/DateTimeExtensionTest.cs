//******************************************************************************************************
//  DateTimeExtensionTest.cs - Gbtc
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
//  03/10/2011 - Aniket Salver
//       Generated original version of source code.
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gemstone.DateTimeExtensions;

namespace Gemstone.Common.UnitTests
{
    [TestClass]
    public class DateTimeExtensionTest
    {
        private readonly double lagTime = 60 * 10;
        private readonly double leadTime = 60 * 10;
        private readonly BaselineTimeInterval dayInterval = BaselineTimeInterval.Day;
        private readonly DateTime utcTime = DateTime.UtcNow;
        private readonly DateTime localTime;

        public DateTimeExtensionTest()
        {
            localTime = utcTime.ToLocalTime();
        }

        // This method will Determines if the specified UTC time is valid or not, by comparing it to the system clock time
        // and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void UtcTimeIsValid_ValidCase()
        {
            // Act
            bool result = localTime.UtcTimeIsValid(lagTime, leadTime);
            // Assert
            Assert.AreEqual(false, result);

        }

        // This method will Determines if the specified UTC time is valid or not, by comparing it to the system clock time
        // and returns boolean variable as true for valid case and test will pass.
        [TestMethod]
        public void UtcTimeIsValid_InValidCase()
        {
            // Act
            bool result = utcTime.UtcTimeIsValid(lagTime, leadTime);
            // Assert
            Assert.AreEqual(true, result);
        }

        //  This method will Determines if the specified local time is valid or not, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]

        public void LocalTimeIsValid_ValidCase()
        {
            // Act
            bool result = utcTime.LocalTimeIsValid(lagTime, leadTime);
            // Assert
            Assert.AreEqual(false, result);
        }

        //  This method will Determines if the specified local time is valid or not, by comparing it to the system clock time
        //  and returns boolean variable as True for valid case and test will pass.
        [TestMethod]
        public void LocalTimeIsValid_InValidCases()
        {
            // Act
            bool result = localTime.LocalTimeIsValid(lagTime, leadTime);
            // Assert
            Assert.AreEqual(true, result);
        }


        // This method will Determines if the specified time is valid, by comparing valid or not, by comparing it to the system clock time
        //  and returns boolean variable as False for valid case and test will pass.
        [TestMethod]
        public void TimeIsValid_ValidCase()
        {
            // Act
            bool result = utcTime.TimeIsValid(localTime, lagTime, leadTime);
            // Assert
            Assert.AreEqual(false, result);
        }


        // This method will Determines if the specified time is valid, by comparing valid or not, by comparing it to the system clock time
        //  and returns boolean variable as True for valid case and test will pass.
        [TestMethod]
        public void TimeIsValid_InValidCase()
        {
            // Act 
            bool result = localTime.TimeIsValid(localTime, lagTime, leadTime);
            // Assert
            Assert.AreEqual(true, result);
        }

        // This method will creates a baselined timestamp which begins at the specified time interval,by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void BaselinedTimestamp_ValidCase()
        {
            // Act
            DateTime result = localTime.BaselinedTimestamp(dayInterval);
            // Assert
            Assert.AreEqual(DateTime.Today, result);
        }

        //  This method Converts given local time to Central timee, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void LocalTimeToCentralTime_Isvalid()
        {
            // Act
            DateTime result = localTime.LocalTimeToCentralTime();
            double offset = -6.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -5.0;
            
            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);

        }

        // This method Converts given local time to Central timee, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass.
        [TestMethod]
        public void LocalTimeToCentralTime_InValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToCentralTime();
            double offset = -6.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -5.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
            
        }

        // This method Converts given local time to Mountain time, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void LocalTimeToMountainTime_ValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToMountainTime();
            double offset = -7.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -6.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
            
        }

        //  This method Converts given local time to Mountain time, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass.

        [TestMethod]
        public void LocalTimeToMountainTime_InValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToMountainTime();
            double offset = -7.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -6.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);

        }

        // This method Converts given local time to Pacific time, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void LocalTimeToPacificTime_ValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToPacificTime();
            double offset = -8.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -7.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts given local time to Pacific time, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass.
        [TestMethod]
        public void LocalTimeToPacificTime_InValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToPacificTime();
            double offset = -8.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -7.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts given local time to Universally Coordinated Time (a.k.a., Greenwich Meridian Time), by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void LocalTimeToUniversalTime_ValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToUniversalTime();
            // Assert
            Assert.AreEqual(utcTime, result);

        }

        ////  This method Converts given local time to Universally Coordinated Time (a.k.a., Greenwich Meridian Time), by comparing it to the system clock time
        ////  and returns boolean variable as true for valid case and test will pass
        //[TestMethod]
        //public void LocalTimeToUniversalTime_InValidCase()
        //{
        //    // Act
        //    DateTime result = testTime2.LocalTimeToUniversalTime();
        //    // Assert
        //    Assert.AreNotEqual(utcTime, result);

        //}

        // This method Converts given local time to time in specified time zone, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void LocalTimeTo_ValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeTo(TimeZoneInfo.Local);
            // Assert
            Assert.AreEqual(localTime, result);

        }

        // This method Converts given local time to time in specified time zone, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void LocalTimeTo_InValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeTo(TimeZoneInfo.Local);
            // Assert
            Assert.AreEqual(localTime, result);

        }

        // This method Converts the specified Universally Coordinated Time timestamp to Eastern time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToEasternTime_ValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToEasternTime();
            double offset = -5.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -4.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to Eastern time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToEasternTime_InValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToEasternTime();
            double offset = -5.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -4.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to Central time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass.
        [TestMethod]
        public void UniversalTimeToCentralTime_ValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToCentralTime();
            double offset = -6.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -5.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to Central time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToCentralTime_InValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToCentralTime();
            double offset = -6.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -5.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        //  This method Converts the specified Universally Coordinated Time timestamp to Mountain time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToMountainTime_ValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToMountainTime();
            double offset = -7.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -6.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to Mountain time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToMountainTime_InValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToMountainTime();
            double offset = -7.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -6.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to Pacific time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToPacificTime_AValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToPacificTime();
            double offset = -8.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -7.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        //  This method Converts the specified Universally Coordinated Time timestamp to Pacific time timestamp, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void UniversalTimeToPacificTime_InValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeToPacificTime();
            double offset = -8.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -7.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);
        }

        // This method Converts the specified Universally Coordinated Time timestamp to timestamp in specified time zone, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void UniversalTimeTo_ValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeTo(TimeZoneInfo.Utc);
            // Assert
            Assert.AreEqual(utcTime, result);

        }

        // This method Converts the specified Universally Coordinated Time timestamp to timestamp in specified time zone, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void UniversalTimeTo_InValidCase()
        {
            // Act
            DateTime result = utcTime.UniversalTimeTo(TimeZoneInfo.Utc);
            // Asserts
            Assert.AreEqual(utcTime, result);

        }

        // This method Converts given timestamp from one time zone to another using standard names for time zones, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void TimeZoneToTimeZone_ValidCase()
        {
            // Act
            DateTime result = localTime.TimeZoneToTimeZone(TimeZoneInfo.Local, TimeZoneInfo.Local);
            // Assert
            Assert.AreEqual(localTime, result);

        }

        // This method Converts given timestamp from one time zone to another using standard names for time zones, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void TimeZoneToTimeZone_InValidCase()
        {
            // Act
            DateTime result = localTime.TimeZoneToTimeZone(TimeZoneInfo.Local, TimeZoneInfo.Local); //(TimeZoneInfo.Utc, TimeZoneInfo.Utc);
            // Assert
            Assert.AreEqual(localTime, result);

        }

        // This method Converts given local time to Eastern time, by comparing it to the system clock time
        //  and returns boolean variable as false for valid case and test will pass
        [TestMethod]
        public void LocalTimeToEasternTime_ValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToEasternTime();
            double offset = -5.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -4.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);

        }

        // This method Converts given local time to Eastern time, by comparing it to the system clock time
        //  and returns boolean variable as true for valid case and test will pass
        [TestMethod]
        public void LocalTimeToEasternTime_InValidCase()
        {
            // Act
            DateTime result = localTime.LocalTimeToEasternTime();
            double offset = -5.0;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                offset = -4.0;

            // Assert
            Assert.AreEqual(utcTime.AddHours(offset), result);

        }
    }
}
