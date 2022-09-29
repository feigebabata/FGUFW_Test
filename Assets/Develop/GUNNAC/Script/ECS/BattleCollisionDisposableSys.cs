
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
    public class BattleCollisionDisposableSys : ISystem
    {
        public int Order => -10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            
            _world.Filter((ref BattleCollisionDisposableMsgComp battleCollisionDisposableMsgComp)=>
            {
                int entityUId = battleCollisionDisposableMsgComp.EntityUId;
                int targetUId = battleCollisionDisposableMsgComp.TargetEUId;
                reCycleBullet(entityUId);
                destroyEnemy(targetUId);
            });


        }

        private void destroyEnemy(int entityUId)
        {
            EnemyDestroyMsgComp enemyDestroyMsgComp = new EnemyDestroyMsgComp();
            _world.AddOrSetComponent(entityUId,enemyDestroyMsgComp);
        }

        private void reCycleBullet(int entityUId)
        {
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

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
