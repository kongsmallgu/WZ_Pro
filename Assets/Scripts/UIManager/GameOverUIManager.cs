using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ϸ��������
public class GameOverUIManager : UIBaseManeger
{
    // ��д���෽��
    public override void ShowUI()
    {
        base.ShowUI(); // ���û��෽��
        Debug.Log("Game Over UI Manager: Showing Game Over UI");
        // �������д��ʾ��Ϸ���� UI ���߼�

    }

    public override void HideUI()
    {
        base.HideUI(); // ���û��෽��
        Debug.Log("Game Over UI Manager: Hiding Game Over UI");
        // �������д������Ϸ���� UI ���߼�
    }
}
