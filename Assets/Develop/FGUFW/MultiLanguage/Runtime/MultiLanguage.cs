using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using FGUFW;

namespace FGUFW.MultiLanguage
{
    
    public static class MultiLanguage
    {
        
        static private IReadOnlyDictionary<string, IReadOnlyList<string>> languageConfig;
        
        public static int LanguageIndex{get; private set;}
        public static Action OnLanguageChanged;
        public static MultiLanguangeFont MultiLanguangeFonts;

        public static void InitConfig()
        {
            var textAsset = AssetHelper.Load<TextAsset>("FGUFW/MultiLanguageConfig");
            languageConfig = textAsset.text.ToCsvDict();
            MultiLanguangeFonts = AssetHelper.Load<MultiLanguangeFont>("Assets/Develop/FGUFW/MultiLanguage/MultiLanguangeFonts.asset");
        }

        public static Font GetMultiLanguangeFont()
        {
            if(MultiLanguangeFonts==null)
            {
                #if UNITY_EDITOR
                    MultiLanguangeFonts = UnityEditor.AssetDatabase.LoadAssetAtPath<MultiLanguangeFont>("Assets/Develop/FGUFW/MultiLanguage/MultiLanguangeFonts.asset");
                #endif
            }
            return MultiLanguangeFonts.Fonts[LanguageIndex];
        }

        public static IReadOnlyList<string> GetLanguageNames()
        {
            return languageConfig["*"];
        }

        public static string GetLanguageText(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Debug.LogError($"[GetLanguageText]出错.id:{id}");
                return string.Empty;
            }
            string text = null;
            if(languageConfig.ContainsKey(id) && languageConfig[id].Count>LanguageIndex+1)
            {
                text = languageConfig[id][LanguageIndex+1];
            }
            else
            {
                text = id;
                // Debug.LogWarning("多语言无TextID:"+text);
            }
            return text;
        }

        public static void SetLanguage(int index)
        {
            // Debug.Log($"语言切换:{index}:{GetLanguageNames()[index+1]}");
            LanguageIndex = index;
            #if UNITY_EDITOR
            if(UnityEditor.EditorApplication.isPlaying)
            {
                // Worlds.GameMap.GameRecordDatas.RecordDatas.LanguageIndex = index;
                // Worlds.GameMap.GameRecordDatas.Save();
                OnLanguageChanged?.Invoke();
            }
            #else
            // Worlds.GameMap.GameRecordDatas.RecordDatas.LanguageIndex = index;
            // Worlds.GameMap.GameRecordDatas.Save();
            OnLanguageChanged?.Invoke();
            #endif
        }

        /// <summary>
        /// 转当前语言
        /// </summary>
        /// <returns></returns>
        public static string ToCL(this string text)
        {
            return GetLanguageText(text);
        }
    }
}