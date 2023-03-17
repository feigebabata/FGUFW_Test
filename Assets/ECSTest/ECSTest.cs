using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSTest : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Rigidbody rigidbody = null;
        
    }
    
}



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(ForceMovementSystem))]
partial struct ForceMovementControlSystem:ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float3 key = float3.zero;
        if(Input.GetKey(KeyCode.W))
        {
            key.y += 10000;
        }
        if(Input.GetKey(KeyCode.S))
        {
            key.y -= 10000;
        }
        if(Input.GetKey(KeyCode.A))
        {
            key.x -= 10000;
        }
        if(Input.GetKey(KeyCode.D))
        {
            key.x += 10000;
        }
        
        foreach (var (move,transform) in SystemAPI.Query<RefRW<ForceMovement>,WorldTransform>())
        {
            if(math.all(key==float3.zero))
            {
                if(!math.all(move.ValueRO.TargetPoint==transform.Position))move.ValueRW.TargetPoint = transform.Position;
            }
            else
            {
                if(!math.all(move.ValueRO.TargetPoint==key))move.ValueRW.TargetPoint = key;
            }
        }

    }
}