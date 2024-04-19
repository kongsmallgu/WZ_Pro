using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������� ����ű�
public class PlayerAttributesManeger : MonoBehaviour
{
    public static PlayerAttributesManeger Instance;
    public PlayerStats playerstats;

    //��������
    public float maxHealth { get; private set; }
    public float attack { get; private set; }
    public float defense { get; private set; }
    public float moveSpeed { get; private set; }
    public float AtkspeedTime { get; private set; }

    private float currentHealth;
    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //��ʼ��
        maxHealth = playerstats.Health;
        attack = playerstats.Attack;
        defense = playerstats.Defense;
        moveSpeed = playerstats.MoveSpeed;
        AtkspeedTime = playerstats.AttackTime;
    }

    private void Update()
    {
        
    }


    // ʹ����Ʒʱ��ǿ�������
    public void UseItem(Item item)
    {
        // �����������ֵ
        maxHealth += item.healthBonus;
        // ������ҹ�����
        attack += item.attackBonus;
        defense += item.defenseBonus;
        AtkspeedTime -= item.atkSpeed;

        Debug.Log("ʹ������Ʒ��" + item.itemName);
        Debug.Log("��ǿ���������ֵ��" + item.healthBonus);
        Debug.Log("��ǿ����ҹ�������" + item.attackBonus);
    }

    //�������
    public void UseItemSpecial(Item item)
    {
        // �����������ֵ
        maxHealth += (item.healthBonus != 0) ? (item.healthBonus * maxHealth) : 0;

        // ������ҹ�����
        attack += (item.attackBonus != 0) ? (item.attackBonus * attack) : 0;

        defense += (item.defenseBonus != 0) ? (item.defenseBonus * defense) : 0;

        Debug.Log("ʹ������Ʒ��" + item.itemName);
        Debug.Log("��ǿ���������ֵ��" + (item.healthBonus * maxHealth - maxHealth)); // ����ʵ�����ӵ�����ֵ
        Debug.Log("��ǿ����ҹ�������" + (item.attackBonus * attack - attack)); // ����ʵ�����ӵĹ�����
        Debug.Log("��ǿ����ҷ�������" + (item.defenseBonus * defense - defense)); // ����ʵ�����ӵĹ�����
    }

    //���ܵ�����ǿ�������
    public void UseEnemyItem(EnemyStats item)
    {
        // �����������ֵ
        maxHealth += item.AddPlayerHealth;

        // ������ҹ�����
        attack += item.AddPlayerAttack;
        defense += item.AddPlayerDefense;

        Debug.Log("��ɱ�˵��ˣ�" + item.EnemyName);
        Debug.Log("��ǿ���������ֵ��" + item.AddPlayerHealth);
        Debug.Log("��ǿ����ҹ�������" + item.AddPlayerAttack);
        Debug.Log("��ǿ����ҷ�������" + item.AddPlayerDefense);
    }


}
