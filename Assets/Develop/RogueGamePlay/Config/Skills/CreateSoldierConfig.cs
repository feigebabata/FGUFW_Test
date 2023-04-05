using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 创建士兵配置
    /// </summary>
    [Serializable]
    public class CreateSoldierConfig
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;


        /// <summary>
        /// 士兵ID
        /// </summary>
        public int SoldierID;


        /// <summary>
        /// 数量
        /// </summary>
        public int Count;


        /// <summary>
        /// 间隔时间
        /// </summary>
        public float Interval;



        public CreateSoldierConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            SoldierID=ScriptTextHelper.Parse_int(table[lineIndex,1]);
            Count=ScriptTextHelper.Parse_int(table[lineIndex,2]);
            Interval=ScriptTextHelper.Parse_float(table[lineIndex,3]);

        }

        public static CreateSoldierConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            CreateSoldierConfig[] list = new CreateSoldierConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new CreateSoldierConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,CreateSoldierConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,CreateSoldierConfig> dict = new Dictionary<int,CreateSoldierConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new CreateSoldierConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
