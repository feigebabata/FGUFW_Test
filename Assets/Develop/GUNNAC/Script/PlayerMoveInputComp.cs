using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerMoveInputComp : MonoBehaviour
    {
        public WorldFrameSync WorldFrameSync;
        private Vector3 _oldPos;

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
            else if( Input.GetMouseButton(0))
            {
                var delta = Input.mousePosition - _oldPos;
                if(delta!=Vector3.zero)
                {
                    if(delta.x>0)
                    {
                        WorldFrameSync.PutCmd((int)OperateId.PlayerMoveR);
                    }
                    else
                    {
                        WorldFrameSync.PutCmd((int)OperateId.PlayerMoveL);
                    }
                    if(delta.y>0)
                    {
                        WorldFrameSync.PutCmd((int)OperateId.PlayerMoveF);
                    }
                    else
                    {
                        WorldFrameSync.PutCmd((int)OperateId.PlayerMoveB);
                    }
                }
                _oldPos = Input.mousePosition;
            }


        }
    }
}
