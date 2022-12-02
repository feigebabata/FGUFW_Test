using System;
using System.Collections.Generic;
using System.Reflection;

namespace FGUFW
{
    public static class TypeExtensions
    {
        private static Type[] reflectionNoneAges = new Type[0];

        public static bool EqualsFileds(this Type self,Type t)
        {
            var fileds1 = self.GetFields();
            var fileds2 = t.GetFields();
            if (fileds1.Length != fileds2.Length) return false;
            foreach (var filed in fileds1)
            {
                if(!Array.Exists(fileds2,f=>f.Name==filed.Name && f.FieldType==filed.FieldType))
                {
                    return false;
                }
            }
            return true;
        }

        public static string ToScript(this FieldInfo self)
        {
            return $"public {GetFiledTypeName(self.FieldType)} {self.Name};";
        }

        public static string GetFiledTypeName(this Type t)
        {
            if (t.IsGenericType)
            {
                var index = t.Name.IndexOf('`');
                var name = t.Name.Substring(0,index);
                var generics = t.GetGenericArguments();
                int length = generics.Length;
                var types = new string[length];

                for (int i = 0; i < length; i++)
                {
                    types[i] = GetFiledTypeName(generics[i]);
                }
                return $"{t.Namespace}.{name}<{string.Join(",", types)}>";
            }
            else
            {
                return t.FullName;
            }
        }

        public static bool ContainsAttribute<T>(this Type self) where T:Attribute
        {
            var at = typeof(T);
            foreach (var item in self.GetCustomAttributesData())
            {
                if (item.AttributeType == at) return true;
            }
            return false;
        }

        public static object ReflectionInvoke(this object obj,string fun_name,params object[] ages)
        {
            Type[] ageTypes = null;
            if(ages==null && ages.Length==0)
            {
                ageTypes = reflectionNoneAges;
            }
            else
            {
                ageTypes = new Type[ages.Length];
                for (int i = 0; i < ages.Length; i++)
                {
                    ageTypes[i] = ages[i].GetType();
                }
            }
            MethodInfo method = obj.GetType().GetMethod
            (
                fun_name,
                BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance,
                null,
                CallingConventions.Any,
                ageTypes,
                null
            );
            return method?.Invoke(obj,ages);
        }

    }
}
