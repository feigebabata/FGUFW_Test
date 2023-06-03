#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace FGUFW.SimpleECS
{
    static class CreateScript
    {
        const string NAME_SPACE = "FGUFW.SimpleECS.Test";
        const string WORLD_NAME = "TestWorld";
        
        [MenuItem("Assets/Create/SimpleECS Script/Component",false,80)]
        static void createComponent()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Component.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateComponentScript>(),createPath,null,null);
        }

        [MenuItem("Assets/Create/SimpleECS Script/System",false,80)]
        static void createSystem()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/System.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateISystemScript>(),createPath,null,null);
        }
        
        [MenuItem("Assets/Create/SimpleECS Script/ComponentGroup",false,80)]
        static void createEventSystem()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/ComponentGroup.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreateComponentGroupScript>(),createPath,null,null);
        }

        

        class CreateComponentGroupScript : EndNameEditAction
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
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;
using FGUFW.SimpleECS;

namespace |NAME_SPACE|
{

    public class |CLASS_NAME| : ComponentGroup
    {

        public |CLASS_NAME|(GameObject prefab, int capacity = 64) : base(prefab, capacity)
        {
            if(capacity<0)capacity=64;
        }

        protected override void OnDispose()
        {
        }

        protected override void OnDestroyEntity(int entityIndex)
        {
        }

        protected override void OnInstantiate(int id,Transform t)
        {
            t.position = Vector3.zero;
        }

    }
}
";
                var className = Path.GetFileName(filePath).Replace(".cs","");

                scriptText = scriptText.Replace("|CLASS_NAME|",className);

                scriptText = scriptText.Replace("|NAME_SPACE|",NAME_SPACE);

                File.WriteAllText(filePath,scriptText,Encoding.UTF8);
                //刷新资源管理器
                AssetDatabase.ImportAsset(filePath);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(filePath, typeof(UnityEngine.Object));
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
@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;

namespace |NAME_SPACE|
{
    [DisallowMultipleComponent]
    public class |CLASS_NAME|Authoring : MonoBehaviour,IAuthoring<|CLASS_NAME|>
    {
        public |CLASS_NAME| Convert()
        {
            var data = new |CLASS_NAME|
            {
                Enabled = this.enabled,
            };
            GameObject.Destroy(this);
            return data;
        }

        void Start(){}
    }

    public struct |CLASS_NAME| : IComponent
    {
        public bool Enabled {get;set;}
    }    
    
}

";
                var className = Path.GetFileName(filePath).Replace(".cs","");

                scriptText = scriptText.Replace("|CLASS_NAME|",className);

                scriptText = scriptText.Replace("|NAME_SPACE|",NAME_SPACE);

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
@"using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using System;
using FGUFW;
using FGUFW.SimpleECS;

namespace |NAME_SPACE|
{

    public struct |CLASS_NAME| : ISystem<|WORLD_NAME|>
    {
        public void OnUpdate(|WORLD_NAME| world, int systemIndex)
        {
            world.Dependency = new Job
            {
                DeltaTime = world.DeltaTime,
            }
            .Schedule(default,world.Dependency);
        }

        private struct Job : IJobParallelForTransform
        {
            public float DeltaTime;

            public void Execute(int index, TransformAccess transform)
            {

            }
        }

    }
}
";
                var className = Path.GetFileName(filePath).Replace(".cs","");

                scriptText = scriptText.Replace("|CLASS_NAME|",className);

                scriptText = scriptText.Replace("|NAME_SPACE|",NAME_SPACE);

                scriptText = scriptText.Replace("|WORLD_NAME|",WORLD_NAME);

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