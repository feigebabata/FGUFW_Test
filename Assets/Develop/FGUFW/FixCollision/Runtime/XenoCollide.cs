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

namespace FGUFW.FixCollision {

    /// <summary>
    /// 定义了形状的形式
    /// The implementation of the ISupportMappable interface defines the form
    /// of a shape. <seealso cref="GJKCollide"/> <seealso cref="XenoCollide"/>
    /// </summary>
    public interface ISupportMappable
    {
        /// <summary>
        /// 支持映射。查找形状中离给定方向最远的点。想象一下，一架飞机在搜索方向上有一个法线。现在沿法线移动平面，直到平面不与形状相交。最后一个交点是结果。
        /// SupportMapping. Finds the point in the shape furthest away from the given direction.
        /// Imagine a plane with a normal in the search direction. Now move the plane along the normal
        /// until the plane does not intersect the shape. The last intersection point is the result.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="result">The result.</param>
        void SupportMapping(ref V3 direction, out V3 result);

        /// <summary>
        /// The center of the SupportMap.
        /// </summary>
        /// <param name="center"></param>
        void SupportCenter(out V3 center);
    }

    /// <summary>
    /// XenoCollide 是一种用于检测无限多种凸形之间碰撞的算法。它基于一种我称之为Minkowski Portal Refinement (MPR) 的新技术。
    /// Implementation of the XenoCollide Algorithm by Gary Snethen. 
    /// Narrowphase collision detection highly optimized for C#.
    /// http://xenocollide.snethen.com/
    /// </summary>
    public sealed class XenoCollide
    {

        private static V1 CollideEpsilon = V1.EN3;
        private const int MaximumIterations = 5;

        private static void SupportMapTransformed(ISupportMappable support,
            ref FixMatrix orientation, ref V3 position, ref V3 direction, out V3 result)
        {
            // THIS IS *THE* HIGH FREQUENCY CODE OF THE COLLLISION PART OF THE ENGINE

            result.x = ((direction.x * orientation.M11) + (direction.y * orientation.M12)) + (direction.z * orientation.M13);
            result.y = ((direction.x * orientation.M21) + (direction.y * orientation.M22)) + (direction.z * orientation.M23);
            result.z = ((direction.x * orientation.M31) + (direction.y * orientation.M32)) + (direction.z * orientation.M33);

            support.SupportMapping(ref result, out result);

            V1 x = ((result.x * orientation.M11) + (result.y * orientation.M21)) + (result.z * orientation.M31);
            V1 y = ((result.x * orientation.M12) + (result.y * orientation.M22)) + (result.z * orientation.M32);
            V1 z = ((result.x * orientation.M13) + (result.y * orientation.M23)) + (result.z * orientation.M33);

            result.x = position.x + x;
            result.y = position.y + y;
            result.z = position.z + z;
        }

