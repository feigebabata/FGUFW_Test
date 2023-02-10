using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFielderTest : MonoBehaviour
{
    public Transform point;
    public Collider2D[] Collider2Ds;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void Update()
    {
        Debug.Log(inObstacle(new Vector2(point.position.x,point.position.y),Collider2Ds));
    }

    private bool inObstacle(Vector2 point,Collider2D[] collider2Ds)
    {
        foreach (var collider2D in collider2Ds)
        {
            if(collider2D.OverlapPoint(point))
            {
                return true;
            }
        }
        return false;
    }
}
