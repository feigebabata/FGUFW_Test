using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class ShootersAuthoring:MonoBehaviour
    {
        public Data[] Datas;

        [System.Serializable]
        public struct Data
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
    }

    class ShooterBaker : Baker<ShootersAuthoring>
    {
        public override void Bake(ShootersAuthoring authoring)
        {
            var buffer = AddBuffer<Shooter>(GetEntity(TransformUsageFlags.Dynamic));
            if(authoring.Datas==null)return;
            foreach (var item in authoring.Datas)
            {
                buffer.Add(new Shooter
                {
                    Triggers = item.Triggers,
                    ShootCount = item.ShootCount,
                    IntervalTime = item.IntervalTime,
                    ShootAngle = item.ShootAngle,
                    Attacker = GetEntity(item.Attacker,TransformUsageFlags.Dynamic),
                    ShootRound = item.ShootRound,
                    ReloadTime = item.ReloadTime,
                    Direction = item.Direction,
                });
            }
        }
    }

    /// <summary>
    /// 射击体
    /// </summary>
    public struct Shooter:IBufferElementData
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
