using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    public class TestSystem : ISystem
    {
        public int Order => 0;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            Debug.Log($"{_world.FrameIndex}  {_world.Time}");
        }

        public void Dispose()
        {
            _world = null;
        }

    }
}
