
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace ECSTest
{
    public class TestSystem1 : ISystem
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
            _world.Filter((ref Test1 test1)=>
            {
                
                
            });

        }

        public void Dispose()
        {
            _world = null;
        }

    }
}
