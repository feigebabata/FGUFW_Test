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
        //AddComponentObject(null); //不能添加MonoBehaviour
    }
}
public struct SpriteRender:IComponentData
{
    public Entity Self;
    public int SpriteIndex;
}


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

CollisionFilter//物理碰撞层
PhysicsMaterialTemplate//物理材质模板

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

//job中修改材质字段 不能用于SpriteRenderer
[MaterialProperty("_Scale")]

//渲染 相机输出到RenderTexture
RenderTexture在ECS中不能正常工作

//材质注册和替换
hybridRendererSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
materialID = hybridRendererSystem.RegisterMaterial(material);
materialMeshInfo.MaterialID = materialID;

//清理组件 销毁实体组件仍在 直到移除组件
ICleanupComponentData

//Query无法获取 用于替换频繁增删组件
IEnableableComponent
//忽略组件未激活
var queryIgnoredEnableable = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState).Build(this);

//标签组件 无字段组件不占内存

//动态缓冲组件
DynamicBuffer<T>

//共享非托管只读数据
BlobAsset BlobAssetReference
//需要使用BlogBuilder来创建，并且用完要Dispose掉哦
BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
//为BlobAsset的顶级字段分配内存，并返回对其的引用
ref AbilityDetail root = ref blobBuilder.ConstructRoot<AbilityDetail>();
//为其中的数据赋值
root.Damage = 10086;
root.CoolDownTime = 10086;
//BlobArray需要用特殊的手段来进行初始化，然后就可像普通数组一样来进行赋值了
BlobBuilderArray<Buff> buffArray = blobBuilder.Allocate(ref root.HitBuffs, 3);
buffArray[0] = new Buff();
buffArray[1] = new Buff();
buffArray[2] = new Buff();
//最后一步创建BlobAsset的引用，注意这个东西用完后也是需要Dispose掉的
BlobAssetReference<AbilityDetail> abilityDetailRef =
    blobBuilder.CreateBlobAssetReference<AbilityDetail>(Allocator.Persistent);
//完事
blobBuilder.Dispose();