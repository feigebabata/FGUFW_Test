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

#region Using Statements
using System;
using System.Collections.Generic;
using FGUFW;
#endregion

namespace FGUFW.FixCollision 
{

    /// <summary>
    /// Gets called when a shape changes one of the parameters.
    /// For example the size of a box is changed.
    /// </summary>
    public delegate void ShapeUpdatedHandler();

    /// <summary>
    /// Represents the collision part of the RigidBody. A shape is mainly definied through it's supportmap.
    /// Shapes represent convex objects. Inherited classes have to overwrite the supportmap function.
    /// To implement you own shape: derive a class from <see cref="Shape"/>, implement the support map function
    /// and call 'UpdateShape' within the constructor. GeometricCenter, Mass, BoundingBox and Inertia is calculated numerically
    /// based on your SupportMap implementation.
    /// </summary>
    public abstract class Shape : ISupportMappable
    {

        // internal values so we can access them fast  without calling properties.
        internal FixMatrix inertia = FixMatrix.Identity;
        internal V1 mass = V1.One;

        internal TSBBox boundingBox = TSBBox.LargeBox;
        internal V3 geomCen = V3.zero;

        /// <summary>
        /// Gets called when the shape changes one of the parameters.
        /// </summary>
        public event ShapeUpdatedHandler ShapeUpdated;

        /// <summary>
        /// Creates a new instance of a shape.
        /// </summary>
        public Shape()
        {
        }

        /// <summary>
        /// Returns the inertia of the untransformed shape.
        /// </summary>
        public FixMatrix Inertia { get { return inertia; } protected set { inertia = value; } }


        /// <summary>
        /// Gets the mass of the shape. This is the volume. (density = 1)
        /// </summary>
        public V1 Mass { get { return mass; } protected set { mass = value; } }

        /// <summary>
        /// Informs all listener that the shape changed.
        /// </summary>
        protected void RaiseShapeUpdated()
        {
            if (ShapeUpdated != null) ShapeUpdated();
        }

        /// <summary>
        /// The untransformed axis aligned bounding box of the shape.
        /// </summary>
        public TSBBox BoundingBox { get { return boundingBox; } }

        /// <summary>
        /// Allows to set a user defined value to the shape.
        /// </summary>
        public object Tag { get; set; }

        private struct ClipTriangle
        {
            public V3 n1;
            public V3 n2;
            public V3 n3;
            public int generation;
        };

