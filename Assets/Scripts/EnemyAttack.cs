using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
/*    public EnemyHealth enemyhealth;
    public EnemyStats enemyStats;
    // ������Χ
    public float attackRange = 2f;

    // ������
    public int attackDamage;

    private int PlayerattackDamage;

    // �������
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    private void Start()
    {
        attackDamage = enemyStats.EnemyAttack;
    }

    private void Update()
    {
        PlayerattackDamage = ParameterManage.Instance.player_attack;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            // �������Ƿ��ڹ�����Χ��
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange && ParameterManage.Instance.IsDie == false)
            {
                // �������ʱ���Ѿ��������й���
                if (Time.time >= nextAttackTime)
                {
                    Attack(player);
                    nextAttackTime = Time.time + 1f / attackRate; // �����´ι���ʱ��
                }
              
            }
            else 
            {
                ParameterManage.Instance.IsAttack = false;
            }
        }
    }

    void Attack(GameObject player)
    {
        // �������Ƿ���� PlayerController ���
        PlayerController playerHealth = player.GetComponent<PlayerController>();
        if (playerHealth != null)
        {
            // ���������˺�
            playerHealth.TakeDamage(attackDamage);

            ParameterManage.Instance.IsAttack = true;


            if (!ParameterManage.Instance.IsDie)
            {
                //���Լ����˺�
                enemyhealth.TakeDamage(PlayerattackDamage);
                Debug.Log("��ҶԵ��˵��˺�" + PlayerattackDamage);
            }

        }
        Debug.Log("��ʼ����");
    }

    // �� Scene ��ͼ����ʾ������Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/
}
