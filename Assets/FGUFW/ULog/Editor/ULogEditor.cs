using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using FGUFW.EditorUtils.Editor;
using System.Linq;

namespace FGUFW.ULogEditor
{
    public static class ULogEditor
    {
        const string logEnabled = "Conditional/ULog enabled"; 

        [MenuItem(logEnabled)]  
        public static void ULogEnable()  
        {  
            bool enabled = Menu.GetChecked(logEnabled);  

            var defines = EditorUtil.GetScriptingDefineSymbols().ToList();

            if(!enabled)
            {
                defines.Add(ULog.Conditional_Log);
                defines.Add("UNITY_ASSERTIONS");
            }
            else
            {

                defines.Remove(ULog.Conditional_Log);
                defines.Remove("UNITY_ASSERTIONS");
            }

            EditorUtil.SetScriptingDefineSymbols(defines.ToArray());

            Menu.SetChecked(logEnabled, !enabled);  
        }  

        [MenuItem(logEnabled,true)]  
        public static bool MenuLogOutCheck()//先刷新状态
        {  
            string[] defines = EditorUtil.GetScriptingDefineSymbols();

            var enabled = defines.IndexOf(ULog.Conditional_Log) != -1;

            Menu.SetChecked(logEnabled, enabled);  
            return true;  
        }

    }
}