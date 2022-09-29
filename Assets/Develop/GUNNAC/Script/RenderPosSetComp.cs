using System.Collections;
using System.Collections.Generic;
using FGUFW;
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
                //if(positionComp.Dirty==0)return;

                Vector3 pos = renderComp.GObject.transform.position;
                Vector3 newPos = positionComp.Pos.xyz;
                float lerpVal = MathHelper.LerpByCycle(ScreenHelper.FPS / World.FRAME_COUNT,0.6f,0.65f);
                //Debug.Log(lerpVal);
                pos = Vector3.Lerp(pos,newPos, lerpVal);
                // pos = Vector3.SmoothDamp(pos,newPos,ref renderComp.SmoothVelocity,lerpVal);
                if(Vector3.Distance(pos,newPos)<0.001f)
                {
                    pos = newPos;
                }
                renderComp.GObject.transform.position = pos;
                
            });

            world.Filter((ref PoolRenderComp poolRenderComp,ref PositionComp positionComp)=>
            {
                Vector3 pos = poolRenderComp.GObject.transform.position;
                Vector3 newPos = positionComp.Pos.xyz;
                float lerpVal = MathHelper.LerpByCycle(ScreenHelper.FPS / World.FRAME_COUNT,0.6f,0.65f);
                pos = Vector3.Lerp(pos,newPos, lerpVal);
                if(Vector3.Distance(pos,newPos)<0.001f)
                {
                    pos = newPos;
                }
                poolRenderComp.GObject.transform.position = pos;
                
            });
        }
    }
}
/*

*/