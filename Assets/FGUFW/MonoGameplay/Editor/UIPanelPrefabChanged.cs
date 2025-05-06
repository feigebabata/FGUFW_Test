using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FGUFW;

namespace FGUFW.MonoGameplay.Editor
{
/*
为避免UIPanel加载时显示在游戏中 预制件的Canvas需要默认隐藏
在预制件打开的时候自动激活Canvas
*/
    public static class UIPanelPrefabChanged
    {
        [InitializeOnLoadMethod]
        static void initialized()
        {
            PrefabStage.prefabStageOpened -= prefabStageOpened;
            PrefabStage.prefabStageOpened += prefabStageOpened;
            
            PrefabStage.prefabStageClosing -= prefabStageClosing;
            PrefabStage.prefabStageClosing += prefabStageClosing;

        }


        private static void prefabStageClosing(PrefabStage stage)
        {
            var gObj = stage.prefabContentsRoot;
            if(gObj.Comp<UIPanel>() == default) return;

            var canvas = gObj.Comp<Canvas>();
            canvas.enabled = false;
            EditorUtility.SetDirty(canvas);
            AssetDatabase.SaveAssetIfDirty(gObj);
        }

        private static void prefabStageOpened(PrefabStage stage)
        {
            
            var gObj = stage.prefabContentsRoot;
            if(gObj.Comp<UIPanel>() == default) return;
            
            gObj.Comp<Canvas>().enabled = true;
        }

    }
}