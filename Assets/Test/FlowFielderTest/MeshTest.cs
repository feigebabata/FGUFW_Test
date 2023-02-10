using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunayTriangulation;
using FGUFW;

public class MeshTest : MonoBehaviour
{

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Vector3 a = new Vector3(1,0,0).normalized;
        Vector3 b = new Vector3(-1,1,0).normalized;
        Vector3 c = new Vector3(-1,-1,0).normalized;
        Debug.Log(Vector3.Dot(a,b));
        Debug.Log(Vector3.Dot(a,c));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        // List<Point> points = new List<Point>();
        // foreach (Transform item in transform)
        // {
        //     points.Add(new Point{X=item.position.x,Y=item.position.y});
        // }
        // var triangles = DelaunayTriangulation.DelaunayTriangulation.Triangulate(points);
        // foreach (var item in triangles)
        // {
        //     Debug.DrawLine(item.A,item.B);
        //     Debug.DrawLine(item.A,item.C);
        //     Debug.DrawLine(item.C,item.B);
        // }
    }
}
