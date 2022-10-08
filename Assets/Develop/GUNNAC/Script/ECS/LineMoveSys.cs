
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
            _world.FilterJob((ref NativeArray<PositionComp> positioncomps,ref NativeArray<LineMoveComp> linemovecomps,ref NativeArray<DirectionComp> directionComps)=>
            {
                int length = positioncomps.Length;
                var job = new Job
                {
                    PositionComps = positioncomps,
                    LineMoveComps = linemovecomps,
                    DirectionComps = directionComps,
                    DeltaTime = _world.DeltaTime
                };
                job.Run(length);
                //code
                var distance = math.distance(positioncomps[0].Pos,positioncomps[0].PrevPos);
                // Debug.Log($"{_world.DeltaTime} : {distance}");
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
            public NativeArray<DirectionComp> DirectionComps;
            public float DeltaTime;

            public void Execute(int index)
            {
                var positioncomp = PositionComps[index];
                var linemovecomp = LineMoveComps[index];
                var directionComp = DirectionComps[index];
                //code
                float3 dir = linemovecomp.DirAndVelocity.xyz;
                float v = linemovecomp.DirAndVelocity.w;
                float3 pos = positioncomp.Pos.xyz + dir*v*DeltaTime;
                float3 prevPos = positioncomp.Pos.xyz;
                if(!math.all(positioncomp.Pos.xyz==pos) || !math.all(positioncomp.PrevPos.xyz==pos))
                {
                    positioncomp.Pos.xyz = pos;
                    positioncomp.PrevPos.xyz = prevPos;
                }

                if(!math.all(pos==prevPos))
                {
                    directionComp.Forward = math.normalize(positioncomp.Pos-positioncomp.PrevPos);
                }

                PositionComps[index] = positioncomp;
                LineMoveComps[index] = linemovecomp;
                DirectionComps[index] = directionComp;
            }

        }


    }
}

/*
2550
2550
2550
5358-2550 = 2808-365 = 2443-850 = 1593-840 = 753
*/