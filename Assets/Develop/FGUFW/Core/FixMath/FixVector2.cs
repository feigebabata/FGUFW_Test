#region License

/*
MIT License
Copyright © 2006 The Mono.Xna Team

All rights reserved.

Authors
 * Alan McGovern

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License

using System;

namespace FGUFW {

    [Serializable]
    public struct V2 : IEquatable<V2>
    {
#region Private Fields

        private static V2 zeroVector = new V2(0, 0);
        private static V2 oneVector = new V2(1, 1);

        private static V2 rightVector = new V2(1, 0);
        private static V2 leftVector = new V2(-1, 0);

        private static V2 upVector = new V2(0, 1);
        private static V2 downVector = new V2(0, -1);

        #endregion Private Fields

        #region Public Fields

        public V1 x;
        public V1 y;

        #endregion Public Fields

#region Properties

        public static V2 zero
        {
            get { return zeroVector; }
        }

        public static V2 one
        {
            get { return oneVector; }
        }

        public static V2 right
        {
            get { return rightVector; }
        }

        public static V2 left {
            get { return leftVector; }
        }

        public static V2 up
        {
            get { return upVector; }
        }

        public static V2 down {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public V2(V1 x, V1 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public V2(V1 value)
        {
            x = value;
            y = value;
        }

        public void Set(V1 x, V1 y) {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref V2 vector, ref V2 normal, out V2 result)
        {
            V1 dot = Dot(vector, normal);
            result.x = vector.x - ((2f*dot)*normal.x);
            result.y = vector.y - ((2f*dot)*normal.y);
        }

        public static V2 Reflect(V2 vector, V2 normal)
        {
            V2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static V2 Add(V2 value1, V2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static V2 Barycentric(V2 value1, V2 value2, V2 value3, V1 amount1, V1 amount2)
        {
            return new V2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref V2 value1, ref V2 value2, ref V2 value3, V1 amount1,
                                       V1 amount2, out V2 result)
        {
            result = new V2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static V2 CatmullRom(V2 value1, V2 value2, V2 value3, V2 value4, V1 amount)
        {
            return new V2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref V2 value1, ref V2 value2, ref V2 value3, ref V2 value4,
                                      V1 amount, out V2 result)
        {
            result = new V2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static V2 Clamp(V2 value1, V2 min, V2 max)
        {
            return new V2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref V2 value1, ref V2 min, ref V2 max, out V2 result)
        {
            result = new V2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns FP precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="V2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="V2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static V1 Distance(V2 value1, V2 value2)
        {
            V1 result;
            DistanceSquared(ref value1, ref value2, out result);
            return (V1) V1.Sqrt(result);
        }


        public static void Distance(ref V2 value1, ref V2 value2, out V1 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (V1) V1.Sqrt(result);
        }

        public static V1 DistanceSquared(V2 value1, V2 value2)
        {
            V1 result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref V2 value1, ref V2 value2, out V1 result)
        {
            result = (value1.x - value2.x)*(value1.x - value2.x) + (value1.y - value2.y)*(value1.y - value2.y);
        }

        /// <summary>
        /// Devide first vector with the secund vector
        /// </summary>
        /// <param name="value1">
        /// A <see cref="V2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="V2"/>
        /// </param>
        /// <returns>
        /// A <see cref="V2"/>
        /// </returns>
        public static V2 Divide(V2 value1, V2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }

        public static void Divide(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = value1.x/value2.x;
            result.y = value1.y/value2.y;
        }

        public static V2 Divide(V2 value1, V1 divider)
        {
            V1 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static void Divide(ref V2 value1, V1 divider, out V2 result)
        {
            V1 factor = 1/divider;
            result.x = value1.x*factor;
            result.y = value1.y*factor;
        }

        public static V1 Dot(V2 value1, V2 value2)
        {
            return value1.x*value2.x + value1.y*value2.y;
        }

        public static void Dot(ref V2 value1, ref V2 value2, out V1 result)
        {
            result = value1.x*value2.x + value1.y*value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is V2) ? this == ((V2) obj) : false;
        }

        public bool Equals(V2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int) (x + y);
        }

        public static V2 Hermite(V2 value1, V2 tangent1, V2 value2, V2 tangent2, V1 amount)
        {
            V2 result = new V2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref V2 value1, ref V2 tangent1, ref V2 value2, ref V2 tangent2,
                                   V1 amount, out V2 result)
        {
            result.x = FixMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = FixMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        public V1 magnitude {
            get {
                V1 result;
                DistanceSquared(ref this, ref zeroVector, out result);
                return V1.Sqrt(result);
            }
        }

        public static V2 ClampMagnitude(V2 vector, V1 maxLength) {
            return Normalize(vector) * maxLength;
        }

        public V1 LengthSquared()
        {
            V1 result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return result;
        }

        public static V2 Lerp(V2 value1, V2 value2, V1 amount) {
            amount = FixMath.Clamp(amount, 0, 1);

            return new V2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static V2 LerpUnclamped(V2 value1, V2 value2, V1 amount)
        {
            return new V2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref V2 value1, ref V2 value2, V1 amount, out V2 result)
        {
            result = new V2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static V2 Max(V2 value1, V2 value2)
        {
            return new V2(
                FixMath.Max(value1.x, value2.x),
                FixMath.Max(value1.y, value2.y));
        }

        public static void Max(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = FixMath.Max(value1.x, value2.x);
            result.y = FixMath.Max(value1.y, value2.y);
        }

        public static V2 Min(V2 value1, V2 value2)
        {
            return new V2(
                FixMath.Min(value1.x, value2.x),
                FixMath.Min(value1.y, value2.y));
        }

        public static void Min(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = FixMath.Min(value1.x, value2.x);
            result.y = FixMath.Min(value1.y, value2.y);
        }

        public void Scale(V2 other) {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static V2 Scale(V2 value1, V2 value2) {
            V2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static V2 Multiply(V2 value1, V2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static V2 Multiply(V2 value1, V1 scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref V2 value1, V1 scaleFactor, out V2 result)
        {
            result.x = value1.x*scaleFactor;
            result.y = value1.y*scaleFactor;
        }

        public static void Multiply(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = value1.x*value2.x;
            result.y = value1.y*value2.y;
        }

        public static V2 Negate(V2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref V2 value, out V2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static V2 Normalize(V2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        public V2 normalized {
            get {
                V2 result;
                V2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref V2 value, out V2 result)
        {
            V1 factor;
            DistanceSquared(ref value, ref zeroVector, out factor);
            factor = 1f/(V1) V1.Sqrt(factor);
            result.x = value.x*factor;
            result.y = value.y*factor;
        }

        public static V2 SmoothStep(V2 value1, V2 value2, V1 amount)
        {
            return new V2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref V2 value1, ref V2 value2, V1 amount, out V2 result)
        {
            result = new V2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static V2 Subtract(V2 value1, V2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref V2 value1, ref V2 value2, out V2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static V1 Angle(V2 a, V2 b) {
            return V1.Acos(a.normalized * b.normalized) * V1.Rad2Deg;
        }

        public V3 ToTSVector() {
            return new V3(this.x, this.y, 0);
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

#region Operators

        public static V2 operator -(V2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(V2 value1, V2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(V2 value1, V2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static V2 operator +(V2 value1, V2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static V2 operator -(V2 value1, V2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static V1 operator *(V2 value1, V2 value2)
        {
            return V2.Dot(value1, value2);
        }


        public static V2 operator *(V2 value, V1 scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static V2 operator *(V1 scaleFactor, V2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static V2 operator /(V2 value1, V2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static V2 operator /(V2 value1, V1 divider)
        {
            V1 factor = 1/divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        #endregion Operators
    }
}