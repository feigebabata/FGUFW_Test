
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    public class TestSystem2 : ISystem
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
            _world.Filter((ref Test1 test1,ref Test2 test2)=>
            {
                _world.DestroyEntity(test1.EntityUId);
                Debug.Log($"TestSystem2  {test1.EntityUId}");
            });

        }

        public void Dispose()
        {
            _world = null;
        }

    }
}
