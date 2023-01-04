using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

public class JobTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var array = new NativeList<int>(1,Allocator.Persistent);
        var job = new Job1
        {
            Array = array.AsParallelWriter()
        };
        var jobHandle = job.Schedule(100,100);
        jobHandle.Complete();
        array.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public struct Job1 : IJobParallelFor
    {
        public NativeList<int>.ParallelWriter Array;

        public void Execute(int index)
        {
            Array.AddNoResize(index);
        }
    }
}
