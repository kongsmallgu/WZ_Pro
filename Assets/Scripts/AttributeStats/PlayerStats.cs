using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("基本属性")]
    [Tooltip("玩家生命值")]
    public int Health;
    [Tooltip("玩家攻击值")]
    public int Attack;
    [Tooltip("玩家防御值")]
    public int Defense;

    [Header("行为属性")]
    [Tooltip("玩家移动速度")]
    public float MoveSpeed;
    [Header("玩家攻击间隔 玩家每几秒攻击一次")]
    public float AttackTime;

}
