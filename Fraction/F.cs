using System;

namespace Fraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public struct F : IComparable<F>, ICustomFormatter
    {
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
        /// <param name="error"></param>
        /// <returns></returns>
        public static F ToFraction(double d, double error)
        {
            int n = (int)Math.Floor(d);
            d -= n;

            if (d < error)
                return new F(n, 1);
            else if ((1-error) < d)
                return new F(n + 1, 1);

            // The lower fraction is 0/1
            int lower_n = 0;
            int lower_d = 1;
            // The upper fraction is 1/1
            int upper_n = 1;
            int upper_d = 1;

            for(;;)
            {
                // The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                int middle_n = lower_n + upper_n;
                int middle_d = lower_d + upper_d;
                // If x + error < middle
                if (middle_d * (d + error) < middle_n)
                {
                    // middle is our new upper
                    upper_n = middle_n;
                    upper_d = middle_d;
                }
                // Else If middle < x - error
                else if (middle_n < (d - error) * middle_d)
                {
                    // middle is our new lower
                    lower_n = middle_n;
                    lower_d = middle_d;
                }
                // Else middle is our best fraction
                else
                    return new F(n * middle_d + middle_n, middle_d);
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

        public static explicit operator double(F f)
        {
            return (double)f.Numerator / f.Denominator;
        }

        public static explicit operator F(double d)
        {
            return ToFraction(d, 0.000000001);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double Absolut()
        {
            return Math.Abs((double)this._numerator / this._denominator);
        }

        /// <summary>
        /// Is this Fraction Zero?
        /// </summary>
        public bool IsNotZero()
        {
            return this._numerator != 0;
        }


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
