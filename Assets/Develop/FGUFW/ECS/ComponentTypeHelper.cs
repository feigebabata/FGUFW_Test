

using System;
using System.Collections.Generic;

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
                    compTypeDict.Add(type,t_obj.CompType);
                }
            }
            return compTypeDict[type];
        }
    }
}