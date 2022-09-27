using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using FGUFW.ECS;

namespace FGUFW.ECS.Editor
{
    public class ECSScriptCreator : EditorWindow
    {
        Label _pathTitle;
        Button _filterCreateBtn,_switchCompBtn,_switchSysBtn,_createCompOrSysBtn;
        SliderInt _filterCountInput,_countInput;
        TextField _namespaceInput,_sysNameInput;
        IntegerField _compTypeOrSysOrderInput;
        ListView _listInput;
        Toggle _useJobOrMonoComp;
        List<string> _sysCompTypeList;

        /// <summary>
        /// 1:组件 2:系统
        /// </summary>
        int _panelType=0;

        [MenuItem("Assets/Create/ECSScriptCreator")]
        public static void ShowExample()
        {
            ECSScriptCreator wnd = GetWindow<ECSScriptCreator>();
            wnd.titleContent = new GUIContent("ECSScriptCreator");
        }

        public void CreateGUI()
        {
            string path = FGUFW.EditorUtils.EditorUtils.GetSeleceFolderPath();

            if(string.IsNullOrEmpty(path))
            {
                this.Close();
            }

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Develop/FGUFW/ECS/Editor/ECSScriptCreator.uxml");
            visualTree.CloneTree(root);
            
            _sysCompTypeList = getCompNames();

            findUIComp(root);
            addListener();

            var config = CompAndSysScriptCreator.GetConfig();
            _pathTitle.text = path;
            _namespaceInput.value = config.PrevNamespace;
            onSwitchPanelBtn(config.PrevPanel);
        }

        private void findUIComp(VisualElement root)
        {
            _pathTitle = root.Q<Label>("path");

            _filterCreateBtn = root.Q<Button>("filterCreateBtn");
            _switchCompBtn = root.Q<Button>("switchCompBtn");
            _switchSysBtn = root.Q<Button>("switchSysBtn");
            _createCompOrSysBtn = root.Q<Button>("createCompOrSysBtn");

            _filterCountInput = root.Q<SliderInt>("filterCountInput");
            _countInput = root.Q<SliderInt>("countInput");
            
            _namespaceInput = root.Q<TextField>("namespaceInput");
            _sysNameInput = root.Q<TextField>("sysNameInput");
            _useJobOrMonoComp = root.Q<Toggle>("useJobOrMonoComp");
            _compTypeOrSysOrderInput = root.Q<IntegerField>("compTypeOrSysOrderInput");
            _listInput = root.Q<ListView>("listInput");

        }

        private List<string> getCompNames()
        {
            List<string> compNames = new List<string>();
            AssemblyHelper.FilterClassAndStruct<IComponent>(t=>compNames.Add(t.Name));
            return compNames;
        }

        private void addListener()
        {
            _filterCreateBtn.clicked += onClickFilterCreate;

            _switchCompBtn.clicked += onClickCompPanel;
            _switchSysBtn.clicked += onClickSysPanel;

            _countInput.RegisterValueChangedCallback(onListCountValueChanged);

            _listInput.makeItem += onCompListMakeItem;
            _listInput.bindItem += onCompListBindItem;

            _createCompOrSysBtn.clicked += onClickCompOrSysCreate;

        }

        private void onClickSysPanel()
        {
            onSwitchPanelBtn(2);
        }

        private void onClickCompPanel()
        {
            onSwitchPanelBtn(1);
        }

        private void onClickFilterCreate()
        {
            WorldFilterScriptCreate.CreateScript(_filterCountInput.value);
        }

        private void onClickCompOrSysCreate()
        {
            if(_panelType==1)
            {
                onCreateComp();
            }
            else
            {
                onCreateSys();
            }
        }

        private void onCreateSys()
        {
            if(string.IsNullOrEmpty(_namespaceInput.text) || string.IsNullOrWhiteSpace(_namespaceInput.text) )
            {
                Debug.LogError("命名空间无效");
                return;
            }
            if(string.IsNullOrEmpty(_sysNameInput.text) || string.IsNullOrWhiteSpace(_sysNameInput.text) )
            {
                Debug.LogError("系统名无效");
                return;
            }

            int length = _listInput.itemsSource.Count;

            List<string> typeNames = new List<string>();
            for (int i = 0; i < length; i++)
            {
                if(_listInput.itemsSource[i]==null)continue;
                var popup = _listInput.itemsSource[i] as PopupField<string>;
                var text = _sysCompTypeList[popup.index];
                if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))continue;
                if(!typeNames.Contains(text)) typeNames.Add(text);
            }
            CompAndSysScriptCreator.CreateSysScript(_pathTitle.text, _namespaceInput.text,_sysNameInput.value, _compTypeOrSysOrderInput.value, typeNames, _useJobOrMonoComp.value);
        }

        private void onCreateComp()
        {
            if(string.IsNullOrEmpty(_namespaceInput.text) || string.IsNullOrWhiteSpace(_namespaceInput.text) )
            {
                Debug.LogError("命名空间无效");
                return;
            }

            int length = _listInput.itemsSource.Count;

            List<string> compNames = new List<string>();
            for (int i = 0; i < length; i++)
            {
                if(_listInput.itemsSource[i]==null)continue;
                var textField = _listInput.itemsSource[i] as TextField;
                var text = textField.text;
                if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))continue;
                if(!compNames.Contains(text)) compNames.Add(text);
            }
            CompAndSysScriptCreator.CreateCompScript(_pathTitle.text,_namespaceInput.text,compNames,_compTypeOrSysOrderInput.value, _useJobOrMonoComp.value);
        }

        private void onCompListBindItem(VisualElement arg1, int arg2)
        {
            _listInput.itemsSource[arg2] = arg1;
        }

        private VisualElement onCompListMakeItem()
        {
            if(_panelType==1)
            {
                var textField = new TextField();
                return textField;
            }
            else
            {
                var popup = new PopupField<string>(getCompNames(),0);
                return popup;
            }
        }

        private void onListCountValueChanged(ChangeEvent<int> evt)
        {
            setListCount(evt.newValue);
        }

        private void setListCount(int length)
        {
            _listInput.itemsSource = new List<VisualElement>(new VisualElement[length]);
        }

        private void onSwitchPanelBtn(int panelType)
        {
            _panelType = panelType;
            Color32 on = new Color32(128,128,128,255);
            Color32 off = new Color32(88,88,88,255);

            _switchCompBtn.style.backgroundColor = new StyleColor(_panelType==1?on:off);
            _switchSysBtn.style.backgroundColor = new StyleColor(_panelType==2?on:off);
            
            var config = CompAndSysScriptCreator.GetConfig();
            _compTypeOrSysOrderInput.label = _panelType==1 ? "组件类型索引:":"系统优先级";
            _compTypeOrSysOrderInput.value = _panelType==1 ? config.CompTypeIndex : 0;

            _countInput.label = _panelType==1 ? "组件个数:":"参数个数";
            _countInput.lowValue = _panelType==1 ? 1:0;
            _countInput.highValue = _panelType==1 ? 32:8;
            _countInput.value = _panelType==1 ? 1:0;

            _sysNameInput.visible = _panelType==2;

            _createCompOrSysBtn.text = _panelType==1 ? "创建组件":"创建系统";

            _useJobOrMonoComp.text = _panelType == 1 ? "Mono脚本特性:" : "使用Job:";

            setListCount(_countInput.value);
        }

    }
}
