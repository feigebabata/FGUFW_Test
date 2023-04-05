using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 添加buff配置
    /// </summary>
    [Serializable]
    public class AddBuffConfig
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;


        /// <summary>
        /// Buff类型
        /// </summary>
        public BuffType Type;


        /// <summary>
        /// 预制
        /// </summary>
        public string Prefab;


        /// <summary>
        /// 时间
        /// </summary>
        public float Time;


        /// <summary>
        /// 值
        /// </summary>
        public float Value;



        public AddBuffConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Type=ScriptTextHelper.Parse_enum<BuffType>(table[lineIndex,1]);
            Prefab=ScriptTextHelper.Parse_string(table[lineIndex,2]);
            Time=ScriptTextHelper.Parse_float(table[lineIndex,3]);
            Value=ScriptTextHelper.Parse_float(table[lineIndex,4]);

        }

        public static AddBuffConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            AddBuffConfig[] list = new AddBuffConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new AddBuffConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,AddBuffConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,AddBuffConfig> dict = new Dictionary<int,AddBuffConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new AddBuffConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
