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

namespace FGUFW
{

    /// <summary>
    /// A Quaternion representing an orientation.
    /// </summary>
    [Serializable]
    public struct FixQuaternion
    {

        /// <summary>The X component of the quaternion.</summary>
        public V1 x;
        /// <summary>The Y component of the quaternion.</summary>
        public V1 y;
        /// <summary>The Z component of the quaternion.</summary>
        public V1 z;
        /// <summary>The W component of the quaternion.</summary>
        public V1 w;

        public static readonly FixQuaternion identity;

        static FixQuaternion() {
            identity = new FixQuaternion(0, 0, 0, 1);
        }

        /// <summary>
        /// Initializes a new instance of the JQuaternion structure.
        /// </summary>
        /// <param name="x">The X component of the quaternion.</param>
        /// <param name="y">The Y component of the quaternion.</param>
        /// <param name="z">The Z component of the quaternion.</param>
        /// <param name="w">The W component of the quaternion.</param>
        public FixQuaternion(V1 x, V1 y, V1 z, V1 w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public void Set(V1 new_x, V1 new_y, V1 new_z, V1 new_w) {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.w = new_w;
        }

        public void SetFromToRotation(V3 fromDirection, V3 toDirection) {
            FixQuaternion targetRotation = FixQuaternion.FromToRotation(fromDirection, toDirection);
            this.Set(targetRotation.x, targetRotation.y, targetRotation.z, targetRotation.w);
        }

        public V3 eulerAngles {
            get {
                V3 result = new V3();

                V1 ysqr = y * y;
                V1 t0 = -2.0f * (ysqr + z * z) + 1.0f;
                V1 t1 = +2.0f * (x * y - w * z);
                V1 t2 = -2.0f * (x * z + w * y);
                V1 t3 = +2.0f * (y * z - w * x);
                V1 t4 = -2.0f * (x * x + ysqr) + 1.0f;

                t2 = t2 > 1.0f ? 1.0f : t2;
                t2 = t2 < -1.0f ? -1.0f : t2;

                result.x = V1.Atan2(t3, t4) * V1.Rad2Deg;
                result.y = V1.Asin(t2) * V1.Rad2Deg;
                result.z = V1.Atan2(t1, t0) * V1.Rad2Deg;

                return result * -1;
            }
        }

        public static V1 Angle(FixQuaternion a, FixQuaternion b) {
            FixQuaternion aInv = FixQuaternion.Inverse(a);
            FixQuaternion f = b * aInv;

            V1 angle = V1.Acos(f.w) * 2 * V1.Rad2Deg;

            if (angle > 180) {
                angle = 360 - angle;
            }

            return angle;
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The sum of both quaternions.</returns>
        #region public static JQuaternion Add(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Add(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Add(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        public static FixQuaternion LookRotation(V3 forward) {
            return CreateFromMatrix(FixMatrix.LookAt(forward, V3.up));
        }

        public static FixQuaternion LookRotation(V3 forward, V3 upwards) {
            return CreateFromMatrix(FixMatrix.LookAt(forward, upwards));
        }

        public static FixQuaternion Slerp(FixQuaternion from, FixQuaternion to, V1 t) {
            t = FixMath.Clamp(t, 0, 1);

            V1 dot = Dot(from, to);

            if (dot < 0.0f) {
                to = Multiply(to, -1);
                dot = -dot;
            }

            V1 halfTheta = V1.Acos(dot);

            return Multiply(Multiply(from, V1.Sin((1 - t) * halfTheta)) + Multiply(to, V1.Sin(t * halfTheta)), 1 / V1.Sin(halfTheta));
        }

        public static FixQuaternion RotateTowards(FixQuaternion from, FixQuaternion to, V1 maxDegreesDelta) {
            V1 dot = Dot(from, to);

            if (dot < 0.0f) {
                to = Multiply(to, -1);
                dot = -dot;
            }

            V1 halfTheta = V1.Acos(dot);
            V1 theta = halfTheta * 2;

            maxDegreesDelta *= V1.Deg2Rad;

            if (maxDegreesDelta >= theta) {
                return to;
            }

            maxDegreesDelta /= theta;

            return Multiply(Multiply(from, V1.Sin((1 - maxDegreesDelta) * halfTheta)) + Multiply(to, V1.Sin(maxDegreesDelta * halfTheta)), 1 / V1.Sin(halfTheta));
        }

        public static FixQuaternion Euler(V1 x, V1 y, V1 z) {
            x *= V1.Deg2Rad;
            y *= V1.Deg2Rad;
            z *= V1.Deg2Rad;

            FixQuaternion rotation;
            FixQuaternion.CreateFromYawPitchRoll(y, x, z, out rotation);

            return rotation;
        }

        public static FixQuaternion Euler(V3 eulerAngles) {
            return Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public static FixQuaternion AngleAxis(V1 angle, V3 axis) {
            axis = axis * V1.Deg2Rad;
            axis.Normalize();

            V1 halfAngle = angle * V1.Deg2Rad * V1.Half;

            FixQuaternion rotation;
            V1 sin = V1.Sin(halfAngle);

            rotation.x = axis.x * sin;
            rotation.y = axis.y * sin;
            rotation.z = axis.z * sin;
            rotation.w = V1.Cos(halfAngle);

            return rotation;
        }

        public static void CreateFromYawPitchRoll(V1 yaw, V1 pitch, V1 roll, out FixQuaternion result)
        {
            V1 num9 = roll * V1.Half;
            V1 num6 = V1.Sin(num9);
            V1 num5 = V1.Cos(num9);
            V1 num8 = pitch * V1.Half;
            V1 num4 = V1.Sin(num8);
            V1 num3 = V1.Cos(num8);
            V1 num7 = yaw * V1.Half;
            V1 num2 = V1.Sin(num7);
            V1 num = V1.Cos(num7);
            result.x = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.w = ((num * num3) * num5) + ((num2 * num4) * num6);
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The sum of both quaternions.</param>
        public static void Add(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            result.x = quaternion1.x + quaternion2.x;
            result.y = quaternion1.y + quaternion2.y;
            result.z = quaternion1.z + quaternion2.z;
            result.w = quaternion1.w + quaternion2.w;
        }
        #endregion

        public static FixQuaternion Conjugate(FixQuaternion value)
        {
            FixQuaternion quaternion;
            quaternion.x = -value.x;
            quaternion.y = -value.y;
            quaternion.z = -value.z;
            quaternion.w = value.w;
            return quaternion;
        }

        public static V1 Dot(FixQuaternion a, FixQuaternion b) {
            return a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static FixQuaternion Inverse(FixQuaternion rotation) {
            V1 invNorm = V1.One / ((rotation.x * rotation.x) + (rotation.y * rotation.y) + (rotation.z * rotation.z) + (rotation.w * rotation.w));
            return FixQuaternion.Multiply(FixQuaternion.Conjugate(rotation), invNorm);
        }

        public static FixQuaternion FromToRotation(V3 fromVector, V3 toVector) {
            V3 w = V3.Cross(fromVector, toVector);
            FixQuaternion q = new FixQuaternion(w.x, w.y, w.z, V3.Dot(fromVector, toVector));
            q.w += V1.Sqrt(fromVector.sqrMagnitude * toVector.sqrMagnitude);
            q.Normalize();

            return q;
        }

        public static FixQuaternion Lerp(FixQuaternion a, FixQuaternion b, V1 t) {
            t = FixMath.Clamp(t, V1.Zero, V1.One);

            return LerpUnclamped(a, b, t);
        }

        public static FixQuaternion LerpUnclamped(FixQuaternion a, FixQuaternion b, V1 t) {
            FixQuaternion result = FixQuaternion.Multiply(a, (1 - t)) + FixQuaternion.Multiply(b, t);
            result.Normalize();

            return result;
        }

        /// <summary>
        /// Quaternions are subtracted.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The difference of both quaternions.</returns>
        #region public static JQuaternion Subtract(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Subtract(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Subtract(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        /// <summary>
        /// Quaternions are subtracted.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The difference of both quaternions.</param>
        public static void Subtract(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            result.x = quaternion1.x - quaternion2.x;
            result.y = quaternion1.y - quaternion2.y;
            result.z = quaternion1.z - quaternion2.z;
            result.w = quaternion1.w - quaternion2.w;
        }
        #endregion

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The product of both quaternions.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, JQuaternion quaternion2)
        public static FixQuaternion Multiply(FixQuaternion quaternion1, FixQuaternion quaternion2)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The product of both quaternions.</param>
        public static void Multiply(ref FixQuaternion quaternion1, ref FixQuaternion quaternion2, out FixQuaternion result)
        {
            V1 x = quaternion1.x;
            V1 y = quaternion1.y;
            V1 z = quaternion1.z;
            V1 w = quaternion1.w;
            V1 num4 = quaternion2.x;
            V1 num3 = quaternion2.y;
            V1 num2 = quaternion2.z;
            V1 num = quaternion2.w;
            V1 num12 = (y * num2) - (z * num3);
            V1 num11 = (z * num4) - (x * num2);
            V1 num10 = (x * num3) - (y * num4);
            V1 num9 = ((x * num4) + (y * num3)) + (z * num2);
            result.x = ((x * num) + (num4 * w)) + num12;
            result.y = ((y * num) + (num3 * w)) + num11;
            result.z = ((z * num) + (num2 * w)) + num10;
            result.w = (w * num) - num9;
        }
        #endregion

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns>The scaled quaternion.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, FP scaleFactor)
        public static FixQuaternion Multiply(FixQuaternion quaternion1, V1 scaleFactor)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref quaternion1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="result">The scaled quaternion.</param>
        public static void Multiply(ref FixQuaternion quaternion1, V1 scaleFactor, out FixQuaternion result)
        {
            result.x = quaternion1.x * scaleFactor;
            result.y = quaternion1.y * scaleFactor;
            result.z = quaternion1.z * scaleFactor;
            result.w = quaternion1.w * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Sets the length of the quaternion to one.
        /// </summary>
        #region public void Normalize()
        public void Normalize()
        {
            V1 num2 = (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z)) + (this.w * this.w);
            V1 num = 1 / (V1.Sqrt(num2));
            this.x *= num;
            this.y *= num;
            this.z *= num;
            this.w *= num;
        }
        #endregion

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <returns>JQuaternion representing an orientation.</returns>
        #region public static JQuaternion CreateFromMatrix(JMatrix matrix)
        public static FixQuaternion CreateFromMatrix(FixMatrix matrix)
        {
            FixQuaternion result;
            FixQuaternion.CreateFromMatrix(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <param name="result">JQuaternion representing an orientation.</param>
        public static void CreateFromMatrix(ref FixMatrix matrix, out FixQuaternion result)
        {
            V1 num8 = (matrix.M11 + matrix.M22) + matrix.M33;
            if (num8 > V1.Zero)
            {
                V1 num = V1.Sqrt((num8 + V1.One));
                result.w = num * V1.Half;
                num = V1.Half / num;
                result.x = (matrix.M23 - matrix.M32) * num;
                result.y = (matrix.M31 - matrix.M13) * num;
                result.z = (matrix.M12 - matrix.M21) * num;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                V1 num7 = V1.Sqrt((((V1.One + matrix.M11) - matrix.M22) - matrix.M33));
                V1 num4 = V1.Half / num7;
                result.x = V1.Half * num7;
                result.y = (matrix.M12 + matrix.M21) * num4;
                result.z = (matrix.M13 + matrix.M31) * num4;
                result.w = (matrix.M23 - matrix.M32) * num4;
            }
            else if (matrix.M22 > matrix.M33)
            {
                V1 num6 = V1.Sqrt((((V1.One + matrix.M22) - matrix.M11) - matrix.M33));
                V1 num3 = V1.Half / num6;
                result.x = (matrix.M21 + matrix.M12) * num3;
                result.y = V1.Half * num6;
                result.z = (matrix.M32 + matrix.M23) * num3;
                result.w = (matrix.M31 - matrix.M13) * num3;
            }
            else
            {
                V1 num5 = V1.Sqrt((((V1.One + matrix.M33) - matrix.M11) - matrix.M22));
                V1 num2 = V1.Half / num5;
                result.x = (matrix.M31 + matrix.M13) * num2;
                result.y = (matrix.M32 + matrix.M23) * num2;
                result.z = V1.Half * num5;
                result.w = (matrix.M12 - matrix.M21) * num2;
            }
        }
        #endregion

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product of both quaternions.</returns>
        #region public static FP operator *(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator *(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Multiply(ref value1, ref value2,out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Add two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The sum of both quaternions.</returns>
        #region public static FP operator +(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator +(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtract two quaternions.
        /// </summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The difference of both quaternions.</returns>
        #region public static FP operator -(JQuaternion value1, JQuaternion value2)
        public static FixQuaternion operator -(FixQuaternion value1, FixQuaternion value2)
        {
            FixQuaternion result;
            FixQuaternion.Subtract(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /**
         *  @brief Rotates a {@link TSVector} by the {@link TSQuanternion}.
         **/
        public static V3 operator *(FixQuaternion quat, V3 vec) {
            V1 num = quat.x * 2f;
            V1 num2 = quat.y * 2f;
            V1 num3 = quat.z * 2f;
            V1 num4 = quat.x * num;
            V1 num5 = quat.y * num2;
            V1 num6 = quat.z * num3;
            V1 num7 = quat.x * num2;
            V1 num8 = quat.x * num3;
            V1 num9 = quat.y * num3;
            V1 num10 = quat.w * num;
            V1 num11 = quat.w * num2;
            V1 num12 = quat.w * num3;

            V3 result;
            result.x = (1f - (num5 + num6)) * vec.x + (num7 - num12) * vec.y + (num8 + num11) * vec.z;
            result.y = (num7 + num12) * vec.x + (1f - (num4 + num6)) * vec.y + (num9 - num10) * vec.z;
            result.z = (num8 - num11) * vec.x + (num9 + num10) * vec.y + (1f - (num4 + num5)) * vec.z;

            return result;
        }

        public override string ToString() {
            return string.Format("({0:f1}, {1:f1}, {2:f1}, {3:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat(), w.AsFloat());
        }

    }
}
