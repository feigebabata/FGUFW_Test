using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

namespace RogueGamePlay
{
    /// <summary>
    /// 在事件添加之前执行
    /// </summary>
    [BurstCompile]
    public partial struct SkillEventDestroySystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new SkillEventSingleton
            {
                Events = new NativeList<SkillEventData>(1024*1,Allocator.Persistent)
            });
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<SkillEventSingleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                singletonRW.ValueRW.Events.Clear();
            }
        }

    }
}
