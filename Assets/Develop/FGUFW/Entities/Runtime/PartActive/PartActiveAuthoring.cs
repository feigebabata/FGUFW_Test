using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using FGUFW;

/*
技能切换
    1.评测活跃等级 大于PartActive轨道的活跃值就把IPartActive的活跃等级和ID替换到PartActive
    2.IPartActive根据PartActive的ID是不是自己禁用或启用IPartActiveEnable
    3.轮询技能被禁用的技能不参与轮询
*/
namespace FGUFW.Entities
{
    [DisallowMultipleComponent]
    public class PartActiveAuthoring:MonoBehaviour
    {
    }

    /// <summary>
    /// 评测Part活跃
    /// </summary>
    [UpdateBefore(typeof(PartActiveEnableSystemGroup))]
    public partial class PartActiveCheckSystemGroup : ComponentSystemGroup{}

    /// <summary>
    /// 根据活跃启用Part
    /// </summary>
    [UpdateBefore(typeof(PartActiveExecuteSystemGroup))]
    public partial class PartActiveEnableSystemGroup : ComponentSystemGroup{}

    /// <summary>
    /// 执行Part
    /// </summary>
    public partial class PartActiveExecuteSystemGroup : ComponentSystemGroup{}

    class PartActiveBaker : Baker<PartActiveAuthoring>
    {
        public override void Bake(PartActiveAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new PartActive
            {
                
            });
        }
    }

    public struct PartActive:IComponentData
    {
        /// <summary>
        /// k:layer 
        /// v:id,level
        /// </summary>
        public Int2s8 ActivesID;

        public PartEnable IsActive(int layer,int id)
        {
            if(ActivesID[layer].x == id)
            {
                return PartEnable.Enabled;
            }
            else
            {
                return PartEnable.Disabled;
            }
        }

        public void SetActive(int layer,int id,int level)
        {
            if(ActivesID[layer].y<level)
            {
                ActivesID[layer] = new int2(id,level);
            }
        }

    }


    /// <summary>
    /// 技能活跃参数
    /// </summary>
    public interface IPartActive:IComponentData
    {
        /// <summary>
        /// 活动标识
        /// </summary>
        public int ActiveID{get;}

        /// <summary>
        /// 活跃层
        /// </summary>
        public int ActiveLayer{get;}

        /// <summary>
        /// 优先级 >0
        /// </summary>
        public int ActiveLevel{get;}

        /// <summary>
        /// 上一帧是否激活
        /// </summary>
        public PartEnable PrevEnable{get;set;}

        /// <summary>
        /// 激活切换时间
        /// </summary>
        public float SetEnableTime{get;set;}
    }

    /// <summary>
    /// 用于技能是否活跃
    /// </summary>
    public interface IPartActiveEnable:IComponentData,IEnableableComponent
    {
    }

    public enum PartEnable
    {
        None = 0,
        Enabled = 1,
        Disabled = 2,
    }

}
