using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using FGUFW;

namespace FGUFW.ECS
{
    public static class ComponentTypeHelper
    {
        static private Dictionary<Type,int> compTypeDict = new Dictionary<Type,int>();

        public static int GetTypeValue<T>()
        {
            var type = typeof(T);
            if(!compTypeDict.ContainsKey(type))
            {
                using (var t_obj = (IComponent)Activator.CreateInstance(type))
                {
                    if(compTypeDict.ContainsValue(t_obj.CompType))
                    {
                        throw new Exception($"组件类型重复 val={t_obj.CompType}");
                    }
                    compTypeDict.Add(type,t_obj.CompType);
                }
            }
            return compTypeDict[type];
        }

        static public void CheckCompType()
        {
            // AssemblyHelper.FilterClassAndStruct<IComponent>(t=>Debug.Log(t));
            Dictionary<int, Type> record = new Dictionary<int, Type>();
            AssemblyHelper.FilterClassAndStruct<IComponent>(t=>
            {
                var comp = (IComponent)Activator.CreateInstance(t);
                if(record.ContainsValue(t))
                {
                    throw new Exception($"组件类型重复 val={comp.CompType}\n{t}");
                }
                if(record.ContainsKey(comp.CompType))
                {
                    throw new Exception($"组件类型重复 val={comp.CompType}\n{t}\n{record[comp.CompType]}");
                }
                record.Add(comp.CompType,t);
            });
            
            record.Clear();
            Debug.Log("类型检测结束!");
        }

    }
}