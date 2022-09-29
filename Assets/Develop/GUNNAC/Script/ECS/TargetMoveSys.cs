
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
            _world.Filter((ref TargetMoveComp targetmovecomp,ref PositionComp positioncomp,ref DirectionComp directioncomp) =>
            {
                var targetEUId = targetmovecomp.TargetEUId;
                PositionComp targetPosComp;

                if (targetEUId == World.ENTITY_NONE || !_world.GetComponent<PositionComp>(targetEUId, out targetPosComp))
                {
                    targetEUId = findEnemy(positioncomp.Pos);
                }


                float4 dir = directioncomp.Forward;
                if (targetEUId!=World.ENTITY_NONE && _world.GetComponent<PositionComp>(targetEUId, out targetPosComp))
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
                }

                if(!math.all(directioncomp.Forward==dir) )
                {
                    directioncomp.Forward = dir;
                }
            });


        }

        private int findEnemy(float4 pos)
        {
            var enemys = _world.GetComponents<EnemyComp>();
            if(enemys==null)return World.ENTITY_NONE;
            float minSpace = float.MaxValue;
            int targetEUId = 0;
            foreach (var item in enemys)
            {
                int entityUId = item.EntityUId;
                PositionComp positionComp;
                _world.GetComponent(entityUId,out positionComp);
                float space = math.distance(positionComp.Pos,pos);
                if(space<minSpace)
                {
                    minSpace = space;
                    targetEUId = entityUId;
                }
            }
            return targetEUId;
        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
