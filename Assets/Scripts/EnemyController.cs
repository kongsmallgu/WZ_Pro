using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //配置文件 敌人基本属性
    public EnemyStats enemyStats;

    // 敌人状态枚举
    private enum EnemyState
    {
        Patrol,
        Track,
        meleeAttack,
        rangedAttack,
        Dead
    }

    // 当前敌人状态
    private EnemyState currentState;
    //动画状态机
    private EnemyFSMControl fsm;
    private Animator animator;
    //血条
    private float maxHealth;
    private float currentHealth;
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
    private float attackDamage;

    private float PlayerattackDamage; //玩家攻击力
    private float PlayerattackDefense; //玩家防御力

    private float enemyDefend;
    private GameObject EnemyHealthUI;
    private GameObject newEnemyHealthUI;

    //敌人是否死亡
    private bool EnemyIsDie = false;
    private bool isAttacking = false;


    //增加属性
    private float addPlayerhp;
    private float addPlayerak;
    private float addPlayerdf;

    // 巡逻范围的半径
    private float patrolRadius;
    private float patrolSpeed;
    private float trackSpeed;
    // 巡逻路径点
    private Vector3[] patrolPoints; // 巡逻路径上的点
    private int currentPatrolIndex = 0; // 当前巡逻点的索引

    // 玩家对象
    private GameObject player;
    private PlayerController playerHealth;

    //敌人死后掉落物品
    private string EnemyName;
    private GameObject enemydropObj;
    private GameObject atkenemydropObj;
    private GameObject lifeenemydropObj;

    private float dropchance;
    private float atkdropchance;
    private float lifedropchance;

    //掉落特效
    private GameObject enemydropEft;
    private GameObject atkenemydropEft;
    private GameObject lifeenemydropEft;

    //敌人技能释放
    private EnemySkillManager skillManager;
    //玩家与敌人的实时距离
    private float distanceToPlayer;
    private void Awake()
    {
        fsm = new EnemyFSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //添加状态
        fsm.AddState(EnemyStateType.Idle, new EnemyIdleState(animator, this.fsm));
        fsm.AddState(EnemyStateType.Walk, new EnemyWalkState(animator));
        fsm.AddState(EnemyStateType.FarAttacking, new EnemyFarAttackingState(animator));
        fsm.AddState(EnemyStateType.NearAttacking, new EnemyNearAttackingState(animator));
        fsm.AddState(EnemyStateType.Hit, new EnemyHitState(animator));
        fsm.AddState(EnemyStateType.Die, new EnemyDieState(animator));
        //设置状态
        fsm.SetState(EnemyStateType.Idle);

        EnemyName = enemyStats.EnemyName;
        attackDamage = enemyStats.EnemyAttack;
        meleeRange = enemyStats.MeleeRange; //近战
        rangedRange = enemyStats.RangedRange;//远战
        meleeRate = enemyStats.MeleeAttackRate; //近战攻击间隔
        rangedRate = enemyStats.RangedAttackRate; //远战攻击间隔
        enemyDefend = enemyStats.EnemyDefense;
        patrolRadius = enemyStats.PatrolRange; //巡逻范围

        patrolSpeed = enemyStats.PatrolSpeed; //巡逻速度
        patrolSpeed = enemyStats.TrackSpeed; //追踪速度
        //掉落物品
        enemydropObj = enemyStats.DropObj; // 玩家死后 掉落物品
        atkenemydropObj = enemyStats.AtkDropObj; // 玩家死后 掉落物品
        lifeenemydropObj = enemyStats.LifeDropObj; // 玩家死后 掉落物品

        dropchance = enemyStats.dropChance; //掉落概率
        atkdropchance = enemyStats.AtkdropChance; //攻击道具掉落概率
        lifedropchance = enemyStats.LifedropChance; //生命道具掉落概率
        //特效
        enemydropEft = enemyStats.DropObjEft;
        atkenemydropEft = enemyStats.AtkDropObjEft;
        lifeenemydropEft = enemyStats.LifeDropObjEft;

        //增加属性
        addPlayerhp = enemyStats.AddPlayerHealth;
        addPlayerak = enemyStats.AddPlayerAttack;
        addPlayerdf = enemyStats.AddPlayerDefense;
    }
    private void Start()
    {
        skillManager = this.gameObject.transform.GetChild(0).GetComponent<EnemySkillManager>();
        if (skillManager != null)
        {
            // 订阅技能使用事件
            skillManager.OnSkillUsed += HandleSkillUsed;
        }

        currentState = EnemyState.Patrol;
        LoadEnemyUIHealth();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerController>();
        InitializePatrolPoints();
        fsm.SetState(EnemyStateType.Idle);

    }

    private void HandleSkillUsed(string skillName, Vector3 skillPos)
    {
        // 在此处执行处理技能使用的逻辑
        Debug.Log("Enemy used skill: " + skillName + " at position: " + skillPos);
    }


    private void Update()
    {
        //状态切换条件
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //近站攻击
        if (distanceToPlayer <= meleeRange && ParameterManage.Instance.IsDie == false)
        {
            currentState = EnemyState.meleeAttack;
            if (newEnemyHealthUI != null)
            {
                newEnemyHealthUI.SetActive(true);
            }

            if (EnemyIsDie)
            {
                currentState = EnemyState.Dead;
            }
        }
        //远战攻击
        else if (distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && !ParameterManage.Instance.IsDie)
        {
            currentState = EnemyState.rangedAttack;
            if (newEnemyHealthUI != null)
            {
                newEnemyHealthUI.SetActive(true);
            }

            if (EnemyIsDie)
            {
                currentState = EnemyState.Dead;
            }
        }
        //追踪
        else if (distanceToPlayer <= patrolRadius && distanceToPlayer > rangedRange && !ParameterManage.Instance.IsDie)
        {
            currentState = EnemyState.Track;
            newEnemyHealthUI.SetActive(true);

        }
        //巡逻
        else if (distanceToPlayer > patrolRadius)
        {
            currentState = EnemyState.Patrol;
            newEnemyHealthUI.SetActive(false);
        }

        //状态切换
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolState();
                break;
            case EnemyState.Track:
                TrackState();
                break;
            case EnemyState.meleeAttack:
                meleeAttackState();
                break;
            case EnemyState.rangedAttack:
                rangeAttackState();
                break;
            case EnemyState.Dead:
                Die();
                break;
        }
    }

    private void PatrolState()
    {
        playerHealth.IsAttack = false;
        // 巡逻逻辑
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
            fsm.SetState(EnemyStateType.Walk);
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

    private void StopPatrol()
    {
        // 停止巡逻
        patrolPoints = null;
    }
    private void TrackState()
    {
        // 追踪逻辑
        StopPatrol();
        // 如果玩家不在视野范围内，则返回巡逻
        if (!PlayerInSight())
        {
            // 如果玩家不在视野范围内，则恢复巡逻
            currentState = EnemyState.Patrol;
            return;
        }
        fsm.SetState(EnemyStateType.Walk);
        Debug.Log("开始追踪===========");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.LookAt(player.transform);
        transform.position += direction * trackSpeed * Time.deltaTime;
        Debug.Log("追踪的方法|||||||||||||||||||"+ transform.position);

        playerHealth.IsAttack = false;
    }

    private bool PlayerInSight()
    {
        if (player == null)
        {
            return false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= patrolRadius * 2;
    }
    private void meleeAttackState()
    {
        if (!ParameterManage.Instance.IsDie)
        {
            // 攻击逻辑
            Debug.Log("敌人正在进行 ===================== 近 ================== 站攻击");
            ParameterManage.Instance.currentEnemyPosition = transform.position;
            ParameterManage.Instance.currentEnemyGameObject = this.gameObject;
            // 如果攻击时间已经到达，则进行攻击
            if (Time.time >= nextAttackTime && !EnemyIsDie)
            {
                isAttacking = true;
                MeleeAttack(player);
                nextAttackTime = Time.time + 1f / meleeRate; // 更新下次攻击时间
                StartCoroutine(ResetAttackStatus()); // 重置攻击状态
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }
    // 重置攻击状态
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(1f / meleeRate);
        isAttacking = false; // 重置攻击状态
    }
    private void MeleeAttack(GameObject player)
    {
        fsm.SetState(EnemyStateType.NearAttacking);
        //PlayerattackDamage = playerHealth.attack;
        //PlayerattackDefense = playerHealth.defense;
        PlayerattackDamage = PlayerAttributesManeger.Instance.attack;
        PlayerattackDefense = PlayerAttributesManeger.Instance.defense;
        // 对玩家造成伤害 以及僵直和击退效果实现

        // 计算敌人与玩家的方向向量
        Vector3 direction = (player.transform.position - transform.position).normalized;
        //对玩家的伤害
        playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);
        playerHealth.IsAttack = true;
        if (!ParameterManage.Instance.IsDie)
        {
            //对自己的伤害
            //TakeDamage(PlayerattackDamage, PlayerattackDefense);
            Debug.Log("玩家对敌人的伤害" + PlayerattackDamage);
           
        }

        // 使敌人面向玩家
        transform.LookAt(player.transform);

        Debug.Log("开始近战攻击==============================");
    }

    public void TakeDamage(float damage, float defense)
    {
        //播放敌人受击动画
        //fsm.SetState(EnemyStateType.Hit);
        // 计算经过防御的伤害

        Debug.Log("玩家造成的伤害-----------"+damage);
        float damageTaken = Mathf.Max(0, damage - defense);
        currentHealth -= damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 限制血量在0和最大血量之间
        UpdateHealthUI();
        //自己的血量为0
        if (currentHealth <= 0)
        {
            EnemyIsDie = true;
            Debug.Log("我已经死了");
            //玩家属性增加
            //player.GetComponent<PlayerController>().UseEnemyItem(enemyStats);
            PlayerAttributesManeger.Instance.UseEnemyItem(enemyStats);
        }
    }

    private void rangeAttackState()
    {
        if (!ParameterManage.Instance.IsDie)
        {
            // 攻击逻辑
            Debug.Log("敌人正在进行 ============ 远 ====== 站攻击");
            ParameterManage.Instance.currentEnemyPosition = transform.position;
            ParameterManage.Instance.currentEnemyGameObject = this.gameObject;
            // 如果攻击时间已经到达，则进行攻击
            if (Time.time >= nextAttackTime && !EnemyIsDie)
            {
                isAttacking = true;
                RangeAttack(player);
                nextAttackTime = Time.time + 1f / meleeRate; // 更新下次攻击时间
                StartCoroutine(ResetAttackStatus()); // 重置攻击状态
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }      
    }

    private void RangeAttack(GameObject player)
    {
        fsm.SetState(EnemyStateType.FarAttacking);

        /*    PlayerattackDamage = playerHealth.attack;
            PlayerattackDefense = playerHealth.defense;*/

        PlayerattackDamage = PlayerAttributesManeger.Instance.attack;
        PlayerattackDefense = PlayerAttributesManeger.Instance.defense;
        // 对玩家造成伤害 以及僵直和击退效果实现
        // 计算敌人与玩家的方向向量
        Vector3 direction = (player.transform.position - transform.position).normalized;
        //对玩家的伤害
        playerHealth.TakeDamage(attackDamage-5, enemyDefend, direction, 0.5f);
        playerHealth.IsAttack = true;
        if (!ParameterManage.Instance.IsDie)
        {
            //对自己的伤害
            //TakeDamage(PlayerattackDamage, PlayerattackDefense);
            Debug.Log("玩家对敌人的伤害" + PlayerattackDamage);

        }

        // 使敌人面向玩家
        transform.LookAt(player.transform);

        Debug.Log("开始远战攻击==============================");
    }

    private void InitializePatrolPoints()
    {
        int numPatrolPoints = 10;
        patrolPoints = new Vector3[numPatrolPoints];
        for (int i = 0; i < numPatrolPoints; i++)
        {
            float angle = i * 2f * Mathf.PI / numPatrolPoints;
            patrolPoints[i] = new Vector3(transform.position.x + Mathf.Cos(angle) * patrolRadius, transform.position.y, transform.position.z + Mathf.Sin(angle) * patrolRadius);
        }
    }

    //血条显示
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
    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
    void Die()
    {  
        playerHealth.IsAttack = false;
        fsm.SetState(EnemyStateType.Die);
       
        //血条预制体也消失
        Destroy(newEnemyHealthUI);
        Debug.Log("我作为一个敌人 已经死啦 不要打啦");
        
        DropDownItem(dropchance, atkdropchance,lifedropchance);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    //掉落道具
    public void DropDownItem(float dropChance, float AtkdropChance, float LifeDropChance) 
    {
        float dropValue = Random.value;
        float atkDropChance = dropChance + AtkdropChance;
        float survivalDropChance = atkDropChance + LifeDropChance;
        Debug.Log("道具随机掉落概率为================" + dropValue);
        // 随机决定是否掉落道具
        // 一般道具
        if (dropValue < dropChance)
        {
            Debug.Log("掉落一般道具");
            string enemydropObjName = enemydropObj.name;
            //加载预制体
            GameObject itemPrefab = Resources.Load<GameObject>("ItemGoods/DropIteam/" + enemydropObjName);

            // 生成道具对象
            Instantiate(itemPrefab, transform.position, Quaternion.identity);

            string enemydropEftName = enemydropEft.name;
            GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/DropIteamEffect/" + enemydropEftName);
            // 播放死亡特效
            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f); // 特效播放结束后销毁
            }
        }
        // 特殊攻击道具
        else if (dropValue < atkDropChance)
        {
            Debug.Log("掉落特殊攻击道具");
            //加载预制体
            GameObject specialAttackPrefab = Resources.Load<GameObject>("ItemGoods/SpecialAttackItem/" + atkenemydropObj.name);

            // 生成特殊攻击道具对象
            Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);

            GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/SpecialAttackItemEffect/" + atkenemydropEft.name);
            // 播放死亡特效
            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f); // 特效播放结束后销毁
            }
        }
        // 特殊生存道具
        else
        {
            Debug.Log("掉落特殊生存道具");
            //加载预制体
            GameObject survivalItemPrefab = Resources.Load<GameObject>("ItemGoods/SurvivalItem/" + lifeenemydropObj.name);

            // 生成特殊生存道具对象
            Instantiate(survivalItemPrefab, transform.position, Quaternion.identity);

            GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/SurvivalItemEffect/" + lifeenemydropEft.name);
            // 播放死亡特效
            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f); // 特效播放结束后销毁
            }
        }

    }
}
