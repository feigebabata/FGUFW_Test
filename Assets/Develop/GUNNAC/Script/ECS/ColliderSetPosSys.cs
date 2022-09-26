
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

namespace GUNNAC
{
    public class ColliderSetPosSys : ISystem
    {
        public int Order => -10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref ColliderComp colliderComp,ref PositionComp positionComp)=>
            {
                //if(positionComp.Dirty>0)
                {
                    colliderComp.GObj.transform.position = positionComp.Pos.xyz;
                }
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
