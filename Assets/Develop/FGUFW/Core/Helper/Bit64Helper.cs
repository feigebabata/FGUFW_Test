using System;
using System.Collections.Generic;
using System.Text;

namespace FGUFW
{
    public static class Bit64Helper
    {
        /// <summary>
        /// 包含
        /// </summary>
        public static bool Contains(Int64 source,Int64 target)
        {
            return (source&target) == target;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static Int64 Add(Int64 source,Int64 target)
        {
            return source|target;
        }

        /// <summary>
        /// 减去
        /// </summary>
        public static Int64 Sub(Int64 source,Int64 target)
        {
            target = ~target;
            return source|target;
        }

    }
}