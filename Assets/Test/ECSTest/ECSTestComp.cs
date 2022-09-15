using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

public class ECSTestComp : MonoBehaviour
{
    World _world;

    // Start is called before the first frame update
    void Start()
    {
        _world = new World();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _world.Dispose();
    }


}
