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
        /// 攻击体开始
        /// </summary>
        AttackerBegin = 1<<0,

        /// <summary>
        /// 攻击体结束
        /// </summary>
        AttackerEnd = 1<<1,

        /// <summary>
        /// 攻击命中
        /// </summary>
        AttackHit = 1<<2,

        /// <summary>
        /// 攻击击杀
        /// </summary>
        AttackKill = 1<<3,

        /// <summary>
        /// 最后一发子弹
        /// </summary>
        LastShoot = 1<<4,

        /// <summary>
        /// 第一发子弹
        /// </summary>
        FristShoot = 1<<5,

        /// <summary>
        /// 经验拾取
        /// </summary>
        ExpReceive = 1<<6,

        /// <summary>
        /// 死亡
        /// </summary>
        SelfDeath = 1<<7,

        /// <summary>
        /// 射击
        /// </summary>
        Shoot = 1<<8,


    }
}
