using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGUFW
{
    public static class AssemblyHelper
    {
        public static void Filter<T>(Action<Type> callback)
        {
            if (callback==null) return;

            var t_type = typeof(T);

            var assemblyName = typeof(T).Assembly.FullName;
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblys)
            {
                if (!Array.Exists(assembly.GetReferencedAssemblies(), a => a.FullName == assemblyName)) continue;
                foreach (var type in assembly.GetTypes())
                {
                    if (!(type.IsClass || type.IsValueType)) continue;
                    
                    if(type.IsAssignableFrom(t_type))
                    {
                        callback(type);
                    }

                }
            }
        }
    }
}