        /// <summary>
        /// Checks two shapes for collisions.
        /// </summary>
        /// <param name="support1">The SupportMappable implementation of the first shape to test.</param>
        /// <param name="support2">The SupportMappable implementation of the seconds shape to test.</param>
        /// <param name="orientation1">The orientation of the first shape.</param>
        /// <param name="orientation2">The orientation of the second shape.</param>
        /// <param name="position1">The position of the first shape.</param>
        /// <param name="position2">The position of the second shape</param>
        /// <param name="point">The pointin world coordinates, where collision occur.</param>
        /// <param name="normal">The normal pointing from body2 to body1.</param>
        /// <param name="penetration">Estimated penetration depth of the collision.</param>
        /// <returns>Returns true if there is a collision, false otherwise.</returns>
        public static bool Detect(ISupportMappable support1, ISupportMappable support2, ref FixMatrix orientation1,
             ref FixMatrix orientation2, ref V3 position1, ref V3 position2,
             out V3 point, out V3 normal, out V1 penetration)
        {
            // Used variables
            V3 temp1, temp2;
            V3 v01, v02, v0;
            V3 v11, v12, v1;
            V3 v21, v22, v2;
            V3 v31, v32, v3;
			V3 v41 = V3.zero, v42 = V3.zero, v4 = V3.zero;
            V3 mn;

            // Initialization of the output
            point = normal = V3.zero;
            penetration = V1.Zero;

            //JVector right = JVector.Right;

            // Get the center of shape1 in world coordinates -> v01
            support1.SupportCenter(out v01);
            V3.Transform(ref v01, ref orientation1, out v01);
            V3.Add(ref position1, ref v01, out v01);

            // Get the center of shape2 in world coordinates -> v02
            support2.SupportCenter(out v02);
            V3.Transform(ref v02, ref orientation2, out v02);
            V3.Add(ref position2, ref v02, out v02);

            // v0 is the center of the minkowski difference
            V3.Subtract(ref v02, ref v01, out v0);

            // Avoid case where centers overlap -- any direction is fine in this case
            if (v0.IsNearlyZero()) v0 = new V3(V1.EN4, 0, 0);

            // v1 = support in direction of origin
            mn = v0;
            V3.Negate(ref v0, out normal);
			//UnityEngine.Debug.Log("normal: " + normal);

            SupportMapTransformed(support1, ref orientation1, ref position1, ref mn, out v11);
            SupportMapTransformed(support2, ref orientation2, ref position2, ref normal, out v12);
            V3.Subtract(ref v12, ref v11, out v1);

            if (V3.Dot(ref v1, ref normal) <= V1.Zero) return false;

            // v2 = support perpendicular to v1,v0
            V3.Cross(ref v1, ref v0, out normal);

            if (normal.IsNearlyZero())
            {
                V3.Subtract(ref v1, ref v0, out normal);
				//UnityEngine.Debug.Log("normal: " + normal);

                normal.Normalize();

                point = v11;
                V3.Add(ref point, ref v12, out point);
                V3.Multiply(ref point, V1.Half, out point);

                V3.Subtract(ref v12, ref v11, out temp1);
                penetration = V3.Dot(ref temp1, ref normal);

                //point = v11;
                //point2 = v12;
                return true;
            }

            V3.Negate(ref normal, out mn);
            SupportMapTransformed(support1, ref orientation1, ref position1, ref mn, out v21);
            SupportMapTransformed(support2, ref orientation2, ref position2, ref normal, out v22);
            V3.Subtract(ref v22, ref v21, out v2);

            if (V3.Dot(ref v2, ref normal) <= V1.Zero) return false;

            // Determine whether origin is on + or - side of plane (v1,v0,v2)
            V3.Subtract(ref v1, ref v0, out temp1);
            V3.Subtract(ref v2, ref v0, out temp2);
            V3.Cross(ref temp1, ref temp2, out normal);

            V1 dist = V3.Dot(ref normal, ref v0);

            // If the origin is on the - side of the plane, reverse the direction of the plane
            if (dist > V1.Zero)
            {
                V3.Swap(ref v1, ref v2);
                V3.Swap(ref v11, ref v21);
                V3.Swap(ref v12, ref v22);
                V3.Negate(ref normal, out normal);
				//UnityEngine.Debug.Log("normal: " + normal);
            }


            int phase2 = 0;
            int phase1 = 0;
            bool hit = false;

            // Phase One: Identify a portal
            while (true)
            {
                if (phase1 > MaximumIterations) return false;

                phase1++;

                // Obtain the support point in a direction perpendicular to the existing plane
                // Note: This point is guaranteed to lie off the plane
                V3.Negate(ref normal, out mn);
				//UnityEngine.Debug.Log("mn: " + mn);
                SupportMapTransformed(support1, ref orientation1, ref position1, ref mn, out v31);
                SupportMapTransformed(support2, ref orientation2, ref position2, ref normal, out v32);
                V3.Subtract(ref v32, ref v31, out v3);


                if (V3.Dot(ref v3, ref normal) <= V1.Zero)
                {
                    return false;
                }

                // If origin is outside (v1,v0,v3), then eliminate v2 and loop
                V3.Cross(ref v1, ref v3, out temp1);
                if (V3.Dot(ref temp1, ref v0) < V1.Zero)
                {
                    v2 = v3;
                    v21 = v31;
                    v22 = v32;
                    V3.Subtract(ref v1, ref v0, out temp1);
                    V3.Subtract(ref v3, ref v0, out temp2);
                    V3.Cross(ref temp1, ref temp2, out normal);
				//	UnityEngine.Debug.Log("normal: " + normal);
                    continue;
                }

                // If origin is outside (v3,v0,v2), then eliminate v1 and loop
                V3.Cross(ref v3, ref v2, out temp1);
                if (V3.Dot(ref temp1, ref v0) < V1.Zero)
                {
                    v1 = v3;
                    v11 = v31;
                    v12 = v32;
                    V3.Subtract(ref v3, ref v0, out temp1);
                    V3.Subtract(ref v2, ref v0, out temp2);
                    V3.Cross(ref temp1, ref temp2, out normal);
					//UnityEngine.Debug.Log("normal: " + normal);
                    continue;
                }

                // Phase Two: Refine the portal
                // We are now inside of a wedge...
                while (true)
                {
                    phase2++;
					/*
					UnityEngine.Debug.LogError(" ::Start STATE");
					UnityEngine.Debug.Log(temp1 + " " +  temp2);
					UnityEngine.Debug.Log( v01 + " " + v02 + " "+ v0);
					UnityEngine.Debug.Log( v11+" "+ v12 +" "+ v1);
	                UnityEngine.Debug.Log( v21 +" "+ v22 +" "+ v2);
	                UnityEngine.Debug.Log( v31 +" "+ v32 +" "+ v3);
	                UnityEngine.Debug.Log( v41 +" "+ v42 +" "+ v4);
	                UnityEngine.Debug.Log( mn);

					UnityEngine.Debug.LogError(" ::END STATE"); 
*/
					// Compute normal of the wedge face
                    V3.Subtract(ref v2, ref v1, out temp1);
                    V3.Subtract(ref v3, ref v1, out temp2);
                    V3.Cross(ref temp1, ref temp2, out normal);
				// Beginer
				//	UnityEngine.Debug.Log("normal: " + normal);

                    // Can this happen???  Can it be handled more cleanly?
                    if (normal.IsNearlyZero()) return true;

                    normal.Normalize();
					//UnityEngine.Debug.Log("normal: " + normal);
                    // Compute distance from origin to wedge face
                    V1 d = V3.Dot(ref normal, ref v1);


                    // If the origin is inside the wedge, we have a hit
                    if (d >= 0 && !hit)
                    {
                        // HIT!!!
                        hit = true;
                    }

                    // Find the support point in the direction of the wedge face
                    V3.Negate(ref normal, out mn);
                    SupportMapTransformed(support1, ref orientation1, ref position1, ref mn, out v41);
                    SupportMapTransformed(support2, ref orientation2, ref position2, ref normal, out v42);
                    V3.Subtract(ref v42, ref v41, out v4);

                    V3.Subtract(ref v4, ref v3, out temp1);
                    V1 delta = V3.Dot(ref temp1, ref normal);
                    penetration = V3.Dot(ref v4, ref normal);

                    // If the boundary is thin enough or the origin is outside the support plane for the newly discovered vertex, then we can terminate
                    if (delta <= CollideEpsilon || penetration <= V1.Zero || phase2 > MaximumIterations)
                    {

                        if (hit)
                        {
                            V3.Cross(ref v1, ref v2, out temp1);
                            V1 b0 = V3.Dot(ref temp1, ref v3);
                            V3.Cross(ref v3, ref v2, out temp1);
                            V1 b1 = V3.Dot(ref temp1, ref v0);
                            V3.Cross(ref v0, ref v1, out temp1);
                            V1 b2 = V3.Dot(ref temp1, ref v3);
                            V3.Cross(ref v2, ref v1, out temp1);
                            V1 b3 = V3.Dot(ref temp1, ref v0);

                            V1 sum = b0 + b1 + b2 + b3;

                            if (sum <= 0)
                            {
                                b0 = 0;
                                V3.Cross(ref v2, ref v3, out temp1);
                                b1 = V3.Dot(ref temp1, ref normal);
                                V3.Cross(ref v3, ref v1, out temp1);
                                b2 = V3.Dot(ref temp1, ref normal);
                                V3.Cross(ref v1, ref v2, out temp1);
                                b3 = V3.Dot(ref temp1, ref normal);

                                sum = b1 + b2 + b3;
                            }

                            V1 inv = V1.One / sum;

                            V3.Multiply(ref v01, b0, out point);
                            V3.Multiply(ref v11, b1, out temp1);
                            V3.Add(ref point, ref temp1, out point);
                            V3.Multiply(ref v21, b2, out temp1);
                            V3.Add(ref point, ref temp1, out point);
                            V3.Multiply(ref v31, b3, out temp1);
                            V3.Add(ref point, ref temp1, out point);

                            V3.Multiply(ref v02, b0, out temp2);
                            V3.Add(ref temp2, ref point, out point);
                            V3.Multiply(ref v12, b1, out temp1);
                            V3.Add(ref point, ref temp1, out point);
                            V3.Multiply(ref v22, b2, out temp1);
                            V3.Add(ref point, ref temp1, out point);
                            V3.Multiply(ref v32, b3, out temp1);
                            V3.Add(ref point, ref temp1, out point);

                            V3.Multiply(ref point, inv * V1.Half, out point);

                        }

                        // Compute the barycentric coordinates of the origin
                        return hit;
                    }

                    //// Compute the tetrahedron dividing face (v4,v0,v1)
                    //JVector.Cross(ref v4, ref v1, out temp1);
                    //FP d1 = JVector.Dot(ref temp1, ref v0);


                    //// Compute the tetrahedron dividing face (v4,v0,v2)
                    //JVector.Cross(ref v4, ref v2, out temp1);
                    //FP d2 = JVector.Dot(ref temp1, ref v0);


                    // Compute the tetrahedron dividing face (v4,v0,v3)
					//UnityEngine.Debug.LogError("v4:" +  v4 + " v0:" + v0);
                    V3.Cross(ref v4, ref v0, out temp1);
					//UnityEngine.Debug.LogError("temp1:"+ temp1);
					
					//Ender
				//	UnityEngine.Debug.Log("normal: " + normal);
                    V1 dot = V3.Dot(ref temp1, ref v1);

                    if (dot >= V1.Zero)
                    {
					//	UnityEngine.Debug.Log("dot >= 0 temp1:" + temp1 + "  v2:" + v2 );
                        dot = V3.Dot(ref temp1, ref v2);

                        if (dot >= V1.Zero)
                        {
					//		UnityEngine.Debug.Log("dot >= 0 v1->v4");
							
                            // Inside d1 & inside d2 ==> eliminate v1
                            v1 = v4;
                            v11 = v41;
                            v12 = v42;
                        }
                        else
                        {
					//		UnityEngine.Debug.Log("dot < v3->v4");
							
                            // Inside d1 & outside d2 ==> eliminate v3
                            v3 = v4;
                            v31 = v41;
                            v32 = v42;
                        }
                    }
                    else
                    {
					//	UnityEngine.Debug.Log("dot < 0 temp1:" + temp1 + "  v3:" + v3 );
                        dot = V3.Dot(ref temp1, ref v3);

                        if (dot >= V1.Zero)
                        {
						//	UnityEngine.Debug.Log("dot >= 0 v2 => v4");
                            // Outside d1 & inside d3 ==> eliminate v2
                            v2 = v4;
                            v21 = v41;
                            v22 = v42;
                        }
                        else
                        {
					//		UnityEngine.Debug.Log("dot < 0 v1 => v4");
                            // Outside d1 & outside d3 ==> eliminate v1
                            v1 = v4;
                            v11 = v41;
                            v12 = v42;
                        }
                    }


                }
            }

        }

    }
}