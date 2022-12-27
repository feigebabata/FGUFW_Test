using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NameSpace
{
    /// <summary>
    /// 类型说明
    /// </summary>
    [Serializable]
    public class ClassTest
    {

        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id;


        /// <summary>
        /// 名称集
        /// </summary>
        public string[] Names;


        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Point;



        public ClassTest(string[,] table,int lineIndex)
        {
            Id=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Names=ScriptTextHelper.Parse_strings(table[lineIndex,1]);
            Point=ScriptTextHelper.Parse_Vector3(table[lineIndex,2]);

        }

        public static ClassTest[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            ClassTest[] list = new ClassTest[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new ClassTest(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,ClassTest> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,ClassTest> dict = new Dictionary<int,ClassTest>();
            for (int i = 4; i < length; i++)
            {
                var data = new ClassTest(table,i);
                dict.Add(data.Id,data);
            }
            return dict;
        }

    }
}
