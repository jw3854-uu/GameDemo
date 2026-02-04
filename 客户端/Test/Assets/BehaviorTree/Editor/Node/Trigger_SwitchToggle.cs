using UnityEditor.Experimental.GraphView;
public class Trigger_SwitchToggle : TriggerNode
{
    public override string stateName => "SwitchToggleState";
    public Trigger_SwitchToggle() : base() 
    {
        title = "SwitchToggle";
        
        Port port_enter = CreatePortForNode(this, Direction.Input, typeof(System.Boolean), Port.Capacity.Single);
        port_enter.portName = "enter";
        inputContainer.Add(port_enter);

        
        Port port_exit = CreatePortForNode(this, Direction.Output, typeof(System.Boolean), Port.Capacity.Single);
        port_exit.portName = "exit";
        outputContainer.Add(port_exit);

        Port port_IsOn = CreatePortForNode(this, Direction.Output, typeof(System.Boolean), Port.Capacity.Multi);
        port_IsOn.portName = "isOn";
        outputContainer.Add(port_IsOn);

        Port port_IsOff = CreatePortForNode(this, Direction.Output, typeof(System.Boolean), Port.Capacity.Multi);
        port_IsOff.portName = "isOff";
        outputContainer.Add(port_IsOff);

    }
}
