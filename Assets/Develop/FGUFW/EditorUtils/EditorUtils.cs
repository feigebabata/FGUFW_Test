using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using FGUFW.Platform;
using FGUFW;

namespace FGUFW.EditorUtils
{
    public static class EditorUtils
    {
        public const string META = ".meta";
        public static string[] GetAllAssetPath(string dirPath)
        {
            var dir = Application.dataPath.Replace("Assets",dirPath);
            if(!Directory.Exists(dir))return new string[0];
            int length = 0;
            var flies = Directory.GetFiles(dir);
            foreach (var path in flies)
            {
                if(Path.GetExtension(path)==META)continue;
                length++;
            }
            var paths = new string[length];
            int index = 0;
            foreach (var path in flies)
            {
                if(Path.GetExtension(path)==META)continue;
                paths[index] = path.Replace(Application.dataPath,"Assets").Replace("\\","/");
                index++;
            }
            return paths;
        }

        
        [MenuItem("Assets/PrintAssetPath")]
        static private void printAssetPath()
        {
            Debug.Log(AssetDatabase.GetAssetPath(Selection.activeObject));
        }

        [MenuItem("文件夹/程序持续存储文件夹")]
        static void openDir()
        {
            WinPlatform.OpenExplorer(Application.persistentDataPath);
        }

    }
}
