using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 移动向玩家配置
    /// </summary>
    [Serializable]
    public class MoveToPlayerConfig
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



        public MoveToPlayerConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Velocity=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Summary=ScriptTextHelper.Parse_string(table[lineIndex,2]);

        }

        public static MoveToPlayerConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            MoveToPlayerConfig[] list = new MoveToPlayerConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new MoveToPlayerConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,MoveToPlayerConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,MoveToPlayerConfig> dict = new Dictionary<int,MoveToPlayerConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new MoveToPlayerConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
