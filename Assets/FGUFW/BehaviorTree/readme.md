1. 新建行为树脚本 继承BehaviorTreeBase
        实现BehaviorTreeBase的抽象函数GetNodeTypes() 返回需要创建的节点类型

2. 新建节点脚本 继承BehaviorTreeNodeBase
        添加接口 IBehaviorTreeNodeInPort 显示接受连接点
        添加接口 IBehaviorTreeNodeOutPort 显示延申连接点
        添加接口 IBehaviorTreeNodeProgress 并添加float Progress;字段 显示进度条

3. 右键 Create/行为树脚本名 创建行为树配置文件 双击打开

4. 或者菜单栏 FGBBT/行为树编辑器

5. 编辑器内鼠标右键选择创建节点 键盘Delete键删除节点或连线

6. 找不到 IsNull() 函数的话把下面代码找个地方粘贴
        public static bool IsNull(this UnityEngine.Object self)
        {
            return self==null || !(self is UnityEngine.Object);
        }