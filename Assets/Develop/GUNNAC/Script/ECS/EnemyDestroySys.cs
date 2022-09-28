
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using System;

namespace GUNNAC
{
    public class EnemyDestroySys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref EnemyDestroyMsgComp enemyDestroyMsgComp,ref RenderComp renderComp,ref ColliderComp colliderComp)=>
            {
                int entityUId = enemyDestroyMsgComp.EntityUId;

                createBoomEffect(renderComp.GObj.transform.position);
                
                renderComp.GObj.SetActive(false);
                GameObjectPool.ReCycle((int)renderComp.GObjType,renderComp.GObj);
                renderComp.GObj = null;

                colliderComp.GObj.SetActive(false);
                GameObjectPool.ReCycle((int)colliderComp.GObjType,colliderComp.GObj);
                colliderComp.GObj = null;


                _world.DestroyEntity(entityUId);
            });


        }

        private void createBoomEffect(Vector3 position)
        {
            
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
