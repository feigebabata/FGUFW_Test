/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using UnityEngine;

namespace FGUFW
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct V3
    {

        private static V1 ZeroEpsilonSq = FixMath.Epsilon;
        internal static V3 InternalZero;
        internal static V3 Arbitrary;

        /// <summary>The X component of the vector.</summary>
        public V1 x;
        /// <summary>The Y component of the vector.</summary>
        public V1 y;
        /// <summary>The Z component of the vector.</summary>
        public V1 z;

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly V3 zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly V3 left;
        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly V3 right;
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly V3 up;
        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly V3 down;
        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly V3 back;
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly V3 forward;
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly V3 one;
        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly V3 MinValue;
        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly V3 MaxValue;
        #endregion

        #region Private static constructor
        static V3()
        {
            one = new V3(1, 1, 1);
            zero = new V3(0, 0, 0);
            left = new V3(-1, 0, 0);
            right = new V3(1, 0, 0);
            up = new V3(0, 1, 0);
            down = new V3(0, -1, 0);
            back = new V3(0, 0, -1);
            forward = new V3(0, 0, 1);
            MinValue = new V3(V1.MinValue);
            MaxValue = new V3(V1.MaxValue);
            Arbitrary = new V3(1, 1, 1);
            InternalZero = zero;
        }
        #endregion

        public static V3 Abs(V3 other) {
            return new V3(V1.Abs(other.x), V1.Abs(other.y), V1.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public V1 sqrMagnitude {
            get { 
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public V1 magnitude {
            get {
                V1 num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return V1.Sqrt(num);
            }
        }

        public static V3 ClampMagnitude(V3 vector, V1 maxLength) {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public V3 normalized {
            get {
                V3 result = new V3(this.x, this.y, this.z);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>

        public V3(int x,int y,int z)
		{
			this.x = (V1)x;
			this.y = (V1)y;
			this.z = (V1)z;
		}

		public V3(V1 x, V1 y, V1 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(V3 other) {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(V1 x, V1 y, V1 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public V3(V1 xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

		public static V3 Lerp(V3 from, V3 to, V1 percent) {
			return from + (to - from) * percent;
		}

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString() {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        #endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>
        #region public override bool Equals(object obj)
        public override bool Equals(object obj)
        {
            if (!(obj is V3)) return false;
            V3 other = (V3)obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static V3 Scale(V3 vecA, V3 vecB) {
            V3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        #region public static bool operator ==(JVector value1, JVector value2)
        public static bool operator ==(V3 value1, V3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        #region public static bool operator !=(JVector value1, JVector value2)
        public static bool operator !=(V3 value1, V3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        #region public static JVector Min(JVector value1, JVector value2)

        public static V3 Min(V3 value1, V3 value2)
        {
            V3 result;
            V3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref V3 value1, ref V3 value2, out V3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        #region public static JVector Max(JVector value1, JVector value2)
        public static V3 Max(V3 value1, V3 value2)
        {
            V3 result;
            V3.Max(ref value1, ref value2, out result);
            return result;
        }
		
		public static V1 Distance(V3 v1, V3 v2) {
			return V1.Sqrt ((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
		}

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref V3 value1, ref V3 value2, out V3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
        public void MakeZero()
        {
            x = V1.Zero;
            y = V1.Zero;
            z = V1.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
        public bool IsZero()
        {
            return (this.sqrMagnitude == V1.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        #region public static JVector Transform(JVector position, JMatrix matrix)
        public static V3 Transform(V3 position, FixMatrix matrix)
        {
            V3 result;
            V3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref V3 position, ref FixMatrix matrix, out V3 result)
        {
            V1 num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            V1 num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            V1 num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransposedTransform(ref V3 position, ref FixMatrix matrix, out V3 result)
        {
            V1 num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            V1 num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            V1 num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        #region public static FP Dot(JVector vector1, JVector vector2)
        public static V1 Dot(V3 vector1, V3 vector2)
        {
            return V3.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static V1 Dot(ref V3 vector1, ref V3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }
        #endregion

        // Projects a vector onto another vector.
        public static V3 Project(V3 vector, V3 onNormal)
        {
            V1 sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < FixMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
        public static V3 ProjectOnPlane(V3 vector, V3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }


        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static V1 Angle(V3 from, V3 to)
        {
            return FixMath.Acos(FixMath.Clamp(Dot(from.normalized, to.normalized), -V1.ONE, V1.ONE)) * FixMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static V1 SignedAngle(V3 from, V3 to, V3 axis)
        {
            V3 fromNorm = from.normalized, toNorm = to.normalized;
            V1 unsignedAngle = FixMath.Acos(FixMath.Clamp(Dot(fromNorm, toNorm), -V1.ONE, V1.ONE)) * FixMath.Rad2Deg;
            V1 sign = FixMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static void Add(JVector value1, JVector value2)
        public static V3 Add(V3 value1, V3 value2)
        {
            V3 result;
            V3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref V3 value1, ref V3 value2, out V3 result)
        {
            V1 num0 = value1.x + value2.x;
            V1 num1 = value1.y + value2.y;
            V1 num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static V3 Divide(V3 value1, V1 scaleFactor) {
            V3 result;
            V3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref V3 value1, V1 scaleFactor, out V3 result) {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector Subtract(JVector value1, JVector value2)
        public static V3 Subtract(V3 value1, V3 value2)
        {
            V3 result;
            V3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref V3 value1, ref V3 value2, out V3 result)
        {
            V1 num0 = value1.x - value2.x;
            V1 num1 = value1.y - value2.y;
            V1 num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        #region public static JVector Cross(JVector vector1, JVector vector2)
        public static V3 Cross(V3 vector1, V3 vector2)
        {
            V3 result;
            V3.Cross(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
        public static void Cross(ref V3 vector1, ref V3 vector2, out V3 result)
        {
            V1 num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            V1 num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            V1 num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }
        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>
        #region public override int GetHashCode()
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        #region public static JVector Negate(JVector value)
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static V3 Negate(V3 value)
        {
            V3 result;
            V3.Negate(ref value,out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref V3 value, out V3 result)
        {
            V1 num0 = -value.x;
            V1 num1 = -value.y;
            V1 num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        #region public static JVector Normalize(JVector value)
        public static V3 Normalize(V3 value)
        {
            V3 result;
            V3.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            V1 num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            V1 num = V1.One / V1.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref V3 value, out V3 result)
        {
            V1 num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            V1 num = V1.One / V1.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }
        #endregion

        #region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref V3 vector1, ref V3 vector2)
        {
            V1 temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;
        }
        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        #region public static JVector Multiply(JVector value1, FP scaleFactor)
        public static V3 Multiply(V3 value1, V1 scaleFactor)
        {
            V3 result;
            V3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref V3 value1, V1 scaleFactor, out V3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        #region public static JVector operator %(JVector value1, JVector value2)
        public static V3 operator %(V3 value1, V3 value2)
        {
            V3 result; V3.Cross(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        #region public static FP operator *(JVector value1, JVector value2)
        public static V1 operator *(V3 value1, V3 value2)
        {
            return V3.Dot(ref value1, ref value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(JVector value1, FP value2)
        public static V3 operator *(V3 value1, V1 value2)
        {
            V3 result;
            V3.Multiply(ref value1, value2,out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(FP value1, JVector value2)
        public static V3 operator *(V1 value1, V3 value2)
        {
            V3 result;
            V3.Multiply(ref value2, value1, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector operator -(JVector value1, JVector value2)
        public static V3 operator -(V3 value1, V3 value2)
        {
            V3 result; V3.Subtract(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static JVector operator +(JVector value1, JVector value2)
        public static V3 operator +(V3 value1, V3 value2)
        {
            V3 result; V3.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static V3 operator /(V3 value1, V1 value2) {
            V3 result;
            V3.Divide(ref value1, value2, out result);
            return result;
        }

        public V2 ToTSVector2() {
            return new V2(this.x, this.y);
        }

        public V4 ToTSVector4()
        {
            return new V4(this.x, this.y, this.z, V1.One);
        }

    }

}