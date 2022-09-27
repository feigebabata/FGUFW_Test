
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
    public class BattleItemSetSys : ISystem
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
                const float size = 90;
                float offset = -positionComp.Pos.z;
                int currentIndex = (int)((offset-size/2)/size);
                // Debug.Log(currentIndex);
                
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
