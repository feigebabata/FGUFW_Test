
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using System;

namespace ECSTest
{
    public class TestSysten5 : ISystem
    {
        public int Order => 5;

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
                
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
