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
    // �ڻ����ж�����Ҫ����д�ķ���
    public virtual void ShowUI()
    {
        Debug.Log("�����UI���� ��ʾ ");
        this.gameObject.SetActive(true);
    }

    public virtual void HideUI()
    {
        Debug.Log("�����UI���� ����");
        this.gameObject.SetActive(false);
    }
}
