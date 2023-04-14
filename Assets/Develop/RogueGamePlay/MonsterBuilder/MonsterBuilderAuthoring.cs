using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class MonsterBuilderAuthoring:MonoBehaviour
    {
        public GameObject MonsterPrefab;
        public int Count;
        public float IntervalTime;
    }

    class MonsterBuilderBaker : Baker<MonsterBuilderAuthoring>
    {
        public override void Bake(MonsterBuilderAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new MonsterBuilder
            {
                MonsterPrefab = GetEntity(authoring.MonsterPrefab,TransformUsageFlags.Dynamic),
                Count = authoring.Count,
                IntervalTime = authoring.IntervalTime,
            });
        }
    }

    public struct MonsterBuilder:IComponentData
    {
        public Entity MonsterPrefab;
        public int Count;
        public float IntervalTime;
        public float LastBuilderTime;
    }
}
