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
    partial struct MaterialBakerSystem : ISystem
    {
        public class FlipbookMaterialsSingleton:IComponentData
        {
            public Dictionary<Material,BatchMaterialID> Materials = new Dictionary<Material,BatchMaterialID>();
            //EntitiesGraphicsSystem s;
        }
        
        public void OnCreate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new FlipbookMaterialsSingleton());
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.ManagedAPI.GetSingleton<FlipbookMaterialsSingleton>();
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var hybridRendererSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();

            foreach (var (buffer,animDatas,entity) in SystemAPI.Query<DynamicBuffer<MaterialFlipbookAnimationData>,MaterialFlipbookAnimatorAuthoring.AnimationDatas>().WithEntityAccess())
            {
                if(animDatas.Anims!=null && animDatas.Anims.Length>0)
                {
                    foreach (var item in animDatas.Anims)
                    {
                        BatchMaterialID materialID;
                        if(singleton.Materials.ContainsKey(item.Mat))
                        {
                            materialID = singleton.Materials[item.Mat];
                        }
                        else
                        {
                            materialID = hybridRendererSystem.RegisterMaterial(item.Mat);
                            singleton.Materials.Add(item.Mat,materialID);
                        }

                        buffer.Add(new MaterialFlipbookAnimationData
                        {
                            MaterialID = materialID,
                            Start = item.Start,
                            Length = item.Length,
                            Time = item.Time,
                        });
                    }
                }
                ecb.RemoveComponent<MaterialFlipbookAnimatorAuthoring.AnimationDatas>(entity);


            }
        }

        public void OnDestroy(ref SystemState state)
        {

        }


    }
}