using System.Collections;
using System.Collections.Generic;
using FGUFW.Entities;
using Unity.Entities;
using UnityEngine;

public class MaterialTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float v = -1.5f;
        Debug.Log((int)v);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}

// [UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
// [UpdateBefore(typeof(MaterialFlipbookAnimDestroySystem))]
[UpdateAfter(typeof(MaterialFlipbookAnimatorSystemGroup))]
partial struct materialEventTestSystem:ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var singleton = SystemAPI.GetSingleton<MaterialFlipbookAnimEventSingleton>();
        state.Dependency = new MaterialEventTestJob{}.Schedule(singleton,state.Dependency);
    }

    partial struct MaterialEventTestJob : IMaterialFlipbookAnimEventJob
    {
        public void Execute(Entity entity, int frameEvent)
        {
            Debug.Log(frameEvent);
        }
    }
}
