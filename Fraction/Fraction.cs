namespace JoFraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // IComparable, IComparable<Double>, IConvertible, IEquatable<Double>, IFormattable
    public struct Fraction : IComparable<Fraction>, IEquatable<Fraction>, IFormattable
    {
        public const double DefaultPrecition = 10e-16;

        public long Numerator { get; }

        public long Denominator { get; }


        /// <summary>
        /// Creates a Fraction: [numerator]/[denominator]
        /// </summary>
        /// <param name="numerator">[x]/y</param>
        /// <param name="denominator">x/[y]</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException(nameof(denominator), "Denominator is 0.");

            if (numerator > 1 || denominator < -1)
                (numerator, denominator) = Reduce(numerator, denominator);
            
            Numerator = numerator;
            Denominator = denominator;
        }

        //public Fraction(double d, double precition = DefaultPrecition)
        //{
        //    var result = GetFraction(d, precition);

        //    Numerator = result.numerator;
        //    Denominator = result.denominator;
        //}

        /// <summary>
        /// Creates a Fraction
        /// </summary>
        /// <example>
        /// new Fraction((1, 2));
        /// </example>
        /// <remarks>
        /// if you must use tuples
        /// </remarks>
        /// <param name="tuple">(long numerator, long denominator)</param>
        public Fraction((long numerator, long denominator) tuple) : this (tuple.numerator, tuple.denominator)
        { }
   
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="f"></param>
        public Fraction(Fraction f) : this(f.Numerator, f.Denominator)
        { }


        /// <summary>
        /// Creates a <see cref="Fraction"/> from a string that contains a number or fraction.
        /// </summary>
        /// <param name="input">A string that contains a number or fraction to convert.</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Fraction Parse(string s)
        {
            return new Fraction(InternalParse(s));
        }

        //public static bool TryParse(string s, out Fraction result)
        //{
        //    Double 
        //}

        private static (long numerator, long denominator) InternalParse(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException(nameof(s));

            if (s.Contains("/"))
            {
                string[] values = s.Split('/', StringSplitOptions.None);

                if (values.Length != 2)
                    throw new FormatException($"Cannot convert {s} to {nameof(Fraction)}");

                if (Int64.TryParse(values[0], out long numerator) &&
                    Int64.TryParse(values[1], out long denominator))
                {
                    if (denominator == 0)
                        throw new ArgumentOutOfRangeException("Denominator is 0.");

                    return (numerator, denominator);
                }

                throw new FormatException($"Cannot convert {s} to {nameof(Fraction)}");
            }

            if (Double.TryParse(s, out double floatingPoint))
            {
                return GetFraction(floatingPoint, DefaultPrecition);
            }
            else
            {
                throw new FormatException($"Cannot convert {s} to {nameof(Fraction)}");
            }
        }

        /// <summary>
        /// Creates a Fraction from a Double.
        /// </summary>
        /// <remarks>
        /// Stern–Brocot tree
        /// </remarks>
        /// <param name="d">Input Double</param>
        /// <param name="precition">Precition when comparing doubles</param>
        /// <returns>a new <see cref="Fraction"/></returns>
        public static Fraction ToFraction(double d, double precition = DefaultPrecition)
        {
            return new Fraction(GetFraction(d, precition));
        }

        private static (long numerator, long denominator) GetFraction(double d, double precition)
        {
            long n = (long)Math.Floor(d);
            d -= n;

            if (d < precition)
                return (n, 1);
            else if ((1 - precition) < d)
                return (n + 1, 1);

            (long numerator, long denominator) lower = (0, 1);
            (long numerator, long denominator) upper = (1, 1);

            for (;;)
            {
                (long numerator, long denominator) middle = (lower.numerator + upper.numerator, lower.denominator + upper.denominator);

                if (middle.denominator * (d + precition) < middle.numerator)
                {
                    upper = middle;
                }
                else if (middle.numerator < (d - precition) * middle.denominator)
                {
                    lower = middle;
                }
                else
                {
                    return (n * middle.denominator + middle.numerator, middle.denominator);
                }
            }
        }

        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }

        /// <summary>
        /// Greatest common factor.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static long ggT(long a, long b)
        {
            if (b == 0)
                return a;
            else return ggT(b, a % b);
        }

        private static long kgV(long a, long b)
        {
            return a / ggT(a, b) * b;
        }

        private static (long, long) Reduce(long numerator, long denominator)
        {
            long tmp = ggT(numerator, denominator);
            denominator /= tmp;
            numerator /= tmp;

            if ((denominator < 0 && numerator > 0) || (denominator < 0 && numerator < 0))
            {
                denominator *= -1;
                numerator *= -1;
            }

            return (numerator, denominator);
        }

        // Multiplizieren Basis Methode
        public Fraction Multiply(Fraction other)
        {
            return new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);
        }

        /// <summary>
        /// Add this to other Fraction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Addition(Fraction other)
        {
            if (Numerator == 0)
                return new Fraction(other);

            if (other.Numerator == 0)
                return new Fraction(this);

            var ErgebnisNenner = kgV(Denominator, other.Denominator);
            var ErgebnisZaehler = Numerator * (ErgebnisNenner / Denominator) +
                other.Numerator * (ErgebnisNenner / other.Denominator);

            return new Fraction(ErgebnisZaehler, ErgebnisNenner);
        }

        /// <summary>
        /// Subtract this with other Fraction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Subtrahieren(Fraction other)
        {
            if (other.Numerator == 0)
                return new Fraction(this);

            var ErgebnisNenner = kgV(Denominator, other.Denominator);
            var ErgebnisZaehler = Numerator * (ErgebnisNenner / Denominator) -
                other.Numerator * (ErgebnisNenner / other.Denominator);
            return new Fraction(ErgebnisZaehler, ErgebnisNenner);
        }

        /// <summary>
        /// Divide this by other fraction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>new Fraction as result</returns>
        /// <exception cref="DivideByZeroException"></exception>
        public Fraction Dividieren(Fraction other)
        {
            if (other.Numerator == 0)
                throw new DivideByZeroException("other Fraction is zero.");

            return new Fraction(Numerator * other.Denominator, Denominator * other.Numerator);
        }

        #region Operators

        // Überladung '*' operator:
        public static Fraction operator *(Fraction a, Fraction b)
        {
            return a.Multiply(b);
        }

        // Überladung '/' operator:
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return a.Dividieren(b);
        }

        // Überladung '+' operator:
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return a.Addition(b);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return a.Subtrahieren(b);
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(Fraction a, Fraction b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <=(Fraction a, Fraction b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(Fraction a, Fraction b)
        {
            return a.CompareTo(b) >= 0;
        }

        #endregion

        #region Converters

        /// <summary>
        /// Explicitly converts a <see cref="Fraction"/> to a <see cref="Double"/>.
        /// </summary>
        /// <example>
        /// double d = (double)(new Fraction(1, 2));
        /// </example>
        public static explicit operator double(Fraction f)
        {
            return f.ToDouble();
        }

        /// <summary>
        /// Explicitly converts a <see cref="Double"/> to a <see cref="Fraction"/>.
        /// </summary>
        /// <example>
        /// Fraction d = (Fraction)1.213d;
        /// </example>
        public static explicit operator Fraction(double d)
        {
            return ToFraction(d);
        }

        /// <summary>
        /// Tuple Deconstruction.
        /// </summary>
        /// <example>
        /// var (n, d) = new Fraction(1, 2);
        /// </example>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        public void Deconstruct(out long numerator, out long denominator)
        {
            numerator = Numerator;
            denominator = Denominator;
        }

        #endregion

        /// <summary>
        /// IComparable implementation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>-1, 1, 0</returns>
        public int CompareTo(Fraction other)
        {
            if (this.Numerator <= other.Numerator && this.Denominator >= other.Denominator)
                return -1;

            if (this.Numerator >= other.Numerator && this.Denominator <= other.Denominator)
                return 1;

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Fraction)
            {
                return Equals((Fraction)obj);
            }

            return false;
        }

        public bool Equals(Fraction other)
        {
            return other.Numerator == Numerator && other.Denominator == Denominator;
        }

        public override int GetHashCode()
        {
            return Denominator.GetHashCode() * 397 ^ Numerator.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{Numerator.ToString(format, formatProvider)}/{Denominator.ToString(format, formatProvider)}";
        }
    }
}
