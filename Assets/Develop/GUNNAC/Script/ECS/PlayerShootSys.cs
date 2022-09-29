
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
    public class PlayerShootSys : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref PositionComp positionComp,ref PlayerShootMsgComp playerShootMsgComp,ref PlayerRenderComp playerRenderComp,ref RenderComp renderComp)=>
            {
                var pos = positionComp.Pos+ playerRenderComp.ShootPoint;
                float3 renderPos = renderComp.GObject.transform.position;
                renderPos += playerRenderComp.ShootPoint.xyz;
                switch (playerShootMsgComp.BulletType)
                {
                    case 1:
                        shoot1(renderPos,pos);
                    break;
                    case 2:
                        shoot2(renderPos,pos);
                    break;
                }


                _world.RemoveComponent<PlayerShootMsgComp>(playerShootMsgComp.EntityUId);
            });


        }

        private void shoot2(float3 renderPos, float4 pos)
        {
            for (int i = -1; i < 2; i++)
            {
                int entityUId = _world.CreateEntity(
                (
                    ref PositionComp positionComp,
                    ref LineMoveComp lineMoveComp,
                    ref BattleOutDestroyComp battleOutDestroyComp
                )=>
                {
                    positionComp.Pos = pos;
                    positionComp.PrevPos = pos;

                    lineMoveComp.DirAndVelocity.xyz = new Vector3(i*0.5f,0,1).normalized;
                    lineMoveComp.DirAndVelocity.w = 100;
                });
                PoolRenderComp poolRenderComp = new PoolRenderComp(entityUId,(int)GameObjectType.PlayerBullet_1);
                poolRenderComp.GObject.transform.position = renderPos;

                PoolColliderComp poolColliderComp = new PoolColliderComp(entityUId,(int)GameObjectType.PlayerBulletCollider_1);
                poolColliderComp.GObject.transform.position = pos.xyz;
                poolColliderComp.GObject.GetComponent<IBulletAttacker>().EntityUId = entityUId;
                CapsuleCollider capsuleCollider = poolColliderComp.Collider as CapsuleCollider;
                capsuleCollider.height = capsuleCollider.radius*2;

                _world.AddOrSetComponent(entityUId,poolRenderComp);
                _world.AddOrSetComponent(entityUId,poolColliderComp);
            }
        }

        private void shoot1(float3 renderPos, float4 pos)
        {
            
            int entityUId = _world.CreateEntity(
            (
                ref PositionComp positionComp,
                ref LineMoveComp lineMoveComp,
                ref BattleOutDestroyComp battleOutDestroyComp
            )=>
            {
                positionComp.Pos = pos;
                positionComp.PrevPos = pos;

                lineMoveComp.DirAndVelocity.xyz = Vector3.forward;
                lineMoveComp.DirAndVelocity.w = 100;
            });
            PoolRenderComp poolRenderComp = new PoolRenderComp(entityUId,(int)GameObjectType.PlayerBullet_1);
            poolRenderComp.GObject.transform.position = renderPos;

            PoolColliderComp poolColliderComp = new PoolColliderComp(entityUId,(int)GameObjectType.PlayerBulletCollider_1);
            poolColliderComp.GObject.transform.position = pos.xyz;
            poolColliderComp.GObject.GetComponent<IBulletAttacker>().EntityUId = entityUId;
            CapsuleCollider capsuleCollider = poolColliderComp.Collider as CapsuleCollider;
            capsuleCollider.height = capsuleCollider.radius*2;

            _world.AddOrSetComponent(entityUId,poolRenderComp);
            _world.AddOrSetComponent(entityUId,poolColliderComp);
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
