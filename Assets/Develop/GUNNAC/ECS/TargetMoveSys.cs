
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using System;
using FGUFW;

namespace GUNNAC
{
    public class TargetMoveSys : ISystem
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
            _world.Filter((ref TargetMoveComp targetmovecomp,ref PositionComp positioncomp,ref DirectionComp directioncomp) =>
            {
                var targetEUId = targetmovecomp.TargetEUId;
                PositionComp targetPosComp;

                if(targetEUId!=World.ENTITY_SINGLE)
                {
                    if (!_world.GetComponent<PositionComp>(targetEUId, out targetPosComp))
                    {
                        targetEUId = findEnemy();
                    }
                }

                float4 dir = directioncomp.Forward;
                if (_world.GetComponent<PositionComp>(targetEUId, out targetPosComp))
                {
                    float4 targetDir = math.normalize(targetPosComp.Pos-positioncomp.Pos);
                    dir = MathHelper.VectorRotateTo(dir,targetDir,targetmovecomp.RotateVelocity*_world.DeltaTime);
                }
                
                float4 pos = positioncomp.Pos + _world.DeltaTime*targetmovecomp.MoveVelocity*dir;
                float4 prevPos = positioncomp.PrevPos;

                if(!math.all(positioncomp.Pos==pos) || !math.all(positioncomp.PrevPos==pos))
                {
                    positioncomp.Pos = pos;
                    positioncomp.PrevPos = prevPos;
                    positioncomp.Dirty++;
                }

                if(!math.all(directioncomp.Forward==dir) )
                {
                    directioncomp.Forward = dir;
                    directioncomp.Dirty++;
                }
            });


        }

        private int findEnemy()
        {
            return 0;
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
