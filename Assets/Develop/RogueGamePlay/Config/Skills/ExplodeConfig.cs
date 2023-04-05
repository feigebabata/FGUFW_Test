using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 爆炸配置
    /// </summary>
    [Serializable]
    public class ExplodeConfig
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;


        /// <summary>
        /// 攻击力
        /// </summary>
        public float Power;


        /// <summary>
        /// 预制
        /// </summary>
        public string Prefab;


        /// <summary>
        /// 范围
        /// </summary>
        public float Range;



        public ExplodeConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Power=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Prefab=ScriptTextHelper.Parse_string(table[lineIndex,2]);
            Range=ScriptTextHelper.Parse_float(table[lineIndex,3]);

        }

        public static ExplodeConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            ExplodeConfig[] list = new ExplodeConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new ExplodeConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,ExplodeConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,ExplodeConfig> dict = new Dictionary<int,ExplodeConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new ExplodeConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
