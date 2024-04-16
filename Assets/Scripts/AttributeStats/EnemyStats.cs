using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyStats")]

public class EnemyStats : ScriptableObject
{
    [Header("��������")]
    [Tooltip("���˵�����")]
    public string EnemyName;
    [Tooltip("���˵�����ֵ")]
    public int EnemyHealth;
    [Tooltip("���˵Ĺ�����")]
    public int EnemyAttack;
    [Tooltip("���˵ķ�����")]
    public int EnemyDefense;

    [Header("��Ϊ����")]
    [Tooltip("���˵�Ѳ�߷�Χ")]
    public float PatrolRange;
    [Tooltip("���˵Ľ�ս������Χ")]
    public float MeleeRange;
    [Tooltip("���˵�Զ�̹�����Χ")]
    public float RangedRange;
    [Tooltip("���˵Ľ�ս�������")]
    public float MeleeAttackRate;
    [Tooltip("���˵�Զ�̹������")]
    public float RangedAttackRate;
    [Tooltip("���˵�Ѳ���ٶ�")]
    public float PatrolSpeed;
    [Tooltip("���˵�׷���ٶ�")]
    public float TrackSpeed;
    [Tooltip("���˵Ļ�������С")]
    public float knockbackForce;

    [Tooltip("���˵�Զ�̹�����Ч")]
    public GameObject RangedAtkEft;


    [Header("��������")]
    [Tooltip("�������������Ӹ���ҵ�����ֵ")]
    public int AddPlayerHealth;
    [Tooltip("�������������Ӹ���ҵĹ�����")]
    public int AddPlayerAttack;
    [Tooltip("�������������Ӹ���ҵķ�����")]
    public int AddPlayerDefense;

    [Header("�����������")]
    [Header("һ�����")]
    [Header("������� 0-1")]
    [Tooltip("������ߵĸ���")]
    public float dropChance;
    [Tooltip("����ĵ���Ԥ����")]
    public GameObject DropObj;
    [Tooltip("����ĵ�����ЧԤ����")]
    public GameObject DropObjEft;

    [Header("���⹥������")]
    [Tooltip("���乥��������ߵĸ���")]
    public float AtkdropChance;
    [Tooltip("������������Ԥ����")]
    public GameObject AtkDropObj;
    [Tooltip("��������������ЧԤ����")]
    public GameObject AtkDropObjEft;

    [Header("�����������")]
    [Tooltip("���乥��������ߵĸ���")]
    public float LifedropChance;
    [Tooltip("������������Ԥ����")]
    public GameObject LifeDropObj;
    [Tooltip("��������������ЧԤ����")]
    public GameObject LifeDropObjEft;


}
