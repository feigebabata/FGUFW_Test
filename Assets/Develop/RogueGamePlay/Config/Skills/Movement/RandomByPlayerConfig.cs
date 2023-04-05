using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 在玩家周围移动配置
    /// </summary>
    [Serializable]
    public class RandomByPlayerConfig
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;


        /// <summary>
        /// 速度
        /// </summary>
        public float Velocity;


        /// <summary>
        /// 概要
        /// </summary>
        public string Summary;



        public RandomByPlayerConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Velocity=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Summary=ScriptTextHelper.Parse_string(table[lineIndex,2]);

        }

        public static RandomByPlayerConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            RandomByPlayerConfig[] list = new RandomByPlayerConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new RandomByPlayerConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,RandomByPlayerConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,RandomByPlayerConfig> dict = new Dictionary<int,RandomByPlayerConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new RandomByPlayerConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
