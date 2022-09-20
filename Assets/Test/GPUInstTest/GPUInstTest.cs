using System.Collections;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

public class GPUInstTest : MonoBehaviour
{
    public GameObject Template;
    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject gObj = Template.Copy(null);
            gObj.transform.position = VectorHelper.RandomInCircle(Vector3.zero,1);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
