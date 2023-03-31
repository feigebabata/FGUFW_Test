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

[UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
[UpdateBefore(typeof(MaterialFlipbookAnimDestroySystem))]
[UpdateAfter(typeof(MaterialFlipbookAnimEventCreateSystem))]
partial struct materialEventTestSystem:ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var singleton = SystemAPI.GetSingleton<MaterialFlipbookAnimEventSingleton>();
        state.Dependency = new materialEventTestJob{}.Schedule(singleton,state.Dependency);
    }

    partial struct materialEventTestJob : IMaterialFlipbookAnimEventJob
    {
        public void Execute(Entity entity, int frameEvent)
        {
            // Debug.Log(frameEvent);
        }
    }
}
