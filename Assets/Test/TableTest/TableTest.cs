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
        int length = 1024*1024*24;
        
        long start = 0;
        
        
        start = DateTime.UtcNow.Ticks;
        for (int i = 0; i < length; i++)
        {
            Vector4 val = Vector4.zero;
            fun1( ref val);
        }
        Debug.Log(DateTime.UtcNow.Ticks-start);

        //-*-

        start = DateTime.UtcNow.Ticks;
        for (int i = 0; i < length; i++)
        {
            Vector4 val = Vector4.zero;
            val = fun2(val);
        }
        Debug.Log(DateTime.UtcNow.Ticks-start);
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
