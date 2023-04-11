using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class BuffPropertyAuthoring:MonoBehaviour
    {
        public SKillBuff[] Buffs;
    }

    class BuffPropertyBaker : Baker<BuffPropertyAuthoring>
    {
        public override void Bake(BuffPropertyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,new BuffProperty
            {
            });
            var buffer = AddBuffer<SKillBuff>(entity);
            if(authoring.Buffs!=null && authoring.Buffs.Length>0)
            {
                buffer.CopyFrom(authoring.Buffs);
            }
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

    public readonly partial struct BuffPropertyAspect:IAspect
    {
        readonly RefRW<BuffProperty> _property;
        readonly DynamicBuffer<SKillBuff> _skillBuffs;

        public void Add(SKillBuff sKillBuff,float time)
        {
            for (int i = 0; i < _skillBuffs.Length; i++)
            {
                var item = _skillBuffs.ElementAt(i);
                if(item.Type==sKillBuff.Type && item.Origin==sKillBuff.Origin)
                {
                    item.StartTime = time;
                    return;
                }
            }
            sKillBuff.StartTime = time;
            switch (sKillBuff.Type)
            {
                case BuffType.ShootCount:
                    _property.ValueRW.ShootCount += (int)sKillBuff.Value;
                break;
                case BuffType.ShootInteval:
                    _property.ValueRW.ShootInteval += sKillBuff.Value;
                break;
                case BuffType.ShootAngle:
                    _property.ValueRW.ShootAngle += sKillBuff.Value;
                break;
                case BuffType.ShootTurn:
                    _property.ValueRW.ShootTurn += (int)sKillBuff.Value;
                break;
                case BuffType.ReloadTime:
                    _property.ValueRW.ReloadTime += sKillBuff.Value;
                break;
                case BuffType.AttackCount:
                    _property.ValueRW.AttackCount += (int)sKillBuff.Value;
                break;
                case BuffType.Repelled:
                    _property.ValueRW.Repelled += sKillBuff.Value;
                break;
                case BuffType.Power:
                    _property.ValueRW.Power += sKillBuff.Value;
                break;
                case BuffType.Velocity:
                    _property.ValueRW.Velocity += sKillBuff.Value;
                break;
                case BuffType.AttackVelocity:
                    _property.ValueRW.AttackVelocity += sKillBuff.Value;
                break;
            }
        }

        public void TryRemove(float time)
        {
            int lastIndex = _skillBuffs.Length-1;
            for (int i = 0; i < _skillBuffs.Length; i++)
            {
                var item = _skillBuffs.ElementAt(i);
                if(item.StartTime+item.Time>time)
                {
                    switch (item.Type)
                    {
                        case BuffType.ShootCount:
                            _property.ValueRW.ShootCount -= (int)item.Value;
                        break;
                        case BuffType.ShootInteval:
                            _property.ValueRW.ShootInteval -= item.Value;
                        break;
                        case BuffType.ShootAngle:
                            _property.ValueRW.ShootAngle -= item.Value;
                        break;
                        case BuffType.ShootTurn:
                            _property.ValueRW.ShootTurn -= (int)item.Value;
                        break;
                        case BuffType.ReloadTime:
                            _property.ValueRW.ReloadTime -= item.Value;
                        break;
                        case BuffType.AttackCount:
                            _property.ValueRW.AttackCount -= (int)item.Value;
                        break;
                        case BuffType.Repelled:
                            _property.ValueRW.Repelled -= item.Value;
                        break;
                        case BuffType.Power:
                            _property.ValueRW.Power -= item.Value;
                        break;
                        case BuffType.Velocity:
                            _property.ValueRW.Velocity -= item.Value;
                        break;
                        case BuffType.AttackVelocity:
                            _property.ValueRW.AttackVelocity -= item.Value;
                        break;
                    }

                    _skillBuffs.RemoveAtSwapBack(i);
                }
            }
        }

    }

}
