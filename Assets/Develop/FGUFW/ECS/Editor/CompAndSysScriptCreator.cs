using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using FGUFW.ECS;

namespace FGUFW.ECS.Editor
{
    public static class CompAndSysScriptCreator
    {
        const string configPath = "Assets/Develop/FGUFW/ECS/Editor/config.json";

        private static Config compAndSysconfig;

        public class Config
        {
            public int CompTypeIndex = 1;
            public string PrevNamespace = "TempNameSpace";
            public int PrevPanel = 1;
        }

        public static void SaveConfig()
        {
            if(compAndSysconfig==null)return;
            var path = Application.dataPath.Replace("Assets",configPath);
            var json = JsonUtility.ToJson(compAndSysconfig);
            File.WriteAllText(path,json);
        }

        public static Config GetConfig()
        {
            if(compAndSysconfig==null)loadConfig();
            return compAndSysconfig;
        }


        private static void loadConfig()
        {
            var path = Application.dataPath.Replace("Assets",configPath);
            if(File.Exists(path))
            {
                var json = File.ReadAllText(path);
                compAndSysconfig = JsonUtility.FromJson<Config>(json);
            }
            else
            {
                compAndSysconfig = new Config();
            }
        }

        public static void CreateCompScript(string folderPath,string nameSpace,List<string> names,int compTypeIndex,bool useMonoComp)
        {
            foreach (var item in names)
            {
                createCompScript(folderPath,nameSpace,item,compTypeIndex++, useMonoComp);
            }
            var config = GetConfig();
            config.CompTypeIndex = compTypeIndex;
            config.PrevNamespace = nameSpace;
            config.PrevPanel = 1;
            SaveConfig();
            AssetDatabase.Refresh();
        }

        private static void createCompScript(string folderPath,string nameSpace,string name,int compTypeIndex, bool useMonoComp)
        {
            var text = compScript.Replace("#NAMESPACE#",nameSpace);
            text = text.Replace("#NAME#",name);
            text = text.Replace("#COMPTYPE#",compTypeIndex.ToString());
            text = text.Replace("#ATTRIBUTE#", useMonoComp? "[GenerateAuthoringComponent]":"");

            string path = $"{Application.dataPath.Replace("Assets",folderPath)}/{name}.cs";
            File.WriteAllText(path,text);
            Debug.Log($"生成组件 {compTypeIndex} {path}");
            
            AssetDatabase.ImportAsset($"{folderPath}/{name}.cs");
        }

        [MenuItem("ECS/组件类型查重")]
        private static void checkCompTypes()
        {
            ComponentTypeHelper.CheckCompType();
        }

        public static void CreateSysScript(string folderPath,string nameSpace,string name,int order,List<string> typeNames,bool useJob)
        {
            var text = sysScript.Replace("#NAMESPACE#",nameSpace);
            text = text.Replace("#NAME#",name);
            text = text.Replace("#ORDER#",order.ToString());

            string filter = "";
            string jobStruct = "";
            if (typeNames.Count>0)
            {
                if(useJob)
                {
                    string firstComp = null;
                    var setJob = new string[typeNames.Count+1];

                    var jobComps = new string[typeNames.Count+1];
                    var compGets = new string[typeNames.Count];
                    var compSets = new string[typeNames.Count];

                    for (int i = 0; i < typeNames.Count; i++)
                    {
                        var typeName = typeNames[i];
                        var minTypeName = $"{typeName.Substring(0,1).ToLower()}{typeName.Substring(1)}";
                        if(i==0)firstComp = $"{minTypeName}s";
                        typeNames[i] = $"ref NativeArray<{typeName}> {minTypeName}s";
                        setJob[i] = $"{typeName}s = {minTypeName}s";

                        jobComps[i] = $"public NativeArray<{typeName}> {typeName}s;";
                        compGets[i] = $"var {minTypeName} = {typeName}s[index];";
                        compSets[i] = $"{typeName}s[index] = {minTypeName};";
                    }
                    jobComps[jobComps.Length-1] = "public float DeltaTime;";
                    setJob[setJob.Length-1] = "DeltaTime = _world.DeltaTime";

                    filter = filterJobScript.Replace("#TYPES_JOB#", string.Join(",", typeNames));
                    filter = filter.Replace("#SET_JOB#", string.Join(",\n                    ", setJob));
                    filter = filter.Replace("#FIRST_COMPS#", firstComp);

                    jobStruct = jobStructScript.Replace("#JOB_COMPS#", string.Join("\n            ", jobComps));
                    jobStruct = jobStruct.Replace("#COMP_GET#", string.Join("\n                ", compGets));
                    jobStruct = jobStruct.Replace("#COMP_SET#", string.Join("\n                ", compSets));

                }
                else
                {
                    for (int i = 0; i < typeNames.Count; i++)
                    {
                        var typeName = typeNames[i];
                        var minTypeName = $"{typeName.Substring(0, 1).ToLower()}{typeName.Substring(1)}";

                        typeNames[i] = $"ref {typeName} {minTypeName}";
                    }
                    filter = filterScript.Replace("#TYPES#", string.Join(",", typeNames));
                }
            }
            text = text.Replace("#FILTER#", filter);
            text = text.Replace("#JOB_STURCT#", jobStruct);

            string path = $"{Application.dataPath.Replace("Assets",folderPath)}/{name}.cs";
            File.WriteAllText(path,text);
            Debug.Log($"生成系统 {order} {path}");

            var config = GetConfig();
            config.PrevNamespace = nameSpace;
            config.PrevPanel = 2;
            SaveConfig();
            AssetDatabase.ImportAsset($"{folderPath}/{name}.cs");
            AssetDatabase.Refresh();
        }

        const string jobStructScript = @"
        public struct Job : IJobParallelFor
        {
            #JOB_COMPS#

            public void Execute(int index)
            {
                #COMP_GET#
                //code
                
                
                #COMP_SET#
            }

        }
";

        const string filterJobScript = @"
            _world.FilterJob((#TYPES_JOB#)=>
            {
                int length = #FIRST_COMPS#.Length;
                var job = new Job
                {
                    #SET_JOB#
                };
                job.Run(length);
                //code

            });
";

        const string filterScript = @"
            _world.Filter((#TYPES#)=>
            {
                
            });
";

        const string sysScript = @"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

namespace #NAMESPACE#
{
    public class #NAME# : ISystem
    {
        public int Order => #ORDER#;

        private World _world;

        public void OnInit(World world)
        {
            _world = world;
        }

        public void OnUpdate()
        {
            //IComponent.Dirty > 0 才会修改源数据#FILTER#

        }

        public void Dispose()
        {
            _world = null;
        }

        #JOB_STURCT#

    }
}
";


        const string compScript = @"
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace #NAMESPACE#
{
    #ATTRIBUTE#
    public struct #NAME# : IComponent
    {
        #region 不可修改
        public int CompType => #COMPTYPE#;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code


        public #NAME#(int entityUId=0)
        {
            #region 不可修改
            EntityUId = entityUId;
            Dirty = 0;
            IsCreated = true;
            #endregion
            //code
            
        }

        public void Dispose()
        {
            #region 不可修改
            IsCreated = false;
            EntityUId = 0;
            Dirty = 0;
            #endregion
            //code
            
        }
    }
}
";

    }
}