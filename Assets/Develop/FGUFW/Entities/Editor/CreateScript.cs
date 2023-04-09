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