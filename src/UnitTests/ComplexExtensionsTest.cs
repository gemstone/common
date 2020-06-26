//******************************************************************************************************
//  ComplexExtensionsTest.cs - Gbtc
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
//  06/25/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Numerics;
using Gemstone.ComplexExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gemstone.Common.UnitTests
{
    [TestClass]
    public class ComplexExtensionsTest
    {
        private Complex[] RealImaginary { get; } =
        {
            new Complex(1.234D, 5.678),
            new Complex(-1.234D, 5.678),
            new Complex(1.234D, -5.678),
            new Complex(-1.234D, -5.678)
        };

        private Complex[] Real { get; } =
        {
            new Complex(1.234D, 0.0D),
            new Complex(-1.234D, 0.0D),
            new Complex(5.678D, 0.0D),
            new Complex(-5.678D, 0.0D)
        };

        private Complex[] Imaginary { get; } =
        {
            new Complex(0.0D, 1.234D),
            new Complex(0.0D, -1.234D),
            new Complex(0.0D, 5.678D),
            new Complex(0.0D, -5.678D)
        };

        private Complex[] ImaginaryOne { get; } =
        {
            new Complex(1.234D, 1.0D),
            new Complex(1.234D, -1.0D),
            new Complex(5.678D, 1.0D),
            new Complex(5.678D, -1.0D)
        };

        private Complex[] Zero { get; } =
        {
            new Complex(0.0D, 0.0D)
        };

        [TestMethod]
        public void ComplexNotation_Default()
        {
            ComplexNotationOptions options = ComplexNotationOptions.Default;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_Prefix()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.PrefixSymbol;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} i{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "" : "-";
                string expected = $"{op}i{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_InsertAsterisk()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.InsertAsterisk;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}*i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}*i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_PrefixAsterisk()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.PrefixSymbol |
                ComplexNotationOptions.InsertAsterisk;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} i*{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"i*{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_ImaginaryFirst()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.ImaginaryFirst;

            foreach (Complex complex in RealImaginary)
            {
                double real = Math.Abs(complex.Real);
                double imaginary = complex.Imaginary;
                string op = complex.Real >= 0.0D ? "+" : "-";
                string expected = $"{imaginary}i {op} {real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = Math.Abs(complex.Real);
                string imaginary = complex.Imaginary >= 0 ? "i" : "-i";
                string op = complex.Real >= 0 ? "+" : "-";
                string expected = $"{imaginary} {op} {real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_NoSubtraction()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default &
                ~ComplexNotationOptions.AllowSubtraction;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "" : "-";
                string expected = $"{real} + {op}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_PrefixNoSubtraction()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default &
                ~ComplexNotationOptions.AllowSubtraction |
                ComplexNotationOptions.PrefixSymbol;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0 ? "" : "-";
                string expected = $"{real} + {op}i{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0 ? "" : "-";
                string expected = $"{op}i{imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "" : "-";
                string expected = $"{real} + {op}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_PreferSubtraction()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.PreferSubtraction;

            foreach (Complex complex in RealImaginary)
            {
                string left = !(complex.Real < 0.0D && complex.Imaginary >= 0.0D)
                    ? $"{complex.Real}"
                    : $"{complex.Imaginary}i";

                string right = (complex.Real < 0.0D && complex.Imaginary >= 0.0D)
                    ? $"{Math.Abs(complex.Real)}"
                    : $"{Math.Abs(complex.Imaginary)}i";

                string op = (complex.Real >= 0.0D && complex.Imaginary >= 0.0D) ? "+" : "-";
                string expected = $"{left} {op} {right}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                string left = !(complex.Real < 0.0D && complex.Imaginary >= 0.0D)
                    ? $"{complex.Real}"
                    : $"i";

                string right = (complex.Real < 0.0D && complex.Imaginary >= 0.0D)
                    ? $"{Math.Abs(complex.Real)}"
                    : $"i";

                string op = (complex.Real >= 0.0D && complex.Imaginary >= 0.0D) ? "+" : "-";
                string expected = $"{left} {op} {right}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_ImaginaryFirstPreferSubtraction()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.ImaginaryFirst |
                ComplexNotationOptions.PreferSubtraction;

            foreach (Complex complex in RealImaginary)
            {
                string left = (complex.Real >= 0.0D && complex.Imaginary < 0.0D)
                    ? $"{complex.Real}"
                    : $"{complex.Imaginary}i";

                string right = !(complex.Real >= 0.0D && complex.Imaginary < 0.0D)
                    ? $"{Math.Abs(complex.Real)}"
                    : $"{Math.Abs(complex.Imaginary)}i";

                string op = (complex.Real >= 0.0D && complex.Imaginary >= 0.0D) ? "+" : "-";
                string expected = $"{left} {op} {right}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                string sign = complex.Imaginary >= 0.0D ? "" : "-";

                string left = (complex.Real >= 0.0D && complex.Imaginary < 0.0D)
                    ? $"{complex.Real}"
                    : $"{sign}i";

                string right = !(complex.Real >= 0.0D && complex.Imaginary < 0.0D)
                    ? $"{Math.Abs(complex.Real)}"
                    : $"i";

                string op = (complex.Real >= 0.0D && complex.Imaginary >= 0.0D) ? "+" : "-";
                string expected = $"{left} {op} {right}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_NoSimplify()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.NoSimplify;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary}i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_UseWhitespace()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.UseWhitespace;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_PrefixUseWhitespace()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.PrefixSymbol |
                ComplexNotationOptions.UseWhitespace;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} i {imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "" : "-";
                string expected = $"{op}i {imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_AsteriskUseWhitespace()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.InsertAsterisk |
                ComplexNotationOptions.UseWhitespace;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"{imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_PrefixAsteriskUseWhitespace()
        {
            ComplexNotationOptions options =
                ComplexNotationOptions.Default |
                ComplexNotationOptions.PrefixSymbol |
                ComplexNotationOptions.InsertAsterisk |
                ComplexNotationOptions.UseWhitespace;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = Math.Abs(complex.Imaginary);
                string op = complex.Imaginary >= 0.0D ? "+" : "-";
                string expected = $"{real} {op} i * {imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                string expected = $"{real}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double imaginary = complex.Imaginary;
                string expected = $"i * {imaginary}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                string op = complex.Imaginary >= 0 ? "+" : "-";
                string expected = $"{real} {op} i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                string expected = $"{0.0D}";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_Verbose()
        {
            ComplexNotationOptions options = ComplexNotationOptions.Verbose;

            foreach (Complex complex in RealImaginary)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Real)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Imaginary)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in ImaginaryOne)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }

            foreach (Complex complex in Zero)
            {
                double real = complex.Real;
                double imaginary = complex.Imaginary;
                string expected = $"{real} + {imaginary} * i";
                string notation = complex.ToComplexNotation("", 'i', options);
                Assert.AreEqual(expected, notation);

                Complex reverse = notation.FromComplexNotation();
                Assert.AreEqual(complex, reverse);
            }
        }

        [TestMethod]
        public void ComplexNotation_InvalidNotation()
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex();
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "-1 - 0.234 + 5.678i";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234, 5.678);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "+-1.234 + 5.678i";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234, 5.678);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "-1.234 + 5.678";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234 + 5.678, 0.0D);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "-1.234i + 5.678i";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(0.0D, -1.234 + 5.678);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "5.678i + - 1.234";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234, 5.678);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "-1.234 + 5.678i*";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234, 5.678);
                Assert.AreEqual(expected, complex);
            });

            Assert.ThrowsException<FormatException>(() =>
            {
                string notation = "-1.234 + 5.678**i";
                Complex complex = notation.FromComplexNotation();
                Complex expected = new Complex(-1.234, 5.678);
                Assert.AreEqual(expected, complex);
            });
        }
    }
}
