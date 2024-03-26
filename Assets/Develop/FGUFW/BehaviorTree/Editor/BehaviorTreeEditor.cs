  using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using System;

namespace FGUFW.BehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView _treeView;
        private Label _assetNameLable;
        private Button _saveBtn;


        [OnOpenAsset]//监听双击文件
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is BehaviorTreeBase)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        [MenuItem("FGBBT/行为树编辑器")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("行为树编辑器");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;


            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Develop/FGUFW/BehaviorTree/Editor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(root);


            _treeView = root.Q<BehaviorTreeView>();
            _assetNameLable = root.Q<Label>("AssetName");
            _saveBtn = root.Q<Button>("Save");

            _saveBtn.clicked -= onClickSave();
            _saveBtn.clicked += onClickSave();

            OnSelectionChange();


            EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
        }

        private Action onClickSave()
        {
            if(!_treeView.BehaivorTree.IsNull())
            {
                _treeView.BehaivorTree.Save();
            }
            return null;
        }


        private void onPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.ExitingPlayMode:
                {
                    if(!_treeView.BehaivorTree.IsNull() && !AssetDatabase.IsNativeAsset(_treeView.BehaivorTree.GetInstanceID()))
                    {
                        // Debug.LogWarning("销毁");
                        DestroyImmediate(_treeView.BehaivorTree);
                        _treeView.BehaivorTree = default;
                        OnSelectionChange();
                    }
                }
                break;
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                {
                    OnSelectionChange();
                }
                break;
            }
        }


        


        void OnSelectionChange()
        {
            BehaviorTreeBase treeBase = default;

            if(Selection.activeObject is BehaviorTreeBase behaviorTreeBase)
            {
                treeBase = behaviorTreeBase;
                _assetNameLable.text = treeBase.name;
            }
            else if(!Selection.activeGameObject.IsNull() && Selection.activeGameObject.GetComponent<BehaviorTreeComp>() && !Selection.activeGameObject.GetComponent<BehaviorTreeComp>().BehaviorTree.IsNull())
            {
                treeBase = Selection.activeGameObject.GetComponent<BehaviorTreeComp>().BehaviorTree;
                _assetNameLable.text = $"{Selection.activeGameObject.name} : {treeBase.name}";
            }
            else
            {
                if(_treeView.BehaivorTree.IsNull())
                {
                    //清理
                    _assetNameLable.text = "未选择BehaviorTree文件";
                    _assetNameLable.style.color = Color.yellow;

                    _treeView.PopulateView(default);
                }
                return;
            }
            
            _assetNameLable.style.color = Color.white;

            _treeView.PopulateView(treeBase);
        }

        


        

        
    }
}
