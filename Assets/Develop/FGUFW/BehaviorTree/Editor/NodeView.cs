using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FGUFW.BehaviorTree.Editor
{
    public class NodeView : Node
    {
        public Port In,Out;

        public BehaviorTreeNodeBase Data;
        private bool _viewNodeActive;
        private ProgressBar _progressBar;
        private bool _enterFlashing = false;
        private float _enterFlashEndTime;
        private Color _activeColor = Color.green;

        public NodeView(BehaviorTreeNodeBase node)
        {
            Data = node;
            
            var point = node.Position;

            style.left = point.x;
            style.top = point.y;
            style.width = new StyleLength(250);

            this.mainContainer.style.backgroundColor = new Color(0.3f,0.3f,0.3f,0.8f);
            this.titleContainer.style.backgroundColor = node.TitleBackgroundColor;
            this.title = node.name;
            
            createPort(node);

            var editor = UnityEditor.Editor.CreateEditor(Data);
            IMGUIContainer container = new IMGUIContainer(()=>
            {   
                if(Data.IsNull())return;
                
                editor.OnInspectorGUI();

                resetNodeViewActive();
                resetProgressBarActive();
                // drawEnterFlash();
            }); 
            //自定义容器 可被展开收起
            extensionContainer.Add(container);
            //显示自定义容器
            RefreshExpandedState();

            //添加进度条
            createProgressBar(node,new Color(0.27f,0.49f,0.45f));

            setMainContainerBorderSize();
        }

        // private void drawEnterFlash()
        // {
        //     if(Data.EnterNextRecord.Count>0)
        //     {
        //         _enterFlashEndTime = Time.time+0.1f;
        //     }

        //     if(!_enterFlashing && _enterFlashEndTime>Time.time)
        //     {
        //         _enterFlashing = true;
        //         foreach (var next in Data.EnterNextRecord)
        //         {
        //             var edge = findEdge(Out,next);

        //             edge.OnSelected();
        //             var nodeView = edge.input.node as NodeView;
        //             nodeView.SetOutlineColor(_activeColor);
                    
        //             Debug.Log($"{nodeView.title} 进入闪烁");
        //         }
                
        //         Data.EnterNextRecord.Clean();
        //     }

        //     if(_enterFlashing && _enterFlashEndTime<Time.time)
        //     {
        //         _enterFlashing = false;
        //         foreach (var edge in Out.connections)
        //         {
        //             var nodeView = edge.input.node as NodeView;
                    
        //             edge.OnUnselected();

        //             if(nodeView.Data.Active)continue;

        //             nodeView.SetOutlineColor(Color.clear);
                    
        //             Debug.Log($"{nodeView.title} 退出闪烁");
        //         }
        //     }
            

        // }

        // private static Edge findEdge(Port port,BehaviorTreeNodeBase nodeBase)
        // {
        //     foreach (var item in port.connections)
        //     {
        //         if(item.input.node is NodeView nodeView && nodeView.Data == nodeBase)
        //         {
        //             return item;
        //         }
        //     }

        //     return default;
        // }

        private void resetProgressBarActive()
        {
            if(_progressBar==default)return;
            _progressBar.visible = _progressBar.value>0;
        }

        private void setMainContainerBorderSize()
        {
            this.mainContainer.style.borderLeftWidth = 2;
            this.mainContainer.style.borderTopWidth = 2;
            this.mainContainer.style.borderRightWidth = 2;
            this.mainContainer.style.borderBottomWidth = 2;
        }

        private void resetNodeViewActive()
        {
            if(Data.Active == _viewNodeActive)return;
            _viewNodeActive = Data.Active;

            var color = _viewNodeActive?_activeColor:Color.clear;
            SetOutlineColor(color);
        }

        public void SetOutlineColor(Color color)
        {
            this.mainContainer.style.borderLeftColor = color;
            this.mainContainer.style.borderTopColor = color;
            this.mainContainer.style.borderRightColor = color;
            this.mainContainer.style.borderBottomColor = color;
        }

        private void createPort(BehaviorTreeNodeBase node)
        {
            if(node is IBehaviorTreeNodeInPort)
            {
                In = InstantiatePort(Orientation.Horizontal,Direction.Input,Port.Capacity.Multi,typeof(bool));
                In.portName = "In";
                inputContainer.Add(In);
            }
            if(node is IBehaviorTreeNodeOutPort)
            {
                Out = InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Multi,typeof(bool));
                Out.portName = "Out";
                outputContainer.Add(Out);
            }

        }

        private void createProgressBar(BehaviorTreeNodeBase node,Color color)
        {
            if(node is IBehaviorTreeNodeProgress)
            {
                var progressBar = new ProgressBar();
                progressBar.highValue = 1;

                var serializedObject = new SerializedObject(node);
                var progressProperty = serializedObject.FindProperty("Progress");
                progressBar.BindProperty(progressProperty);
                var actualBar = progressBar.Q(className: "unity-progress-bar__progress");
                actualBar.style.backgroundColor = color;

                mainContainer.Add(progressBar);
                _progressBar = progressBar;
            }

        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Data.Position.x = newPos.xMin;
            Data.Position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            Selection.activeObject = Data;
        }

    }
}
