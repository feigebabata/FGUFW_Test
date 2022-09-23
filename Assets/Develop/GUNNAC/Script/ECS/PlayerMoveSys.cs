
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
    public class PlayerMoveSys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            //IComponent.Dirty > 0 才会修改源数据
            _world.Filter((ref PlayerMoveMsgComp playerMoveMsgComp,ref PlayerSizeComp playerSizeComp,ref PositionComp positionComp)=>
            {
                BattleViewRectComp battleViewRectComp;
                _world.GetComponent(World.ENTITY_SINGLE,out battleViewRectComp);
                float2 newPos = positionComp.Pos.xz + playerMoveMsgComp.Velocity*_world.DeltaTime;
                if(newPos.x-playerSizeComp.Rect.z<battleViewRectComp.Rect.z)
                {
                    newPos.x = battleViewRectComp.Rect.z+playerSizeComp.Rect.z;
                }
                if(newPos.x+playerSizeComp.Rect.x>battleViewRectComp.Rect.x)
                {
                    newPos.x = battleViewRectComp.Rect.x-playerSizeComp.Rect.x;
                }
                if(newPos.y+playerSizeComp.Rect.y>battleViewRectComp.Rect.y)
                {
                    newPos.y = battleViewRectComp.Rect.y-playerSizeComp.Rect.y;
                }
                if(newPos.y-playerSizeComp.Rect.w<battleViewRectComp.Rect.w)
                {
                    newPos.y = battleViewRectComp.Rect.w+playerSizeComp.Rect.w;
                }
                positionComp.PrevPos = positionComp.Pos;
                positionComp.Pos.xz = newPos;
                positionComp.Dirty++;

                _world.RemoveComponent<PlayerMoveMsgComp>(playerMoveMsgComp.EntityUId);
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
