using System;
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

namespace FGUFW {

    /// <summary>
    /// Contains common math operations.
    /// </summary>
    public static class FixMath {

        /// <summary>
        /// PI constant.
        /// </summary>
        public static V1 Pi = V1.Pi;

        /**
        *  @brief PI over 2 constant.
        **/
        public static V1 PiOver2 = V1.PiOver2;

        /// <summary>
        /// A small value often used to decide if numeric 
        /// results are zero.
        /// </summary>
		public static V1 Epsilon = V1.Epsilon;

        /**
        *  @brief Degree to radians constant.
        **/
        public static V1 Deg2Rad = V1.Deg2Rad;

        /**
        *  @brief Radians to degree constant.
        **/
        public static V1 Rad2Deg = V1.Rad2Deg;


        /**
         * @brief FP infinity.
         * */
        public static V1 Infinity = V1.MaxValue;

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="number">The number to get the square root from.</param>
        /// <returns></returns>
        #region public static FP Sqrt(FP number)
        public static V1 Sqrt(V1 number) {
            return V1.Sqrt(number);
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FP Max(FP val1, FP val2)
        public static V1 Max(V1 val1, V1 val2) {
            return (val1 > val2) ? val1 : val2;
        }
        #endregion

        /// <summary>
        /// Gets the minimum number of two values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <returns>Returns the smallest value.</returns>
        #region public static FP Min(FP val1, FP val2)
        public static V1 Min(V1 val1, V1 val2) {
            return (val1 < val2) ? val1 : val2;
        }
        #endregion

        /// <summary>
        /// Gets the maximum number of three values.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="val3">The third value.</param>
        /// <returns>Returns the largest value.</returns>
        #region public static FP Max(FP val1, FP val2,FP val3)
        public static V1 Max(V1 val1, V1 val2, V1 val3) {
            V1 max12 = (val1 > val2) ? val1 : val2;
            return (max12 > val3) ? max12 : val3;
        }
        #endregion

        /// <summary>
        /// Returns a number which is within [min,max]
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        #region public static FP Clamp(FP value, FP min, FP max)
        public static V1 Clamp(V1 value, V1 min, V1 max) {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }
        #endregion

        /// <summary>
        /// Returns a number which is within [FP.Zero, FP.One]
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static V1 Clamp01(V1 value)
        {
            if (value < V1.Zero)
                return V1.Zero;

            if (value > V1.One)
                return V1.One;

            return value;
        }

        /// <summary>
        /// Changes every sign of the matrix entry to '+'
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="result">The absolute matrix.</param>
        #region public static void Absolute(ref JMatrix matrix,out JMatrix result)
        public static void Absolute(ref FixMatrix matrix, out FixMatrix result) {
            result.M11 = V1.Abs(matrix.M11);
            result.M12 = V1.Abs(matrix.M12);
            result.M13 = V1.Abs(matrix.M13);
            result.M21 = V1.Abs(matrix.M21);
            result.M22 = V1.Abs(matrix.M22);
            result.M23 = V1.Abs(matrix.M23);
            result.M31 = V1.Abs(matrix.M31);
            result.M32 = V1.Abs(matrix.M32);
            result.M33 = V1.Abs(matrix.M33);
        }
        #endregion

        /// <summary>
        /// Returns the sine of value.
        /// </summary>
        public static V1 Sin(V1 value) {
            return V1.Sin(value);
        }

        /// <summary>
        /// Returns the cosine of value.
        /// </summary>
        public static V1 Cos(V1 value) {
            return V1.Cos(value);
        }

        /// <summary>
        /// Returns the tan of value.
        /// </summary>
        public static V1 Tan(V1 value) {
            return V1.Tan(value);
        }

        /// <summary>
        /// Returns the arc sine of value.
        /// </summary>
        public static V1 Asin(V1 value) {
            return V1.Asin(value);
        }

        /// <summary>
        /// Returns the arc cosine of value.
        /// </summary>
        public static V1 Acos(V1 value) {
            return V1.Acos(value);
        }

        /// <summary>
        /// Returns the arc tan of value.
        /// </summary>
        public static V1 Atan(V1 value) {
            return V1.Atan(value);
        }

        /// <summary>
        /// Returns the arc tan of coordinates x-y.
        /// </summary>
        public static V1 Atan2(V1 y, V1 x) {
            return V1.Atan2(y, x);
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static V1 Floor(V1 value) {
            return V1.Floor(value);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static V1 Ceiling(V1 value) {
            return value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static V1 Round(V1 value) {
            return V1.Round(value);
        }

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(V1 value) {
            return V1.Sign(value);
        }

        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        public static V1 Abs(V1 value) {
            return V1.Abs(value);                
        }

        public static V1 Barycentric(V1 value1, V1 value2, V1 value3, V1 amount1, V1 amount2) {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static V1 CatmullRom(V1 value1, V1 value2, V1 value3, V1 value4, V1 amount) {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using FPs not to lose precission
            V1 amountSquared = amount * amount;
            V1 amountCubed = amountSquared * amount;
            return (V1)(0.5 * (2.0 * value2 +
                                 (value3 - value1) * amount +
                                 (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                                 (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        public static V1 Distance(V1 value1, V1 value2) {
            return V1.Abs(value1 - value2);
        }

        public static V1 Hermite(V1 value1, V1 tangent1, V1 value2, V1 tangent2, V1 amount) {
            // All transformed to FP not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            V1 v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            V1 sCubed = s * s * s;
            V1 sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                         (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                         t1 * s +
                         v1;
            return (V1)result;
        }

        public static V1 Lerp(V1 value1, V1 value2, V1 amount) {
            return value1 + (value2 - value1) * Clamp01(amount);
        }

        public static V1 InverseLerp(V1 value1, V1 value2, V1 amount) {
            if (value1 != value2)
                return Clamp01((amount - value1) / (value2 - value1));
            return V1.Zero;
        }

        public static V1 SmoothStep(V1 value1, V1 value2, V1 amount) {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            V1 result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }


        /// <summary>
        /// Returns 2 raised to the specified power.
        /// Provides at least 6 decimals of accuracy.
        /// </summary>
        internal static V1 Pow2(V1 x)
        {
            if (x.RawValue == 0)
            {
                return V1.One;
            }

            // Avoid negative arguments by exploiting that exp(-x) = 1/exp(x).
            bool neg = x.RawValue < 0;
            if (neg)
            {
                x = -x;
            }

            if (x == V1.One)
            {
                return neg ? V1.One / (V1)2 : (V1)2;
            }
            if (x >= V1.Log2Max)
            {
                return neg ? V1.One / V1.MaxValue : V1.MaxValue;
            }
            if (x <= V1.Log2Min)
            {
                return neg ? V1.MaxValue : V1.Zero;
            }

            /* The algorithm is based on the power series for exp(x):
             * http://en.wikipedia.org/wiki/Exponential_function#Formal_definition
             * 
             * From term n, we get term n+1 by multiplying with x/n.
             * When the sum term drops to zero, we can stop summing.
             */

            int integerPart = (int)Floor(x);
            // Take fractional part of exponent
            x = V1.FromRaw(x.RawValue & 0x00000000FFFFFFFF);

            var result = V1.One;
            var term = V1.One;
            int i = 1;
            while (term.RawValue != 0)
            {
                term = V1.FastMul(V1.FastMul(x, term), V1.Ln2) / (V1)i;
                result += term;
                i++;
            }

            result = V1.FromRaw(result.RawValue << integerPart);
            if (neg)
            {
                result = V1.One / result;
            }

            return result;
        }

        /// <summary>
        /// Returns the base-2 logarithm of a specified number.
        /// Provides at least 9 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        internal static V1 Log2(V1 x)
        {
            if (x.RawValue <= 0)
            {
                throw new ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
            }

            // This implementation is based on Clay. S. Turner's fast binary logarithm
            // algorithm (C. S. Turner,  "A Fast Binary Logarithm Algorithm", IEEE Signal
            //     Processing Mag., pp. 124,140, Sep. 2010.)

            long b = 1U << (V1.FRACTIONAL_PLACES - 1);
            long y = 0;

            long rawX = x.RawValue;
            while (rawX < V1.ONE)
            {
                rawX <<= 1;
                y -= V1.ONE;
            }

            while (rawX >= (V1.ONE << 1))
            {
                rawX >>= 1;
                y += V1.ONE;
            }

            var z = V1.FromRaw(rawX);

            for (int i = 0; i < V1.FRACTIONAL_PLACES; i++)
            {
                z = V1.FastMul(z, z);
                if (z.RawValue >= (V1.ONE << 1))
                {
                    z = V1.FromRaw(z.RawValue >> 1);
                    y += b;
                }
                b >>= 1;
            }

            return V1.FromRaw(y);
        }

        /// <summary>
        /// Returns the natural logarithm of a specified number.
        /// Provides at least 7 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        public static V1 Ln(V1 x)
        {
            return V1.FastMul(Log2(x), V1.Ln2);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// Provides about 5 digits of accuracy for the result.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// The base was zero, with a negative exponent
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The base was negative, with a non-zero exponent
        /// </exception>
        public static V1 Pow(V1 b, V1 exp)
        {
            if (b == V1.One)
            {
                return V1.One;
            }

            if (exp.RawValue == 0)
            {
                return V1.One;
            }

            if (b.RawValue == 0)
            {
                if (exp.RawValue < 0)
                {
                    //throw new DivideByZeroException();
                    return V1.MaxValue;
                }
                return V1.Zero;
            }

            V1 log2 = Log2(b);
            return Pow2(exp * log2);
        }

        public static V1 MoveTowards(V1 current, V1 target, V1 maxDelta)
        {
            if (Abs(target - current) <= maxDelta)
                return target;
            return (current + (Sign(target - current)) * maxDelta);
        }

        public static V1 Repeat(V1 t, V1 length)
        {
            return (t - (Floor(t / length) * length));
        }

        public static V1 DeltaAngle(V1 current, V1 target)
        {
            V1 num = Repeat(target - current, (V1)360f);
            if (num > (V1)180f)
            {
                num -= (V1)360f;
            }
            return num;
        }

        public static V1 MoveTowardsAngle(V1 current, V1 target, float maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }

        public static V1 SmoothDamp(V1 current, V1 target, ref V1 currentVelocity, V1 smoothTime, V1 maxSpeed)
        {
            V1 deltaTime = V1.EN2;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static V1 SmoothDamp(V1 current, V1 target, ref V1 currentVelocity, V1 smoothTime)
        {
            V1 deltaTime = V1.EN2;
            V1 positiveInfinity = -V1.MaxValue;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, positiveInfinity, deltaTime);
        }

        public static V1 SmoothDamp(V1 current, V1 target, ref V1 currentVelocity, V1 smoothTime, V1 maxSpeed, V1 deltaTime)
        {
            smoothTime = Max(V1.EN4, smoothTime);
            V1 num = (V1)2f / smoothTime;
            V1 num2 = num * deltaTime;
            V1 num3 = V1.One / (((V1.One + num2) + (((V1)0.48f * num2) * num2)) + ((((V1)0.235f * num2) * num2) * num2));
            V1 num4 = current - target;
            V1 num5 = target;
            V1 max = maxSpeed * smoothTime;
            num4 = Clamp(num4, -max, max);
            target = current - num4;
            V1 num7 = (currentVelocity + (num * num4)) * deltaTime;
            currentVelocity = (currentVelocity - (num * num7)) * num3;
            V1 num8 = target + ((num4 + num7) * num3);
            if (((num5 - current) > V1.Zero) == (num8 > num5))
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }
    }
}
