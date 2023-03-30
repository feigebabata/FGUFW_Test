using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Rendering;

namespace FGUFW.Entities
{
    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    partial struct MaterialBakerSystem : ISystem
    {
        public class MaterialsSingleton:IComponentData
        {
            public Dictionary<Material,BatchMaterialID> Materials = new Dictionary<Material,BatchMaterialID>();
        }
        
        public void OnCreate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new MaterialsSingleton());
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.ManagedAPI.GetSingleton<MaterialsSingleton>();
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var hybridRendererSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            foreach (var (anim,entity) in SystemAPI.Query<MaterialFlipbookAnimation>().WithEntityAccess())
            {
                BatchMaterialID materialID;
                if(singleton.Materials.ContainsKey(anim.Mat))
                {
                    materialID = singleton.Materials[anim.Mat];
                }
                else
                {
                    materialID = hybridRendererSystem.RegisterMaterial(anim.Mat);
                    singleton.Materials.Add(anim.Mat,materialID);
                }

                ecb.AddComponent(entity,new MaterialFlipbookAnimationData
                {
                    MaterialID = materialID,
                    Start = anim.StartFrame,
                    Length = anim.FrameLength,
                    Time = anim.Time,
                    Loop = anim.Loop,
                });
                
                ecb.RemoveComponent<MaterialFlipbookAnimation>(entity);

            }
        }

        public void OnDestroy(ref SystemState state)
        {
            //系统销毁中不允许获取系统
            // var singleton = SystemAPI.ManagedAPI.GetSingleton<FlipbookMaterialsSingleton>();
            // var hybridRendererSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
            // foreach (var item in singleton.Materials)
            // {
            //     hybridRendererSystem.UnregisterMaterial(item.Value);
            // }
        }


    }
}