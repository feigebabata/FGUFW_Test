#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace FGUFW.GamePlay
{
    static class CreateScript
    {
        const string NAME_SPACE = "RogueGamePlay";

        [MenuItem("Assets/Create/GamePlay/Play",false,80)]
        static void createPlay()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Play.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreatePlayScript>(),createPath,null,null);
        }

        class CreatePlayScript : EndNameEditAction
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
using FGUFW.GamePlay;
using FGUFW;
using System.Threading.Tasks;

namespace |NAME_SPACE|
{
    public class |CLASS_NAME| : Play<|CLASS_NAME|>
    {
        public override async Task OnCreating(IPart parent)
        {
            await AssetHelper.LoadSceneAsync(null);
            //code
            SubParts.Add(new GameEnter());
            _ = base.OnCreating(parent);
        }

        public override Task OnDestroying(IPart parent)
        {
            return base.OnDestroying(parent);
            //code

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
        

        [MenuItem("Assets/Create/GamePlay/Part",false,80)]
        static void createPart()
        {
            string createPath = EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Part.cs";
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CreatePlayScript>(),createPath,null,null);
        }

        class CreatePartScript : EndNameEditAction
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
using FGUFW.GamePlay;
using FGUFW;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

namespace |NAME_SPACE|
{
    public class |CLASS_NAME| : IPart
    {
        public IList<IPart> SubParts{get;set;}=new List<IPart>();

        public Task OnCreating(IPart parent)
        {
            //code

            addListener();
            return null;
        }

        public Task OnDestroying(IPart parent)
        {
            removeListener();
            return null;
        }

        private void addListener()
        {
            
        }

        private void removeListener()
        {
            
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

    }
}
#endif