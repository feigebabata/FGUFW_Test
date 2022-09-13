using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FGUFW;
using System;

public static class TableTest
{
    static TableInt<ValueType> tab;
    static object obj;

    [MenuItem("Test/TableTest")]
    static void tableTest()
    {
        var table = new TableInt<ValueType>();
        table[1] = new Vector2(1,1);
        table[5] = new Vector2(5,5);
        table[4] = new Vector2(4,4);
        
        tab = table;
        Debug.Log(tab);
    }
}