        /// <summary>
        /// Hull making.
        /// </summary>
        /// <remarks>Based/Completely from http://www.xbdev.net/physics/MinkowskiDifference/index.php
        /// I don't (100%) see why this should always work.
        /// </remarks>
        /// <param name="triangleList"></param>
        /// <param name="generationThreshold"></param>
        public virtual void MakeHull(ref List<V3> triangleList, int generationThreshold)
        {
            V1 distanceThreshold = V1.Zero;

            if (generationThreshold < 0) generationThreshold = 4;

            Stack<ClipTriangle> activeTriList = new Stack<ClipTriangle>();

            V3[] v = new V3[] // 6 Array
		    {
			new V3( -1,  0,  0 ),
			new V3(  1,  0,  0 ),

			new V3(  0, -1,  0 ),
			new V3(  0,  1,  0 ),

			new V3(  0,  0, -1 ),
			new V3(  0,  0,  1 ),
		    };

            int[,] kTriangleVerts = new int[8, 3] // 8 x 3 Array
		    {
			{ 5, 1, 3 },
			{ 4, 3, 1 },
			{ 3, 4, 0 },
			{ 0, 5, 3 },

			{ 5, 2, 1 },
			{ 4, 1, 2 },
			{ 2, 0, 4 },
			{ 0, 2, 5 }
		    };

            for (int i = 0; i < 8; i++)
            {
                ClipTriangle tri = new ClipTriangle();
                tri.n1 = v[kTriangleVerts[i, 0]];
                tri.n2 = v[kTriangleVerts[i, 1]];
                tri.n3 = v[kTriangleVerts[i, 2]];
                tri.generation = 0;
                activeTriList.Push(tri);
            }

            // surfaceTriList
            while (activeTriList.Count > 0)
            {
                ClipTriangle tri = activeTriList.Pop();

                V3 p1; SupportMapping(ref tri.n1, out p1);
                V3 p2; SupportMapping(ref tri.n2, out p2);
                V3 p3; SupportMapping(ref tri.n3, out p3);

                V1 d1 = (p2 - p1).sqrMagnitude;
                V1 d2 = (p3 - p2).sqrMagnitude;
                V1 d3 = (p1 - p3).sqrMagnitude;

                if (FixMath.Max(FixMath.Max(d1, d2), d3) > distanceThreshold && tri.generation < generationThreshold)
                {
                    ClipTriangle tri1 = new ClipTriangle();
                    ClipTriangle tri2 = new ClipTriangle();
                    ClipTriangle tri3 = new ClipTriangle();
                    ClipTriangle tri4 = new ClipTriangle();

                    tri1.generation = tri.generation + 1;
                    tri2.generation = tri.generation + 1;
                    tri3.generation = tri.generation + 1;
                    tri4.generation = tri.generation + 1;

                    tri1.n1 = tri.n1;
                    tri2.n2 = tri.n2;
                    tri3.n3 = tri.n3;

                    V3 n = V1.Half * (tri.n1 + tri.n2);
                    n.Normalize();

                    tri1.n2 = n;
                    tri2.n1 = n;
                    tri4.n3 = n;

                    n = V1.Half * (tri.n2 + tri.n3);
                    n.Normalize();

                    tri2.n3 = n;
                    tri3.n2 = n;
                    tri4.n1 = n;

                    n = V1.Half * (tri.n3 + tri.n1);
                    n.Normalize();

                    tri1.n3 = n;
                    tri3.n1 = n;
                    tri4.n2 = n;

                    activeTriList.Push(tri1);
                    activeTriList.Push(tri2);
                    activeTriList.Push(tri3);
                    activeTriList.Push(tri4);
                }
                else
                {
                    if (((p3 - p1) % (p2 - p1)).sqrMagnitude > FixMath.Epsilon)
                    {
                        triangleList.Add(p1);
                        triangleList.Add(p2);
                        triangleList.Add(p3);
                    }
                }
            }
        }


        /// <summary>
        /// Uses the supportMapping to calculate the bounding box. Should be overidden
        /// to make this faster.
        /// </summary>
        /// <param name="orientation">The orientation of the shape.</param>
        /// <param name="box">The resulting axis aligned bounding box.</param>
        public virtual void GetBoundingBox(ref FixMatrix orientation, out TSBBox box)
        {
            // I don't think that this can be done faster.
            // 6 is the minimum number of SupportMap calls.

            V3 vec = V3.zero;

            vec.Set(orientation.M11, orientation.M21, orientation.M31);
            SupportMapping(ref vec, out vec);
            box.max.x = orientation.M11 * vec.x + orientation.M21 * vec.y + orientation.M31 * vec.z;

            vec.Set(orientation.M12, orientation.M22, orientation.M32);
            SupportMapping(ref vec, out vec);
            box.max.y = orientation.M12 * vec.x + orientation.M22 * vec.y + orientation.M32 * vec.z;

            vec.Set(orientation.M13, orientation.M23, orientation.M33);
            SupportMapping(ref vec, out vec);
            box.max.z = orientation.M13 * vec.x + orientation.M23 * vec.y + orientation.M33 * vec.z;

            vec.Set(-orientation.M11, -orientation.M21, -orientation.M31);
            SupportMapping(ref vec, out vec);
            box.min.x = orientation.M11 * vec.x + orientation.M21 * vec.y + orientation.M31 * vec.z;

            vec.Set(-orientation.M12, -orientation.M22, -orientation.M32);
            SupportMapping(ref vec, out vec);
            box.min.y = orientation.M12 * vec.x + orientation.M22 * vec.y + orientation.M32 * vec.z;

            vec.Set(-orientation.M13, -orientation.M23, -orientation.M33);
            SupportMapping(ref vec, out vec);
            box.min.z = orientation.M13 * vec.x + orientation.M23 * vec.y + orientation.M33 * vec.z;
        }

