using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 射击配置
    /// </summary>
    [Serializable]
    public class ShootConfig
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int ID;


        /// <summary>
        /// 数量
        /// </summary>
        public int Count;


        /// <summary>
        /// 发射间隔
        /// </summary>
        public float Interval;


        /// <summary>
        /// 夹角
        /// </summary>
        public float Angle;


        /// <summary>
        /// 子弹ID
        /// </summary>
        public int BulletID;


        /// <summary>
        /// 换弹时间
        /// </summary>
        public float Reload;



        public ShootConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Count=ScriptTextHelper.Parse_int(table[lineIndex,1]);
            Interval=ScriptTextHelper.Parse_float(table[lineIndex,2]);
            Angle=ScriptTextHelper.Parse_float(table[lineIndex,3]);
            BulletID=ScriptTextHelper.Parse_int(table[lineIndex,4]);
            Reload=ScriptTextHelper.Parse_float(table[lineIndex,5]);

        }

        public static ShootConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            ShootConfig[] list = new ShootConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new ShootConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,ShootConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,ShootConfig> dict = new Dictionary<int,ShootConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new ShootConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
