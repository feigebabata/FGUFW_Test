
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
    public class LineMoveSys : ISystem
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
            _world.FilterJob((ref NativeArray<PositionComp> positioncomps,ref NativeArray<LineMoveComp> linemovecomps)=>
            {
                int length = positioncomps.Length;
                var job = new Job
                {
                    PositionComps = positioncomps,
                    LineMoveComps = linemovecomps
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
            public NativeArray<PositionComp> PositionComps;
            public NativeArray<LineMoveComp> LineMoveComps;

            public void Execute(int index)
            {
                var positioncomp = PositionComps[index];
                var linemovecomp = LineMoveComps[index];
                //code
                float3 dir = linemovecomp.DirAndVelocity.xyz;
                float v = linemovecomp.DirAndVelocity.w;
                float3 pos = positioncomp.Pos.xyz + (dir*v);
                float3 prevPos = positioncomp.Pos.xyz;
                if(!math.all(positioncomp.Pos.xyz==pos) || !math.all(positioncomp.PrevPos.xyz==pos))
                {
                    positioncomp.Pos.xyz = pos;
                    positioncomp.PrevPos.xyz = prevPos;
                    positioncomp.Dirty++;
                }
            }

        }


    }
}
