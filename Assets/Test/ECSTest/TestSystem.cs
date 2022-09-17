
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
        public int Order => 9;

        private World _world;



        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            //IComponent.Dirty > 0 才会修改源数据
            Debug.Log("---");
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
