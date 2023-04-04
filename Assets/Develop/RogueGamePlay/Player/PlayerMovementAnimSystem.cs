using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using FGUFW.Entities;
using Unity.Transforms;
using Unity.Burst;

namespace RogueGamePlay
{
    [UpdateAfter(typeof(GamePlayEventCreateSystemGroup))]
    partial struct PlayerMovementAnimSystem:ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var singleton = SystemAPI.GetSingleton<GamePlayEventSingleton>();
            state.Dependency = new PlayerMovementAnimEventJob
            {
                ECB = ecb,
            }
            .Schedule(singleton,state.Dependency);
        }

        struct PlayerMovementAnimEventJob : IGamePlayEventJob
        {
            public EntityCommandBuffer ECB;
            
            public void Execute(Entity entity, GamePlayEventID eventID)
            {
                // Debug.Log(eventID);
                switch (eventID)
                {
                    case GamePlayEventID.PlayerMovementStart:
                    {
                        ECB.AddComponent(entity,new MaterialFlipbookAnimationSwitch
                        {
                            AnimationID = 2,
                        });
                    }
                    break;
                    case GamePlayEventID.PlayerMovementEnd:
                    {
                        ECB.AddComponent(entity,new MaterialFlipbookAnimationSwitch
                        {
                            AnimationID = 1,
                        });
                    }
                    break;
                }
            }
        }

    }
}