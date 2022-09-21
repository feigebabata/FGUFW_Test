
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

                if(targetEUId!=0)
                {

                    PositionComp targetPosComp;
                    if (!_world.GetComponent<PositionComp>(targetEUId, out targetPosComp))
                    {
                        targetEUId = 0;
                    }
                }
            });


        }

        public void Dispose()
        {
            _world = null;
        }

        

    }
}
