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
    public interface ISystem<T> where T : World<T>
    {
        void OnUpdate(T world,int systemIndex);
    } 

    public interface ISystemOnCreate<T> where T : World<T>
    {
        void OnCreate(T world,int systemIndex);
    } 

    public interface ISystemOnDestroy<T> where T : World<T>
    {
        void OnDestroy(T world,int systemIndex);
    } 
    
}
