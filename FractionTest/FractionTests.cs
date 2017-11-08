using System;
using System.Collections.Generic;
using NUnit.Framework;
using JH.Fraction;

namespace FractionTest
{
    [TestFixture]
    public class FractionTests
    {

        [Test]
        public void CreateFractionTest()
        {
            // Arrange

            // Act
            var newFraction = new Fraction(4, 2);

            // Assert
            Assert.AreEqual(1, newFraction.Denominator);
            Assert.AreEqual(2, newFraction.Numerator);
        }

        [Test]
        public void AdditionTest1()
        {
            // Arrange
            var a = new Fraction(5, 11);
            var b = new Fraction(7, 3);

            // Act
            var result = a + b;

            // Assert
            Assert.AreEqual(33, result.Denominator);
            Assert.AreEqual(92, result.Numerator);
        }

        [Test]
        public void AdditionTest2()
        {
            // Arrange
            var a = new Fraction(-5, 11);
            var b = new Fraction(7, 3);

            // Act
            var result = a + b;

            // Assert
            Assert.AreEqual(33, result.Denominator);
            Assert.AreEqual(62, result.Numerator);
        }

        [Test]
        public void AdditionTest3()
        {
            // Arrange
            var a = new Fraction(7, 11);
            var b = new Fraction(0, 3);

            // Act
            var result = a + b;

            // Assert
            Assert.AreEqual(11, result.Denominator);
            Assert.AreEqual(7, result.Numerator);
        }

        [Test]
        public void MultiplyTest()
        {
            // Arrange
            var a = new Fraction(5, 11);
            var b = new Fraction(7, 3);

            // Act
            var result = a * b;

            // Assert
            Assert.AreEqual(33, result.Denominator);
            Assert.AreEqual(35, result.Numerator);
        }

        [Test]
        public void DevideTest()
        {
            // Arrange
            var a = new Fraction(5, 11);
            var b = new Fraction(7, 3);

            // Act
            var result = a / b;

            // Assert
            Assert.AreEqual(77, result.Denominator);
            Assert.AreEqual(15, result.Numerator);
        }

