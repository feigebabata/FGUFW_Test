using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;

public class PhysicsTest : MonoBehaviour
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
    partial struct TestTriggerEventSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = new TriggerEventJob().Schedule(singleton,state.Dependency);
        }
    }

    partial struct TriggerEventJob : ITriggerEventsJob
    {
        public void Execute(TriggerEvent triggerEvent)
        {
            Debug.Log(triggerEvent);
        }
    }