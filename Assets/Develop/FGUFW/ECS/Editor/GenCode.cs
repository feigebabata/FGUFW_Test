#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System;
using FGUFW;
using FGUFW.ECS;
using System.IO;

namespace FGUFW.ECS.Editor
{
    public static class GenCode
    {
        const string GEN_CODE_DIR = "Assets/Develop/ECSGenCode";


        [MenuItem("ECS/生成被标记组件的Mono脚本")]
        private static void compilationFinished()
        {

            AssemblyHelper.FilterClassAndStruct<IConvertToComponent>(t =>
            {
                deleteType(t);
            });

            AssemblyHelper.FilterClassAndStruct<IComponent>(t =>
            {
                checkType(t);
            });


        }

        private static void deleteType(Type t)
        {
            var originType = getOriginType(t);
            if (originType != null && existsGenAttribute(originType)) return;

            var filePath = getOriginScriptPath(t);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                AssetDatabase.Refresh();
            }
        }

        private static void checkType(Type t)
        {
            if (!existsGenAttribute(t)) return;
            var genType = getAuthoringType(t);
            if (genType != null && genType.EqualsFileds(t)) return;

            var script = getAuthoringScript(t);
            var filePath = getAuthoringScriptPath(t);
            var dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }

        private static bool existsGenAttribute(Type t)
        {
            // foreach (var item in t.GetCustomAttributesData())
            // {
            //     if (item.AttributeType == typeof(GenerateAuthoringComponentAttribute)) return true;
            // }
            // return false;
            return t.ContainsAttribute<GenerateAuthoringComponentAttribute>();
        }

        private static bool existsAuthoring(Type t)
        {
            return File.Exists(getAuthoringScriptPath(t));
        }

        private static Type getAuthoringType(Type t)
        {
            var fullName = $"{t.FullName}Authoring";
            return Type.GetType(fullName);
        }

        private static Type getOriginType(Type t)
        {
            var fullName = t.FullName.Replace("Authoring","");
            return Type.GetType(fullName);
        }

        private static string getAuthoringScriptPath(Type t)
        {
            return $"{Application.dataPath.Replace("Assets", GEN_CODE_DIR)}/{t.Namespace}/{t.Name}Authoring.cs";
        }

        private static string getOriginScriptPath(Type t)
        {
            return $"{Application.dataPath.Replace("Assets", GEN_CODE_DIR)}/{t.Name}.cs";
        }

        private static string getAuthoringScript(Type t)
        {
            string script = "";
            var nameSpace = t.Namespace;
            var name = t.Name;

            var fileds = t.GetFields();
            var length = fileds.Length;
            string[] fieldls = new string[length];
            string[] setFields = new string[length];
            for (int i = 0; i < length; i++)
            {
                var filed = fileds[i];
                fieldls[i] = filed.ToScript();
                setFields[i] = $"comp.{filed.Name} = this.{filed.Name};";
            }
            script = compAuthoringScript.Replace("|NAMESPACE|",nameSpace);
            script = script.Replace("|NAME|", name);
            script = script.Replace("|FILEDS|", string.Join("\n        ", fieldls));
            script = script.Replace("|SET_FILEDS|", string.Join("\n            ", setFields));

            return script;
        }

        const string compAuthoringScript = @"
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using FGUFW.ECS;

namespace |NAMESPACE|
{
    [DisallowMultipleComponent]
    public class |NAME|Authoring:MonoBehaviour, IConvertToComponent
    {
        |FILEDS|

        public void Convert(World world, int entityUId)
        {
            var comp = new |NAME|(entityUId);

            |SET_FILEDS|

            world.AddOrSetComponent(entityUId , comp);
        }
    }
}
";

    }
}
#endif