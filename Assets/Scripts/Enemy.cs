using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //配置文件 敌人基本属性
    public EnemyStats enemyStats;

    //血条
    private int maxHealth;
    private int currentHealth;
    private Text Enemyname;
    private Slider healthSlider;    
    // 攻击
    private float attackRange = 0f;
    private float meleeRange;//近战
    private float rangedRange;//远战
    // 攻击间隔
    private float meleeRate = 0f;
    private float rangedRate = 0f;
    private float nextAttackTime = 0f;
    private int attackDamage;

    private int PlayerattackDamage; //玩家攻击力
    private int PlayerattackDefense; //玩家防御力

    private int enemyDefend;
    private GameObject EnemyHealthUI;
    private GameObject newEnemyHealthUI;
    //敌人是否死亡
    private bool EnemyIsDie = false;
    private bool isAttacking = false;

    //增加属性
    private float addPlayerhp;
    private float addPlayerak;
    private float addPlayerdf;

    private GameObject player;
    //动画状态机
    private EnemyFSMControl fsm;
    private Animator animator;
    // 巡逻范围的半径
    private float patrolRadius; 
    private float patrolSpeed;
    private float trackSpeed;

    //敌人巡逻与追踪状态
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyState currentState = EnemyState.Patrol;
    private float chaseRange;
    private Vector3[] patrolPoints; // 巡逻路径上的点
    private int currentPatrolIndex = 0; // 当前巡逻点的索引

    private void Awake()
    {
        fsm = new EnemyFSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //添加状态
        fsm.AddState(EnemyStateType.Idle, new EnemyIdleState(animator, this.fsm));
        fsm.AddState(EnemyStateType.Walk, new EnemyWalkState(animator));
       /* fsm.AddState(EnemyStateType.Attacking, new EnemyAttackingState(animator));*/
        fsm.AddState(EnemyStateType.Hit, new EnemyHitState(animator));
        fsm.AddState(EnemyStateType.Die, new EnemyDieState(animator));
        //设置状态
        fsm.SetState(EnemyStateType.Idle);

        attackDamage = enemyStats.EnemyAttack;
        meleeRange = enemyStats.MeleeRange; //近战
        rangedRange = enemyStats.RangedRange;//远战
        meleeRate = enemyStats.MeleeAttackRate; //近战攻击间隔
        rangedRate = enemyStats.RangedAttackRate; //远战攻击间隔
        enemyDefend = enemyStats.EnemyDefense;
        patrolRadius = enemyStats.PatrolRange; //巡逻范围
       
        patrolSpeed = enemyStats.PatrolSpeed; //巡逻速度
        patrolSpeed = enemyStats.TrackSpeed; //追踪速度

       //增加属性
        addPlayerhp = enemyStats.AddPlayerHealth;
        addPlayerak = enemyStats.AddPlayerAttack;
        addPlayerdf = enemyStats.AddPlayerDefense;

    }
    private void Start()
    {
        LoadEnemyUIHealth();
        player = GameObject.FindGameObjectWithTag("Player");
        // 初始化巡逻路径上的点，以敌人初始位置为中心画圆
        int numPatrolPoints = 10;
        patrolPoints = new Vector3[numPatrolPoints];
        for (int i = 0; i < numPatrolPoints; i++)
        {
            float angle = i * 2f * Mathf.PI / numPatrolPoints;
            patrolPoints[i] = new Vector3(transform.position.x + Mathf.Cos(angle) * patrolRadius, transform.position.y, transform.position.z + Mathf.Sin(angle) * patrolRadius);
        }
    }

    private void Update()
    {
        if (isAttacking) return; // 如果已经有敌人在攻击，则返回
    
        // 检测玩家是否存在
        if (player != null)
        {
            fsm.SetState(EnemyStateType.Walk);
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            // 检测玩家是否在攻击范围内
            if (distanceToPlayer <= meleeRange && ParameterManage.Instance.IsDie == false)
            {
                newEnemyHealthUI.SetActive(true);
                // 传出敌人的位置信息
                Debug.Log("传出敌人的位置信息" + transform.position);
                ParameterManage.Instance.currentEnemyPosition = transform.position;

                // 如果攻击时间已经到达，则进行攻击
                if (Time.time >= nextAttackTime && !EnemyIsDie)
                {
                    isAttacking = true;
                    MeleeAttack(player);
                    nextAttackTime = Time.time + 1f / meleeRate; // 更新下次攻击时间
                    StartCoroutine(ResetAttackStatus()); // 重置攻击状态
                }

                // 停止巡逻，切换为追踪模式
                StopPatrol();
            }
            else if (distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && !ParameterManage.Instance.IsDie)
            {
                newEnemyHealthUI.SetActive(true);
                // 传出敌人的位置信息
                Debug.Log("传出敌人的位置信息" + transform.position + "==========================");
                ParameterManage.Instance.currentEnemyPosition = transform.position;
                // 远程攻击逻辑
                if (Time.time >= nextAttackTime && !EnemyIsDie)
                {
                    isAttacking = true;
                    RangedAttack(player); // 进行远程攻击
                    nextAttackTime = Time.time + 1f / rangedRate; // 更新下次攻击时间
                    StartCoroutine(ResetAttackStatus()); // 重置攻击状态
                }
            }
            else
            {
                ParameterManage.Instance.IsAttack = false;
                newEnemyHealthUI.SetActive(false);               
                if (PlayerInSight())
                {
                    // 如果玩家进入视野范围，则开始追踪
                    TrackPlayer();
                }
                else
                {
                    // 如果玩家不在视野范围内，则恢复巡逻
                    ResumePatrol();
                    
                }
            }
        }
        else
        {
            Debug.LogError("未找到玩家对象！");
        }
    }

    // 重置攻击状态
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(1f / meleeRate);
        isAttacking = false; // 重置攻击状态
    }
    private void LoadEnemyUIHealth() 
    {
        //加载血条预制体
        EnemyHealthUI = Resources.Load<GameObject>("EnemyHealthCanvas");

        if (EnemyHealthUI != null)
        {
            // 实例化预制体，设置父对象为 content
            newEnemyHealthUI = Instantiate(EnemyHealthUI);
            Enemyname = newEnemyHealthUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            healthSlider = newEnemyHealthUI.transform.GetChild(0).GetChild(1).GetComponent<Slider>();

        }
        else
        {
            Debug.LogWarning("找不到预制体：EnemyHealthCanvas");
        }
        Enemyname.text = enemyStats.EnemyName;
        maxHealth = enemyStats.EnemyHealth;
        currentHealth = maxHealth;
        UpdateHealthUI(); //敌人血条以及名称显示
    }


    // 在 Scene 视图中显示攻击范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        // 在编辑器中绘制巡逻范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
    // 近战攻击方法
    private void MeleeAttack(GameObject player)
    {
        /*fsm.SetState(EnemyStateType.Attacking);*/
        // 检测玩家是否具有 PlayerController 组件
        PlayerController playerHealth = player.GetComponent<PlayerController>();

        /*PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;*/

        if (playerHealth != null)
        {
            // 对玩家造成伤害 以及僵直和击退效果实现
            // 计算敌人与玩家的方向向量
            Vector3 direction = (player.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);

            ParameterManage.Instance.IsAttack = true;

            if (!ParameterManage.Instance.IsDie)
            {
                //对自己的伤害
                TakeDamage(PlayerattackDamage, PlayerattackDefense);
                Debug.Log("玩家对敌人的伤害" + PlayerattackDamage);
            }
        }
        // 使敌人面向玩家
        transform.LookAt(player.transform);

        Debug.Log("开始近战攻击==============================");
    }

    // 远程攻击方法
    private void RangedAttack(GameObject player)
    {
        //播放敌人受击动画
        /*fsm.SetState(EnemyStateType.Attacking);*/
        // 检测玩家是否具有 PlayerController 组件
        PlayerController playerHealth = player.GetComponent<PlayerController>();
       /* PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;*/
        if (playerHealth != null)
        {
            // 对玩家造成伤害
            Vector3 direction = (player.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);
            ParameterManage.Instance.IsAttack = true;

            if (!ParameterManage.Instance.IsDie)
            {
                //对自己的伤害
                TakeDamage(PlayerattackDamage, PlayerattackDefense);
                Debug.Log("玩家对敌人的伤害" + PlayerattackDamage);
            }
        }
        // 使敌人面向玩家
        transform.LookAt(player.transform);

        Debug.Log("开始远战攻击");
    }
    public void TakeDamage(int damage,int defense)
    {
        //播放敌人受击动画
        //fsm.SetState(EnemyStateType.Hit);
        // 计算经过防御的伤害
        int damageTaken = Mathf.Max(0, damage - defense);
        currentHealth -= damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 限制血量在0和最大血量之间
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            ParameterManage.Instance.IsAttack = false;
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        EnemyIsDie = true;
        // 死亡逻辑
        Destroy(gameObject);
        //血条预制体也消失
        Destroy(newEnemyHealthUI);
        //玩家属性增加
        //player.GetComponent<PlayerController>().UseEnemyItem(enemyStats);

    }

    //设置巡逻逻辑

    private void StopPatrol()
    {
        // 停止巡逻
        patrolPoints = null;
    }

    private void ResumePatrol()
    {
        // 恢复巡逻
        if (patrolPoints == null)
        {
            ParameterManage.Instance.IsAttack = false;
            // 重新生成巡逻路径
            int numPatrolPoints = 10;
            patrolPoints = new Vector3[numPatrolPoints];
            for (int i = 0; i < numPatrolPoints; i++)
            {
                float angle = i * 2f * Mathf.PI / numPatrolPoints;
                patrolPoints[i] = new Vector3(transform.position.x + Mathf.Cos(angle) * patrolRadius, transform.position.y, transform.position.z + Mathf.Sin(angle) * patrolRadius);
            }
        }
        else
        {
            // 沿着巡逻路径移动
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
            {
                // 到达当前巡逻点，前往下一个巡逻点
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }

            // 计算朝向当前巡逻点的方向
            Vector3 direction = (patrolPoints[currentPatrolIndex] - transform.position).normalized;
            // 使敌人面向当前巡逻点
            transform.LookAt(patrolPoints[currentPatrolIndex]);
            // 向当前巡逻点移动
            transform.position += direction * patrolSpeed * Time.deltaTime;
        }
    }

    //追踪
    private void TrackPlayer()
    {
        // 如果玩家不在视野范围内，则返回巡逻
        if (!PlayerInSight())
        {
            // 如果玩家不在视野范围内，则恢复巡逻
            ResumePatrol();
            return;
        }

        Debug.Log("开始追踪===========");
        // 如果玩家在视野范围内，则朝向玩家并向其移动
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.LookAt(player.transform);
        transform.position += direction * trackSpeed * Time.deltaTime;

    }

    // 检测玩家是否在视野范围内
    private bool PlayerInSight()
    {
        if (player == null)
        {
            return false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= patrolRadius*2;
    }


}
