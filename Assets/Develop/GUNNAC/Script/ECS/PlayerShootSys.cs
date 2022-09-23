
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
            //IComponent.Dirty > 0 才会修改源数据
            _world.Filter((ref PositionComp positionComp,ref PlayerShootMsgComp playerShootMsgComp)=>
            {
                var pos = positionComp.Pos;
                _world.CreateEntity(
                (
                    ref BulletComp bulletComp,
                    ref PositionComp positionComp1,
                    ref RenderComp renderComp,
                    ref LineMoveComp lineMoveComp
                )=>
                {
                    renderComp.GObjType = GameObjectType.PlayerBullet_1;
                    renderComp.GObj = GameObjectPool.Get((int)renderComp.GObjType);
                    positionComp1.Pos = pos;
                    renderComp.GObj.transform.position = pos.xyz;
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
