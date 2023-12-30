using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;

namespace FGUFW.MonoGameplay
{
    [FilePath("ProjectSettings/MonoGameplaySettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class MonoGameplaySettings : ScriptableSingleton<MonoGameplaySettings>
    {
        public string NameSpace = "TestSpace";
        public string PlayName = "TestPlay";


        internal void Save() { Save(true); }
        private void OnDisable() { Save(); }
        internal SerializedObject GetSerializedObject() { return new SerializedObject(this); }
    }

    public class MonoGameplaySettingsProvider : SettingsProvider
    {
        private SerializedObject _serializedObject;
        private SerializedProperty _nameSpace;
        private SerializedProperty _playName;

        public MonoGameplaySettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            MonoGameplaySettings.instance.Save();
            _serializedObject = MonoGameplaySettings.instance.GetSerializedObject();
            _nameSpace = _serializedObject.FindProperty("NameSpace");
            _playName = _serializedObject.FindProperty("PlayName");
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            // EditorGUILayout.LabelField("MonoGameplay", EditorStyles.boldLabel);

            _nameSpace.stringValue = EditorGUILayout.TextField("NameSpace",_nameSpace.stringValue);
            _playName.stringValue = EditorGUILayout.TextField("PlayName",_playName.stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
                MonoGameplaySettings.instance.Save();
            }
        }

        [SettingsProvider()]
        public static SettingsProvider Launcher()
        {
            return new MonoGameplaySettingsProvider("FGUFW/MonoGameplay",SettingsScope.Project)
            {

            };
        }
    
    }
}


