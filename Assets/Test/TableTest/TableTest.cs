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
        test1(0.5f,2);
    }

    [MenuItem("Test/TableTest2")]
    static void tableTest2()
    {
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

    static float unLerp(int n)
    {
        int count = 0;
        float length = 0;
        float t = 0.5f;
        
        float min = 0;
        float max = 1;
        const float side = 0.9f;
        do
        {
            t = Mathf.Lerp(min,max,0.5f);
            length = 1;  
            for (int i = 0; i < n; i++)
            {
                length -= t*length;
            } 
            length = 1-length;
            if(length<side)
            {
                return t;
            }
            // if()
            // {

            // }
            // else
            // {

            // }
            count++;
        } 
        while (count>100000000);
        return 0;
    }


}

/*
0.1
0.09
0.081
0.0729
*/
