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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}

[UpdateInGroup(typeof(MaterialFlipbookAnimatorSystemGroup))]
[UpdateBefore(typeof(MaterialFlipbookAnimEventDestroySystem))]
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
            Debug.Log(frameEvent);
        }
    }
}
