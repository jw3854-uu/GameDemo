using UnityEngine;

public class UIWindowComponentBase : MonoBehaviour
{
    protected virtual void Init()
    {

    }
    protected virtual void OnOpen()
    {
        Init();
    }
    protected virtual void Uninit() 
    {

    }
    protected virtual void OnClose()
    {
        Uninit();
    }
}
