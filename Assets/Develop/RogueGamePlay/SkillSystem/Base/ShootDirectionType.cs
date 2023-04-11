namespace RogueGamePlay
{
    public enum ShootDirectionType:int
    {
        /// <summary>
        /// 瞄准
        /// </summary>
        Aim = 0,

        /// <summary>
        /// 逆瞄准
        /// </summary>
        AimInverse = 1,

        /// <summary>
        /// 最近的怪物
        /// </summary>
        MonsterNear = 2,

        /// <summary>
        /// 玩家
        /// </summary>
        Player = 3,

        Count = 4,
    }
}