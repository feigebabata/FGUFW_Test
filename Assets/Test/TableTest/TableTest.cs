using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FGUFW;
using System;
using Unity.Collections;
using ECSTest;
using System.Reflection;

public static class TableTest
{

    [MenuItem("Test/TableTest")]
    static void tableTest()
    {
        var t = typeof(Action<int,MonoBehaviour,Action<int,object>>);
        Debug.Log(t.GetFiledTypeName());

    }



    private static Type getAuthoringType(Type t)
    {
        var fullName = $"{t.FullName}Authoring";
        return Type.GetType(fullName);
    }



}
