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
        List<Vector3> points = new List<Vector3>();
        foreach (Transform item in transform)
        {
            points.Add(item.position);
        }
        bool error = false;
        var triangles = Polygon2TrianglesHelper.ToTriangles(points.ToArray(),Vector3.forward,ref error);
        for (int i = 0; i < triangles.Length; i+=3)
        {
            Debug.DrawLine(points[triangles[i]],points[triangles[i+1]],error?Color.red:Color.green);
            Debug.DrawLine(points[triangles[i+1]],points[triangles[i+2]],error?Color.red:Color.green);
            Debug.DrawLine(points[triangles[i+2]],points[triangles[i]],error?Color.red:Color.green);
        }
    }
}
