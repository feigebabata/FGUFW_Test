using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class PlayerAuthoring:MonoBehaviour
    {
        public float HP;
    }

    class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Player
            {
                HP = authoring.HP,
            });
        }
    }

    /// <summary>
    /// 玩家
    /// </summary>
    public struct Player:IComponentData,IEnableableComponent
    {
        public float HP;
    }
}
