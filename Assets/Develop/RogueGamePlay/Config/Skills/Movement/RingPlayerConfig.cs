using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 环绕玩家配置
    /// </summary>
    [Serializable]
    public class RingPlayerConfig
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
        /// 半径
        /// </summary>
        public float Radius;



        public RingPlayerConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Velocity=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Radius=ScriptTextHelper.Parse_float(table[lineIndex,2]);

        }

        public static RingPlayerConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            RingPlayerConfig[] list = new RingPlayerConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new RingPlayerConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,RingPlayerConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,RingPlayerConfig> dict = new Dictionary<int,RingPlayerConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new RingPlayerConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
