using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//玩家属性 管理脚本
public class PlayerAttributesManeger : MonoBehaviour
{
    public static PlayerAttributesManeger Instance;
    public PlayerStats playerstats;
    //基本属性
    public float maxHealth { get; private set; }
    public float attack { get; private set; }
    public float defense { get; private set; }
    public float moveSpeed { get; private set; }
    public float AtkspeedTime { get; private set; }

    public float Atkrange { get; private set; }

    public float currentHealth { get; private set; }
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
        currentHealth = playerstats.Health;
        attack = playerstats.Attack;
        defense = playerstats.Defense;
        moveSpeed = playerstats.MoveSpeed;
        AtkspeedTime = playerstats.AttackTime;
        Atkrange = playerstats.AttackRange;
    }
    public List<EnemyController> enemiesInRange = new List<EnemyController>(); // 在攻击范围内的敌人列表
    private void Update()
    {
        
    }


    // 使用物品时增强玩家属性
    public void UseItem(Item item)
    {
        // 增加玩家生命值
        maxHealth += item.healthBonus;
        // 增加玩家攻击力
        attack += item.attackBonus;
        defense += item.defenseBonus;
        AtkspeedTime -= item.atkSpeed;

        Debug.Log("使用了物品：" + item.itemName);
        Debug.Log("增强了玩家生命值：" + item.healthBonus);
        Debug.Log("增强了玩家攻击力：" + item.attackBonus);
    }

    //特殊道具
    public void UseItemSpecial(Item item)
    {
        // 增加玩家生命值
        maxHealth += (item.healthBonus != 0) ? (item.healthBonus * maxHealth) : 0;

        // 增加玩家攻击力
        attack += (item.attackBonus != 0) ? (item.attackBonus * attack) : 0;

        defense += (item.defenseBonus != 0) ? (item.defenseBonus * defense) : 0;

        Debug.Log("使用了物品：" + item.itemName);
        Debug.Log("增强了玩家生命值：" + (item.healthBonus * maxHealth - maxHealth)); // 计算实际增加的生命值
        Debug.Log("增强了玩家攻击力：" + (item.attackBonus * attack - attack)); // 计算实际增加的攻击力
        Debug.Log("增强了玩家防御力：" + (item.defenseBonus * defense - defense)); // 计算实际增加的攻击力
    }

    //击败敌人增强玩家属性
    public void UseEnemyItem(EnemyStats item)
    {
        // 增加玩家生命值
        maxHealth += item.AddPlayerHealth;

        // 增加玩家攻击力
        attack += item.AddPlayerAttack;
        defense += item.AddPlayerDefense;

        Debug.Log("击杀了敌人：" + item.EnemyName);
        Debug.Log("增强了玩家生命值：" + item.AddPlayerHealth);
        Debug.Log("增强了玩家攻击力：" + item.AddPlayerAttack);
        Debug.Log("增强了玩家防御力：" + item.AddPlayerDefense);
    }

    //减少玩家血量
    public void DecPlayerHp(float damage, float Defense)
    {
        //僵直 
        //fsm.SetState(StateType.Dizzy);
        //击退
        //Knockback(Direction,knockbackForce);

        // 计算经过防御的伤害
        float damageTaken = Mathf.Max(0, damage - Defense);

        // 更新玩家的生命值
        currentHealth -= damageTaken;

    }



}
