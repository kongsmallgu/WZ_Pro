using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 界面用的 父类
public class UiLayerParent : MonoBehaviour
{
    /// <summary> 界面类型 </summary>        
    public enum LayerMold
    {
        FullScreen,     // 全屏
        PopupWindow     // 非全屏弹窗
    }
    [Tooltip("窗口类型")]
    public LayerMold LayerMoldSelf;
    [Tooltip("总节点")]
    public GameObject LayerRoot;
    [Tooltip("遮罩节点")]
    public GameObject LayerMask;
    [Tooltip("关闭按钮")]
    public GameObject BtnClose;
    [Tooltip("任意位置关闭界面的按钮")]
    public GameObject BtnCloseMask;

    private void Awake()
    {
        InitParent();
    }

    /// <summary> 打开界面 </summary>  
    public void Open(object data = null)
    {
        OpenSelf(data);
    }
    /// <summary> 关闭界面 </summary>
    public void Close()
    {
        CloseSelf();
        UiLayerManager.Instance.CloseLayer(this.gameObject.name);
    }

    /// <summary> 打开界面（子界面自己的内容） </summary>
    public virtual void OpenSelf(object data = null) { }

    /// <summary> 关闭界面（子界面自己的内容） </summary>
    public virtual void CloseSelf() { }

    /// <summary> 初始化基础功能 </summary>
    void InitParent()
    {
       /* UiButtonManager.Instance.AddButtonListener(BtnClose, Close);
        switch (LayerMoldSelf)
        {
            case LayerMold.FullScreen:
                LayerMask.SetActive(true);
                break;
            case LayerMold.PopupWindow:
                UiButtonManager.Instance.AddButtonListener(BtnCloseMask, Close);
                break;
            default:
                break;
        }*/
    }
}