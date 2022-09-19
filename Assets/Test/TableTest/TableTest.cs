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

public static class TableTest
{

    [MenuItem("Test/TableTest")]
    static void tableTest()
    {
        var path = "E:/Downloads/WEB-001-677-番外20话.txt";
        var lines = File.ReadAllLines(path);

        var ls = new List<string>(lines.Length);
        int index = 1;
        string key = $"（{index:D3}）";
        
        foreach (var line in lines)
        {
            if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))continue;

            var newLine = line.Trim();
            int keyIndex = newLine.IndexOf(key);
            if(keyIndex!=-1)
            {
                string name = null;
                try
                {
                    name = newLine.Substring(keyIndex+key.Length);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"{key} {index} {key.Length}");
                    return;
                }
                newLine = $"第{index:D3}章 {name}";
                
                index++;
                key = $"（{index:D3}）";

                ls.Add($"\n{newLine}");
            }
            else
            {
                ls.Add($"\n    {newLine}");
            }
        }
        File.WriteAllLines(path,ls);
    }




}

//180:(右上 上左 右下 上左)*5 
//顺90:(右上 上左 右下 后右)*7
//逆90:(左上 上右 左下 后左)*7
