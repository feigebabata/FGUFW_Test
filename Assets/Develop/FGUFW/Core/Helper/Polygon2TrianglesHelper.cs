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

        public unsafe static int[] ToTriangles(Vector3[] vertices,Vector3 normal,ref bool error,int[] triangles=null)
        {
            if(vertices==null || vertices.Length<3)return null;

            int triangleLength = (vertices.Length-2)*3;
            if(triangles==null || triangles.Length!=triangleLength)triangles=new int[triangleLength];
            int triangleIndex = 0;
            var polygonLength = vertices.Length;
            var polygonPointer = stackalloc Vector3[polygonLength];
            var polygonIndex = stackalloc int[polygonLength];
            var polygon = new StackPolygon(polygonPointer,polygonIndex,vertices,normal.normalized);
            error = false;
            while (!polygon.ConvexPolygon())
            {
                error = true;
                for (int i = 0; i < polygon.Length; i++)
                {
                    //DO:切耳朵
                    if(polygon.CanCutConcaveAngle(i))
                    {
                        polygon.CutConvexAngle(i,triangles,ref triangleIndex);
                        error = false;
                        break;
                    }
                }
                if(error)break;
            }
            polygon.SqlitConvex(triangles,ref triangleIndex);
            
            return triangles;
        }


        private unsafe struct StackPolygon
        {
            private Vector3* _vertices;
            private int* _verticeIndexs;
            private Vector3 _normal;
            public int Length{get;private set;}
            public StackPolygon(Vector3* vertices,int* verticeIndexs,Vector3[] polygon,Vector3 normal)
            {
                _vertices = vertices;
                _verticeIndexs = verticeIndexs;
                Length = polygon.Length;
                for (int i = 0; i < Length; i++)
                {
                    vertices[i] = polygon[i];
                    verticeIndexs[i] = i;
                }
                _normal = normal;
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

                var val = Vector3.Dot(normal, _normal);
                return val < 0;
            }

            /// <summary>
            /// 能否切去耳朵
            /// </summary>
            /// <param name="vertexIndex"></param>
            /// <returns></returns>
            public bool CanCutConcaveAngle(int vertexIndex)
            {
                if(!IsConcaveAngle(vertexIndex))return false;
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

            /// <summary>
            /// 切去凹角旁边的凸角
            /// </summary>
            public void CutConvexAngle(int vertexIndex,int[] triangles,ref int triangleIndex)
            {
                int l_idx = (vertexIndex-1).RoundIndex(Length);
                int r_idx = (vertexIndex+1).RoundIndex(Length);
                if(!IsConcaveAngle(l_idx))
                {
                    vertexIndex-=2;
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex++).RoundIndex(Length)];
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex++).RoundIndex(Length)];
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex++).RoundIndex(Length)];
                    RemoveVertex(l_idx);
                }
                else if(!IsConcaveAngle(r_idx))
                {
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex--).RoundIndex(Length)];
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex--).RoundIndex(Length)];
                    triangles[triangleIndex++] = _verticeIndexs[(vertexIndex--).RoundIndex(Length)];
                    RemoveVertex(r_idx);
                }
            }

            public void RemoveVertex(int vertexIndex)
            {
                if(vertexIndex>=Length)return;
                Length--;
                for (int i = vertexIndex; i < Length; i++)
                {
                    _vertices[i]=_vertices[i+1];
                    _verticeIndexs[i]=_verticeIndexs[i+1];
                }
            }

            /// <summary>
            /// 切割凸多边形
            /// </summary>
            public void SqlitConvex(int[] triangles,ref int triangleIndex)
            {
                if(Length<3)return;
                for (int i = 1; i < Length-1; i++)
                {
                    triangles[triangleIndex++] = _verticeIndexs[0];
                    triangles[triangleIndex++] = _verticeIndexs[(i).RoundIndex(Length)];
                    triangles[triangleIndex++] = _verticeIndexs[(i+1).RoundIndex(Length)];
                }
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