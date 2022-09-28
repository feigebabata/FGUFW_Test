
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
                float3 renderPos = renderComp.GObj.transform.position;
                renderPos += playerRenderComp.ShootPoint.xyz;
                _world.CreateEntity(
                (
                    ref BulletComp bulletComp,
                    ref PositionComp positionComp1,
                    ref RenderComp renderComp,
                    ref LineMoveComp lineMoveComp,
                    ref ColliderComp colliderComp
                )=>
                {
                    positionComp1.Pos = pos;
                    positionComp1.PrevPos = pos;

                    renderComp.GObjType = GameObjectType.PlayerBullet_1;
                    renderComp.GObj = GameObjectPool.Get((int)renderComp.GObjType);
                    renderComp.GObj.name = $"renderComp{bulletComp.EntityUId}";
                    renderComp.GObj.transform.parent = null;
                    renderComp.GObj.transform.position = renderPos;
                    renderComp.GObj.SetActive(true);

                    colliderComp.GObjType = GameObjectType.PlayerBulletCollider_1;
                    colliderComp.GObj = GameObjectPool.Get((int)colliderComp.GObjType);
                    colliderComp.GObj.name = $"colliderComp{bulletComp.EntityUId}";
                    colliderComp.Collider = colliderComp.GObj.GetComponent<CapsuleCollider>();
                    colliderComp.Collider.height = colliderComp.Collider.radius*2;
                    colliderComp.GObj.GetComponent<IBulletAttacker>().EntityUId = bulletComp.EntityUId;
                    colliderComp.GObj.transform.parent = null;
                    colliderComp.GObj.transform.position = pos.xyz;
                    colliderComp.GObj.SetActive(true);

                    lineMoveComp.DirAndVelocity.xyz = Vector3.forward;
                    lineMoveComp.DirAndVelocity.w = 100;
                });
                _world.RemoveComponent<PlayerShootMsgComp>(playerShootMsgComp.EntityUId);
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
