using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyStats")]

public class EnemyStats : ScriptableObject
{
    [Header("基本属性")]
    [Tooltip("敌人的名称")]
    public string EnemyName;
    [Tooltip("敌人的生命值")]
    public int EnemyHealth;
    [Tooltip("敌人的攻击力")]
    public int EnemyAttack;
    [Tooltip("敌人的防御力")]
    public int EnemyDefense;

    [Header("行为属性")]
    [Tooltip("敌人的巡逻范围")]
    public float PatrolRange;
    [Tooltip("敌人的近战攻击范围")]
    public float MeleeRange;
    [Tooltip("敌人的远程攻击范围")]
    public float RangedRange;
    [Tooltip("敌人的近战攻击间隔")]
    public float MeleeAttackRate;
    [Tooltip("敌人的远程攻击间隔")]
    public float RangedAttackRate;
    [Tooltip("敌人的巡逻速度")]
    public float PatrolSpeed;
    [Tooltip("敌人的追踪速度")]
    public float TrackSpeed;
    [Tooltip("敌人的击退力大小")]
    public float knockbackForce;

    [Tooltip("敌人的远程攻击特效")]
    public GameObject RangedAtkEft;


    [Header("死亡属性")]
    [Tooltip("敌人死亡后增加给玩家的生命值")]
    public int AddPlayerHealth;
    [Tooltip("敌人死亡后增加给玩家的攻击力")]
    public int AddPlayerAttack;
    [Tooltip("敌人死亡后增加给玩家的防御力")]
    public int AddPlayerDefense;

    [Header("掉落道具属性")]
    [Header("一般道具")]
    [Header("掉落概率 0-1")]
    [Tooltip("掉落道具的概率")]
    public float dropChance;
    [Tooltip("掉落的道具预制体")]
    public GameObject DropObj;
    [Tooltip("掉落的道具特效预制体")]
    public GameObject DropObjEft;

    [Header("特殊攻击道具")]
    [Tooltip("掉落攻击特殊道具的概率")]
    public float AtkdropChance;
    [Tooltip("掉落的特殊道具预制体")]
    public GameObject AtkDropObj;
    [Tooltip("掉落的特殊道具特效预制体")]
    public GameObject AtkDropObjEft;

    [Header("特殊生存道具")]
    [Tooltip("掉落攻击特殊道具的概率")]
    public float LifedropChance;
    [Tooltip("掉落的特殊道具预制体")]
    public GameObject LifeDropObj;
    [Tooltip("掉落的特殊道具特效预制体")]
    public GameObject LifeDropObjEft;


}
