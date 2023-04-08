namespace RogueGamePlay
{
    /// <summary>
    /// 阵营
    /// </summary>
    public enum Faction:uint
    {
        Nothing = uint.MinValue,

        Everything = uint.MaxValue,

        Player = 1<<0,

        Monster = 1<<1,

    }
}
