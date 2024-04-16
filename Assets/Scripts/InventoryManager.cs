using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 物品图标的映射关系
    public Dictionary<string, Sprite> itemIcons;

    void Start()
    {
        // 初始化物品图标的映射关系
        InitializeItemIcons();
    }

    // 初始化物品图标的映射关系
    void InitializeItemIcons()
    {
        itemIcons = new Dictionary<string, Sprite>();

        // 添加物品图标的映射关系，这里仅作示例，实际情况需要根据具体的需求添加
        itemIcons.Add("Potion", Resources.Load<Sprite>("PotionIcon"));
        itemIcons.Add("Sword", Resources.Load<Sprite>("SwordIcon"));
        // 添加其他物品的图标映射关系
    }

    // 添加物品到背包中
    public void AddItem(string itemIdentifier)
    {
        // 在背包中添加物品
        // ...
    }

    // 根据物品标识符获取物品图标
    public Sprite GetItemIcon(string itemIdentifier)
    {
        // 如果映射关系中存在对应的图标，则返回该图标，否则返回null
        if (itemIcons.ContainsKey(itemIdentifier))
        {
            return itemIcons[itemIdentifier];
        }
        else
        {
            Debug.LogWarning("物品图标映射关系中不存在标识符为 " + itemIdentifier + " 的物品图标！");
            return null;
        }
    }
}
