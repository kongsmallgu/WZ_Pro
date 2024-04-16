using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("��������")]
    [Tooltip("�������ֵ")]
    public int Health;
    [Tooltip("��ҹ���ֵ")]
    public int Attack;
    [Tooltip("��ҷ���ֵ")]
    public int Defense;

    [Header("��Ϊ����")]
    [Tooltip("����ƶ��ٶ�")]
    public float MoveSpeed;
    [Tooltip("��ҹ������")]
    public float RangedAttackRate;

}
