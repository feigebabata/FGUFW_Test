
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
    public class DelayDestroySys : ISystem
    {
        public int Order => 10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            
            _world.Filter((ref DelayDestroyComp delayDestroyComp)=>
            {
                delayDestroyComp.Delay--;
                if(delayDestroyComp.Delay>0)return;
                int entityUId = delayDestroyComp.EntityUId;

                // RenderComp renderComp;
                // _world.GetComponent(entityUId,out renderComp);
                // renderComp.GObject.SetActive(false);
                // GameObjectPool.ReCycle((int)renderComp.GObjType,renderComp.GObject);
                // renderComp.GObject = null;

                _world.DestroyEntity(entityUId);
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
