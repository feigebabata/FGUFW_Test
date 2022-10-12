using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerMoveInputComp : MonoBehaviour
    {
        public WorldFrameSync WorldFrameSync;
        
        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            
            if(Input.GetKey(KeyCode.W))
            {
                WorldFrameSync.PutCmd((int)OperateId.PlayerMoveF);
            }
            if(Input.GetKey(KeyCode.S))
            {
                WorldFrameSync.PutCmd((int)OperateId.PlayerMoveB);
            }
            if(Input.GetKey(KeyCode.A))
            {
                WorldFrameSync.PutCmd((int)OperateId.PlayerMoveL);
            }
            if(Input.GetKey(KeyCode.D))
            {
                WorldFrameSync.PutCmd((int)OperateId.PlayerMoveR);
            }

        }
    }
}
