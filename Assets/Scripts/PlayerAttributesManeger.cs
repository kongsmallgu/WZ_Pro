using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������� ����ű�
public class PlayerAttributesManeger : MonoBehaviour
{
    public static PlayerAttributesManeger Instance;
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

    public PlayerStats playerstats;

    //��������
    public int maxHealth;
    public int attack;
    public int defense;
    public float moveSpeed; // ��ɫ�ƶ��ٶ�
    public float AtkspeedTime = 1f; // ��ɫ�����ٶ�

}
