using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGUFW.Play
{
    public class ShowFPS : MonoBehaviour
    {
        public static ShowFPS I;
        public GUIStyle UIStyle;

        public int FPS;
        public int BulletCount;
        public int MonsterCount;

        string _fpsText="";
        int _seconds;
        void OnGUI()
        {
            GUILayout.Label(_fpsText,UIStyle);
        }

        void Awake()
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if((int)Time.unscaledTime > _seconds)
            {
                _seconds = (int)Time.unscaledTime;
                _fpsText = $"FPS:{FPS:D3}  BulletCount:{BulletCount:D3}  MonsterCount:{MonsterCount:D3}";
            }
        }
    }
}
