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
        var ls = new List<int>();
        ls.Add(0);
        ls.Add(0);
        var ns = new NativeArray<int>(3, Allocator.Temp);
        var s1 = new StructTest
        {
            List = ls,
            NS = ns
        };

        var s2 = s1;
        object[] s_arr = { s1, s2 };

        var s3 = (IDisposable)s_arr[0];
        s3.Dispose();
        //((IDisposable)s_arr[1]).Dispose();

        Debug.Log(s_arr[0]);
        Debug.Log(s_arr[1]);

    }

    public struct StructTest:IDisposable
    {
        public List<int> List;
        public NativeArray<int> NS;

        public void Dispose()
        {
            List?.Clear();
            List = null;
            NS.Dispose();
        }

        public override string ToString()
        {
            return $"{List?.Count} {NS.IsCreated}";
        }
    }
}
