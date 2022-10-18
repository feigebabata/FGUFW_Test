namespace GUNNAC
{
    public enum OperateId
    {
        None = 0,
        Nothing = 1,
        PlayerMoveL = 1 << 1,
        PlayerMoveR = 1 << 2,
        PlayerMoveF = 1 << 3,
        PlayerMoveB = 1 << 4,
        PlayerShoot1 = 1 << 5,
        PlayerShoot2 = 1 << 6,
        PlayerShoot3 = 1 << 7,

        MaxValue = 1 << 7 + 1,

    }
}