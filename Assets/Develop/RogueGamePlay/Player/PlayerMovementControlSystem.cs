using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using FGUFW.Entities;
using Unity.Transforms;

namespace RogueGamePlay
{
    [UpdateInGroup(typeof(GamePlayEventCreateSystemGroup))]
    partial struct PlayerMovementControlSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<ForceMovement,PlayerMovementControl,LocalTransform>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        public void OnUpdate(ref SystemState state)
        {
            var singletonRW = SystemAPI.GetSingletonRW<GamePlayEventSingleton>();

            foreach (var (movementRW,controlRW,localTransform,entity) in SystemAPI.Query<RefRW<ForceMovement>,RefRW<PlayerMovementControl>,LocalTransform>().WithEntityAccess())
            {
                float3 orientation = float3.zero;

                if(Input.GetKey(KeyCode.W))
                {
                    orientation.y += 1;
                }
                if(Input.GetKey(KeyCode.S))
                {
                    orientation.y -= 1;
                }
                if(Input.GetKey(KeyCode.A))
                {
                    orientation.x -= 1;
                }
                if(Input.GetKey(KeyCode.D))
                {
                    orientation.x += 1;
                }

                if(math.all(controlRW.ValueRW.Orientation==float3.zero))
                {
                    if(!math.all(orientation==float3.zero))singletonRW.ValueRW.Events.Add(new GamePlayEventData
                    {
                        Self = entity,
                        EventID = GamePlayEventID.PlayerMovementStart,
                    });
                }
                else
                {
                    if(math.all(orientation==float3.zero))singletonRW.ValueRW.Events.Add(new GamePlayEventData
                    {
                        Self = entity,
                        EventID = GamePlayEventID.PlayerMovementEnd,
                    });
                }

                controlRW.ValueRW.Orientation = math.normalize(orientation);

                movementRW.ValueRW.TargetPoint = localTransform.Position + orientation*100;
            }
        }


    }

}