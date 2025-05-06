# if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using FGUFW;
using FGUFW.EditorUtils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FGUFW.MonoGameplay.Editor
{

    public static class CreateScript
    {
        [MenuItem("Assets/Create/MonoGameplay/Play",false,80)]
        static void createPlay()
        {
            string createPath = EditorUtil.GetSeleceFolderPath()+"/Play.cs";
            var endNameEditAction = ScriptableObject.CreateInstance<CreateScriptHelper>();
            endNameEditAction.Callback = (filePath)=>
            {
                var scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace |NAME_SPACE|
{
    public class |CLASS_NAME| : Play<|CLASS_NAME|>
    {
        public override IEnumerator OnCreating(Part play,Part parent)
        {
            //AddPart<MonoGameplayTestPart>();
            
            yield return base.OnCreating(this,this);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}

";

                var className = Path.GetFileName(filePath).Replace(".cs","");
                scriptText = scriptText.Replace("|CLASS_NAME|",className);

                MonoGameplaySettingsProvider.SettingData.NameSpace = className.Replace("Play","");
                MonoGameplaySettingsProvider.SettingData.PlayName = className;
                MonoGameplaySettingsProvider.SettingData.Save();

                scriptText = scriptText.Replace("|NAME_SPACE|",MonoGameplaySettingsProvider.SettingData.NameSpace);

                return scriptText;
            };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,endNameEditAction,createPath,null,null);
        }

        [MenuItem("Assets/Create/MonoGameplay/Part",false,80)]
        static void createPart()
        {
            string createPath = EditorUtil.GetSeleceFolderPath()+"/Part.cs";
            var endNameEditAction = ScriptableObject.CreateInstance<CreateScriptHelper>();
            endNameEditAction.Callback = (filePath)=>
            {
                var scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace |NAME_SPACE|
{
    
    public class |CLASS_NAME| : Part
    {
        private |PLAY_NAME| _play;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as |PLAY_NAME|;
            addListener();
            yield return base.OnCreating(play,parent);
        }

        protected override void OnDispose()
        {
            removeListener();
            base.OnDispose();
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
                scriptText = scriptText.Replace("|NAME_SPACE|",MonoGameplaySettingsProvider.SettingData.NameSpace);
                scriptText = scriptText.Replace("|PLAY_NAME|",MonoGameplaySettingsProvider.SettingData.PlayName);

                return scriptText;
            };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,endNameEditAction,createPath,null,null);
        }

        [MenuItem("Assets/Create/MonoGameplay/PartFolder",false,80)]
        static void createPartFolder()
        {

            string createPath = EditorUtil.GetSeleceFolderPath()+"/Part";
            var endNameEditAction = ScriptableObject.CreateInstance<CreateFolderHelper>();
            endNameEditAction.Callback = (folderPath)=>
            {
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Directory.CreateDirectory(Path.Combine(folderPath,"Sprite"));
                }
                

                var scriptText = 
@"using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace |NAME_SPACE|
{
    [UIPanelLoader("""")]
    public class |CLASS_NAME| : Part
    {
        private |PLAY_NAME| _play;
        //private |CLASS_NAME|PanelComps _panelComps;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as |PLAY_NAME|;
            yield return base.OnCreating(play,parent);
        }

        public override IEnumerator OnPreload()
        {
            yield return base.OnPreload();
            //_panelComps = _uiPanel.Comp<|CLASS_NAME|PanelComps>();
            addListener();
        }

        protected override void OnDispose()
        {
            removeListener();
            base.OnDispose();
        }

        private void addListener()
        {
            //_panelComps.TryAddAllBtnListener(this);
        }

        private void removeListener()
        {
            //_panelComps.TryRemoveAllBtnListener();
        }

    }
}

";

                var className = Path.GetFileName(folderPath);
                scriptText = scriptText.Replace("|CLASS_NAME|",className);
                scriptText = scriptText.Replace("|NAME_SPACE|",MonoGameplaySettingsProvider.SettingData.NameSpace);
                scriptText = scriptText.Replace("|PLAY_NAME|",MonoGameplaySettingsProvider.SettingData.PlayName);
                File.WriteAllText(Path.Combine(folderPath,className+".cs"),scriptText);

                var panelGO = new GameObject($"{className}Panel");
                var canvas = panelGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var canvasGroup = panelGO.AddComponent<CanvasGroup>();
                panelGO.AddComponent<GraphicRaycaster>();

                var canvasScaler = panelGO.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1080,1920);

                var safeAreaGO = new GameObject("SafeArea");
                safeAreaGO.transform.SetParent(panelGO.transform);
                safeAreaGO.AddComponent<SafeAreaAdapter>();

                PrefabUtility.SaveAsPrefabAsset(panelGO,Path.Combine(folderPath,$"{className}Panel.prefab"));
                GameObject.DestroyImmediate(panelGO);

            };
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,endNameEditAction,createPath,null,null);
        }
    }


}

#endif