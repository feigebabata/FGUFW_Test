
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

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
            _world.FilterJob((ref NativeArray<Test1> test1s,ref NativeArray<Test2> test2s)=>
            {
                int length = test1s.Length;
                var job = new Job
                {
                    Test1s = test1s,
                    Test2s = test2s
                };
                job.Run(length);
                //code

            });

        }

        public void Dispose()
        {
            _world = null;
        }

        public struct Job : IJobParallelFor
        {
            public NativeArray<Test1> Test1s;
            public NativeArray<Test2> Test2s;

            public void Execute(int index)
            {
                var test1 = Test1s[index];
                var test2 = Test2s[index];
                //code
                
            }

        }

    }
}
