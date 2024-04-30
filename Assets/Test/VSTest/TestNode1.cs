using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("测试节点1")]
[UnitSubtitle("这是一个测试节点")]
[UnitShortTitle("测试简称")]
[UnitCategory("Test/TestNode1")]
[TypeIcon(typeof(Vector2Divide))]
public class TestNode1 : Unit
{   
    [PortLabelHidden]
    [DoNotSerialize]
    public ControlInput input;

    [PortLabelHidden]
    [DoNotSerialize]
    public ControlOutput output;

    protected override void Definition()
    {
        this.input = ControlInput("input",onInputAction);
        this.output = ControlOutput("output");
    }

    private ControlOutput onInputAction(Flow flow)
    {
        return output;
    }
}
