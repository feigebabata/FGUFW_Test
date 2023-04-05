using FGUFW;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RogueGamePlay
{
    /// <summary>
    /// 子弹配置
    /// </summary>
    [Serializable]
    public class BulletConfig
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
        /// 速度
        /// </summary>
        public float Velocity;


        /// <summary>
        /// 穿透
        /// </summary>
        public int Pierce;


        /// <summary>
        /// 预制
        /// </summary>
        public string Prefab;


        /// <summary>
        /// 时间
        /// </summary>
        public float Time;



        public BulletConfig(string[,] table,int lineIndex)
        {
            ID=ScriptTextHelper.Parse_int(table[lineIndex,0]);
            Power=ScriptTextHelper.Parse_float(table[lineIndex,1]);
            Velocity=ScriptTextHelper.Parse_float(table[lineIndex,2]);
            Pierce=ScriptTextHelper.Parse_int(table[lineIndex,3]);
            Prefab=ScriptTextHelper.Parse_string(table[lineIndex,4]);
            Time=ScriptTextHelper.Parse_float(table[lineIndex,5]);

        }

        public static BulletConfig[] ToArray(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0)-4;
            BulletConfig[] list = new BulletConfig[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = new BulletConfig(table,i+4);
            }
            return list;
        }

        public static Dictionary<int,BulletConfig> ToDict(string csvText)
        {
            var table = CsvHelper.Parse2(csvText);
            int length = table.GetLength(0);
            Dictionary<int,BulletConfig> dict = new Dictionary<int,BulletConfig>();
            for (int i = 4; i < length; i++)
            {
                var data = new BulletConfig(table,i);
                dict.Add(data.ID,data);
            }
            return dict;
        }

    }
}
