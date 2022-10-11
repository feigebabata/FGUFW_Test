namespace GUNNAC
{
    public enum OperateId
    {
        //0123 4567 89ab cdef
        None=0x00000000,
        Nothing=0x00000001,
        PlayerMoveL=0x00000002,
        PlayerMoveR=0x00000004,
        PlayerMoveF=0x00000008,
        PlayerMoveB=0x00000010,
        PlayerShoot1=0x00000020,
        PlayerShoot2=0x00000040,
        PlayerShoot3=0x00000080,

        MaxValue=0x00000081,
    
    }
}