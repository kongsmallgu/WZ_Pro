using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �����õ� ����
public class UiLayerParent : MonoBehaviour
{
    /// <summary> �������� </summary>        
    public enum LayerMold
    {
        FullScreen,     // ȫ��
        PopupWindow     // ��ȫ������
    }
    [Tooltip("��������")]
    public LayerMold LayerMoldSelf;
    [Tooltip("�ܽڵ�")]
    public GameObject LayerRoot;
    [Tooltip("���ֽڵ�")]
    public GameObject LayerMask;
    [Tooltip("�رհ�ť")]
    public GameObject BtnClose;
    [Tooltip("����λ�ùرս���İ�ť")]
    public GameObject BtnCloseMask;

    private void Awake()
    {
        InitParent();
    }

    /// <summary> �򿪽��� </summary>  
    public void Open(object data = null)
    {
        OpenSelf(data);
    }
    /// <summary> �رս��� </summary>
    public void Close()
    {
        CloseSelf();
        UiLayerManager.Instance.CloseLayer(this.gameObject.name);
    }

    /// <summary> �򿪽��棨�ӽ����Լ������ݣ� </summary>
    public virtual void OpenSelf(object data = null) { }

    /// <summary> �رս��棨�ӽ����Լ������ݣ� </summary>
    public virtual void CloseSelf() { }

    /// <summary> ��ʼ���������� </summary>
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