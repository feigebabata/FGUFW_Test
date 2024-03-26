using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace FGUFW.BehaviorTree.Editor
{
    public class BehaviorTreeView : GraphView
    {
        public BehaviorTreeBase BehaivorTree;
        private GridBackground _gridBackground;

        public new class UxmlFactory : UxmlFactory<BehaviorTreeView,GraphView.UxmlTraits>{}
        public BehaviorTreeView()
        {
            _gridBackground = new GridBackground();
            //插入背景
            Insert(0,_gridBackground);
            
            //添加背景缩放拖拽
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            //读取网格线配置
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Develop/FGUFW/BehaviorTree/Editor/BehaviorTreeView.uss");
            styleSheets.Add(styleSheet);
        }

        //右键菜单
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);

            
            //获取鼠标坐标转网格视图坐标 必须在回调外面记录下来
            var newMousePos = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

            if(BehaivorTree==default)return;

            var nodeCount = BehaivorTree.GetNodeTypes().Length;

            for (int i = 0; i < nodeCount; i++)
            {
                var nodeName = BehaivorTree.GetNodeNames()[i];
                var nodeType = BehaivorTree.GetNodeTypes()[i];
                evt.menu.AppendAction(nodeName,a=>
                {
                    var node = BehaivorTree.CreateNode(nodeType,nodeName);
                    node.Position = newMousePos;
                    createNodeView(node);
                });
            }
        }

        
        //连线端口 返回startPort可以连接的端口
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            // return base.GetCompatiblePorts(startPort, nodeAdapter);
            // 剔除相同方向(In/Out) 所属同一个Node的
            return ports.ToList().Where(endPort=> endPort.direction!=startPort.direction && endPort.node!=startPort.node).ToList();
        }

        //刷新视图
        public void PopulateView(BehaviorTreeBase tree)
        {

            //初始化 清理所有
            graphViewChanged -= OnGraphViewChanged;
            
            DeleteElements(graphElements);

            BehaivorTree = tree;

            if(tree==default)
            {
                return;
            }

            BehaivorTree = tree;
            
            graphViewChanged += OnGraphViewChanged;

            //根据BehaviorTreeBase 创建NodeView
            foreach (var node in tree.Nodes)
            {
                createNodeView(node);
            }

            //根据BehaviorTreeBase 创建NodeView连线
            foreach (var node in tree.Nodes)
            {
                var parent = FindNode(node);
                foreach (var item in node.Nexts)
                {
                    var child = FindNode(item);
                    var edge = parent.Out.ConnectTo(child.In);
                    AddElement(edge);
                }
            }
        }

        public NodeView FindNode(BehaviorTreeNodeBase nodeBase)
        {
            foreach (NodeView item in nodes.ToList())
            {
                if(item.Data==nodeBase)
                {
                    return item;
                }
            }
            return default;
        }


        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {

            //移除的元素
            if(graphViewChange.elementsToRemove!=null)
            {
                foreach (var item in graphViewChange.elementsToRemove)
                {
                    if(item is NodeView nodeView)
                    {
                        BehaivorTree.DeleteNode(nodeView.Data);
                    }
                    else if(item is Edge edge)
                    {
                        var outNode = edge.output.node as NodeView;
                        var inNode = edge.input.node as NodeView;
                        BehaivorTree.RemoveChild(outNode.Data,inNode.Data);
                    }
                }


            }

            //添加的连接
            if(graphViewChange.edgesToCreate!=null)
            {
                foreach (var item in graphViewChange.edgesToCreate)
                {
                    var outNode = item.output.node as NodeView;
                    var inNode = item.input.node as NodeView;

                    BehaivorTree.AddChild(outNode.Data,inNode.Data);
                }
            }
            return graphViewChange;
        }

        private void createNodeView(BehaviorTreeNodeBase node)
        {
            var nodeView = new NodeView(node);
            
            AddElement(nodeView);
        }
    }

}