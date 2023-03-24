using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

public class LightMask : MonoBehaviour
{
    public Material MaskMaterial;
    // public int3[] LightPoints;
    public int LightPointCount;
    private NativeArray<int3> _lightPoints;
    private Texture2D _tex2D;
    public byte Alpha;
    public Vector2Int Size;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _tex2D = new Texture2D(Size.x,Size.y);
        MaskMaterial.mainTexture = _tex2D;
        _lightPoints = new NativeArray<int3>(LightPointCount,Allocator.Persistent);
        for (int i = 0; i < LightPointCount; i++)
        {
            _lightPoints[i] = new int3
            {
                x = UnityEngine.Random.Range(0,Size.x),
                y = UnityEngine.Random.Range(0,Size.y),
                z = (int)UnityEngine.Random.Range(Size.y*0.02f,Size.y*0.25f),
            };
        }
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _lightPoints.Dispose();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        var pixels = _tex2D.GetPixelData<Color32>(0);

        var initJob = new PixelsInitJob
        {
            Pixels = pixels,
            Alpha = Alpha
        }
        .Schedule(pixels.Length,256);
        // initJob.Complete();

        var drawJob = new DrawLightPointsJob
        {
            Alpha = Alpha,
            Width = _tex2D.width,
            LightPoints = _lightPoints,
            Pixels = pixels
        }
        .Schedule(pixels.Length,256,initJob);
        drawJob.Complete();

        _tex2D.SetPixelData<Color32>(pixels,0);
        _tex2D.Apply();
    }

    [BurstCompile]
    struct DrawLightPointsJob : IJobParallelFor
    {
        [ReadOnly]public byte Alpha;//[0,255]
        [ReadOnly]public int Width;
        [ReadOnly]public NativeArray<int3> LightPoints;
        public NativeArray<Color32> Pixels;

        public void Execute(int index)
        {
            var pixel = Pixels[index];
            int2 pixelCoord = new int2(index%Width,index/Width);
            foreach (var lightPoint in LightPoints)
            {
                int2 lightCoord = new int2(lightPoint.x,lightPoint.y);
                int range = lightPoint.z;
                var distance = math.distance(pixelCoord,lightCoord);
                if(distance>range)continue;
                byte alpha = (byte)math.remap(0,range,0,Alpha,distance);
                if(pixel.a>alpha)pixel.a=alpha;
            }
            Pixels[index] = pixel;
        }
    }

    [BurstCompile]
    struct PixelsInitJob : IJobParallelFor
    {
        [ReadOnly]public byte Alpha;
        public NativeArray<Color32> Pixels;

        public void Execute(int index)
        {
            Pixels[index] = new Color32(0,0,0,Alpha);
        }
    }

}
