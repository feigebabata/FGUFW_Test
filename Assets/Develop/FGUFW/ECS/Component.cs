using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using System;

namespace FGUFW.ECS
{
    /// <summary>
    /// 在构造函数中给成员复制 在Reset中重置成员 在析构中释放成员
    /// </summary>
    public class Component:IReset
    {
        public int EntityUId;
        public int CompType;

        public virtual void Reset(int entityId)
        {
            EntityUId = entityId;
        }
    }

    public interface IReset
    {
        /// <summary>
        /// 重置对象 回收/复用时调用
        /// </summary>
        void Reset(int entityId);
    }

    public class TestComponent : Component
    {
        public TestComponent()
        {
            CompType = 2;
        }
    }

    public class test
    {
        [UnityEditor.MenuItem("Test/Log")]
        static void test1()
        {
            var comp = new TestComponent();
            comp.Reset(2);
            Debug.Log(JsonUtility.ToJson(comp));
        }
    }
}
