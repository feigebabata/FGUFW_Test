using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FGUFW.ECS
{
    public class ECSScriptCreator : EditorWindow
    {
        VisualElement _filterPanel,_compPanel;
        Button _filterPanelBtn,_compPanelBtn,_sysPanelBtn,sysPanelBtn,_filterPanelCreateBtn,_compPanelCreateBtn;
        Label _pathTitle;
        SliderInt _filterCountInput,_compCountInput;
        TextField _compNamespaceInput;
        IntegerField _compTypeValue;
        ListView _compNameList;

        [MenuItem("Assets/Create/ECSScriptCreator")]
        public static void ShowExample()
        {
            if(FGUFW.EditorUtils.EditorUtils.GetSeleceFolderPath()==null)return;
            ECSScriptCreator wnd = GetWindow<ECSScriptCreator>();
            wnd.titleContent = new GUIContent("ECSScriptCreator");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Develop/FGUFW/ECS/Editor/ECSScriptCreator.uxml");
            visualTree.CloneTree(root);
            
            findUIComp(root);
            addListener();

            _pathTitle.text = FGUFW.EditorUtils.EditorUtils.GetSeleceFolderPath();

            onClickCompPanel();
        }

        private void findUIComp(VisualElement root)
        {
            _pathTitle = root.Q<Label>("path");

            _filterPanel = root.Q<VisualElement>("filterPanel");
            _filterPanelBtn = root.Q<Button>("filterPanelBtn");
            _filterPanelCreateBtn = root.Q<Button>("CreateFilter");
            _filterCountInput = root.Q<SliderInt>("FilterInput");

            _compPanel = root.Q<VisualElement>("compPanel");
            _compPanelBtn = root.Q<Button>("compPanelBtn");
            _compPanelCreateBtn = root.Q<Button>("CreateComp");
            _compCountInput = root.Q<SliderInt>("compCount");
            _compNamespaceInput = root.Q<TextField>("compNamespaceInput");
            _compTypeValue = root.Q<IntegerField>("compTypeInput");
            _compNameList = root.Q<ListView>("compNameList");

            _sysPanelBtn = root.Q<Button>("sysPanelBtn");
        }

        private void addListener()
        {
            _filterPanelBtn.clicked += onClickFilterPanel;
            _filterPanelCreateBtn.clicked += onClickFilterCreate;

            _compPanelBtn.clicked += onClickCompPanel;
            _compCountInput.RegisterValueChangedCallback(onCompCountValueChanged);
            _compNameList.makeItem += onCompListMakeItem;
            _compNameList.bindItem += onCompListBindItem;
            _compPanelCreateBtn.clicked += onClickCompPanelCreate;
        }

        private void onClickFilterCreate()
        {
            WorldFilterScriptCreate.CreateScript(_filterCountInput.value);
        }

        private void onClickCompPanelCreate()
        {
            int length = _compNameList.itemsSource.Count;

            List<string> compNames = new List<string>();
            for (int i = 0; i < length; i++)
            {
                if(_compNameList.itemsSource[i]==null)continue;
                var textField = _compNameList.itemsSource[i] as TextField;
                var text = textField.text;
                if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))continue;
                compNames.Add(text);
            }
            CompAndSysScriptCreator.CreateCompScript(_pathTitle.text,_compNamespaceInput.text,compNames,_compTypeValue.value);
        }

        private void onCompListBindItem(VisualElement arg1, int arg2)
        {
            _compNameList.itemsSource[arg2] = arg1;
        }

        private VisualElement onCompListMakeItem()
        {
            var textField = new TextField();

            return textField;
        }

        private void onCompCountValueChanged(ChangeEvent<int> evt)
        {
            setCompList(evt.newValue);
        }

        private void setCompList(int length)
        {
            _compNameList.itemsSource = new List<VisualElement>(new VisualElement[length]);
        }

        private void onSwitchPanelBtn(Button panelBtn)
        {
            Color32 on = new Color32(128,128,128,255);
            Color32 off = new Color32(88,88,88,255);
            _filterPanelBtn.style.backgroundColor = new StyleColor(_filterPanelBtn==panelBtn?on:off);
            _compPanelBtn.style.backgroundColor = new StyleColor(_compPanelBtn==panelBtn?on:off);

            _filterPanel.visible = _filterPanelBtn==panelBtn;
            _compPanel.visible = _compPanelBtn==panelBtn;

            if(_filterPanelBtn==panelBtn)
            {
                rootVisualElement.Insert(2,_filterPanel);
            }
            else if(_compPanelBtn==panelBtn)
            {
                rootVisualElement.Insert(2,_compPanel);
            }
        }

        private void onClickCompPanel()
        {
            onSwitchPanelBtn(_compPanelBtn);
            setCompList(_compCountInput.value);
            var config = CompAndSysScriptCreator.GetConfig();
            _compNamespaceInput.value = config.PrevNamespace;
            _compTypeValue.value = config.CompTypeIndex;
        }

        private void onClickFilterPanel()
        {
            onSwitchPanelBtn(_filterPanelBtn);
        }
    }
}
