using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("��������")]
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;

    [Header("��������")]
    public float healthBonus;
    public float attackBonus;
    public float defenseBonus;
    public float moveSpeed;
    public float atkSpeed;

    [Header("��������")]
    [Tooltip("����ʱ��")]
    public float itemDuration;
}
