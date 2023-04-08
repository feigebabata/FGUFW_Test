using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class BuffPropertyAuthoring:MonoBehaviour
    {

    }

    class BuffPropertyBaker : Baker<BuffPropertyAuthoring>
    {
        public override void Bake(BuffPropertyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new BuffProperty
            {
            });
            AddBuffer<SKillBuff>(entity);
        }
    }

    public struct BuffProperty:IComponentData
    {
        public int ShootCount;
        public float ShootInteval;
        public float ShootAngle;
        public int ShootTurn;
        public float ReloadTime;
        public int AttackCount;
        public float Repelled;
        public float Power;
        public float Velocity;
        public float AttackVelocity;
    }

    public struct SKillBuff:IBufferElementData
    {
        public BuffType Type;
        public float Value;
        public Entity Effect;
        
        public float Time;
        public float StartTime;

        /// <summary>
        /// 重复则只叠加时间
        /// </summary>
        public int Origin;
    }

    public partial struct BuffPropertyAspect:IAspect
    {
        readonly RefRW<BuffProperty> _property;
        readonly DynamicBuffer<SKillBuff> _skillBuffs;

        public void Add(SKillBuff sKillBuff)
        {

        }        
        
        public void Remove(SKillBuff sKillBuff)
        {

        }

        public void TryRemove()
        {

        }

    }

}
