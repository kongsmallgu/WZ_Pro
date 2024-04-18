using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItemShow : MonoBehaviour
{
    [Header("ҩƷ�����ļ�")]
    public Item potionitem;
    private string itemname;
    private Sprite itemicon;
    private string itemdescription;

    // ��Ʒ��������
    private float healthbonus;
    private float attackbonus;
    private float defensebonus;
    private float movespeed;
    private float atkspeed;

    //��������
    private float itemduration;

    private void Awake()
    {
        itemname = potionitem.itemName;
        itemicon = potionitem.itemIcon;
        itemdescription = potionitem.itemDescription;

        healthbonus = potionitem.healthBonus;
        attackbonus = potionitem.attackBonus;
        defensebonus = potionitem.defenseBonus;
        movespeed = potionitem.moveSpeed;
        atkspeed = potionitem.atkSpeed;
        itemduration = potionitem.itemDuration;
    }

}
