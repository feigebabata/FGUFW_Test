using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW
{
    public static class GameObjectExtensions
    {
        public static GameObject Copy(this GameObject self,Transform parent)
        {
            return GameObject.Instantiate(self,parent);
        }
    }
}