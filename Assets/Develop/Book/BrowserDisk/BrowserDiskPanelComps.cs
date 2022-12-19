using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;
using FGUFW.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Book
{
    public class BrowserDiskPanelComps : MonoBehaviour
    {
        public Transform ListRoot;
        public Button CloseBtn,BackBtn;
        public Toggle UseGB2312;
        public Text Title;
        public Color32 FileColor,DirectoryColor;
    }
}