
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
    public class BulletOutDestroySys : ISystem
    {
        public int Order => 10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref BattleOutDestroyComp battleOutRecycleComp,ref PositionComp positionComp)=>
            {
                BattleViewRectComp battleViewRectComp;
                _world.GetComponent(World.ENTITY_SINGLE,out battleViewRectComp);
                
                bool outBattle = false;
                if(positionComp.Pos.x<battleViewRectComp.Rect.z*1.5f)outBattle=true;
                if(positionComp.Pos.x>battleViewRectComp.Rect.x*1.5f)outBattle=true;
                if(positionComp.Pos.z<battleViewRectComp.Rect.w*1.5f)outBattle=true;
                if(positionComp.Pos.z>battleViewRectComp.Rect.y*1.5f)outBattle=true;

                if(outBattle)
                {
                    int entityUId = battleOutRecycleComp.EntityUId;

                    // RenderComp renderComp;
                    // _world.GetComponent(entityUId,out renderComp);
                    // renderComp.GObject.SetActive(false);
                    // GameObjectPool.ReCycle((int)renderComp.GObjType,renderComp.GObject);
                    // renderComp.GObject = null;

                    // ColliderComp colliderComp;
                    // _world.GetComponent(entityUId,out colliderComp);
                    // colliderComp.GObj.SetActive(false);
                    // GameObjectPool.ReCycle((int)colliderComp.GObjType,colliderComp.GObj);
                    // colliderComp.GObj = null;

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
