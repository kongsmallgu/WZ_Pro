using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
/*    public EnemyHealth enemyhealth;
    public EnemyStats enemyStats;
    // 攻击范围
    public float attackRange = 2f;

    // 攻击力
    public int attackDamage;

    private int PlayerattackDamage;

    // 攻击间隔
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
            // 检测玩家是否在攻击范围内
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange && ParameterManage.Instance.IsDie == false)
            {
                // 如果攻击时间已经到达，则进行攻击
                if (Time.time >= nextAttackTime)
                {
                    Attack(player);
                    nextAttackTime = Time.time + 1f / attackRate; // 更新下次攻击时间
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
        // 检测玩家是否具有 PlayerController 组件
        PlayerController playerHealth = player.GetComponent<PlayerController>();
        if (playerHealth != null)
        {
            // 对玩家造成伤害
            playerHealth.TakeDamage(attackDamage);

            ParameterManage.Instance.IsAttack = true;


            if (!ParameterManage.Instance.IsDie)
            {
                //对自己的伤害
                enemyhealth.TakeDamage(PlayerattackDamage);
                Debug.Log("玩家对敌人的伤害" + PlayerattackDamage);
            }

        }
        Debug.Log("开始攻击");
    }

    // 在 Scene 视图中显示攻击范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/
}
