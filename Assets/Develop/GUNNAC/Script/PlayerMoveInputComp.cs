using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerMoveInputComp : MonoBehaviour
    {
        public Vector2 Velocity = new Vector2(20f,18f);
        public int PlayerEUId;

        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            
            if(Input.GetKey(KeyCode.W))
            {
                PlayerMoveMsgComp playerMoveMsgComp;
                if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                {
                    playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                }
                playerMoveMsgComp.Velocity.y = Velocity.y;
                world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
            }
            if(Input.GetKey(KeyCode.S))
            {
                PlayerMoveMsgComp playerMoveMsgComp;
                if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                {
                    playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                }
                playerMoveMsgComp.Velocity.y = -Velocity.y;
                world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
            }
            if(Input.GetKey(KeyCode.A))
            {
                PlayerMoveMsgComp playerMoveMsgComp;
                if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                {
                    playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                }
                playerMoveMsgComp.Velocity.x = -Velocity.x;
                world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);

            }
            if(Input.GetKey(KeyCode.D))
            {
                PlayerMoveMsgComp playerMoveMsgComp;
                if(!world.GetComponent(PlayerEUId,out playerMoveMsgComp))
                {
                    playerMoveMsgComp = new PlayerMoveMsgComp(PlayerEUId);
                }
                playerMoveMsgComp.Velocity.x = Velocity.x;
                world.AddOrSetComponent(PlayerEUId,playerMoveMsgComp);
            }

        }
    }
}
