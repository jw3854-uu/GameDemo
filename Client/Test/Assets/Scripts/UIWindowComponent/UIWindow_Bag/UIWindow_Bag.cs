using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow_Bag : UIWindowComponentBase
{
    public BTRuntimeComponent bTRuntimeComp;
    public GameObject itemPrefab;
    public List<int> testItems;
    public UIPanel_Bag_ItemView itemView;
    public UIPanel_Bag_DescriptionView descriptionView;
    public UIPanel_Bag_ButtonGroup buttonGroup;

    private Action<ItemConfig> action;
    private void Start()
    {
        OnOpen();
    }
    protected override void Init()
    {
        base.Init();
        BTRuntimeController.ins.SendToTag("UIWindow_Bag_Init",EBTState.½øÈë);
        //bTRuntimeComp.SendMsgToBTRuntime("UIWindow_Bag_Init");
        action += descriptionView.SetData;
        action += buttonGroup.SetUseButtonShowOrHide;
        itemView.SetItemBroadcastEvent(action);
        itemView.Initialize(testItems, itemPrefab);
    }

    protected override void Uninit()
    {
        action = null;
        itemView.Uninitialize();
        base.Uninit();
    }
    public void OnItemSelect() 
    {
        bTRuntimeComp.SendMsgToBTRuntime("UIWindow_Bag_ItemSelect");
    }
    public void OnItemUnselect()
    {
        bTRuntimeComp.SendMsgToBTRuntime("UIWindow_Bag_ItemUnselect");
    }
}
