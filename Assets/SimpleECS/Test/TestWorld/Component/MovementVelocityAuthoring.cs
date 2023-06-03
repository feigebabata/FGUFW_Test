using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;
using FGUFW.SimpleECS;

namespace FGUFW.SimpleECS.Test
{
    public class MovementVelocityAuthoring : MonoBehaviour,IAuthoring<MovementVelocity>
    {
        public Vector3 Value;

        public MovementVelocity Convert()
        {
            var data = new MovementVelocity
            {
                Enabled = this.enabled,
                Value = this.Value,
            };
            GameObject.Destroy(this);
            return data;
        }
        
        void Start(){}
    }

    public struct MovementVelocity : IComponent
    {
        public bool Enabled {get;set;}

        public Vector3 Value;
        
    }
    
}