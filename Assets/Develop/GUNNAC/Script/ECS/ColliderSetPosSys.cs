
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
    public class ColliderSetPosSys : ISystem
    {
        public int Order => -10;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            _world.Filter((ref ColliderComp colliderComp,ref PositionComp positionComp)=>
            {
                float offset = math.distance(positionComp.Pos,positionComp.PrevPos);
                if(offset==0)return;
                colliderComp.GObj.transform.position = positionComp.Pos.xyz;
                if(colliderComp.Collider!=null && colliderComp.Collider is CapsuleCollider)
                {
                    var collider = colliderComp.Collider;
                    collider.height = offset+collider.radius*2;
                    var center = collider.center;
                    center.z = -offset/2;
                    collider.center = center;
                }
            });

            _world.Filter((ref PoolColliderComp colliderComp,ref PositionComp positionComp,ref DirectionComp directionComp)=>
            {
                float offset = math.distance(positionComp.Pos,positionComp.PrevPos);
                if(offset==0)return;
                colliderComp.GObject.transform.position = positionComp.Pos.xyz;
                if(colliderComp.Collider!=null && colliderComp.Collider is CapsuleCollider)
                {
                    CapsuleCollider collider = colliderComp.Collider as CapsuleCollider;
                    collider.height = offset+collider.radius*2;
                    var center = collider.center;
                    center.z = -offset/2;
                    collider.center = center;
                }

                colliderComp.GObject.transform.forward = directionComp.Forward.xyz;

            });



        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
