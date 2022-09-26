using System.Collections;
using System.Collections.Generic;
using FGUFW;
using FGUFW.ECS;
using UnityEngine;

namespace GUNNAC
{
    public class PlayerMoveShowComp : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(World.Current==null)return;
            var world = World.Current;
            world.Filter((ref RenderComp renderComp,ref PlayerRenderComp playerRenderComp)=>
            {
                //战机角度 尾焰长度
                PlayerMoveMsgComp playerMoveMsgComp;
                Quaternion rotateTarget = Quaternion.Euler(Vector3.forward);
                Vector3 tailFlame = Vector3.one*0.6f;
                if(world.GetComponent(renderComp.EntityUId,out playerMoveMsgComp))
                {
                    var velocity = playerMoveMsgComp.Velocity;
                    if(velocity.x>0)
                    {
                        rotateTarget = Quaternion.Euler(Vector3.forward*-30);
                    }
                    else if(velocity.x<0)
                    {
                        rotateTarget = Quaternion.Euler(Vector3.forward*30);
                    }
                    if(velocity.y>0)
                    {
                        tailFlame = Vector3.one;
                    }
                    else if(velocity.y<0)
                    {
                        tailFlame = Vector3.one*0.2f;
                    }
                }
                renderComp.GObj.transform.rotation = Quaternion.Lerp(renderComp.GObj.transform.rotation,rotateTarget,0.05f);
                // tailFlame = Vector3.Lerp()
                var color = playerRenderComp.TailFlameMat.GetColor(playerRenderComp.PropertyID);
                var current = new Vector3(color.r,color.g,color.b);
                current = Vector3.Lerp(current,tailFlame,0.025f);
                color.r=current.x;color.g=current.y;color.b=current.z;
                playerRenderComp.TailFlameMat.SetColor(playerRenderComp.PropertyID,color);
            });
        }
    }
}
