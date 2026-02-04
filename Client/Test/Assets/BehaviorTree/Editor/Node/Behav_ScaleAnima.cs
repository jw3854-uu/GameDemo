
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class Behav_ScaleAnima : BehaviorNode
{
    public override string stateName => "ScaleAnimaState";
    public Behav_ScaleAnima() : base()
    {
        title = "ScaleAnima";

        Port port_enter = CreatePortForNode(this, Direction.Input, typeof(Boolean), Port.Capacity.Single);
        port_enter.portName = "enter";
        inputContainer.Add(port_enter);

        Port port_exit = CreatePortForNode(this, Direction.Output, typeof(Boolean), Port.Capacity.Single);
        port_exit.portName = "exit";
        outputContainer.Add(port_exit);

    }
}
