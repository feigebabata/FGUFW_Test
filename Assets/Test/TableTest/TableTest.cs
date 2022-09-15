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
        List<int> ls = new List<int>();
        ls.Add(5);
        ls.Add(4);
        ls.Add(7);
        ls.Add(1);
        ls.Sort((l,r)=>{return r-l;});
        foreach (var item in ls)
        {
            Debug.Log(item);
        }
    }

    static void fun1(ref Vector4 val)
    {
        val = Vector4.one;
    }

    static Vector4 fun2(Vector4 val)
    {
        val = Vector4.one;
        return val;
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
