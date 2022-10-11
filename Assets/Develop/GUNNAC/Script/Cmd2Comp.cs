using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class Cmd2Comp:ICmd2Comp
    {
        public Vector2 Velocity = new Vector2(40f,36f);
        public int PlayerEUId;

        public void Convert(World world,uint cmd)
        {
            // Debug.Log(System.Convert.ToString(cmd,2));
            uint max = (uint)OperateId.MaxValue;
            for (uint i = 1; i < max; i*=2)
            {
                if((i&cmd) == 0 )continue;

                OperateId operateId = (OperateId)i;
                switch (operateId)
                {
                    case OperateId.PlayerMoveB:
                    {
                        PlayerMoveMsgComp playerMoveMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                        {
                            playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                        }
                        playerMoveMsgComp.Velocity.y = -Velocity.y;
                        world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
                    }
                    break;
                    case OperateId.PlayerMoveF:
                    {
                        PlayerMoveMsgComp playerMoveMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                        {
                            playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                        }
                        playerMoveMsgComp.Velocity.y = Velocity.y;
                        world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
                    }
                    break;
                    case OperateId.PlayerMoveL:
                    {
                        PlayerMoveMsgComp playerMoveMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                        {
                            playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                        }
                        playerMoveMsgComp.Velocity.x = -Velocity.x;
                        world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
                    }
                    break;
                    case OperateId.PlayerMoveR:
                    {
                        PlayerMoveMsgComp playerMoveMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                        {
                            playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                        }
                        playerMoveMsgComp.Velocity.x = Velocity.x;
                        world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
                    }
                    break;
                    case OperateId.PlayerShoot1:
                    {
                        PlayerShootMsgComp playerShootMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerShootMsgComp))
                        {
                            playerShootMsgComp = new PlayerShootMsgComp(PlayerEUId);
                        }
                        playerShootMsgComp.BulletType = 1;
                        world.AddOrSetComponent(PlayerEUId,playerShootMsgComp);
                    }
                    break;
                    case OperateId.PlayerShoot2:
                    {
                        PlayerShootMsgComp playerShootMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerShootMsgComp))
                        {
                            playerShootMsgComp = new PlayerShootMsgComp(PlayerEUId);
                        }
                        playerShootMsgComp.BulletType = 2;
                        world.AddOrSetComponent(PlayerEUId,playerShootMsgComp);
                    }
                    break;
                    case OperateId.PlayerShoot3:
                    {
                        PlayerShootMsgComp playerShootMsgComp;
                        if(!world.GetComponent(PlayerEUId,out playerShootMsgComp))
                        {
                            playerShootMsgComp = new PlayerShootMsgComp(PlayerEUId);
                        }
                        playerShootMsgComp.BulletType = 3;
                        world.AddOrSetComponent(PlayerEUId,playerShootMsgComp);
                    }
                    break;
                }
            }
        }
    }
}