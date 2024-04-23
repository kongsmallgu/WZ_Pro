using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseManeger : MonoBehaviour
{
    private Button UIBasePanel;

    private void Start()
    {
        UIBasePanel = this.gameObject.transform.GetChild(0).GetComponent<Button>();
        UIBasePanel.onClick.AddListener(HideUI);

    }
    // 在基类中定义需要被重写的方法
    public virtual void ShowUI()
    {
        Debug.Log("这个是UI基类 显示 ");
        this.gameObject.SetActive(true);
    }

    public virtual void HideUI()
    {
        Debug.Log("这个是UI基类 隐藏");
        this.gameObject.SetActive(false);
    }
}
