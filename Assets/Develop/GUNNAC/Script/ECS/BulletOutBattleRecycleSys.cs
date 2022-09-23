
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
    public class BulletOutBattleRecycleSys : ISystem
    {
        public int Order => -10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            //IComponent.Dirty > 0 才会修改源数据
            _world.Filter((ref BulletComp bulletComp,ref PositionComp positionComp)=>
            {
                BattleViewRectComp battleViewRectComp;
                _world.GetComponent(World.ENTITY_SINGLE,out battleViewRectComp);
                
                bool outBattle = false;
                if(positionComp.Pos.x<battleViewRectComp.Rect.z-10)outBattle=true;
                if(positionComp.Pos.x>battleViewRectComp.Rect.x+10)outBattle=true;
                if(positionComp.Pos.z<battleViewRectComp.Rect.w-10)outBattle=true;
                if(positionComp.Pos.z>battleViewRectComp.Rect.y+10)outBattle=true;

                if(outBattle)
                {
                    int entityUId = bulletComp.EntityUId;

                    RenderComp renderComp;
                    _world.GetComponent(entityUId,out renderComp);
                    GameObjectPool.ReCycle((int)renderComp.GObjType,renderComp.GObj);
                    renderComp.GObj = null;

                    ColliderComp colliderComp;
                    _world.GetComponent(entityUId,out colliderComp);
                    GameObjectPool.ReCycle((int)colliderComp.GObjType,colliderComp.GObj);
                    colliderComp.GObj = null;

                    _world.DestroyEntity(entityUId);
                }
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}