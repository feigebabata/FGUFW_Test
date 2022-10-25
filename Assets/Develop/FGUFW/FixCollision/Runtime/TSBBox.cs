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

namespace FGUFW.FixCollision
{
    /// <summary>
    /// Bounding Box defined through min and max vectors.
    /// </summary>
    public struct TSBBox
    {
        /// <summary>
        /// Containment type used within the <see cref="TSBBox"/> structure.
        /// </summary>
        public enum ContainmentType
        {
            /// <summary>
            /// The objects don't intersect.
            /// </summary>
            Disjoint,
            /// <summary>
            /// One object is within the other.
            /// </summary>
            Contains,
            /// <summary>
            /// The two objects intersect.
            /// </summary>
            Intersects
        }

        /// <summary>
        /// The maximum point of the box.
        /// </summary>
        public V3 min;

        /// <summary>
        /// The minimum point of the box.
        /// </summary>
        public V3 max;

        /// <summary>
        /// Returns the largest box possible.
        /// </summary>
        public static readonly TSBBox LargeBox;

        /// <summary>
        /// Returns the smalltest box possible.
        /// </summary>
        public static readonly TSBBox SmallBox;

        static TSBBox()
        {
            LargeBox.min = new V3(V1.MinValue);
            LargeBox.max = new V3(V1.MaxValue);
            SmallBox.min = new V3(V1.MaxValue);
            SmallBox.max = new V3(V1.MinValue);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">The minimum point of the box.</param>
        /// <param name="max">The maximum point of the box.</param>
        public TSBBox(V3 min, V3 max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Transforms the bounding box into the space given by orientation and position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="orientation"></param>
        /// <param name="result"></param>
        internal void InverseTransform(ref V3 position, ref FixMatrix orientation)
        {
            V3.Subtract(ref max, ref position, out max);
            V3.Subtract(ref min, ref position, out min);

            V3 center;
            V3.Add(ref max, ref min, out center);
            center.x *= V1.Half; center.y *= V1.Half; center.z *= V1.Half;

            V3 halfExtents;
            V3.Subtract(ref max, ref min, out halfExtents);
            halfExtents.x *= V1.Half; halfExtents.y *= V1.Half; halfExtents.z *= V1.Half;

            V3.TransposedTransform(ref center, ref orientation, out center);

            FixMatrix abs; FixMath.Absolute(ref orientation, out abs);
            V3.TransposedTransform(ref halfExtents, ref abs, out halfExtents);

            V3.Add(ref center, ref halfExtents, out max);
            V3.Subtract(ref center, ref halfExtents, out min);
        }

        public void Transform(ref FixMatrix orientation)
        {
            V3 halfExtents = V1.Half * (max - min);
            V3 center = V1.Half * (max + min);

            V3.Transform(ref center, ref orientation, out center);

            FixMatrix abs; FixMath.Absolute(ref orientation, out abs);
            V3.Transform(ref halfExtents, ref abs, out halfExtents);

            max = center + halfExtents;
            min = center - halfExtents;
        }

        /// <summary>
        /// Checks whether a point is inside, outside or intersecting
        /// a point.
        /// </summary>
        /// <returns>The ContainmentType of the point.</returns>
        #region public Ray/Segment Intersection

        private bool Intersect1D(V1 start, V1 dir, V1 min, V1 max,
            ref V1 enter,ref V1 exit)
        {
            if (dir * dir < FixMath.Epsilon * FixMath.Epsilon) return (start >= min && start <= max);

            V1 t0 = (min - start) / dir;
            V1 t1 = (max - start) / dir;

            if (t0 > t1) { V1 tmp = t0; t0 = t1; t1 = tmp; }

            if (t0 > exit || t1 < enter) return false;

            if (t0 > enter) enter = t0;
            if (t1 < exit) exit = t1;
            return true;
        }


        public bool SegmentIntersect(ref V3 origin,ref V3 direction)
        {
            V1 enter = V1.Zero, exit = V1.One;

            if (!Intersect1D(origin.x, direction.x, min.x, max.x,ref enter,ref exit))
                return false;

            if (!Intersect1D(origin.y, direction.y, min.y, max.y, ref enter, ref exit))
                return false;

            if (!Intersect1D(origin.z, direction.z, min.z, max.z,ref enter,ref exit))
                return false;

            return true;
        }

        public bool RayIntersect(ref V3 origin, ref V3 direction)
        {
            V1 enter = V1.Zero, exit = V1.MaxValue;

            if (!Intersect1D(origin.x, direction.x, min.x, max.x, ref enter, ref exit))
                return false;

            if (!Intersect1D(origin.y, direction.y, min.y, max.y, ref enter, ref exit))
                return false;

            if (!Intersect1D(origin.z, direction.z, min.z, max.z, ref enter, ref exit))
                return false;

            return true;
        }

        public bool SegmentIntersect(V3 origin, V3 direction)
        {
            return SegmentIntersect(ref origin, ref direction);
        }

        public bool RayIntersect(V3 origin, V3 direction)
        {
            return RayIntersect(ref origin, ref direction);
        }

        /// <summary>
        /// Checks wether a point is within a box or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public ContainmentType Contains(V3 point)
        {
            return this.Contains(ref point);
        }

        /// <summary>
        /// Checks whether a point is inside, outside or intersecting
        /// a point.
        /// </summary>
        /// <param name="point">A point in space.</param>
        /// <returns>The ContainmentType of the point.</returns>
        public ContainmentType Contains(ref V3 point)
        {
            return ((((this.min.x <= point.x) && (point.x <= this.max.x)) &&
                ((this.min.y <= point.y) && (point.y <= this.max.y))) &&
                ((this.min.z <= point.z) && (point.z <= this.max.z))) ? ContainmentType.Contains : ContainmentType.Disjoint;
        }

        #endregion

        /// <summary>
        /// Retrieves the 8 corners of the box.
        /// </summary>
        /// <returns>An array of 8 JVector entries.</returns>
        #region public void GetCorners(JVector[] corners)

        public void GetCorners(V3[] corners)
        {
            corners[0].Set(this.min.x, this.max.y, this.max.z);
            corners[1].Set(this.max.x, this.max.y, this.max.z);
            corners[2].Set(this.max.x, this.min.y, this.max.z);
            corners[3].Set(this.min.x, this.min.y, this.max.z);
            corners[4].Set(this.min.x, this.max.y, this.min.z);
            corners[5].Set(this.max.x, this.max.y, this.min.z);
            corners[6].Set(this.max.x, this.min.y, this.min.z);
            corners[7].Set(this.min.x, this.min.y, this.min.z);
        }

        #endregion


        public void AddPoint(V3 point)
        {
            AddPoint(ref point);
        }

        public void AddPoint(ref V3 point)
        {
            V3.Max(ref this.max, ref point, out this.max);
            V3.Min(ref this.min, ref point, out this.min);
        }

        /// <summary>
        /// Expands a bounding box with the volume 0 by all points
        /// given.
        /// </summary>
        /// <param name="points">A array of JVector.</param>
        /// <returns>The resulting bounding box containing all points.</returns>
        #region public static JBBox CreateFromPoints(JVector[] points)

        public static TSBBox CreateFromPoints(V3[] points)
        {
            V3 vector3 = new V3(V1.MaxValue);
            V3 vector2 = new V3(V1.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                V3.Min(ref vector3, ref points[i], out vector3);
                V3.Max(ref vector2, ref points[i], out vector2);
            }
            return new TSBBox(vector3, vector2);
        }

        #endregion

        /// <summary>
        /// Checks whether another bounding box is inside, outside or intersecting
        /// this box. 
        /// </summary>
        /// <param name="box">The other bounding box to check.</param>
        /// <returns>The ContainmentType of the box.</returns>
        #region public ContainmentType Contains(JBBox box)

        public ContainmentType Contains(TSBBox box)
        {
            return this.Contains(ref box);
        }

        /// <summary>
        /// Checks whether another bounding box is inside, outside or intersecting
        /// this box. 
        /// </summary>
        /// <param name="box">The other bounding box to check.</param>
        /// <returns>The ContainmentType of the box.</returns>
        public ContainmentType Contains(ref TSBBox box)
        {
            ContainmentType result = ContainmentType.Disjoint;
            if ((((this.max.x >= box.min.x) && (this.min.x <= box.max.x)) && ((this.max.y >= box.min.y) && (this.min.y <= box.max.y))) && ((this.max.z >= box.min.z) && (this.min.z <= box.max.z)))
            {
                result = ((((this.min.x <= box.min.x) && (box.max.x <= this.max.x)) && ((this.min.y <= box.min.y) && (box.max.y <= this.max.y))) && ((this.min.z <= box.min.z) && (box.max.z <= this.max.z))) ? ContainmentType.Contains : ContainmentType.Intersects;
            }

            return result;
        }

        #endregion

		public static TSBBox CreateFromCenter(V3 center, V3 size) {
			V3 half = size * V1.Half;
			return new TSBBox (center - half, center + half);
		}

        /// <summary>
        /// Creates a new box containing the two given ones.
        /// </summary>
        /// <param name="original">First box.</param>
        /// <param name="additional">Second box.</param>
        /// <returns>A JBBox containing the two given boxes.</returns>
        #region public static JBBox CreateMerged(JBBox original, JBBox additional)

        public static TSBBox CreateMerged(TSBBox original, TSBBox additional)
        {
            TSBBox result;
            TSBBox.CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        /// Creates a new box containing the two given ones.
        /// </summary>
        /// <param name="original">First box.</param>
        /// <param name="additional">Second box.</param>
        /// <param name="result">A JBBox containing the two given boxes.</param>
        public static void CreateMerged(ref TSBBox original, ref TSBBox additional, out TSBBox result)
        {
            V3 vector;
            V3 vector2;
            V3.Min(ref original.min, ref additional.min, out vector2);
            V3.Max(ref original.max, ref additional.max, out vector);
            result.min = vector2;
            result.max = vector;
        }

        #endregion

        public V3 center { get { return (min + max) * (V1.Half); } }

        public V3 size {
            get {
                return (max - min);
            }
        }

        public V3 extents {
            get {
                return size * V1.Half;
            }
        }

        internal V1 Perimeter
        {
            get
            {
                return (2 * V1.One) * ((max.x - min.x) * (max.y - min.y) +
                    (max.x - min.x) * (max.z - min.z) +
                    (max.z - min.z) * (max.y - min.y));
            }
        }

        public override string ToString() {
            return min + "|" + max;
        }

    }
}
