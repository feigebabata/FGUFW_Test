using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerMoveInputComp : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            
            if(Input.GetKey(KeyCode.W))
            {
                world.PutCmd((int)OperateId.PlayerMoveF);
            }
            if(Input.GetKey(KeyCode.S))
            {
                world.PutCmd((int)OperateId.PlayerMoveB);
            }
            if(Input.GetKey(KeyCode.A))
            {
                world.PutCmd((int)OperateId.PlayerMoveL);
            }
            if(Input.GetKey(KeyCode.D))
            {
                world.PutCmd((int)OperateId.PlayerMoveR);
            }

        }
    }
}
