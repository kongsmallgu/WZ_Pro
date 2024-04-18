using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//玩家属性 管理脚本
public class PlayerAttributesManeger : MonoBehaviour
{
    public static PlayerAttributesManeger Instance;
    private void Awake()
    {
        // 确保只有一个实例存在
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //初始化
        maxHealth = playerstats.Health;
        attack = playerstats.Attack;
        defense = playerstats.Defense;
        moveSpeed = playerstats.MoveSpeed;
        AtkspeedTime = playerstats.AttackTime;
    }

    public PlayerStats playerstats;

    //基本属性
    public int maxHealth;
    public int attack;
    public int defense;
    public float moveSpeed; // 角色移动速度
    public float AtkspeedTime = 1f; // 角色攻击速度

}
