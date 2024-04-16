using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("基本属性")]
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;

    [Header("功能属性")]
    public int healthBonus;
    public int attackBonus;
    public int defenseBonus;
    public float moveSpeed;
    public float atkSpeed;

    [Header("特殊属性")]
    [Tooltip("持续时间")]
    public int itemDuration;
}
