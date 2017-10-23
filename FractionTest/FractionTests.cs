using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fraction;
using System.Collections.Generic;

namespace FractionTest
{
    [TestClass]
    public class FractionTests
    {

        [TestMethod]
        public void CreateFractionTest()
        {
            // Arrange

            // Act
            var newFraction = new F(4, 2);

            // Assert
            Assert.AreEqual(1, newFraction.Denominator);
            Assert.AreEqual(2, newFraction.Numerator);
        }

        [TestMethod]
        public void MultiplyTest()
        {
            // Arrange
            var a = new F(5, 11);
            var b = new F(7, 3);

            // Act
            var result = a * b;

            // Assert
            Assert.AreEqual(33, result.Denominator);
            Assert.AreEqual(35, result.Numerator);
        }

        [TestMethod]
        public void DevideTest()
        {
            // Arrange
            var a = new F(5, 11);
            var b = new F(7, 3);

            // Act
            var result = a / b;

            // Assert
            Assert.AreEqual(77, result.Denominator);
            Assert.AreEqual(15, result.Numerator);
        }

        [TestMethod]
        public void EqualsTest1()
        {
            // Arrange
            var a = new F(5, 11);
            var b = new F(7, 3);

            // Act
            var result = a == b;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EqualsTest2()
        {
            // Arrange
            var a = new F(4, 2);
            var b = new F(2, 1);

            // Act
            var result = a == b;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsTest3()
        {
            // Arrange
            var a = new F(3, 7);
            var b = new F(3, 7);

            // Act
            var result = a == b;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComparerTest1()
        {
            // Arrange
            var a = new F(3, 7);
            var b = new F(4, 7);

            // Act
            var result = a < b;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComparerTest2()
        {
            // Arrange
            var a = new F(3, 13);
            var b = new F(3, 7);

            // Act
            var result = a < b;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SortTest1()
        {
            // Arrange
            var a = new F(3, 13);
            var b = new F(3, 7);
            var c = new F(2, 7);

            var list = new List<F> { a, b, c };

            // Act
            list.Sort();

            // Assert
            Assert.AreEqual(list[0], a);
            Assert.AreEqual(list[1], c);
            Assert.AreEqual(list[2], b);
        }

        [TestMethod]
        public void ImplicidConverterTest1()
        {
            // Arrange
            var a = new F(3, 2);

            // Act
            double value = (double)a;

            // Assert
            Assert.AreEqual(1.5D, value);
        }

        [TestMethod]
        public void ExplicidConverterTest2()
        {
            // Arrange
            double d = 5.0D;

            // Act
            F f = (F)d;

            // Assert
            Assert.AreEqual(5, f.Numerator);
            Assert.AreEqual(1, f.Denominator);
        }

        [TestMethod]
        public void DoubleToFractionTest1()
        {
            // Arrange
            double d = 5.125D;

            // Act
            F f = F.ToFraction(d);

            Console.WriteLine(f);

            // Assert
            Assert.AreEqual(41, f.Numerator);
            Assert.AreEqual(8, f.Denominator);
        }

        [TestMethod]
        public void DoubleToFractionTest2()
        {
            // Arrange
            double d = -5.125D;

            // Act
            F f = F.ToFraction(d, 0.000001);

            // Assert
            Assert.AreEqual(-41, f.Numerator);
            Assert.AreEqual(8, f.Denominator);
        }

        [TestMethod]
        public void DoubleToFractionTest3()
        {
            // Arrange
            double d = 0.01276993355481727574750830564784;

            // Act
            F f = F.ToFraction(d, 0.000000000000000000000000000000000001);

            // Assert
            Assert.AreEqual(123, f.Numerator);
            Assert.AreEqual(9632, f.Denominator);
        }

        [TestMethod]
        public void FractionToTupleTest()
        {
            // Arrange
            F f = new F(11,37);

            // Act
            var (n, d) = f;

            // Assert
            Assert.AreEqual(f.Numerator, n);
            Assert.AreEqual(f.Denominator, d);
        }
    }
}
