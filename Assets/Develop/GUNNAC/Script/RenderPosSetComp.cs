using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class RenderPosSetComp : MonoBehaviour
    {
        // Update is called once per frame
        void LateUpdate()
        {
            if(World.Current==null)return;
            var world = World.Current;
            world.Filter((ref RenderComp renderComp,ref PositionComp positionComp)=>
            {
                if(positionComp.Dirty==0)return;

                Vector3 pos = renderComp.GObj.transform.position;
                Vector3 newPos = positionComp.Pos.xyz;
                // pos = Vector3.Lerp(pos,newPos,0.06f);
                pos = Vector3.SmoothDamp(pos,newPos,ref renderComp.SmoothVelocity,0.05f);
                if(Vector3.Distance(pos,newPos)<0.0001f)
                {
                    pos = newPos;
                    positionComp.Dirty=0;
                }
                renderComp.GObj.transform.position = pos;
                renderComp.Dirty=1;
            });
        }
    }
}
/*

*/