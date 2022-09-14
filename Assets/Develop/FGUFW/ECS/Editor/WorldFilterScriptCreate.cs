using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace FGUFW.ECS
{
    public static class WorldFilterScriptCreate
    {
        const string scriptPath = "Assets/Develop/FGUFW/ECS/WorldFilter.cs";
        
        public static void CreateScript(int count)
        {
            File.WriteAllText(Application.dataPath.Replace("Assets",scriptPath),generate(count));
        }

        private static string generate(int count)
        {
            string functions = "";
            string delegates = "";
            for (int length = 1; length <= count; length++)
            {
                string func = "";
                string types = "";
                string types_where = "";
                string types_val = "";
                string types_return = "";
                string comp_dicts = "";
                string types_min = "";
                string types_comp = "";
                string callback = "";
                string setComps = "";

                string ref_types = "";

                for (int i = 0; i < length; i++)
                {
                    if(i==0)
                    {
                        types += $"T{i}";
                        ref_types += $"ref T{i} t{i}";
                        types_where += $"where T{i}:struct,IComponent";
                        types_val += $"var t{i}_tVal = ComponentTypeHelper.GetTypeValue<T{i}>();";
                        types_return += $"!_compDict.ContainsKey(t{i}_tVal) || _compDict[t{i}_tVal].Count==0";
                        comp_dicts += $"Dictionary<int, T{i}> t{i}_dict = _compDict[t{i}_tVal] as Dictionary<int, T{i}>;";
                        types_min += $"if(t{i}_dict.Count<minCount){{minCount = t{i}_dict.Count;minCountEntityUIds = t{i}_dict.Keys;}}";
                        types_comp += $"T{i} t{i}_comp;if(!t{i}_dict.TryGetValue(entityUId,out t{i}_comp)) continue;";
                        callback += $"ref t{i}_comp";
                        setComps += $"if(t{i}_comp.Dirty>0)t{i}_dict[entityUId]=t{i}_comp;";
                    }
                    else
                    {
                        types += $",T{i}";
                        ref_types += $",ref T{i} t{i}";
                        types_where += $"\n        where T{i}:struct,IComponent";
                        types_val += $"\n            var t{i}_tVal = ComponentTypeHelper.GetTypeValue<T{i}>();";
                        types_return += $"\n                || !_compDict.ContainsKey(t{i}_tVal) || _compDict[t{i}_tVal].Count==0";
                        comp_dicts += $"\n            Dictionary<int, T{i}> t{i}_dict = _compDict[t{i}_tVal] as Dictionary<int, T{i}>;";
                        types_min += $"\n            if(t{i}_dict.Count<minCount){{minCount = t{i}_dict.Count;minCountEntityUIds = t{i}_dict.Keys;}}";
                        types_comp += $"\n                T{i} t{i}_comp;if(!t{i}_dict.TryGetValue(entityUId,out t{i}_comp)) continue;";
                        callback += $",ref t{i}_comp";
                        setComps += $"\n                if(t{i}_comp.Dirty>0)t{i}_dict[entityUId]=t{i}_comp;";
                    }
                }

                func = function.Replace("#TYPES#",types);
                func = func.Replace("#COUNT#",length.ToString());
                func = func.Replace("#TYPES_WHERE#",types_where);
                func = func.Replace("#TYPES_VAL#",types_val);
                func = func.Replace("#TYPES_RETURN#",types_return);
                func = func.Replace("#COMP_DICTS#",comp_dicts);
                func = func.Replace("#TYPES_MIN#",types_min);
                func = func.Replace("#TYPES_COMP#",types_comp);
                func = func.Replace("#CALLBACK#",callback);
                func = func.Replace("#SET_COMPS#",setComps);

                functions +=$"\n{func}";
                delegates +=$"\n    public delegate void WorldFilterDelegate_R{length}<{types}>({ref_types});";
            }
            var scriptText = script.Replace("#DELEGATES#",delegates);
            return scriptText.Replace("#FUNCTIONS#",functions);
        }

        const string function = @"
        public void Filter<#TYPES#>(WorldFilterDelegate_R#COUNT#<#TYPES#> callback)
        #TYPES_WHERE#
        {
            #TYPES_VAL#

            if
            (
                #TYPES_RETURN#
            )return;

            #COMP_DICTS#

            ICollection<int> minCountEntityUIds = null;
            int minCount = int.MaxValue;

            #TYPES_MIN#

            foreach (var entityUId in minCountEntityUIds)
            {
                #TYPES_COMP#

                callback(#CALLBACK#);

                #SET_COMPS#
            }
        }     
";

        const string script = @"
using System.Collections.Generic;

namespace FGUFW.ECS
{
    #DELEGATES#

    public partial class World
    {
        #FUNCTIONS#
    }
}
";


    }
}