using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace FGUFW.ECS
{
    public static class WorldFilterScriptCreate
    {
        const string scriptPath = "Assets/Develop/FGUFW/ECS/WorldFilter.cs";
        
        [MenuItem("ECS/创建组件筛选脚本")]
        private static void createScript()
        {
            File.WriteAllText(Application.dataPath.Replace("Assets",scriptPath),generate(8));
        }

        private static string generate(int count)
        {
            string functions = "";
            for (int length = 1; length <= count; length++)
            {
                string func = "";
                string types = "";
                string types_where = "";
                string types_val = "";
                string types_return = "";
                string types_min = "";
                string types_comp = "";
                string callback = "";

                for (int i = 0; i < length; i++)
                {
                    if(i==0)
                    {
                        types += $"T{i}";
                        types_where += $"where T{i}:Component";
                        types_val += $"var t{i}_tVal = ComponentTypeHelper.GetTypeValue<T{i}>();";
                        types_return += $"!_compDict.ContainsKey(t{i}_tVal) || _compDict[t{i}_tVal].Count==0";
                        types_min += @$"if(_compDict[t{i}_tVal].Count<minCount)
            {{
                minCount = _compDict[t{i}_tVal].Count;
                minCountType = t{i}_tVal;
            }}";
                        types_comp += $"var t{i}_comp = GetComponent<T{i}>(entityUId);if(t{i}_comp==null)continue;";
                        callback += $"t{i}_comp";
                    }
                    else
                    {
                        types += $",T{i}";
                        types_where += $"\n        where T{i}:Component";
                        types_val += $"\n            var t{i}_tVal = ComponentTypeHelper.GetTypeValue<T{i}>();";
                        types_return += $"\n                || !_compDict.ContainsKey(t{i}_tVal) || _compDict[t{i}_tVal].Count==0";
                        types_min += @$"
            if(_compDict[t{i}_tVal].Count<minCount)
            {{
                minCount = _compDict[t{i}_tVal].Count;
                minCountType = t{i}_tVal;
            }}";
                        types_comp += $"\n                var t{i}_comp = GetComponent<T{i}>(entityUId);if(t{i}_comp==null)continue;";
                        callback += $",t{i}_comp";
                    }
                }

                func = function.Replace("#TYPES#",types);
                func = func.Replace("#TYPES_WHERE#",types_where);
                func = func.Replace("#TYPES_VAL#",types_val);
                func = func.Replace("#TYPES_RETURN#",types_return);
                func = func.Replace("#TYPES_MIN#",types_min);
                func = func.Replace("#TYPES_COMP#",types_comp);
                func = func.Replace("#CALLBACK#",callback);

                functions +=$"\n{func}";
            }

            return script.Replace("#FUNCTIONS#",functions);
        }

        const string function = @"
        public void Filter<#TYPES#>(Action<#TYPES#> callback)
        #TYPES_WHERE#
        {
            #TYPES_VAL#

            if
            (
                #TYPES_RETURN#
            )return;

            int minCountType = -1;
            int minCount = int.MaxValue;

            #TYPES_MIN#

            var minComps = _compDict[minCountType];
            
            foreach (var item in minComps)
            {
                var entityUId = item.EntityUId;

                #TYPES_COMP#

                callback(#CALLBACK#);

            }
        }     
";

        const string script = @"
using System;
using FGUFW;

namespace FGUFW.ECS
{
    public partial class World
    {
        #FUNCTIONS#
    }
}
";


    }
}