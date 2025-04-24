using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FGUFW.ExcelUtils
{
    public static class ExcelCsharpToJson
    {
        public static void ToJson(Excel excel,string path)
        {
            var jsonBuilder = new StringBuilder();

            jsonBuilder.Append('{'); 
            for (int i = 0; i < excel.SheetCount; i++)
            {
                var sheet = excel[i];
                sheetToJson(sheet,jsonBuilder);
                
                if(i<excel.SheetCount-1)
                {
                    jsonBuilder.Append(',');
                }
            }
            jsonBuilder.Append('}');

            var directory = Path.Combine(Application.dataPath,"ECJsonData");

            var name = Path.GetFileNameWithoutExtension(path);

            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(Path.Combine(directory,$"{name}.json"),jsonBuilder.ToString());
        }

        private static void sheetToJson(ISheet sheet, StringBuilder jsonBuilder)
        {
            var firstRow = sheet.GetRow(0);
            if(firstRow==default)return;

            //第一行第一列 校验标识
            if(firstRow.GetCell(0)?.ToString() != "GenerateExcelCsharpCode")return;

            var collection = firstRow.GetCell(1)?.ToString();
            if(collection.IsNull())return;

            jsonBuilder.Append($"\"{sheet.SheetName}\":");

            //第三行字段类型
            var types = sheet.GetRow(2);
            //第四行字段名
            var names = sheet.GetRow(3);

            int maxRowIdx = sheet.LastRowNum;
            int maxCellIdx = types.LastCellNum;

            if(collection == "List")
            {
                jsonBuilder.Append('[');

                for (int ri = 4; ri < maxRowIdx; ri++)
                {
                    var row = sheet.GetRow(ri);
                    
                    jsonBuilder.Append('{');
                    for (int ci = 0; ci < maxCellIdx; ci++)
                    {
                        jsonBuilder.Append($"\"{names.GetCell(ci)}\":");
                        jsonBuilder.Append(getValueByType(types.GetCell(ci).ToString(),row.GetCell(ci).ToString()));
                        if(ci<maxCellIdx-1)
                        {
                            jsonBuilder.Append(',');
                        }
                    }
                    jsonBuilder.Append('}');

                    if(ri<maxRowIdx-1)
                    {
                        jsonBuilder.Append(',');
                    }
                }

                jsonBuilder.Append(']');
            }
            else if(collection == "Dictionary")
            {
                jsonBuilder.Append('{');

                for (int ri = 4; ri < maxRowIdx; ri++)
                {
                    var row = sheet.GetRow(ri);

                    jsonBuilder.Append($"\"{row.GetCell(0)}\":");
                    
                    jsonBuilder.Append('{');
                    for (int ci = 0; ci < maxCellIdx; ci++)
                    {
                        Debug.Log($"{sheet.SheetName} {ri} {ci}");
                        jsonBuilder.Append($"\"{names.GetCell(ci)}\":");
                        jsonBuilder.Append(getValueByType(types.GetCell(ci).ToString(),row.GetCell(ci)?.ToString()));
                        if(ci<maxCellIdx-1)
                        {
                            jsonBuilder.Append(',');
                        }
                    }
                    jsonBuilder.Append('}');

                    if(ri<maxRowIdx-1)
                    {
                        jsonBuilder.Append(',');
                    }
                }

                jsonBuilder.Append('}');
            }      

        }

        static string getValueByType(string type,string value)
        {
            switch (type)
            {
                case "int":
                {
                    if(value.IsNull())
                    {
                        return "0";
                    }
                    else
                    {
                        return value;
                    }
                }
                case "float":
                {
                    if(value.IsNull())
                    {
                        return "0.0";
                    }
                    else
                    {
                        return value;
                    }
                }
                case "bool":
                {
                    if(value.IsNull())
                    {
                        return "false";
                    }
                    else
                    {
                        return value;
                    }
                }
                case "string":
                {
                    return $"\"{value}\"";
                }
                
                default:
                    Debug.LogError($"未知类型:[{type}]");
                break;
            }

            return default;
        }
    }
}