        /// <summary>
        /// This method uses the <see cref="ISupportMappable"/> implementation
        /// to calculate the local bounding box, the mass, geometric center and 
        /// the inertia of the shape. In custom shapes this method should be overidden
        /// to compute this values faster.
        /// </summary>
        public virtual void UpdateShape()
        {
            GetBoundingBox(ref FixMatrix.InternalIdentity, out boundingBox);

            CalculateMassInertia();
            RaiseShapeUpdated();
        }
        
        /// <summary>
        /// Calculates the inertia of the shape relative to the center of mass.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="centerOfMass"></param>
        /// <param name="inertia">Returns the inertia relative to the center of mass, not to the origin</param>
        /// <returns></returns>
        #region  public static FP CalculateMassInertia(Shape shape, out JVector centerOfMass, out JMatrix inertia)
        public static V1 CalculateMassInertia(Shape shape, out V3 centerOfMass,
            out FixMatrix inertia)
        {
            V1 mass = V1.Zero;
            centerOfMass = V3.zero; inertia = FixMatrix.Zero;

            if (shape is Multishape) throw new ArgumentException("Can't calculate inertia of multishapes.", "shape");

            // build a triangle hull around the shape
            List<V3> hullTriangles = new List<V3>();
            shape.MakeHull(ref hullTriangles, 3);

            // create inertia of tetrahedron with vertices at
            // (0,0,0) (1,0,0) (0,1,0) (0,0,1)
            V1 a = V1.One / (60 * V1.One), b = V1.One / (120 * V1.One);
            FixMatrix C = new FixMatrix(a, b, b, b, a, b, b, b, a);

            for (int i = 0; i < hullTriangles.Count; i += 3)
            {
                V3 column0 = hullTriangles[i + 0];
                V3 column1 = hullTriangles[i + 1];
                V3 column2 = hullTriangles[i + 2];

                FixMatrix A = new FixMatrix(column0.x, column1.x, column2.x,
                    column0.y, column1.y, column2.y,
                    column0.z, column1.z, column2.z);

                V1 detA = A.Determinant();

                // now transform this canonical tetrahedron to the target tetrahedron
                // inertia by a linear transformation A
                FixMatrix tetrahedronInertia = FixMatrix.Multiply(A * C * FixMatrix.Transpose(A), detA);

                V3 tetrahedronCOM = (V1.One / (4 * V1.One)) * (hullTriangles[i + 0] + hullTriangles[i + 1] + hullTriangles[i + 2]);
                V1 tetrahedronMass = (V1.One / (6 * V1.One)) * detA;

                inertia += tetrahedronInertia;
                centerOfMass += tetrahedronMass * tetrahedronCOM;
                mass += tetrahedronMass;
            }

            inertia = FixMatrix.Multiply(FixMatrix.Identity, inertia.Trace()) - inertia;
            centerOfMass = centerOfMass * (V1.One / mass);

            V1 x = centerOfMass.x;
            V1 y = centerOfMass.y;
            V1 z = centerOfMass.z;

            // now translate the inertia by the center of mass
            FixMatrix t = new FixMatrix(
                -mass * (y * y + z * z), mass * x * y, mass * x * z,
                mass * y * x, -mass * (z * z + x * x), mass * y * z,
                mass * z * x, mass * z * y, -mass * (x * x + y * y));

            FixMatrix.Add(ref inertia, ref t, out inertia);

            return mass;
        }
        #endregion
        
        /// <summary>
        /// Numerically calculates the inertia, mass and geometric center of the shape.
        /// This gets a good value for "normal" shapes. The algorithm isn't very accurate
        /// for very flat shapes. 
        /// </summary>
        public virtual void CalculateMassInertia()
        {
            this.mass = Shape.CalculateMassInertia(this, out geomCen, out inertia);
        }

        /// <summary>
        /// SupportMapping. Finds the point in the shape furthest away from the given direction.
        /// Imagine a plane with a normal in the search direction. Now move the plane along the normal
        /// until the plane does not intersect the shape. The last intersection point is the result.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="result">The result.</param>
        public abstract void SupportMapping(ref V3 direction, out V3 result);

        /// <summary>
        /// The center of the SupportMap.
        /// </summary>
        /// <param name="geomCenter">The center of the SupportMap.</param>
        public void SupportCenter(out V3 geomCenter)
        {
            geomCenter = this.geomCen;
        }

    }
}
