
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
    public class EnemyCreateSys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            
            _world.Filter((ref EnemyCreateComp enemyCreateComp)=>
            {
                if(_world.FrameIndex-enemyCreateComp.PrevCreateTime<enemyCreateComp.Delay)return;
                enemyCreateComp.PrevCreateTime = _world.FrameIndex;

                var pos = new Vector3(UnityEngine.Random.Range(-50,50),0,60);
                int entityUId = _world.CreateEntity(
                (
                    ref PositionComp positionComp,
                    ref LineMoveComp lineMoveComp,
                    ref BattleOutDestroyComp battleOutDestroyComp,
                    ref EnemyComp enemyComp,
                    ref DirectionComp directionComp
                )=>
                {

                    positionComp.Pos.xyz = pos;
                    positionComp.PrevPos.xyz = pos;

                    // renderComp.GObjType = GameObjectType.Enemy;
                    // renderComp.GObject = GameObjectPool.Get((int)renderComp.GObjType);
                    // renderComp.GObject.name = $"renderComp{entityUId}";
                    // renderComp.GObject.transform.parent = null;
                    // renderComp.GObject.transform.position = pos;
                    // renderComp.GObject.SetActive(true);

                    // colliderComp.GObjType = GameObjectType.EnemyCollider;
                    // colliderComp.GObj = GameObjectPool.Get((int)colliderComp.GObjType);
                    // colliderComp.GObj.name = $"colliderComp{entityUId}";
                    
                    // colliderComp.GObj.GetComponent<IAttacked>().EntityUId = entityUId;
                    // colliderComp.GObj.transform.parent = null;
                    // colliderComp.GObj.transform.position = pos;
                    // colliderComp.GObj.SetActive(true);

                    lineMoveComp.DirAndVelocity.xyz = Vector3.back;
                    lineMoveComp.DirAndVelocity.w = 20;
                });

                PoolRenderComp poolRenderComp = new PoolRenderComp(entityUId,(int)GameObjectType.Enemy);
                poolRenderComp.GObject.transform.position = pos;

                PoolColliderComp poolColliderComp = new PoolColliderComp(entityUId,(int)GameObjectType.EnemyCollider);
                poolColliderComp.GObject.transform.position = pos;
                poolColliderComp.GObject.GetComponent<IAttacked>().EntityUId = entityUId;

                _world.AddOrSetComponent(entityUId,poolRenderComp);
                _world.AddOrSetComponent(entityUId,poolColliderComp);

            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
