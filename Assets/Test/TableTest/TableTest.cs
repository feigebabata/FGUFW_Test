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
        
    }




}

//180:(右上 上左 右下 上左)*5 
//顺90:(右上 上左 右下 后右)*7
//逆90:(左上 上右 左下 后左)*7
