using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FGUFW
{
    public static class UGUIExtensions
    {
        public static void AddListener(this Button self,UnityAction call)
        {
            self.onClick.AddListener(call);
        }
        public static void RemoveListener(this Button self,UnityAction call)
        {
            self.onClick.RemoveListener(call);
        }
    }
}