        [Test]
        public void EqualsTest1()
        {
            // Arrange
            var a = new Fraction(5, 11);
            var b = new Fraction(7, 3);

            // Act
            var result = a == b;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void EqualsTest2()
        {
            // Arrange
            var a = new Fraction(4, 2);
            var b = new Fraction(2, 1);

            // Act
            var result = a == b;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void EqualsTest3()
        {
            // Arrange
            var a = new Fraction(3, 7);
            var b = new Fraction(3, 7);

            // Act
            var result = a == b;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ComparerTest1()
        {
            // Arrange
            var a = new Fraction(3, 7);
            var b = new Fraction(4, 7);

            // Act
            var result = a < b;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ComparerTest2()
        {
            // Arrange
            var a = new Fraction(3, 13);
            var b = new Fraction(3, 7);

            // Act
            var result = a < b;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SortTest1()
        {
            // Arrange
            var a = new Fraction(3, 13);
            var b = new Fraction(3, 7);
            var c = new Fraction(2, 7);

            var list = new List<Fraction> { a, b, c };

            // Act
            list.Sort();

            // Assert
            Assert.AreEqual(list[0], a);
            Assert.AreEqual(list[1], c);
            Assert.AreEqual(list[2], b);
        }

        [Test]
        public void ImplicidConverterTest1()
        {
            // Arrange
            var a = new Fraction(3, 2);

            // Act
            double value = (double)a;

            // Assert
            Assert.AreEqual(1.5D, value);
        }

        [Test]
        public void ExplicidConverterTest2()
        {
            // Arrange
            double d = 5.0D;

            // Act
            Fraction f = (Fraction)d;

            // Assert
            Assert.AreEqual(5, f.Numerator);
            Assert.AreEqual(1, f.Denominator);
        }


        [Test]
        public void IConvertibleTest1()
        {
            // Arrange
            Fraction f = new Fraction(0, 1);

            // Act
            bool result = Convert.ToBoolean(f);

            // Assert
            Assert.IsFalse(result, "Zero maps to false");
        }

        [Test]
        public void IConvertibleTest2()
        {
            // Arrange
            Fraction f = new Fraction(12, -23);

            // Act
            bool result = Convert.ToBoolean(f);

            // Assert
            Assert.IsTrue(result, "Anything but Zero maps to true");
        }

        [Test]
        public void IConvertibleTest3()
        {
            // Arrange
            Fraction f = new Fraction(5, 1);

            // Act
            var result = Convert.ToByte(f);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void IConvertibleTest4()
        {
            // Arrange
            Fraction f = new Fraction(61, 1);

            // Act

            Assert.Throws<InvalidCastException>(() =>
            {
                var result = Convert.ToChar(f);
            });
        }

        [Test]
        public void DoubleToFractionTest1()
        {
            // Arrange
            double d = 5.125D;

            // Act
            Fraction f = Fraction.ToFraction(d);

            Console.WriteLine(f);

            // Assert
            Assert.AreEqual(41, f.Numerator);
            Assert.AreEqual(8, f.Denominator);
        }

        [Test]
        public void DoubleToFractionTest2()
        {
            // Arrange
            double d = -5.125D;

            // Act
            Fraction f = Fraction.ToFraction(d, 0.000001);

            // Assert
            Assert.AreEqual(-41, f.Numerator);
            Assert.AreEqual(8, f.Denominator);
        }

        [Test]
        public void DoubleToFractionTest3()
        {
            // Arrange
            double d = 0.01276993355481727574750830564784;

            // Act
            Fraction f = Fraction.ToFraction(d, 10e-32);

            // Assert
            Assert.AreEqual(123, f.Numerator);
            Assert.AreEqual(9632, f.Denominator);
        }

        [Test]
        public void DoubleToFractionTest4()
        {
            // Arrange
            double d = Math.PI; // Why not Pi?

            // Act
            Fraction f = Fraction.ToFraction(d, 10e-32);

            // Assert
            Assert.AreEqual(Math.PI, f.ToDouble());
        }

        [Test]
        public void FractionToTupleTest()
        {
            // Arrange
            Fraction f = new Fraction(11,37);

            // Act
            var (n, d) = f;

            // Assert
            Assert.AreEqual(f.Numerator, n);
            Assert.AreEqual(f.Denominator, d);
        }

        [Test]
        public void FractionEdgeTest1()
        {
            // Arrange
            var a = new Fraction(0, 1);
            var b = new Fraction(4, 1);

            // Act
            var c = a + b;

            // Assert
            Assert.AreEqual(b, c);
        }

        [Test]
        public void FractionEdgeTest2()
        {
            // Arrange
            var a = new Fraction(0, 1);
            var b = new Fraction(4, 1);

            // Act
            var c = b - a;

            // Assert
            Assert.AreEqual(b, c);
        }

        [Test]
        public void FractionEdgeTest3()
        {
            // Arrange
            var a = new Fraction(0, 11);
            var b = new Fraction(4, 1);

            // Act
            

            // Assert
            Assert.Throws<DivideByZeroException>(() => {
                var c = b / a; // Devide by zero
            });
        }

        [Test]
        public void FractionFormattingTest()
        {
            // Arrange
            var a = new Fraction(4521, 13);

            // Act
            string result = $"{a:N0}";

            // Assert
            Assert.AreEqual($"{4521:N0}/{13:N0}", result);
        }

        [Test]
        public void FractionParseTest1()
        {
            // Arrange

            // Act
            var a = Fraction.Parse("4521/13");

            // Assert
            Assert.AreEqual(4521, a.Numerator);
            Assert.AreEqual(13, a.Denominator);
        }

        [Test]
        public void FractionParseTest2()
        {
            // Arrange

            // Act
            var a = Fraction.Parse("4521 /13");

            // Assert
            Assert.AreEqual(4521, a.Numerator);
            Assert.AreEqual(13, a.Denominator);
        }

        [Test]
        public void FractionParseTest3()
        {
            // Arrange

            // Act
            var a = Fraction.Parse("4521 /  13");

            // Assert
            Assert.AreEqual(4521, a.Numerator);
            Assert.AreEqual(13, a.Denominator);
        }

        [Test]
        public void FractionParseTest4()
        {
            // Arrange

            // Act
            var a = Fraction.Parse("4521 /  0013");

            // Assert
            Assert.AreEqual(4521, a.Numerator);
            Assert.AreEqual(13, a.Denominator);
        }

        [Test]
        public void FractionParseTest5()
        {
            // Arrange
            double input = 13.12;

            // Act
            var a = Fraction.Parse(input.ToString());

            // Assert
            Assert.AreEqual(328, a.Numerator);
            Assert.AreEqual(25, a.Denominator);
        }

        [Test]
        [Ignore("Will cause a Int overflow")]
        public void ComplexTest1()
        {
            // Approx. Pi

            // Arrange

            // Act
            Fraction sum = new Fraction(0, 1);

            for (int i = 0; i < 10; i++)
            {
                sum += new Fraction((long)Math.Pow(-1, i + 1), 2 * i - 1);
            }

            sum *= (Fraction)4;

            // Assert
            Assert.AreEqual(3.41, sum.ToDouble());
        }

        [Test]
        public void ComplexTest2()
        {
            // Arrange
            Fraction c = new Fraction(1, 5);
            var tmp = c;

            // Act
            for (int i = 0; i < 5; i++)
            {
                tmp = z(tmp);
            }

            // Assert
            Assert.AreEqual(0.2, tmp.ToDouble(), "It is approx 0.2 -> 0.19999999999999999990384 ...");

            Fraction z(Fraction zN) {
                return zN * zN + c;
            }
        }
    }
}
