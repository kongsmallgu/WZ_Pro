using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // ��Ʒͼ���ӳ���ϵ
    public Dictionary<string, Sprite> itemIcons;

    void Start()
    {
        // ��ʼ����Ʒͼ���ӳ���ϵ
        InitializeItemIcons();
    }

    // ��ʼ����Ʒͼ���ӳ���ϵ
    void InitializeItemIcons()
    {
        itemIcons = new Dictionary<string, Sprite>();

        // �����Ʒͼ���ӳ���ϵ���������ʾ����ʵ�������Ҫ���ݾ�����������
        itemIcons.Add("Potion", Resources.Load<Sprite>("PotionIcon"));
        itemIcons.Add("Sword", Resources.Load<Sprite>("SwordIcon"));
        // ���������Ʒ��ͼ��ӳ���ϵ
    }

    // �����Ʒ��������
    public void AddItem(string itemIdentifier)
    {
        // �ڱ����������Ʒ
        // ...
    }

    // ������Ʒ��ʶ����ȡ��Ʒͼ��
    public Sprite GetItemIcon(string itemIdentifier)
    {
        // ���ӳ���ϵ�д��ڶ�Ӧ��ͼ�꣬�򷵻ظ�ͼ�꣬���򷵻�null
        if (itemIcons.ContainsKey(itemIdentifier))
        {
            return itemIcons[itemIdentifier];
        }
        else
        {
            Debug.LogWarning("��Ʒͼ��ӳ���ϵ�в����ڱ�ʶ��Ϊ " + itemIdentifier + " ����Ʒͼ�꣡");
            return null;
        }
    }
}
