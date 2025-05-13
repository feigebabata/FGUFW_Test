using System;
using System.Collections.Generic;
using System.IO;
using FGUFW.Platform;
using UnityEditor;
using UnityEngine;
using static FGUsing;

namespace FGUFW.EditorUtils.Editor
{
    public static class OpenFolder
    {
        [MenuItem("文件夹/持续存储地址")]
        static void openPersistentDataPath()
        {
            Open(Application.persistentDataPath);
        }

        [MenuItem("文件夹/项目根目录")]
        static void openDataPath()
        {
            Open(Application.dataPath);
        }


        public static void Open(string path)
        {
            #if UNITY_EDITOR_WIN
            WinPlatform.OpenExplorer(path);
            #endif
        }

    }
}