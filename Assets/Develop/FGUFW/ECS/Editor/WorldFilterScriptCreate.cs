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
            AssetDatabase.ImportAsset(scriptPath);
            AssetDatabase.Refresh();
        }

        private static string generate(int count)
        {
            string filter_fun = "";
            string filter_job_fun = "";
            string createFun = "";
            string delegates = "";

            for (int length = 1; length <= count; length++)
            {
                var func = "";
                var func_job = "";
                string create = "";

                var types = new string[length];
                var types_where = new string[length];
                var types_val = new string[length];
                var types_return = new string[length];
                var comp_dicts = new string[length];
                var types_min = new string[length];
                var types_comp = new string[length];
                var callback = new string[length];
                var setComps = new string[length];
                var instance = new string[length];
                var addComps = new string[length];
                var ref_types = new string[length];

                var types_job = new string[length];
                var types_comp_job = new string[length];
                var callback_job = new string[length];
                var setComp_job = new string[length];
                var comp_job_dispose = new string[length];
                var comp_job_set = new string[length];

                for (int i = 0; i < length; i++)
                {
                    types[i] = $"T{i}";
                    ref_types[i] = $"ref T{i} t{i}";
                    types_where[i] = $"where T{i}:struct,IComponent";
                    types_val[i] = $"var t{i}_tVal = ComponentTypeHelper.GetTypeValue<T{i}>();";
                    types_return[i] = $"!_compDict.ContainsKey(t{i}_tVal) || _compDict[t{i}_tVal].Count==0";
                    comp_dicts[i] = $"Dictionary<int, T{i}> t{i}_dict = _compDict[t{i}_tVal] as Dictionary<int, T{i}>;";
                    types_min[i] = $"if(t{i}_dict.Count<minCount){{minCount = t{i}_dict.Count;minCountEntityUIds = t{i}_dict.Keys;}}";
                    types_comp[i] = $"T{i} t{i}_comp;if(!t{i}_dict.TryGetValue(entityUId,out t{i}_comp)) continue;";
                    callback[i] = $"ref t{i}_comp";
                    setComps[i] = $"if(t{i}_comp.Dirty>0)t{i}_dict[entityUId]=t{i}_comp;";
                    instance[i] = $"var t{i}_comp = (T{i})Activator.CreateInstance(typeof(T{i}),entityUId);";
                    addComps[i] = $"AddOrSetComponent(entityUId,t{i}_comp);";

                    types_job[i] = $"NativeArray<T{i}>";
                    types_comp_job[i] = $"NativeArray<T{i}> t{i}s = new NativeArray<T{i}>(entityCount,Allocator.Temp);";
                    callback_job[i] = $"ref t{i}s";
                    setComp_job[i] = $"if(t{i}s[i].Dirty>0)t{i}_dict[entityUId]=t{i}s[i];";
                    comp_job_dispose[i] = $"t{i}s.Dispose();";
                    comp_job_set[i] = $"t{i}s[i] = t{i}_dict[entityUId];";
                }

                func = filter_fun_Script.Replace("#TYPES#", string.Join(",",types));
                func = func.Replace("#COUNT#",length.ToString());
                func = func.Replace("#TYPES_WHERE#",string.Join("\n        ",types_where));
                func = func.Replace("#TYPES_VAL#",string.Join("\n            ",types_val));
                func = func.Replace("#TYPES_RETURN#",string.Join("\n                ||",types_return));
                func = func.Replace("#COMP_DICTS#",string.Join("\n            ",comp_dicts));
                func = func.Replace("#TYPES_MIN#",string.Join("\n            ",types_min));
                func = func.Replace("#TYPES_COMP#",string.Join("\n                ",types_comp));
                func = func.Replace("#CALLBACK#",string.Join(",",callback));
                func = func.Replace("#SET_COMPS#",string.Join("\n                ",setComps));

                create = create_fun_Script.Replace("#TYPES#", string.Join(",",types));
                create = create.Replace("#TYPES_WHERE#", string.Join("\n        ",types_where));
                create = create.Replace("#INSTANCE#", string.Join("\n            ",instance));
                create = create.Replace("#ADD_COMP#", string.Join("\n            ",addComps));
                create = create.Replace("#COUNT#", length.ToString());
                create = create.Replace("#CALLBACK#", string.Join(",",callback));

                func_job = filterJob_fun_script.Replace("#TYPES#", string.Join(",",types));
                func_job = func_job.Replace("#TYPES_JOB#",string.Join(",",types_job));
                func_job = func_job.Replace("#TYPES_WHERE#",string.Join("\n        ",types_where));
                func_job = func_job.Replace("#TYPES_VAL#",string.Join("\n            ",types_val));
                func_job = func_job.Replace("#TYPES_RETURN#",string.Join("\n                ||",types_return));
                func_job = func_job.Replace("#COMP_DICTS#",string.Join("\n            ",comp_dicts));
                func_job = func_job.Replace("#TYPES_MIN#",string.Join("\n            ",types_min));
                func_job = func_job.Replace("#TYPES_COMP#",string.Join("\n                ",types_comp));
                func_job = func_job.Replace("#CALLBACK_JOB#",string.Join(",",callback_job));
                func_job = func_job.Replace("#SET_COMPS_JOB#",string.Join("\n                ",setComp_job));
                func_job = func_job.Replace("#TYPES_COMP_JOB#",string.Join("\n            ",types_comp_job));
                func_job = func_job.Replace("#TYPES_COMP_JOB_DISPOSE#",string.Join("\n            ",comp_job_dispose));
                func_job = func_job.Replace("#COMPS_JOB_SET#",string.Join("\n                ",comp_job_set));
                func_job = func_job.Replace("#COUNT#",length.ToString());


                filter_fun +=$"\n{func}";
                filter_job_fun +=$"\n{func_job}";
                createFun += $"\n{create}";
                delegates +=$"\n    public delegate void WorldFilterDelegate_R{length}<{string.Join(",",types)}>({string.Join(",",ref_types)});";
            }
            var scriptText = script.Replace("#DELEGATES#",delegates);
            scriptText = scriptText.Replace("#CREATE_FUN#", createFun);
            scriptText = scriptText.Replace("#FILTER_JOB_FUN#", filter_job_fun);
            return scriptText.Replace("#FILTER_FUN#", filter_fun);
        }

        const string filterJob_fun_script = @"
        public void FilterJob<#TYPES#>(WorldFilterDelegate_R#COUNT#<#TYPES_JOB#> callback)
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

            setFilterKeyCache(minCountEntityUIds);

            int entityCount = 0;

            for(int i=0 ; i<_filterKeyCacheLength ; i++)
            {
                var entityUId = _filterKeyCache[i];
                
                #TYPES_COMP#

                _filterKeyCache[entityCount++] = entityUId;
            }

            if(entityCount==0)return;

            #TYPES_COMP_JOB#

            for (int i = 0; i < entityCount; i++)
            {
                var entityUId = _filterKeyCache[i];

                #COMPS_JOB_SET#
            }

            callback(#CALLBACK_JOB#);

            for (int i = 0; i < entityCount; i++)
            {
                int entityUId = t0s[i].EntityUId;

                #SET_COMPS_JOB#
            }

            #TYPES_COMP_JOB_DISPOSE#
        }    
";

        const string create_fun_Script = @"
        public int CreateEntity<#TYPES#>(WorldFilterDelegate_R#COUNT#<#TYPES#> callback = null)
        #TYPES_WHERE#
        {
            int entityUId = CreateEntity();

            #INSTANCE#

            if(callback!=null)callback(#CALLBACK#);

            #ADD_COMP#

            return entityUId;
        }
";

        const string filter_fun_Script = @"
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

            setFilterKeyCache(minCountEntityUIds);

            for(int i=0 ; i<_filterKeyCacheLength ; i++)
            {
                var entityUId = _filterKeyCache[i];
                
                #TYPES_COMP#

                callback(#CALLBACK#);

                #SET_COMPS#
            }
        }     
";

        const string script = @"
using System;
using System.Collections.Generic;
using Unity.Collections;

namespace FGUFW.ECS
{
    #DELEGATES#

    public partial class World
    {
        #FILTER_FUN#

        #FILTER_JOB_FUN#
        
        #CREATE_FUN#
    }
}
";


    }
}