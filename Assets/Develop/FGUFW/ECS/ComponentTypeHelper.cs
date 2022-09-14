

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var compTypeName = typeof(IComponent).FullName;
            Dictionary<int,Type> record = new Dictionary<int, Type>();
            foreach (var type in types)
            {
                Debug.Log(type);
                if(type.IsValueType && type.GetInterface(compTypeName)!=null)
                {
                    var val = (IComponent)Activator.CreateInstance(type);
                    var compType = val.CompType;
                    if(record.ContainsKey(compType))
                    {
                        var old_type = record[compType];
                        Debug.LogError($"组件类型冲突 {compType} {type.FullName} : {old_type.FullName}");
                    }
                }
            }
            record.Clear();
        }
    }
}