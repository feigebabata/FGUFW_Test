using System;
using UnityEngine;

namespace FGUFW
{
    public static class MeshHelper
    {
        /// <summary>
        /// 创建2D的Mesh
        /// </summary>
        /// <param name="size">尺寸</param>
        /// <param name="pivot">轴点</param>
        /// <returns></returns>
        public static Mesh CreateQuad(Vector2 size,Vector2 pivot)
        {
            /*  
                23
                01
            */
            Vector3 v_offset = -VectorHelper.Multiply(size,pivot);
            Mesh mesh = new Mesh
            {
                vertices = new Vector3[]
                {
                    Vector3.zero+v_offset,
                    Vector3.right*size.x+v_offset,
                    Vector3.up*size.y+v_offset,
                    new Vector3(size.x,size.y)+v_offset
                },
                triangles = new int[]{0,3,1,3,0,2},
                normals = new Vector3[]{Vector3.back,Vector3.back,Vector3.back,Vector3.back},
                uv = new Vector2[]{Vector2.zero,Vector2.right,Vector2.up,Vector2.one}
            };
            return mesh;
        }

    }
}