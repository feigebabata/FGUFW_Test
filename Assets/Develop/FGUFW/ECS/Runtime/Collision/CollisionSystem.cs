
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using FGUFW;

namespace GUNNAC
{
    public class CollisionSystem : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            CollisionSystemComp collisionSystemComp;
            if(!_world.GetComponent(World.ENTITY_SINGLE,out collisionSystemComp))
            {
                collisionSystemComp = new CollisionSystemComp(World.ENTITY_SINGLE);
            }
            detectAABB(collisionSystemComp);
            
        }


        public void Dispose()
        {
            _world = null;
        }

        private void detectAABB(CollisionSystemComp collisionSystemComp)
        {

        }

    }
}
