using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FGUFW;
using System;
using Unity.Collections;
using ECSTest;
using System.Reflection;
using System.IO;
using Unity.Mathematics;

public static class TableTest
{

    [MenuItem("Test/TableTest")]
    static void tableTest()
    {
        int i = 1;
        Debug.Log(0.5f/i);
    }

    [MenuItem("Test/TableTest2")]
    static void tableTest2()
    {
        test1(0.125f, 17);
    }

    static void test1(float t,int n)
    {
        float length = 1;
        for (int i = 0; i < n; i++)
        {
            length -= t*length;
        }

        Debug.Log(1-length);

    }


}

/*
0.1
0.09
0.081
0.0729
*/
