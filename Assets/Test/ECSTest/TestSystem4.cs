
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
    public class TestSystem4 : ISystem
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
            _world.FilterJob((ref NativeArray<Test3> test3s,ref NativeArray<Test1> test1s,ref NativeArray<Test4> test4s)=>
            {
                int length = test3s.Length;
                var job = new Job
                {
                    Test3s = test3s,
                    Test1s = test1s,
                    Test4s = test4s
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
            public NativeArray<Test3> Test3s;
            public NativeArray<Test1> Test1s;
            public NativeArray<Test4> Test4s;

            public void Execute(int index)
            {
                var test3 = Test3s[index];
                var test1 = Test1s[index];
                var test4 = Test4s[index];
                //code
                
            }

        }


    }
}
