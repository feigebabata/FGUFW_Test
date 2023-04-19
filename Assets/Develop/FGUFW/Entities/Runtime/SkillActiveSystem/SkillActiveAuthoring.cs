using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

/*
技能切换
    1.评测活跃等级 大于当前轨道的活跃值就把自己的活跃等级和ID替换上去
    2.根据当前活跃的ID是不是自己禁用或启用自身
    3.轮询技能被禁用的技能不参与轮询
*/
namespace FGUFW.Entities
{
    public class SkillActiveAuthoring:MonoBehaviour
    {
    }

    class SkillActiveBaker : Baker<SkillActiveAuthoring>
    {
        public override void Bake(SkillActiveAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new SkillActive
            {
                ActiveIDs = new NativeHashMap<int, int2>(32,Allocator.Persistent),
            });
        }
    }

    public struct SkillActive:IComponentData
    {
        public NativeHashMap<int,int2> ActiveIDs;
    }

    public interface ISkillActive:IEnableableComponent,IComponentData
    {
        public int ActiveID{get;}
        public int ActiveLayer{get;}
        public int ActiveLevel{get;}
    }

}
