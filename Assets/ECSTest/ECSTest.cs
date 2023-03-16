using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSTest : MonoBehaviour
{

}



[UpdateBefore(typeof(ForceMovementSystem))]
partial struct ForceMovementControlSystem:ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        const float space = 1;

        float3 down = float3.zero;
        if(Input.GetKeyDown(KeyCode.W))
        {
            down.y += space;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            down.y += -space;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            down.x += -space;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            down.x += space;
        }

        if(!math.all(down==float3.zero))
        {
            foreach (var forceMovement in SystemAPI.Query<RefRW<ForceMovement>>())
            {
                var targetPoint = forceMovement.ValueRW.TargetPoint;
                if(down.x==space)
                {
                    targetPoint.x = 10000;
                }
                if(down.x==-space)
                {
                    targetPoint.x = -10000;
                }

                
                if(down.y==space)
                {
                    targetPoint.y = 10000;
                }
                if(down.y==-space)
                {
                    targetPoint.y = -10000;
                }
                forceMovement.ValueRW.TargetPoint = targetPoint;
            }
        }

        float3 up = float3.zero;
        if(Input.GetKeyUp(KeyCode.W))
        {
            up.y += space;
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            up.y += -space;
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            up.x += -space;
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            up.x += space;
        }


        if(!math.all(up==float3.zero))
        {
            foreach (var (forceMovement,transform) in SystemAPI.Query<RefRW<ForceMovement>,WorldTransform>())
            {
                var targetPoint = forceMovement.ValueRW.TargetPoint;
                if(up.x==space)
                {
                    targetPoint.x = transform.Position.x;
                }
                if(up.x==-space)
                {
                    targetPoint.x = transform.Position.x;
                }

                
                if(up.y==space)
                {
                    targetPoint.y = transform.Position.y;
                }
                if(up.y==-space)
                {
                    targetPoint.y = transform.Position.y;
                }
                Debug.LogWarning(targetPoint);
                forceMovement.ValueRW.TargetPoint = targetPoint;
            }
        }


    }
}