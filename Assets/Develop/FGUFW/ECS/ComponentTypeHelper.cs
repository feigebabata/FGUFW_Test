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
            Debug.Log(typeof(Temp).IsAssignableFrom(typeof(System.Collections.IList)));
            return;
            AssemblyHelper.Filter<IComponent>(t=>Debug.Log(t));
            return;
            var ecsAssemblyName = Assembly.GetExecutingAssembly().GetName();
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            var compTypeName = typeof(IComponent).FullName;
            foreach (var assembly in assemblys)
            {
                if(!Array.Exists<AssemblyName>(assembly.GetReferencedAssemblies(), an=>an.FullName==ecsAssemblyName.FullName)) continue;

                var types = assembly.GetTypes();
                Dictionary<int, Type> record = new Dictionary<int, Type>();
                foreach (var type in types)
                {
                    if (type.IsValueType && type.GetInterface(compTypeName) != null)
                    {
                        Debug.Log(type);
                        var val = (IComponent)Activator.CreateInstance(type);
                        var compType = val.CompType;
                        if (record.ContainsKey(compType))
                        {
                            var old_type = record[compType];
                            Debug.LogError($"组件类型冲突 {compType} {type.FullName} : {old_type.FullName}");
                        }
                    }
                }
                record.Clear();
            }
        }

        public struct Temp : IComponent
        {
            public int CompType => throw new NotImplementedException();

            public int EntityUId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int Dirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool IsCreated => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

    }
}