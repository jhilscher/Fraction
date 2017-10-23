namespace Fraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public struct F : IComparable<F>, ICustomFormatter
    {
        public const double DefaultPrecition = 10e-16;

        private int _numerator;
        private int _denominator;

        public int Numerator
        {
            get { return this._numerator; }
        }

        public int Denominator
        {
            get { return this._denominator; }
        }

        /// <summary>
        /// Creates a Fraction: [numerator]/[denominator]
        /// </summary>
        /// <param name="numerator">[x]/y</param>
        /// <param name="denominator">x/[y]</param>
        public F(int numerator, int denominator)
        {
            if (denominator == 0 && numerator != 0)
                throw new ArgumentException("Denominator is 0.");

            this._numerator = numerator;
            this._denominator = denominator;

            Reduce();
        }

        /// <summary>
        /// Tuple Constructor
        /// </summary>
        /// <example>
        /// new F((1, 2));
        /// </example>
        /// <remarks>
        /// if you must use tuples
        /// </remarks>
        /// <param name="tuple">(int, int)</param>
        public F((int numerator, int denominator) tuple) : this (tuple.numerator, tuple.denominator)
        { }
   
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="f"></param>
        public F(F f) : this(f.Numerator, f.Denominator)
        { }

        public F(string input)
        {
            if (input.Contains("/"))
            {
                string[] trenner = input.Split('/');
                _numerator = Int32.Parse(trenner[0]);
                _denominator = Int32.Parse(trenner[1]);

                if (_denominator == 0)
                    throw new ArgumentException("Denominator is 0.");

                Reduce();
            }
            else
            {
                _numerator = Int32.Parse(input);
                _denominator = 1;
            }
        }

        /// <summary>
        /// Stern–Brocot tree
        /// </summary>
        /// <param name="d"></param>
        /// <param name="precition"></param>
        /// <returns></returns>
        public static F ToFraction(double d, double precition = DefaultPrecition)
        {
            int n = (int)Math.Floor(d);
            d -= n;

            if (d < precition)
                return new F(n, 1);
            else if ((1 - precition) < d)
                return new F(n + 1, 1);

            (int numerator, int denominator) lower = (0, 1);
            (int numerator, int denominator) upper = (1, 1);

            for(;;)
            {
                (int numerator, int denominator) middle = (lower.numerator + upper.numerator, lower.denominator + upper.denominator);

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
                    return new F(n * middle.denominator + middle.numerator, middle.denominator);
                }
            }
        }

        public static int ggT(int a, int b)
        {
            if (b == 0)
                return a;
            else return ggT(b, a % b);
        }

        public static int kgV(int a, int b)
        {
            return a / ggT(a, b) * b;
        }

        private void Reduce()
        {
            int tmp = ggT(this._numerator, this._denominator);
            this._denominator /= tmp;
            this._numerator /= tmp;

            if ((this._denominator < 0 && this._numerator > 0) || (this._denominator < 0 && this._numerator < 0))
            {
                this._denominator *= -1;
                this._numerator *= -1;
            }
        }

        // Multiplizieren Basis Methode
        public F Multiply(F BruchZwei)
        {
            return new F(this._numerator * BruchZwei.Numerator, this._denominator * BruchZwei.Denominator);
        }

        // Addition Basis Methode
        public F Addition(F zweiterBruch)
        {
            int ErgebnisNenner = kgV(this._denominator, zweiterBruch.Denominator);
            int ErgebnisZaehler = this.Numerator * (ErgebnisNenner / this._denominator) +
                zweiterBruch.Numerator * (ErgebnisNenner / zweiterBruch.Denominator);
            return new F(ErgebnisZaehler, ErgebnisNenner);
        }

        // Subtraktion basis Methode
        public F Subtrahieren(F zweiterBruch)
        {
            int ErgebnisNenner = kgV(this._denominator, zweiterBruch.Denominator);
            int ErgebnisZaehler = this.Numerator * (ErgebnisNenner / this._denominator) -
                zweiterBruch.Numerator * (ErgebnisNenner / zweiterBruch.Denominator);
            return new F(ErgebnisZaehler, ErgebnisNenner);
        }

        // Dividieren Basis Methode
        public F Dividieren(F BruchZwei)
        {
            return new F(this._numerator * BruchZwei.Denominator, this._denominator * BruchZwei.Numerator);
        }

        #region Operators

        // Überladung '*' operator:
        public static F operator *(F a, F b)
        {
            return a.Multiply(b);
        }

        // Überladung '/' operator:
        public static F operator /(F a, F b)
        {
            return new F(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        // Überladung '+' operator:
        public static F operator +(F a, F b)
        {
            return a.Addition(b);
        }

        public static F operator -(F a, F b)
        {
            return a.Subtrahieren(b);
        }

        public static bool operator ==(F a, F b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(F a, F b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(F a, F b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(F a, F b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <=(F a, F b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(F a, F b)
        {
            return a.CompareTo(b) >= 0;
        }

        #endregion

        #region Converters

        /// <summary>
        /// Convert <see cref="F"/> to
        /// </summary>
        /// <param name="f"></param>
        public static explicit operator double(F f)
        {
            return (double)f.Numerator / f.Denominator;
        }

        public static explicit operator F(double d)
        {
            return ToFraction(d);
        }

        /// <summary>
        /// Tuple Deconstruction.
        /// </summary>
        /// <example>
        /// var (n, d) = new F(1, 2);
        /// </example>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        public void Deconstruct(out int numerator, out int denominator)
        {
            numerator = Numerator;
            denominator = Denominator;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is F)
            {
                return Equals((F)obj);
            }

            return false;
        }

        private bool Equals(F f)
        {
            return f.Numerator == this.Numerator && f.Denominator == this.Denominator;
        }

        public override int GetHashCode()
        {
            return _denominator.GetHashCode() * 397 ^ _numerator.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(F other)
        {
            if (this.Numerator <= other.Numerator && this.Denominator >= other.Denominator)
                return -1;

            if (this.Numerator >= other.Numerator && this.Denominator <= other.Denominator)
                return 1;

            return 0;
        }
    }
}
