using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class AttackerAuthoring:MonoBehaviour
    {
        public int AttackCount;
        public float HitVelocity;
        public float Power;
        public float Time;
    }

    class AttackerBaker : Baker<AttackerAuthoring>
    {
        public override void Bake(AttackerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Attacker
            {
                AttackCount = authoring.AttackCount,
                HitVelocity = authoring.HitVelocity,
                Power = authoring.Power,
                Time = authoring.Time,
            });
        }
    }

    /// <summary>
    /// 攻击体
    /// </summary>
    public struct Attacker:IComponentData
    {
        public int AttackCount;
        public float HitVelocity;
        public float Power;
        public float Time;
        public float StartTime;
    }
}
