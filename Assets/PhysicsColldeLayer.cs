public enum PhysicsColldeLayer:uint
{
    Nothing = uint.MinValue,
    Everything = uint.MaxValue,
    Player = 1<<0,
    PlayerBullet = 1<<1,
    Monster = 1<<2,
    MonsterBullet = 1<<3,
    Exp = 1<<4,
    ExpRing = 1<<5,
    PropBox = 1<<6,
}
