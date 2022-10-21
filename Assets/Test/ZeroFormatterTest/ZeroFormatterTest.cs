using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroFormatter;

public class ZeroFormatterTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] buffer = new byte[512];
        var data = new Data(123,1);
        var length = ZeroFormatterSerializer.Serialize(ref buffer,0,data);

        data = ZeroFormatterSerializer.Deserialize<Data>(buffer,0);

        Debug.Log($"id={data.Id} len={length}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ZeroFormattable]
    public struct Data
    {
        [Index(0)]
        public int Id{get;set;}

        [Index(1)]
        public byte Val{get;set;}

        public Data(int id,byte val)
        {
            Id = id;
            Val = val;
        }

    }
}
