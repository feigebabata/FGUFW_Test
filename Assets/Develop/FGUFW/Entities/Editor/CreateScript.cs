#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace FGUFW.Entities
{
    static class CreateScript
    {
        [MenuItem("Assets/Create/FGUFW/ISystem Script",false,80)]
        static void createSystem()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/System.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateISystemScript>(),createPath,null,null);
        }
        
        [MenuItem("Assets/Create/FGUFW/IComponentData Script",false,80)]
        static void createComponent()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Component.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateComponentScript>(),createPath,null,null);
        }
        
        [MenuItem("Assets/Create/FGUFW/EventSystem",false,80)]
        static void createEventSystem()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Event";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateEventSystem>(),createPath,null,null);
        }

        class CreateEventSystem : EndNameEditAction
        {

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                //创建资源
                UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
            }
    
            internal static UnityEngine.Object CreateScriptAssetFromTemplate(string filePath, string resourceFile)
            {
                var dirPath = filePath;
                if(!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var className = Path.GetFileName(dirPath);
                Debug.Log(className);
                string iJobScriptText = 
@"
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using System;

namespace RogueGamePlay
{

    public struct |CLASS_NAME|Data
    {

    }

    public struct |CLASS_NAME|Singleton:IComponentData
    {
        public NativeList<|CLASS_NAME|Data> Events;
    }

    /// <summary>
    /// 在事件添加之后执行
    /// </summary>
    [JobProducerType(typeof(I|CLASS_NAME|JobExtensions.|CLASS_NAME|JobProcess<>))]
    public interface I|CLASS_NAME|Job
    {
        void Execute(|CLASS_NAME|Data eventData);
    }

    public unsafe static class I|CLASS_NAME|JobExtensions
    {
        public static JobHandle Schedule<T>(this T jobData, |CLASS_NAME|Singleton singleton, JobHandle inputDeps)
            where T : struct, I|CLASS_NAME|Job
        {
            if(singleton.Events.Length==0)
            {
                return inputDeps;
            }
            var data = new |CLASS_NAME|JobData<T>
            {
                UserJobData = jobData,
                Events = singleton.Events
            };
            var jobReflectionData = |CLASS_NAME|JobProcess<T>.JobReflectionData.Data;
            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data),jobReflectionData,inputDeps,ScheduleMode.Single);
            return JobsUtility.Schedule(ref parameters);
        }

        internal unsafe struct |CLASS_NAME|JobData<T> where T : struct
        {
            public T UserJobData;
            public NativeList<|CLASS_NAME|Data> Events;
        }

        internal unsafe struct |CLASS_NAME|JobProcess<T> where T : struct, I|CLASS_NAME|Job
        {
            internal static readonly SharedStatic<IntPtr> JobReflectionData = SharedStatic<IntPtr>.GetOrCreate<|CLASS_NAME|JobProcess<T>>();

            internal static void Initialize()
            {
                if (JobReflectionData.Data == IntPtr.Zero)
                {
                    JobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(|CLASS_NAME|JobData<T>), typeof(T), (ExecuteJobFunction)Execute);
                }
                    
            }

            public delegate void ExecuteJobFunction(ref |CLASS_NAME|JobData<T> jobData);

            public static void Execute(ref |CLASS_NAME|JobData<T> jobData)
            {
                foreach (var eventData in jobData.Events)
                {
                    jobData.UserJobData.Execute(eventData);
                }
            }
        }        
        
        //自动执行
        public static void EarlyJobInit<T>() where T : struct, I|CLASS_NAME|Job
        {
            |CLASS_NAME|JobProcess<T>.Initialize();
        }

    }
}
";              
                var destroyScriptText = 
@"using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

namespace RogueGamePlay
{
    /// <summary>
    /// 在事件添加之前执行
    /// </summary>
    [BurstCompile]
    public partial struct |CLASS_NAME|DestroySystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(ecb.CreateEntity(),new |CLASS_NAME|Singleton
            {
                Events = new NativeList<|CLASS_NAME|Data>(1024*1,Allocator.Persistent)
            });
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            var singletonRW = SystemAPI.GetSingletonRW<|CLASS_NAME|Singleton>();
            if(singletonRW.ValueRO.Events.Length>0)
            {
                singletonRW.ValueRW.Events.Clear();
            }
        }

    }
}
";
                iJobScriptText = iJobScriptText.Replace("|CLASS_NAME|",className);
                destroyScriptText = destroyScriptText.Replace("|CLASS_NAME|",className);
                var IjobPath = $"{dirPath}/I{className}Job.cs";
                var destroyPath = $"{dirPath}/{className}DestroySystem.cs";
                File.WriteAllText(IjobPath,iJobScriptText,Encoding.UTF8);
                File.WriteAllText(destroyPath,destroyScriptText,Encoding.UTF8);
                //刷新资源管理器
                AssetDatabase.ImportAsset(filePath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(dirPath, typeof(UnityEngine.Object));
            }
        }

        class CreateComponentScript : EndNameEditAction
        {

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                //创建资源
                UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
            }
    
            internal static UnityEngine.Object CreateScriptAssetFromTemplate(string filePath, string resourceFile)
            {
                string scriptText = 
@"using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace RogueGamePlay
{
    public class |CLASS_NAME|Authoring:MonoBehaviour
    {
    }

    class |CLASS_NAME|Baker : Baker<|CLASS_NAME|Authoring>
    {
        public override void Bake(|CLASS_NAME|Authoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic),new |CLASS_NAME|
            {
            });
        }
    }

    public struct |CLASS_NAME|:IComponentData
    {
    }
}
";
                var className = Path.GetFileName(filePath).Replace(".cs","");
                scriptText = scriptText.Replace("|CLASS_NAME|",className);
                filePath = filePath.Replace($"{className}.cs",$"{className}Authoring.cs");
                File.WriteAllText(filePath,scriptText,Encoding.UTF8);
                //刷新资源管理器
                AssetDatabase.ImportAsset(filePath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(filePath, typeof(UnityEngine.Object));
            }
        }

        class CreateISystemScript : EndNameEditAction
        {

            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                //创建资源
                UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
            }
    
            internal static UnityEngine.Object CreateScriptAssetFromTemplate(string filePath, string resourceFile)
            {
                string scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Transforms;
using FGUFW.Entities;

namespace RogueGamePlay
{
    [BurstCompile]
    partial struct |CLASS_NAME| : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQuery eq = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>().Build(ref state);
            state.RequireForUpdate(eq);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new Job
            {
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        partial struct Job:IJobEntity
        {
            void Execute([ChunkIndexInQuery] int chunkInQueryIndex,Entity entity)
            {
                
            }
        }
    }
}
";
                var className = Path.GetFileName(filePath).Replace(".cs","");
                scriptText = scriptText.Replace("|CLASS_NAME|",className);
                File.WriteAllText(filePath,scriptText,Encoding.UTF8);
                //刷新资源管理器
                AssetDatabase.ImportAsset(filePath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(filePath, typeof(UnityEngine.Object));
            }
        }
    }
}
#endif