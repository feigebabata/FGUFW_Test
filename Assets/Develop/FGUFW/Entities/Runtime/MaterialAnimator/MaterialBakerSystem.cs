using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Collections.LowLevel.Unsafe;

namespace FGUFW.Entities
{
    [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
    [UpdateAfter(typeof(MaterialFlipbookAnimDestroySystem))]
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
        }

        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.ManagedAPI.GetSingleton<MaterialsSingleton>();
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var hybridRendererSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            foreach (var (anim,entity) in SystemAPI.Query<MaterialFlipbookAnimation>().WithEntityAccess())
            {
                BatchMaterialID materialID = getMaterialID(ref hybridRendererSystem,singleton,anim.Mat);

                ecb.AddComponent(entity,new MaterialFlipbookAnimationData
                {
                    MaterialID = materialID,
                    AnimationID = anim.AnimationID,
                    Start = anim.StartFrame,
                    Length = anim.FrameLength,
                    Time = anim.Time,
                });
                
                ecb.RemoveComponent<MaterialFlipbookAnimation>(entity);
            }

            foreach (var (anims,entity) in SystemAPI.Query<MaterialFlipbookAnimationsBakerData>().WithEntityAccess())
            {
                var buffer = ecb.AddBuffer<MaterialFlipbookAnimations>(entity);
                foreach (var item in anims.Animations)
                {
                    BatchMaterialID materialID = getMaterialID(ref hybridRendererSystem,singleton,item.Anim.Mat);
                    var animConfig = new MaterialFlipbookAnimations();
                    animConfig.Anim = new MaterialFlipbookAnimationData
                    {
                        AnimationID = item.Anim.AnimationID,
                        MaterialID = materialID,
                        Start = item.Anim.StartFrame,
                        Length = item.Anim.FrameLength,
                        Time = item.Anim.Time,
                    };
                    // var events = new UnsafeList<MaterialFlipbookAnimEventData>(item.Events.Length,Allocator.Persistent);
                    // foreach (var eventData in item.Events)
                    // {
                    //     events.Add(eventData);
                    // }
                    // animConfig.Events = events;
                    buffer.Add(animConfig);
                }
                ecb.RemoveComponent<MaterialFlipbookAnimationsAuthoring>(entity);
            }


        }

        BatchMaterialID getMaterialID(ref EntitiesGraphicsSystem hybridRendererSystem,MaterialsSingleton singleton,Material material)
        {
            BatchMaterialID materialID;
            if(singleton.Materials.ContainsKey(material))
            {
                materialID = singleton.Materials[material];
            }
            else
            {
                materialID = hybridRendererSystem.RegisterMaterial(material);
                singleton.Materials.Add(material,materialID);
            }
            return materialID;
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