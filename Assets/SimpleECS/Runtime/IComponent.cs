using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

namespace FGUFW.SimpleECS
{
    public interface IComponent
    {
        public bool Enabled{get;set;}
    } 
    
}
