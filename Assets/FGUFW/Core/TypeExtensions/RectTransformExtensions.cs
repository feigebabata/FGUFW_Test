using UnityEngine;
using System;

namespace FGUFW
{
    public static class RectTransformExtensions
    {
        public static void SetAnchoredY(this RectTransform self, float y)
        {
            var pos = self.anchoredPosition;
            pos.y = y;
            self.anchoredPosition = pos;
        }

        public static void SetAnchoredX(this RectTransform self, float x)
        {
            var pos = self.anchoredPosition;
            pos.x = x;
            self.anchoredPosition = pos;
        }
        public static void SetSizeY(this RectTransform self, float y)
        {
            var sizeDelta = self.sizeDelta;
            sizeDelta.y = y;
            self.sizeDelta = sizeDelta;
        }

        public static void SetSizeX(this RectTransform self, float x)
        {
            var sizeDelta = self.sizeDelta;
            sizeDelta.x = x;
            self.sizeDelta = sizeDelta;
        }

        public static void SetX(this Transform self, float x)
        {
            var pos = self.position;
            pos.x = x;
            self.position = pos;
        }

        public static void SetY(this Transform self, float y)
        {
            var pos = self.position;
            pos.y = y;
            self.position = pos;
        }

        public static void SetZ(this Transform self, float z)
        {
            var pos = self.position;
            pos.z = z;
            self.position = pos;
        }

        public static void SetLocalX(this Transform self, float x)
        {
            var pos = self.localPosition;
            pos.x = x;
            self.localPosition = pos;
        }

        public static void SetLocalY(this Transform self, float y)
        {
            var pos = self.localPosition;
            pos.y = y;
            self.localPosition = pos;
        }

        public static void SetLocalZ(this Transform self, float z)
        {
            var pos = self.localPosition;
            pos.z = z;
            self.localPosition = pos;
        }

    }
}