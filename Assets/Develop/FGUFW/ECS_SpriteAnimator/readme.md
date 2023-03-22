//https://zhuanlan.zhihu.com/p/573071972
实体查询:
EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<SpriteRender,SpriteRenderer>().Build(ref state);
eq.ToComponentDataArray<SpriteRender>(Allocator.Temp);
eq.ToEntityArray(Allocator.Temp);
[RequireMatchingQueriesForUpdate]//需要匹配查询更新
state.RequireForUpdate(eq);//限制Update执行

获取所有组件:
var lockup = state.GetComponentLookup<TriggerGravityFactor>(true);
lockup.Update(ref state);

获取组件:
SystemAPI.Query<SpriteRender>();
ManagedAPI.GetComponent<SpriteRenderer>(entity);
SystemAPI.Query<RefRW<SpriteRender>>()//可直接修改

job写组件:
var ecb = new EntityCommandBuffer(Allocator.Temp);
ecb.SetComponent(entity,spriteRender);
ecb.Playback(state.EntityManager);
ecb.Dispose();
//省去playback和dispose
SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged)

组件配置:
class继承IComponentData可以被SystemAPI.Query查询
public class SpriteRendererAuthoring : MonoBehaviour{}
public class SpriteRendererBaker : Baker<SpriteRendererAuthoring>
{
    public override void Bake(SpriteRendererAuthoring authoring)
    {
        AddComponent(new SpriteRender
        {
            Self = GetEntity(authoring)
        });
        //AddComponentObject(authoring);
    }
}
public struct SpriteRender:IComponentData
{
    public Entity Self;
    public int SpriteIndex;
}
DynamicBuffer<T>//存储集合

//组件合并 针对业务生成组件集Aspect 字段必须只读
public readonly partial struct SpriteRenderAspect:IAspect
{
    public readonly Entity Self;
    public readonly RefRW<SpriteRender> Render;
    readonly TransformAspect _transform;
}

//IJobEntity示例 并行写入时需要chunkInQueryIndex
partial struct Job : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter CommandBufferParallel;

    void Execute([ChunkIndexInQuery] int chunkInQueryIndex,in SpriteRenderAspect aspect)
    {

    }
}

物理系统:
PhysicsWorld world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
var rayInput = new RaycastInput
{
    Start = newPositionFrom,
    End = newPositionTo,
    Filter = CollisionFilter.Default
};
if (world.CastRay(rayInput, out RaycastHit rayResult))//射线检测
{
    newPositionFrom = rayResult.Position;
}
//物理系统之前
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))] //同组之内设置先后才有效?
触发器job:ITriggerEventsJob
碰撞器job:ICollisionEventsJob
state.Dependency = new TriggerGravityFactorJob
{
}.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//动态物理组件
ComponentLookup<PhysicsVelocity> PhysicsVelocityData;
PhysicsVelocityData.HasComponent(entity);

//尝试自定义job
static readonly SharedStatic<IntPtr> jobReflectionData = SharedStatic<IntPtr>.GetOrCreate<TriggerEventJobProcess<T>>(); //Job里共享的静态只读字段

//unity对象
UnityObjectRef<>