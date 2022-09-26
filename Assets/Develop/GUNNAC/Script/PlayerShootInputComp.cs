using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerShootInputComp : MonoBehaviour
    {
        public int PlayerEUId;

        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space))
            {
                PlayerShootMsgComp playerShootMsgComp;
                if(!world.GetComponent(PlayerEUId,out playerShootMsgComp))
                {
                    playerShootMsgComp = new PlayerShootMsgComp(PlayerEUId);
                }
                playerShootMsgComp.BulletType = 1;
                world.AddOrSetComponent(PlayerEUId,playerShootMsgComp);
            }

        }
    }
}
