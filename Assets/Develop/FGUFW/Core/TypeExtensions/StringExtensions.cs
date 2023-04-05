using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace FGUFW
{
    public static class StringExtensions
    {
        public static Uri ToUri(this string text)
        {
            return new Uri(text);
        }
        
        public static IPAddress ToIP(this string text)
        {
            return IPAddress.Parse(text);
        }

        public static int ToInt32(this string text)
        {
            return int.Parse(text);
        }
        
        public static float ToFloat(this string text)
        {
            return float.Parse(text);
        }

        public static T FromJson<T>(this string text)
        {
            return JsonUtility.FromJson<T>(text);
        }

        public static Color ToColor(this string self)
        {
            Color color;
            ColorUtility.TryParseHtmlString(self,out color);
            return color;
        }

        public static T ToEnum<T>(this string self) where T:Enum
        {
            return (T)Enum.Parse(typeof(T),self);
        }

    }
}