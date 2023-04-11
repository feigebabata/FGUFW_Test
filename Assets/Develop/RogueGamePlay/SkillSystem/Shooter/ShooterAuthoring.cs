using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class ShooterAuthoring:MonoBehaviour
    {
        public SkillEvent Triggers;
        public int ShootCount;
        public float IntervalTime;
        public float ShootAngle;
        public GameObject Attacker;
        public int ShootRound;
        public float ReloadTime; 
        public ShootDirectionType Direction;
    }

    class ShooterBaker : Baker<ShooterAuthoring>
    {
        public override void Bake(ShooterAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Shooter
            {
                Triggers = authoring.Triggers,
                ShootCount = authoring.ShootCount,
                IntervalTime = authoring.IntervalTime,
                ShootAngle = authoring.ShootAngle,
                Attacker = GetEntity(authoring.Attacker,TransformUsageFlags.Dynamic),
                ShootRound = authoring.ShootRound,
                ReloadTime = authoring.ReloadTime,
                Direction = authoring.Direction,
            });
        }
    }

    /// <summary>
    /// 射击体
    /// </summary>
    public struct Shooter:IComponentData
    {
        public SkillEvent Triggers;
        public int ShootCount;
        public float IntervalTime;
        public float ShootAngle;
        public Entity Attacker;
        public int ShootRound;
        public float ReloadTime;
        public float LastShootTime;
        public ShootDirectionType Direction;
    }
}
