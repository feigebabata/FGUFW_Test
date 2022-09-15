using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FGUFW.ECS
{
    public static class CompAndSysScriptCreator
    {
        const string configPath = "Assets/Develop/FGUFW/ECS/Editor/config.json";

        private static Config compAndSysconfig;

        public class Config
        {
            public int CompTypeIndex = 1;
            public int SysTypeIndex = 1;
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

        public static void CreateCompScript(string folderPath,string nameSpace,List<string> names,int compTypeIndex)
        {
            foreach (var item in names)
            {
                createCompScript(folderPath,nameSpace,item,compTypeIndex++);
            }
            var config = GetConfig();
            config.CompTypeIndex = compTypeIndex;
            config.PrevNamespace = nameSpace;
            SaveConfig();
            AssetDatabase.Refresh();
        }

        private static void createCompScript(string folderPath,string nameSpace,string name,int compTypeIndex)
        {
            var text = compScript.Replace("#NAMESPACE#",nameSpace);
            text = text.Replace("#NAME#",name);
            text = text.Replace("#COMPTYPE#",compTypeIndex.ToString());

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

        public static void CreateSysScript(string folderPath,string nameSpace,string name,int order,List<string> typeNames)
        {
            var text = sysScript.Replace("#NAMESPACE#",nameSpace);
            text = text.Replace("#NAME#",name);
            text = text.Replace("#ORDER#",order.ToString());

            string path = $"{Application.dataPath.Replace("Assets",folderPath)}/{name}.cs";
            File.WriteAllText(path,text);
            Debug.Log($"生成系统 {order} {path}");
            
            SaveConfig();
            AssetDatabase.ImportAsset($"{folderPath}/{name}.cs");
            AssetDatabase.Refresh();
        }

        const string sysScript = @"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

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
            //IComponent.Dirty > 0 才会修改源数据
            #Filter#
        }

        public void Dispose()
        {
            _world = null;
        }

    }
}
";


        const string compScript = @"
using FGUFW.ECS;
using Unity.Mathematics;
using Unity.Collections;

namespace #NAMESPACE#
{
    public struct #NAME# : IComponent
    {
        #region 不可修改
        public int CompType => #COMPTYPE#;
        public int EntityUId { get; set; }
        public int Dirty { get; set; }
        public bool IsCreated { get; private set; }
        #endregion
        //code


        public #NAME#(int entityUId)
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