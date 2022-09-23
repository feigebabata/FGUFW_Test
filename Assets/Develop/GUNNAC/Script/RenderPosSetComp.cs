using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class RenderPosSetComp : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            world.Filter((ref RenderComp renderComp,ref PositionComp positionComp)=>
            {
                var pos = renderComp.GObj.transform.position;
                var newPos = positionComp.Pos.xyz;
                pos = Vector3.Lerp(pos,newPos,0.035f);
                renderComp.GObj.transform.position = pos;
                renderComp.Dirty=0;
            });
        }
    }
}
