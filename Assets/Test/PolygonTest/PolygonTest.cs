using System.Collections;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

public class PolygonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] line = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            line[i] = transform.GetChild(i).transform.position;
        }

        var collider = GetComponent<PolygonCollider2D>();
        var points = LineHelper.GetPolygon2DPoints(line,50,Vector3.back);
        collider.pathCount = points.Length;
        collider.points = points;
    }


}
