using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    // ��������
    public int Health;
    public int Attack;
    public int Defense;
}
