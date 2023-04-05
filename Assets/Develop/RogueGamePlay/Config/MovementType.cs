namespace RogueGamePlay
{
    /// <summary>
    /// 移动类型
    /// </summary>
    public enum MovementType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 向玩家移动
        /// </summary>
        MoveToPlayer = 1,

        /// <summary>
        /// 环绕玩家
        /// </summary>
        RingPlayer = 2,

        /// <summary>
        /// 玩家周围随机
        /// </summary>
        RandomByPlayer = 3,


    }
}
