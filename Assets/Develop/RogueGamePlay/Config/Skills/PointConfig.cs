using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 单点伤害配置
    /// </summary>
    [Serializable]
    public class PointConfig
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



        public PointConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Power=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Prefab=ScriptTextHelper.Parse_string(table[lineIndex,2]);

        }

        public static PointConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            PointConfig[] list = new PointConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new PointConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,PointConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,PointConfig> dict = new Dictionary<int,PointConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new PointConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
