using UnityEditor;
using UnityEngine;

namespace FGUFW.MonoGameplay
{
    [CustomEditor(typeof(UIPanel))]
    public class UIPanelEditor : Editor
    {
        private UIPanel _target;
        private SerializedProperty _alphaCurve;
        private SerializedProperty _scaleCurve;
        private SerializedProperty _moveCurve;
        private SerializedProperty _moveVector;
        private SerializedProperty _switingTime;

        void OnEnable()
        {
            _target = target as UIPanel;

            _alphaCurve = serializedObject.FindProperty("AlphaCurve");
            _scaleCurve = serializedObject.FindProperty("ScaleCurve");
            _moveCurve = serializedObject.FindProperty("MoveCurve");
            _moveVector = serializedObject.FindProperty("MoveVector");
            _switingTime = serializedObject.FindProperty("SwitingTime");
        }

        void OnDisable()
        {
            _target = default;
        }

        public override void OnInspectorGUI()
        {
            _target.SwitingEffect = (UIPanel.Effect)EditorGUILayout.EnumFlagsField("Switing",_target.SwitingEffect);

            base.OnInspectorGUI();

            if(_target.SwitingEffect != UIPanel.Effect.Nothing)
            {
                EditorGUILayout.PropertyField(_switingTime);
            }

            if((_target.SwitingEffect & UIPanel.Effect.Alpha) == UIPanel.Effect.Alpha)
            {
                EditorGUILayout.PropertyField(_alphaCurve);
            }
            if((_target.SwitingEffect & UIPanel.Effect.Scale) == UIPanel.Effect.Scale)
            {
                EditorGUILayout.PropertyField(_scaleCurve);
            }
            if((_target.SwitingEffect & UIPanel.Effect.Move) == UIPanel.Effect.Move)
            {
                EditorGUILayout.PropertyField(_moveCurve);
                EditorGUILayout.PropertyField(_moveVector);
            }

            serializedObject.ApplyModifiedProperties();

        }
    }

}
