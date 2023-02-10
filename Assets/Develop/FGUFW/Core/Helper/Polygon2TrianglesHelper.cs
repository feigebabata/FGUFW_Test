using UnityEngine;
using System;

namespace FGUFW
{
    /// <summary>
    /// 多边形转三角形 顺时针 法线为相机方向 
    /// n个顶点的多边形可以切成n-2个三角形
    /// </summary>
    public static class Polygon2TrianglesHelper
    {

        public unsafe static int[] ToTriangles(Vector3[] vertices,int[] triangles=null)
        {
            if(vertices==null || vertices.Length<3)return null;

            int triangleLength = (vertices.Length-2)*3;
            if(triangles==null || triangles.Length!=triangleLength)triangles=new int[triangleLength];
            
            var polygonLength = vertices.Length;
            var polygonPointer = stackalloc Vector3[polygonLength];
            var polygon = new StackPolygon(polygonPointer,vertices);
            while (!polygon.ConvexPolygon())
            {
                for (int i = 0; i < polygon.Length; i++)
                {
                    //DO:切耳朵
                }
            }
            
            return triangles;
        }

        static ValueTuple<Vector3,Vector3> getVector(Vector3[] polygon,int vertexIndex)
        {
            int length = polygon.Length;
            // 计算该内角对应的两个相邻顶点的向量
            Vector3 v1 = polygon[(vertexIndex + length - 1) % length] - polygon[vertexIndex];
            Vector3 v2 = polygon[(vertexIndex + 1) % length] - polygon[vertexIndex];

            return (v1,v2);
        }

        /// <summary>
        /// 凸多边形
        /// </summary>
        public unsafe static bool ConvexPolygon(Vector3* polygon,int startIndex,int length)
        {
            return true;
        }



        /// <summary>
        /// 凹顶点
        /// </summary>
        public static bool IsConcaveAngle(Vector3[] polygon,Vector3 center, int vertexIndex)
        {
            var (v1,v2) = getVector(polygon,vertexIndex);

            // 计算该内角的外向法线
            Vector3 normal = Vector3.Cross(v1, v2).normalized;

            // 计算该内角的外向法线与多边形内部的向量
            Vector3 direction = polygon[vertexIndex] - center;

            // 判断该内角的外向法线与多边形内部的向量的点积是否为负
            return Vector3.Dot(normal, direction) < 0;
        }

        public static Vector3 CenterPoint(Vector3[] polygon)
        {
            // 计算多边形的中心点
            Vector3 center = Vector3.zero;
            for (int i = 0; i < polygon.Length; i++)
            {
                center += polygon[i];
            }
            center /= polygon.Length;
            return center;
        }


        private unsafe struct StackPolygon
        {
            private Vector3* _vertices;
            private Vector3 _center;
            public int Length{get;private set;}
            public StackPolygon(Vector3* vertices,Vector3[] polygon)
            {
                _vertices = vertices;
                Length = polygon.Length;
                for (int i = 0; i < Length; i++)
                {
                    vertices[i] = polygon[i];
                }
                _center = centerPoint(_vertices,Length);
            }

            /// <summary>
            /// 凸多边形
            /// </summary>
            /// <returns></returns>
            public bool ConvexPolygon()
            {
                for (int i = 0; i < Length; i++)
                {
                    if(IsConcaveAngle(i))
                    {
                        return false;
                    }
                }
                return true;
            }
            
            /// <summary>
            /// 凹顶点
            /// </summary>
            public bool IsConcaveAngle(int vertexIndex)
            {
                var (v1,v2) = getVector(vertexIndex);

                // 计算该内角的外向法线
                Vector3 normal = Vector3.Cross(v1, v2).normalized;

                // 计算该内角的外向法线与多边形内部的向量
                Vector3 direction = _vertices[vertexIndex] - _center;

                // 判断该内角的外向法线与多边形内部的向量的点积是否为负
                return Vector3.Dot(normal, direction) < 0;
            }

            public bool CanCutConcaveAngle(int vertexIndex)
            {
                int l_idx = (vertexIndex-1).RoundIndex(Length);
                int r_idx = (vertexIndex+1).RoundIndex(Length);
                return !IsConcaveAngle(l_idx) || !IsConcaveAngle(r_idx);
            }

            ValueTuple<Vector3,Vector3> getVector(int vertexIndex)
            {
                // 计算该内角对应的两个相邻顶点的向量
                Vector3 v1 = _vertices[(vertexIndex + Length - 1) % Length] - _vertices[vertexIndex];
                Vector3 v2 = _vertices[(vertexIndex + 1) % Length] - _vertices[vertexIndex];
                return (v1,v2);
            }

            void cutConvexAngle(int vertex)
            {

            }

            static Vector3 centerPoint(Vector3* vertices,int length)
            {
                // 计算多边形的中心点
                Vector3 center = Vector3.zero;
                for (int i = 0; i < length; i++)
                {
                    center += vertices[i];
                }
                center /= length;
                return center;
            }
        }


    }

    public class Polygon
    {
        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector3 Normal;
    }

}