
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class SwitchToggleState : TiggerBaseState
{
    #region AutoContext

    public System.Boolean exit;
    public System.Boolean isOn;
    public System.Boolean isOff;
    public System.Boolean enter;
    public BTTargetObject toggleObj;
    public BTTargetObject toggleGroupObj;

    public override BTStateObject stateObj
    {
        get
        {
            if (_stateObj == null)
            {
                _stateObj = ScriptableObject.CreateInstance<SwitchToggleStateObj>();
                _stateObj.state = state;
                _stateObj.output = output;
                _stateObj.interruptible = interruptible;
                _stateObj.interruptTag = interruptTag;

                _stateObj.exit = exit;
                _stateObj.IsOn = isOn;
                _stateObj.IsOff = isOff;
                _stateObj.enter = enter;
                _stateObj.toggleObj = toggleObj;
                _stateObj.toggleGroupObj = toggleGroupObj;
            }
            return _stateObj;
        }
    }
    private SwitchToggleStateObj _stateObj;
    public override void InitParam(string param)
    {
        base.InitParam(param);
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(SwitchToggleStateObj));
        using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(param)))
        {
            _stateObj = ScriptableObject.CreateInstance<SwitchToggleStateObj>();
            var json = new StreamReader(stream).ReadToEnd();
            JsonUtility.FromJsonOverwrite(json, _stateObj);

            output = _stateObj.output;

            exit = _stateObj.exit;
            isOn = _stateObj.IsOn;
            isOff = _stateObj.IsOff;
            enter = _stateObj.enter;
            toggleObj = _stateObj.toggleObj;
            toggleGroupObj = _stateObj.toggleGroupObj; triggerTag = _stateObj.triggerTag;
        }
    }
    protected override ESetFieldValueResult SetFieldValue(string fieldName, object value)
    {
        if (StringComparer.Ordinal.Equals(fieldName, default)) return ESetFieldValueResult.Succ;

        else if (StringComparer.Ordinal.Equals(fieldName, "exit") && value is System.Boolean exitValue) exit = exitValue;
        else if (StringComparer.Ordinal.Equals(fieldName, "isOn") && value is System.Boolean IsOnValue) isOn = IsOnValue;
        else if (StringComparer.Ordinal.Equals(fieldName, "isOff") && value is System.Boolean IsOffValue) isOff = IsOffValue;
        else if (StringComparer.Ordinal.Equals(fieldName, "enter") && value is System.Boolean enterValue) enter = enterValue;
        else if (StringComparer.Ordinal.Equals(fieldName, "toggleObj") && value is BTTargetObject toggleObjValue) toggleObj = toggleObjValue;
        else if (StringComparer.Ordinal.Equals(fieldName, "toggleGroupObj") && value is BTTargetObject toggleGroupObjValue) toggleGroupObj = toggleGroupObjValue;
        else return ESetFieldValueResult.Fail;

        return ESetFieldValueResult.Succ;
    }
    public override void Save()
    {
        if (stateObj == null) return;
        output = _stateObj.output;
        interruptible = _stateObj.interruptible;
        interruptTag = _stateObj.interruptTag;

        exit = _stateObj.exit;
        isOn = _stateObj.IsOn;
        enter = _stateObj.enter;
        toggleObj = _stateObj.toggleObj;
        toggleGroupObj = _stateObj.toggleGroupObj; triggerTag = _stateObj.triggerTag;
    }
    #endregion
    private bool isExecuted;
    private Toggle toggle;
    private ToggleGroup toggleGroup;

    public override void OnEnter()
    {
        base.OnEnter();
        isExecuted = false;
        if (toggleGroup == null)
        {
            toggleGroup = toggleGroupObj.target.GetComponent<ToggleGroup>();
            toggleGroup.allowSwitchOff = true;
        }
        if (toggle == null)
        {
            toggle = toggleObj.target.GetComponent<Toggle>();
            toggle.SetIsOnWithoutNotify(false);
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((_isOn) => { OnExecute(); });
        }
    }
    public override void OnRefresh()
    {
        enter = _stateObj.enter;
        isExecuted = false;
        isOn = false;
        isOff = false;
        base.OnRefresh();
    }
    public override void OnExecute()
    {
        base.OnExecute();
        isExecuted = true;
        isOn = toggle.isOn;
        isOff = !toggle.isOn;
        OnExit();
    }
    public override void OnExit()
    {
        for (int i = 0; i < output.Count; i++)
        {
            BTOutputInfo info = output[i];
            if (info.fromPortName == "isOff") info.value = isOff;
            if (info.fromPortName == "isOn") info.value = isOn;
            if (info.fromPortName == "exit") info.value = isExecuted;
        }
        isExecuted = false;
        isOn = false;
        isOff = false;
        base.OnExit();
    }
}

#region AutoContext_BTStateObject
public class SwitchToggleStateObj : BTTiggerStateObject
{
    public EBTState state;

    public System.Boolean exit;
    public System.Boolean IsOn;
    public System.Boolean IsOff;
    public System.Boolean enter;
    public BTTargetObject toggleObj;
    public BTTargetObject toggleGroupObj;
}
#endregion
