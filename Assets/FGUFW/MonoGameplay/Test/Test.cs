using System.Collections;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool Assert;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        this.log("测试");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        this.log(Random.Range(0f,1f));
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        this.assert(Assert,"断言");
    }

}
