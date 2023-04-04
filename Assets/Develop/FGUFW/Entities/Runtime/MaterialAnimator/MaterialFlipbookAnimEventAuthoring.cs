using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Collections;
using UnityEngine.Rendering;

namespace FGUFW.Entities
{
    [AddComponentMenu("MaterialFlipbookAnim/Events")]
    public class MaterialFlipbookAnimEventAuthoring : MonoBehaviour
    {
        public MaterialFlipbookAnimEventData[] Events;
    }

    class MaterialFlipbookAnimEventBaker : Baker<MaterialFlipbookAnimEventAuthoring>
    {
        public override void Bake(MaterialFlipbookAnimEventAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<MaterialFlipbookAnimEventData>(entity);

            if(authoring.Events!=null && authoring.Events.Length>0)
            {
                foreach (var item in authoring.Events)
                {
                    buffer.Add(item);
                }
            }
        }
    }

    [Serializable]
    public struct MaterialFlipbookAnimEventData:IBufferElementData
    {
        public int FrameIndex;
        public int FrameEvent;
    }
}