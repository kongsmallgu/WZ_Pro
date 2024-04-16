using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;

    // 物品功能属性
    public int healthBonus;
    public int attackBonus;
    public int defenseBonus;
}
