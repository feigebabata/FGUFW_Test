
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
                BattleConfigComp battleConfigComp;
                _world.GetComponent(World.ENTITY_SINGLE,out battleConfigComp);

                int index = battleDataComp.BattleItemIndex;
                if(index==-1)index=0;
                float velocity = battleConfigComp.BattleItemScorllVelocity[index];
                var pos = positionComp.Pos;
                pos.z -= velocity*_world.DeltaTime;
                positionComp.Pos = pos;
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
