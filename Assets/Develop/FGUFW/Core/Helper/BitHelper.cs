using System;
using System.Collections.Generic;
using System.Text;

namespace FGUFW
{
    public static class BitHelper
    {
        /// <summary>
        /// 包含
        /// </summary>
        public static bool Contains(Int32 source,Int32 target)
        {
            return (source&target) == target;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static Int32 Add(Int32 source,Int32 target)
        {
            return source|target;
        }

        /// <summary>
        /// 减去
        /// </summary>
        public static Int32 Sub(Int32 source,Int32 target)
        {
            target = ~target;
            return source|target;
        }

    }
}