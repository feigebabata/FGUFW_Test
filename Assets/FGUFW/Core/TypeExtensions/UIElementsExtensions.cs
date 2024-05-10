#if UNITY_EDITOR

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace FGUFW
{
    public static class UIElementsExtensions
    {
        public static void SetTitleColor(this Node self,Color color)
        {
            var lable = self.titleContainer.Q<Label>("title-label");
            lable.style.color = color;
        }

        public static void SetTitleSize(this Node self,int size)
        {
            var lable = self.titleContainer.Q<Label>("title-label");
            lable.style.fontSize = size;
        }
    }
}

#endif