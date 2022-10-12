using FGUFW;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class Cmd2Comp:ICmd2Comp
    {
        public Vector2 Velocity = new Vector2(40f,36f);
        public int[] PlayerEUIds;

        public void Convert(World world,UInts8 cmds)
        {
            int length = PlayerEUIds.Length;

            for (int j = 0; j < length; j++)
            {
                int playerEUId = PlayerEUIds[j];
                uint cmd = cmds[j];

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
                            if(!world.GetComponent(playerEUId,out playerMoveMsgComp))
                            {
                                playerMoveMsgComp = new PlayerMoveMsgComp(playerEUId);
                            }
                            playerMoveMsgComp.Velocity.y = -Velocity.y;
                            world.AddOrSetComponent(playerEUId,playerMoveMsgComp);
                        }
                        break;
                        case OperateId.PlayerMoveF:
                        {
                            PlayerMoveMsgComp playerMoveMsgComp;
                            if(!world.GetComponent(playerEUId,out playerMoveMsgComp))
                            {
                                playerMoveMsgComp = new PlayerMoveMsgComp(playerEUId);
                            }
                            playerMoveMsgComp.Velocity.y = Velocity.y;
                            world.AddOrSetComponent(playerEUId,playerMoveMsgComp);
                        }
                        break;
                        case OperateId.PlayerMoveL:
                        {
                            PlayerMoveMsgComp playerMoveMsgComp;
                            if(!world.GetComponent(playerEUId,out playerMoveMsgComp))
                            {
                                playerMoveMsgComp = new PlayerMoveMsgComp(playerEUId);
                            }
                            playerMoveMsgComp.Velocity.x = -Velocity.x;
                            world.AddOrSetComponent(playerEUId,playerMoveMsgComp);
                        }
                        break;
                        case OperateId.PlayerMoveR:
                        {
                            PlayerMoveMsgComp playerMoveMsgComp;
                            if(!world.GetComponent(playerEUId,out playerMoveMsgComp))
                            {
                                playerMoveMsgComp = new PlayerMoveMsgComp(playerEUId);
                            }
                            playerMoveMsgComp.Velocity.x = Velocity.x;
                            world.AddOrSetComponent(playerEUId,playerMoveMsgComp);
                        }
                        break;
                        case OperateId.PlayerShoot1:
                        {
                            PlayerShootMsgComp playerShootMsgComp;
                            if(!world.GetComponent(playerEUId,out playerShootMsgComp))
                            {
                                playerShootMsgComp = new PlayerShootMsgComp(playerEUId);
                            }
                            playerShootMsgComp.BulletType = 1;
                            world.AddOrSetComponent(playerEUId,playerShootMsgComp);
                        }
                        break;
                        case OperateId.PlayerShoot2:
                        {
                            PlayerShootMsgComp playerShootMsgComp;
                            if(!world.GetComponent(playerEUId,out playerShootMsgComp))
                            {
                                playerShootMsgComp = new PlayerShootMsgComp(playerEUId);
                            }
                            playerShootMsgComp.BulletType = 2;
                            world.AddOrSetComponent(playerEUId,playerShootMsgComp);
                        }
                        break;
                        case OperateId.PlayerShoot3:
                        {
                            PlayerShootMsgComp playerShootMsgComp;
                            if(!world.GetComponent(playerEUId,out playerShootMsgComp))
                            {
                                playerShootMsgComp = new PlayerShootMsgComp(playerEUId);
                            }
                            playerShootMsgComp.BulletType = 3;
                            world.AddOrSetComponent(playerEUId,playerShootMsgComp);
                        }
                        break;
                    }
                }
            }
        }


    }
}