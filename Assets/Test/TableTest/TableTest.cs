using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FGUFW;
using System;
using Unity.Collections;

public static class TableTest
{

    [MenuItem("Test/TableTest")]
    static void tableTest()
    {
        NativeArray<int> nas = new NativeArray<int>(1,Allocator.Temp);
        nas[0] = 1;
        setValue(ref nas);
        Debug.Log(nas[0]);
        Debug.Log(nas.IsCreated);
        // nas.Dispose();
    }

    static void setValue(ref NativeArray<int> nas)
    {
        nas[0] = 233;
        // nas.Dispose();
        Debug.Log(nas.IsCreated);
    }

}
