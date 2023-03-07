using System;
using System.Collections;
using System.Collections.Generic;

namespace FGUFW
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this List<T> self)
        {
            if(self==null || self.Count==0)
            {
                return default(T);
            }
            int idx = UnityEngine.Random.Range(0,self.Count);
            return self[idx];
        }

        public static T Random<T>(this T[] self)
        {
            if(self==null || self.Length==0)
            {
                return default(T);
            }
            int idx = UnityEngine.Random.Range(0,self.Length);
            return self[idx];
        }

        public static int IndexOf<T>(this T[] self,T val,int startIndex=0)
        {
            if(self!=null)
            {
                return Array.IndexOf<T>(self,val,startIndex);
            }
            return -1;
        }

        public static int IndexOf<T>(this T[] self,Predicate<T> match,int startIndex=0,int length=0)
        {
            if(self!=null)
            {
                if(length==0)length = self.Length;
                
                for (int i = startIndex; i < length; i++)
                {
                    var t_obj = self[i];
                    if(match(t_obj))return i;
                }
            }
            return -1;
        }

        public static T Find<T>(this T[] self,Predicate<T> match)
        {
            if(self!=null)
            {
                int length = self.Length;
                for (int i = 0; i < length; i++)
                {
                    var t_obj = self[i];
                    if(match(t_obj))return t_obj;
                }
            }
            return default(T);
        }

        
        /// <summary>
        /// 按权重随机
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="getWeight"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomByWeight<T>(this IEnumerable collection,Func<T,float> getWeight)
        {
            float maxValue = 0;
            foreach (T item in collection)
            {
                maxValue += getWeight(item);
            }
            float val = UnityEngine.Random.Range(0,maxValue);
            float weight = 0;
            foreach (T item in collection)
            {
                weight += getWeight(item);
                if(val<weight)
                {
                    return item;
                }
            }
            return default(T);
        }

        public static T[] Copy<T>(this T[] self,int length)
        {
            if(self==null || self.Length==0)
            {
                return null;
            }
            T[] newArray = new T[length];
            Array.Copy(self,newArray,length);
            return newArray;
        }

        public static U[] To<T,U>(this T[] self,Func<T,U> convert)
        {
            var length = self.Length;
            U[] array = new U[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = convert(self[i]);
            }
            return array;
        }



    }
}