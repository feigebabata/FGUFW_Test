using System;
using System.Collections.Generic;
using System.Reflection;

namespace FGUFW
{
    public static class TypeHelper
    {
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

    }
}
