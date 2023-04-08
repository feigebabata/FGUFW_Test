namespace RogueGamePlay
{
    /// <summary>
    /// 技能事件
    /// </summary>
    public enum SkillEvent:uint
    {
        Nothing = uint.MinValue,

        Everything = uint.MaxValue,

        /// <summary>
        /// 普攻开始
        /// </summary>
        SkillBegin = 1<<0,

        /// <summary>
        /// 普攻结束
        /// </summary>
        SkillEnd = 1<<1,

        /// <summary>
        /// 普攻命中
        /// </summary>
        SkillHit = 1<<2,

        /// <summary>
        /// 普攻击杀
        /// </summary>
        SkillKill = 1<<3,

        /// <summary>
        /// 最后一发子弹
        /// </summary>
        LastAttack = 1<<4,

        /// <summary>
        /// 第一发子弹
        /// </summary>
        FristAttack = 1<<5,

        /// <summary>
        /// 经验拾取
        /// </summary>
        ExpReceive = 1<<6,

        /// <summary>
        /// 死亡
        /// </summary>
        SelfDeath = 1<<7,


    }
}
