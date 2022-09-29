
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
            _world.Filter((ref EnemyDestroyMsgComp enemyDestroyMsgComp,ref PositionComp positionComp)=>
            {
                int entityUId = enemyDestroyMsgComp.EntityUId;

                createBoomEffect(positionComp.Pos.xyz);
                
                // renderComp.GObject.SetActive(false);
                // GameObjectPool.ReCycle((int)renderComp.GObjType,renderComp.GObject);
                // renderComp.GObject = null;

                // colliderComp.GObj.SetActive(false);
                // GameObjectPool.ReCycle((int)colliderComp.GObjType,colliderComp.GObj);
                // colliderComp.GObj = null;


                _world.DestroyEntity(entityUId);
            });


        }

        private void createBoomEffect(Vector3 position)
        {
            // _world.CreateEntity((ref RenderComp renderComp)=>
            // {
            //     int entityUId = renderComp.EntityUId;

            //     renderComp.GObjType = GameObjectType.Boom;
            //     renderComp.GObject = GameObjectPool.Get((int)renderComp.GObjType);
            //     renderComp.GObject.name = $"boom{entityUId}";
            //     renderComp.GObject.transform.parent = null;
            //     renderComp.GObject.transform.position = position;
            //     renderComp.GObject.SetActive(true);

            //     renderComp.GObject.GetComponent<IConvertToComponent>().Convert(_world,entityUId);
            // });
            int entityUId = _world.CreateEntity();
            PoolRenderComp poolRenderComp = new PoolRenderComp(entityUId,(int)GameObjectType.Boom);
            poolRenderComp.GObject.GetComponent<IConvertToComponent>().Convert(_world,entityUId);
            poolRenderComp.GObject.transform.position = position;

            _world.AddOrSetComponent(entityUId,poolRenderComp);
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
