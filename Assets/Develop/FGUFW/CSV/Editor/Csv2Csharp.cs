using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FGUFW.CSV
{
    public static class Csv2Csharp
    {
        const string Extension = ".csv";
        

        [MenuItem("Assets/Csv2Csharp")]
        private static void Build()
        {
            var selects = Selection.objects;
            if(selects==null)
            {
                return;
            }

            //筛选csv文件
            List<string> paths = new List<string>();
            foreach (var obj in selects)
            {
                string path = Application.dataPath.Replace("Assets",AssetDatabase.GetAssetPath(obj));
                if(Path.GetExtension(path)==Extension)
                {
                    paths.Add(path);
                }
            }
            if(paths.Count==0)return;
            
            foreach (var path in paths)
            {
                var table = CsvHelper.Parse2(File.ReadAllText(path));
                var savePath = path.Replace(Extension,".cs");
                var csharptype = table[0,0];
                switch (csharptype)
                {
                    case "class":
                    {
                        File.WriteAllText(savePath,ScriptTextHelper.Csv2CsharpClass(table));
                    }
                    break;
                    case "enum":
                        
                    break;
                    default:
                        Debug.LogError($"未标注类型 {csharptype}");
                    break;
                }
            }
            
            AssetDatabase.Refresh();
        }


    }
}