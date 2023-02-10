using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunayTriangulation
{
    // 定义点类
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector3(Point point)
        {
            return new Vector3((float)point.X,(float)point.Y,0);
        }

        public static implicit operator Point(Vector3 point)
        {
            return new Point(point.x,point.y);
        }
    }

    // 定义三角形类
    struct Triangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }

        public Triangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
        }

        // 判断给定的点是否在三角形内
        public bool Contains(Point p)
        {
            double ABC = Math.Abs(0.5 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)));
            double ABP = Math.Abs(0.5 * (A.X * (B.Y - p.Y) + B.X * (p.Y - A.Y) + p.X * (A.Y - B.Y)));
            double ACP = Math.Abs(0.5 * (A.X * (p.Y - C.Y) + p.X * (C.Y - A.Y) + C.X * (A.Y - p.Y)));
            double PBC = Math.Abs(0.5 * (p.X * (B.Y - C.Y) + B.X * (C.Y - p.Y) + C.X * (p.Y - B.Y)));

            // 三角形的面积公式
            return ABP + ACP + PBC == ABC;
        }

        //获取顶点
        public Point GetVertex(int index)
        {
            switch (index)
            {
                case 0:
                    return A;
                case 1:
                    return B;
                case 2:
                    return C;
                default:
                    throw new ArgumentException("Invalid index");
            }
        }
    }

    class DelaunayTriangulation
    {
        // 计算 Delaunay 三角剖分
        public static List<Triangle> Triangulate(List<Point> points)
        {
            List<Triangle> triangles = new List<Triangle>();

            // 创建超级三角形，它将包含所有点
            Point min = new Point(points[0].X, points[0].Y);
            Point max = new Point(points[0].X, points[0].Y);

            // 找出所有点的最小和最大坐标
            for (int i = 1; i < points.Count; i++)
            {
                Point p = points[i];
                if (p.X < min.X) min.X = p.X;
                if (p.Y < min.Y) min.Y = p.Y;
                if (p.X > max.X) max.X = p.X;
                if (p.Y > max.Y) max.Y = p.Y;
            }

            // 让超级三角形覆盖所有点
            double dx = max.X - min.X;
            double dy = max.Y - min.Y;
            double deltaMax = Math.Max(dx, dy);
            Point midpoint = new Point((min.X + max.X) / 2, (min.Y + max.Y) / 2);

            // 创建超级三角形
            Point p1 = new Point(midpoint.X - 20 * deltaMax, midpoint.Y - deltaMax);
            Point p2 = new Point(midpoint.X, midpoint.Y + 20 * deltaMax);
            Point p3 = new Point(midpoint.X + 20 * deltaMax, midpoint.Y - deltaMax);
            Triangle superTriangle = new Triangle(p1, p2, p3);

            // 添加超级三角形到三角剖分中
            triangles.Add(superTriangle);

            // 遍历所有点，并对每个点执行插入操作
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];

                // 创建一个面积为负数的三角形列表，它们将被删除
                List<Triangle> badTriangles = new List<Triangle>();

                // 枚举所有当前三角形，并将与当前点不符合 Delaunay 剖分条件的三角形放入 badTriangles 列表中
                for (int j = 0; j < triangles.Count; j++)
                {
                    Triangle t = triangles[j];
                    if (t.Contains(p))
                    {
                        badTriangles.Add(t);
                    }
                }

                // 从三角剖分中删除不符合条件的三角形
                triangles.RemoveAll(t => badTriangles.Contains(t));

                // 创建所有边的列表，它们将被处理为三角形
                List<Tuple<Point, Point>> edges = new List<Tuple<Point, Point>>();
                for (int j = 0; j < badTriangles.Count; j++)
                {
                    Triangle t = badTriangles[j];
                    for (int k = 0; k < 3; k++)
                    {
                        Point edgeStart = t.GetVertex(k);
                        Point edgeEnd = t.GetVertex((k + 1) % 3);
                        if (!edges.Contains(new Tuple<Point, Point>(edgeEnd, edgeStart)))
                        {
                            edges.Add(new Tuple<Point, Point>(edgeStart, edgeEnd));
                        }
                    }
                }
                            // 将每个边处理为一个新的三角形，并将它们添加到三角剖分中
                foreach (var edge in edges)
                {
                    Triangle triangle = new Triangle(edge.Item1, edge.Item2, p);
                    triangles.Add(triangle);
                }
            }

            // 删除超级三角形及其关联的三角形
            triangles.RemoveAll(t => t.Contains(p1) || t.Contains(p2) || t.Contains(p3));
            
            return triangles;
        }
    }
}
