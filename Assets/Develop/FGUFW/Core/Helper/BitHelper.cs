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
        public static bool Contains(UInt32 source,UInt32 target)
        {
            return (source&target) == target;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public static UInt32 Add(UInt32 source,UInt32 target)
        {
            return source|target;
        }

        /// <summary>
        /// 减去
        /// </summary>
        public static UInt32 Sub(UInt32 source,UInt32 target)
        {
            target = ~target;
            return source|target;
        }

    }
}