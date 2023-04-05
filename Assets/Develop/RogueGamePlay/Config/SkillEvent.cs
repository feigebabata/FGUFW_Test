namespace RogueGamePlay
{
    /// <summary>
    /// 技能事件
    /// </summary>
    public enum SkillEvent
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 普攻开始
        /// </summary>
        SkillBegin = 1,

        /// <summary>
        /// 普攻结束
        /// </summary>
        SkillEnd = 2,

        /// <summary>
        /// 普攻命中
        /// </summary>
        SkillHit = 3,

        /// <summary>
        /// 普攻击杀
        /// </summary>
        SkillKill = 4,

        /// <summary>
        /// 最后一发子弹
        /// </summary>
        LastAttack = 5,

        /// <summary>
        /// 第一发子弹
        /// </summary>
        FristAttack = 6,

        /// <summary>
        /// 经验拾取
        /// </summary>
        ExpReceive = 7,

        /// <summary>
        /// 死亡
        /// </summary>
        SelfDeath = 8,


    }
}
