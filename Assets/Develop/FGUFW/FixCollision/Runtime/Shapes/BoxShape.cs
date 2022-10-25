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
    /// A <see cref="Shape"/> representing a box.
    /// </summary>
    public class BoxShape : Shape
    {
        internal V3 size = V3.zero;

        /// <summary>
        /// The sidelength of the box.
        /// </summary>
        public V3 Size { 
            get { return size; }
            set { size = value; UpdateShape(); }
        }
        
        /// <summary>
        /// Creates a new instance of the BoxShape class.
        /// </summary>
        /// <param name="size">The size of the box.</param>
        public BoxShape(V3 size)
        {
            this.size = size;
            this.UpdateShape();
        }

        /// <summary>
        /// Creates a new instance of the BoxShape class.
        /// </summary>
        /// <param name="length">The length of the box.</param>
        /// <param name="height">The height of the box.</param>
        /// <param name="width">The width of the box</param>
        public BoxShape(V1 length, V1 height, V1 width)
        {
            this.size.x = length;
            this.size.y = height;
            this.size.z = width;
            this.UpdateShape();
        }

        internal V3 halfSize = V3.zero;

        /// <summary>
        /// This method uses the <see cref="ISupportMappable"/> implementation
        /// to calculate the local bounding box, the mass, geometric center and 
        /// the inertia of the shape. In custom shapes this method should be overidden
        /// to compute this values faster.
        /// </summary>
        public override void UpdateShape()
        {
            this.halfSize = size * V1.Half;
            base.UpdateShape();
        }

        /// <summary>
        /// Gets the axis aligned bounding box of the orientated shape.
        /// </summary>
        /// <param name="orientation">The orientation of the shape.</param>
        /// <param name="box">The axis aligned bounding box of the shape.</param>
        public override void GetBoundingBox(ref FixMatrix orientation, out TSBBox box)
        {
            FixMatrix abs; FixMath.Absolute(ref orientation, out abs);
            V3 temp;
            V3.Transform(ref halfSize, ref abs, out temp);

            box.max = temp;
            V3.Negate(ref temp, out box.min);
        }

        /// <summary>
        /// This method uses the <see cref="ISupportMappable"/> implementation
        /// to calculate the local bounding box, the mass, geometric center and 
        /// the inertia of the shape. In custom shapes this method should be overidden
        /// to compute this values faster.
        /// </summary>
        public override void CalculateMassInertia()
        {
            mass = size.x * size.y * size.z;

            inertia = FixMatrix.Identity;
            inertia.M11 = (V1.One / (12 * V1.One)) * mass * (size.y * size.y + size.z * size.z);
            inertia.M22 = (V1.One / (12 * V1.One)) * mass * (size.x * size.x + size.z * size.z);
            inertia.M33 = (V1.One / (12 * V1.One)) * mass * (size.x * size.x + size.y * size.y);

            this.geomCen = V3.zero;
        }

        /// <summary>
        /// SupportMapping. Finds the point in the shape furthest away from the given direction.
        /// Imagine a plane with a normal in the search direction. Now move the plane along the normal
        /// until the plane does not intersect the shape. The last intersection point is the result.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="result">The result.</param>
        public override void SupportMapping(ref V3 direction, out V3 result)
        {
            result.x = V1.Sign(direction.x) * halfSize.x;
            result.y = V1.Sign(direction.y) * halfSize.y;
            result.z = V1.Sign(direction.z) * halfSize.z;
        }
    }
}
