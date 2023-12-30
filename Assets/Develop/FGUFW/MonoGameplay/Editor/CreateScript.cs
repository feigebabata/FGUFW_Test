# if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using FGUFW;
using FGUFW.EditorUtils;
using UnityEditor;
using UnityEngine;

namespace FGUFW.MonoGameplay
{

    public static class CreateScript
    {
        [MenuItem("Assets/Create/MonoGameplay/Play",false,80)]
        static void createPlay()
        {
            string createPath = FGUFW.EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Play.cs";
            var endNameEditAction = ScriptableObject.CreateInstance<CreateScriptHelper>();
            endNameEditAction.Callback = (filePath)=>
            {
                var scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace |NAME_SPACE|
{
    public class |CLASS_NAME| : Play<|CLASS_NAME|>
    {
        public override UniTask OnCreating(Part parent)
        {
            //AddPart<MonoGameplayTestPart>();
            
            return base.OnCreating(parent);
        }

        public override UniTask OnDestroying(Part parent)
        {

            return base.OnDestroying(parent);
        }
    }
}

";

                var className = Path.GetFileName(filePath).Replace(".cs","");
                scriptText = scriptText.Replace("|CLASS_NAME|",className);

                MonoGameplaySettings.instance.NameSpace = className.Replace("Play","");
                MonoGameplaySettings.instance.PlayName = className;
                MonoGameplaySettings.instance.Save();

                scriptText = scriptText.Replace("|NAME_SPACE|",MonoGameplaySettings.instance.NameSpace);

                return scriptText;
            };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,endNameEditAction,createPath,null,null);
        }

        [MenuItem("Assets/Create/MonoGameplay/Part",false,80)]
        static void createPart()
        {
            string createPath = FGUFW.EditorUtils.EditorUtils.GetSeleceFolderPath()+"/Part.cs";
            var endNameEditAction = ScriptableObject.CreateInstance<CreateScriptHelper>();
            endNameEditAction.Callback = (filePath)=>
            {
                var scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace |NAME_SPACE|
{
    
    public class |CLASS_NAME| : Part
    {
        private |PLAY_NAME| _play;

        public override UniTask OnCreating(Part parent)
        {
            _play = |PLAY_NAME|.I;

            addListener();
            return base.OnCreating(parent);
        }

        public override UniTask OnDestroying(Part parent)
        {

            removeListener();
            return base.OnDestroying(parent);
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
                scriptText = scriptText.Replace("|NAME_SPACE|",MonoGameplaySettings.instance.NameSpace);
                scriptText = scriptText.Replace("|PLAY_NAME|",MonoGameplaySettings.instance.PlayName);

                return scriptText;
            };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,endNameEditAction,createPath,null,null);
        }
    }


}

#endif