using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class MonsterAuthoring:MonoBehaviour
    {
        public float HP;
    }

    class MonsterBaker : Baker<MonsterAuthoring>
    {
        public override void Bake(MonsterAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Monster
            {
                HP = authoring.HP,
            });
        }
    }

    /// <summary>
    /// 怪
    /// </summary>
    public struct Monster:IComponentData
    {
        public float HP;
    }
}
