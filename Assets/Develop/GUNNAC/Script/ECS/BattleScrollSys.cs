
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
    public class BattleScrollSys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref BattleDataComp battleDataComp,ref PositionComp positionComp)=>
            {
                
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
