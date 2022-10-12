using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerShootInputComp : MonoBehaviour
    {
        public int PlayerEUId;
        public int Delay=16;
        public WorldFrameSync WorldFrameSync;
        private int shootDowncount;

        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            shootDowncount--;
            if(shootDowncount>0)return;

            var world = World.Current;
            if(Input.GetKey(KeyCode.Return))
            {
                WorldFrameSync.PutCmd((uint)OperateId.PlayerShoot1);
                shootDowncount = Delay;
            }
            else if(Input.GetKey(KeyCode.Space))
            {
                WorldFrameSync.PutCmd((uint)OperateId.PlayerShoot2);
                shootDowncount = Delay;
            }
            else if( Input.GetKey(KeyCode.KeypadEnter))
            {
                WorldFrameSync.PutCmd((uint)OperateId.PlayerShoot3);
                shootDowncount = Delay;
            }

        }
    }
}
