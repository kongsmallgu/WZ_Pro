using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //�����ļ� ���˻�������
    public EnemyStats enemyStats;

    // ����״̬ö��
    private enum EnemyState
    {
        Patrol,
        Track,
        meleeAttack,
        rangedAttack,
        Dead
    }

    // ��ǰ����״̬
    private EnemyState currentState;
    //����״̬��
    private EnemyFSMControl fsm;
    private Animator animator;
    //Ѫ��
    private int maxHealth;
    private int currentHealth;
    private Text Enemyname;
    private Slider healthSlider;

    // ����
    private float attackRange = 0f;
    private float meleeRange;//��ս
    private float rangedRange;//Զս
    // �������
    private float meleeRate = 0f;
    private float rangedRate = 0f;
    private float nextAttackTime = 0f;
    private int attackDamage;

    private int PlayerattackDamage; //��ҹ�����
    private int PlayerattackDefense; //��ҷ�����

    private int enemyDefend;
    private GameObject EnemyHealthUI;
    private GameObject newEnemyHealthUI;

    //�����Ƿ�����
    private bool EnemyIsDie = false;
    private bool isAttacking = false;


    //��������
    private float addPlayerhp;
    private float addPlayerak;
    private float addPlayerdf;

    // Ѳ�߷�Χ�İ뾶
    private float patrolRadius;
    private float patrolSpeed;
    private float trackSpeed;
    // Ѳ��·����
    private Vector3[] patrolPoints; // Ѳ��·���ϵĵ�
    private int currentPatrolIndex = 0; // ��ǰѲ�ߵ������

    // ��Ҷ���
    private GameObject player;
    private PlayerController playerHealth;

    //�������������Ʒ
    private string EnemyName;
    private GameObject enemydropObj;
    private float dropchance;

    //���˼����ͷ�
    private EnemySkillManager skillManager;
    //�������˵�ʵʱ����
    private float distanceToPlayer;
    private void Awake()
    {
        fsm = new EnemyFSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //����״̬
        fsm.AddState(EnemyStateType.Idle, new EnemyIdleState(animator, this.fsm));
        fsm.AddState(EnemyStateType.Walk, new EnemyWalkState(animator));
        fsm.AddState(EnemyStateType.FarAttacking, new EnemyFarAttackingState(animator));
        fsm.AddState(EnemyStateType.NearAttacking, new EnemyNearAttackingState(animator));
        fsm.AddState(EnemyStateType.Hit, new EnemyHitState(animator));
        fsm.AddState(EnemyStateType.Die, new EnemyDieState(animator));
        //����״̬
        fsm.SetState(EnemyStateType.Idle);

        EnemyName = enemyStats.EnemyName;
        attackDamage = enemyStats.EnemyAttack;
        meleeRange = enemyStats.MeleeRange; //��ս
        rangedRange = enemyStats.RangedRange;//Զս
        meleeRate = enemyStats.MeleeAttackRate; //��ս�������
        rangedRate = enemyStats.RangedAttackRate; //Զս�������
        enemyDefend = enemyStats.EnemyDefense;
        patrolRadius = enemyStats.PatrolRange; //Ѳ�߷�Χ

        patrolSpeed = enemyStats.PatrolSpeed; //Ѳ���ٶ�
        patrolSpeed = enemyStats.TrackSpeed; //׷���ٶ�
        enemydropObj = enemyStats.DropObj; // ������� ������Ʒ
        dropchance = enemyStats.dropChance; //�������

        //��������
        addPlayerhp = enemyStats.AddPlayerHealth;
        addPlayerak = enemyStats.AddPlayerAttack;
        addPlayerdf = enemyStats.AddPlayerDefense;
    }
    private void Start()
    {
        skillManager = this.gameObject.transform.GetChild(0).GetComponent<EnemySkillManager>();
        if (skillManager != null)
        {
            // ���ļ���ʹ���¼�
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
        // �ڴ˴�ִ�д�������ʹ�õ��߼�
        Debug.Log("Enemy used skill: " + skillName + " at position: " + skillPos);
    }


    private void Update()
    {
        //״̬�л�����
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //��վ����
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
        //Զս����
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
        //׷��
        else if (distanceToPlayer <= patrolRadius && distanceToPlayer > rangedRange && !ParameterManage.Instance.IsDie)
        {
            currentState = EnemyState.Track;
            newEnemyHealthUI.SetActive(true);

        }
        //Ѳ��
        else if (distanceToPlayer > patrolRadius)
        {
            currentState = EnemyState.Patrol;
            newEnemyHealthUI.SetActive(false);
        }

        //״̬�л�
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
        // Ѳ���߼�
        if (patrolPoints == null)
        {
            ParameterManage.Instance.IsAttack = false;
            // ��������Ѳ��·��
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
            // ����Ѳ��·���ƶ�
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
            {
                // ���ﵱǰѲ�ߵ㣬ǰ����һ��Ѳ�ߵ�
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }

            // ���㳯��ǰѲ�ߵ�ķ���
            Vector3 direction = (patrolPoints[currentPatrolIndex] - transform.position).normalized;
            // ʹ��������ǰѲ�ߵ�
            transform.LookAt(patrolPoints[currentPatrolIndex]);
            // ��ǰѲ�ߵ��ƶ�
            transform.position += direction * patrolSpeed * Time.deltaTime;
        }
    }

    private void StopPatrol()
    {
        // ֹͣѲ��
        patrolPoints = null;
    }
    private void TrackState()
    {
        // ׷���߼�
        StopPatrol();
        // �����Ҳ�����Ұ��Χ�ڣ��򷵻�Ѳ��
        if (!PlayerInSight())
        {
            // �����Ҳ�����Ұ��Χ�ڣ���ָ�Ѳ��
            currentState = EnemyState.Patrol;
            return;
        }
        fsm.SetState(EnemyStateType.Walk);
        Debug.Log("��ʼ׷��===========");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.LookAt(player.transform);
        transform.position += direction * trackSpeed * Time.deltaTime;
        Debug.Log("׷�ٵķ���|||||||||||||||||||"+ transform.position);

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
        // �����߼�
        Debug.Log("�������ڽ��� ===================== �� ================== վ����");
        ParameterManage.Instance.currentEnemyPosition = transform.position;

        // �������ʱ���Ѿ��������й���
        if (Time.time >= nextAttackTime && !EnemyIsDie)
        {
            isAttacking = true;
            MeleeAttack(player);
            nextAttackTime = Time.time + 1f / meleeRate; // �����´ι���ʱ��
            StartCoroutine(ResetAttackStatus()); // ���ù���״̬
        }

    }
    // ���ù���״̬
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(1f / meleeRate);
        isAttacking = false; // ���ù���״̬
    }
    private void MeleeAttack(GameObject player)
    {
        fsm.SetState(EnemyStateType.NearAttacking);
        PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;
       
        // ���������˺� �Լ���ֱ�ͻ���Ч��ʵ��
        // �����������ҵķ�������
        Vector3 direction = (player.transform.position - transform.position).normalized;
        //����ҵ��˺�
        playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);
        playerHealth.IsAttack = true;
        if (!ParameterManage.Instance.IsDie)
        {
            //���Լ����˺�
            TakeDamage(PlayerattackDamage, PlayerattackDefense);
            Debug.Log("��ҶԵ��˵��˺�" + PlayerattackDamage);
           
        }

        // ʹ�����������
        transform.LookAt(player.transform);

        Debug.Log("��ʼ��ս����==============================");
    }
    public void TakeDamage(int damage, int defense)
    {
        //���ŵ����ܻ�����
        //fsm.SetState(EnemyStateType.Hit);
        // ���㾭���������˺�
        int damageTaken = Mathf.Max(0, damage - defense);
        currentHealth -= damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ����Ѫ����0�����Ѫ��֮��
        UpdateHealthUI();
        //�Լ���Ѫ��Ϊ0
        if (currentHealth <= 0)
        {
            EnemyIsDie = true;
            Debug.Log("���Ѿ�����");
            //�����������
            player.GetComponent<PlayerController>().UseEnemyItem(enemyStats);
        }
    }

    private void rangeAttackState()
    {
        // �����߼�
        Debug.Log("�������ڽ��� ============ Զ ====== վ����");
        ParameterManage.Instance.currentEnemyPosition = transform.position;

        // �������ʱ���Ѿ��������й���
        if (Time.time >= nextAttackTime && !EnemyIsDie)
        {
            isAttacking = true;
            RangeAttack(player);
            nextAttackTime = Time.time + 1f / meleeRate; // �����´ι���ʱ��
            StartCoroutine(ResetAttackStatus()); // ���ù���״̬
        }
    }

    private void RangeAttack(GameObject player)
    {
        fsm.SetState(EnemyStateType.FarAttacking);

        PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;
        // ���������˺� �Լ���ֱ�ͻ���Ч��ʵ��
        // �����������ҵķ�������
        Vector3 direction = (player.transform.position - transform.position).normalized;
        //����ҵ��˺�
        playerHealth.TakeDamage(attackDamage-5, enemyDefend, direction, 0.5f);
        playerHealth.IsAttack = true;
        if (!ParameterManage.Instance.IsDie)
        {
            //���Լ����˺�
            TakeDamage(PlayerattackDamage, PlayerattackDefense);
            Debug.Log("��ҶԵ��˵��˺�" + PlayerattackDamage);

        }

        // ʹ�����������
        transform.LookAt(player.transform);

        Debug.Log("��ʼԶս����==============================");
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

    //Ѫ����ʾ
    private void LoadEnemyUIHealth()
    {
        //����Ѫ��Ԥ����
        EnemyHealthUI = Resources.Load<GameObject>("EnemyHealthCanvas");

        if (EnemyHealthUI != null)
        {
            // ʵ����Ԥ���壬���ø�����Ϊ content
            newEnemyHealthUI = Instantiate(EnemyHealthUI);
            Enemyname = newEnemyHealthUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            healthSlider = newEnemyHealthUI.transform.GetChild(0).GetChild(1).GetComponent<Slider>();

        }
        else
        {
            Debug.LogWarning("�Ҳ���Ԥ���壺EnemyHealthCanvas");
        }
        Enemyname.text = enemyStats.EnemyName;
        maxHealth = enemyStats.EnemyHealth;
        currentHealth = maxHealth;
        UpdateHealthUI(); //����Ѫ���Լ�������ʾ
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
       
        //Ѫ��Ԥ����Ҳ��ʧ
        Destroy(newEnemyHealthUI);
        Debug.Log("����Ϊһ������ �Ѿ����� ��Ҫ����");
        
        string enemydropObjName = enemydropObj.name;
        // ��������Ƿ�������
        if (Random.value < dropchance)
        {
            //����Ԥ����
            GameObject itemPrefab = Resources.Load<GameObject>("DropIteam/"+ enemydropObjName);
            
            // ���ɵ��߶���
            Instantiate(itemPrefab, transform.position, Quaternion.identity);

            GameObject pickupEffectPrefab = Resources.Load<GameObject>("DropIteamEffect/" + enemydropObjName);
            // ����������Ч
            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f); // ��Ч���Ž���������
            }

        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public void DropDownItem(float dropChance, float AtkdropChance, string enemyDropObjName) 
    {
        float dropValue = Random.value;
        // ��������Ƿ�������
        if (dropValue < 1)
        {
            // һ�����
            if (dropValue < dropChance)
            {
                //����Ԥ����
                GameObject itemPrefab = Resources.Load<GameObject>("ItemGoods/DropItem/" + enemyDropObjName);

                // ���ɵ��߶���
                Instantiate(itemPrefab, transform.position, Quaternion.identity);

                GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/DropItemEffect/" + enemyDropObjName);
                // ����������Ч
                if (pickupEffectPrefab != null)
                {
                    GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, 2f); // ��Ч���Ž���������
                }
            }
            // ���⹥������
            else if (dropValue < AtkdropChance)
            {
                //����Ԥ����
                GameObject specialAttackPrefab = Resources.Load<GameObject>("ItemGoods/SpecialAttackItem/" + enemyDropObjName);

                // �������⹥�����߶���
                Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);

                GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/SpecialAttackItemEffect/" + enemyDropObjName);
                // ����������Ч
                if (pickupEffectPrefab != null)
                {
                    GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, 2f); // ��Ч���Ž���������
                }
            }
            // �����������
            else
            {
                //����Ԥ����
                GameObject survivalItemPrefab = Resources.Load<GameObject>("ItemGoods/SurvivalItem/" + enemyDropObjName);

                // ��������������߶���
                Instantiate(survivalItemPrefab, transform.position, Quaternion.identity);

                GameObject pickupEffectPrefab = Resources.Load<GameObject>("ItemGoods/SurvivalItemEffect/" + enemyDropObjName);
                // ����������Ч
                if (pickupEffectPrefab != null)
                {
                    GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, 2f); // ��Ч���Ž���������
                }
            }
        }



    }
}