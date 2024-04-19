using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //�����ļ� ���˻�������
    public EnemyStats enemyStats;

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

    private GameObject player;
    //����״̬��
    private EnemyFSMControl fsm;
    private Animator animator;
    // Ѳ�߷�Χ�İ뾶
    private float patrolRadius; 
    private float patrolSpeed;
    private float trackSpeed;

    //����Ѳ����׷��״̬
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyState currentState = EnemyState.Patrol;
    private float chaseRange;
    private Vector3[] patrolPoints; // Ѳ��·���ϵĵ�
    private int currentPatrolIndex = 0; // ��ǰѲ�ߵ������

    private void Awake()
    {
        fsm = new EnemyFSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //���״̬
        fsm.AddState(EnemyStateType.Idle, new EnemyIdleState(animator, this.fsm));
        fsm.AddState(EnemyStateType.Walk, new EnemyWalkState(animator));
       /* fsm.AddState(EnemyStateType.Attacking, new EnemyAttackingState(animator));*/
        fsm.AddState(EnemyStateType.Hit, new EnemyHitState(animator));
        fsm.AddState(EnemyStateType.Die, new EnemyDieState(animator));
        //����״̬
        fsm.SetState(EnemyStateType.Idle);

        attackDamage = enemyStats.EnemyAttack;
        meleeRange = enemyStats.MeleeRange; //��ս
        rangedRange = enemyStats.RangedRange;//Զս
        meleeRate = enemyStats.MeleeAttackRate; //��ս�������
        rangedRate = enemyStats.RangedAttackRate; //Զս�������
        enemyDefend = enemyStats.EnemyDefense;
        patrolRadius = enemyStats.PatrolRange; //Ѳ�߷�Χ
       
        patrolSpeed = enemyStats.PatrolSpeed; //Ѳ���ٶ�
        patrolSpeed = enemyStats.TrackSpeed; //׷���ٶ�

       //��������
        addPlayerhp = enemyStats.AddPlayerHealth;
        addPlayerak = enemyStats.AddPlayerAttack;
        addPlayerdf = enemyStats.AddPlayerDefense;

    }
    private void Start()
    {
        LoadEnemyUIHealth();
        player = GameObject.FindGameObjectWithTag("Player");
        // ��ʼ��Ѳ��·���ϵĵ㣬�Ե��˳�ʼλ��Ϊ���Ļ�Բ
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
        if (isAttacking) return; // ����Ѿ��е����ڹ������򷵻�
    
        // �������Ƿ����
        if (player != null)
        {
            fsm.SetState(EnemyStateType.Walk);
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            // �������Ƿ��ڹ�����Χ��
            if (distanceToPlayer <= meleeRange && ParameterManage.Instance.IsDie == false)
            {
                newEnemyHealthUI.SetActive(true);
                // �������˵�λ����Ϣ
                Debug.Log("�������˵�λ����Ϣ" + transform.position);
                ParameterManage.Instance.currentEnemyPosition = transform.position;

                // �������ʱ���Ѿ��������й���
                if (Time.time >= nextAttackTime && !EnemyIsDie)
                {
                    isAttacking = true;
                    MeleeAttack(player);
                    nextAttackTime = Time.time + 1f / meleeRate; // �����´ι���ʱ��
                    StartCoroutine(ResetAttackStatus()); // ���ù���״̬
                }

                // ֹͣѲ�ߣ��л�Ϊ׷��ģʽ
                StopPatrol();
            }
            else if (distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && !ParameterManage.Instance.IsDie)
            {
                newEnemyHealthUI.SetActive(true);
                // �������˵�λ����Ϣ
                Debug.Log("�������˵�λ����Ϣ" + transform.position + "==========================");
                ParameterManage.Instance.currentEnemyPosition = transform.position;
                // Զ�̹����߼�
                if (Time.time >= nextAttackTime && !EnemyIsDie)
                {
                    isAttacking = true;
                    RangedAttack(player); // ����Զ�̹���
                    nextAttackTime = Time.time + 1f / rangedRate; // �����´ι���ʱ��
                    StartCoroutine(ResetAttackStatus()); // ���ù���״̬
                }
            }
            else
            {
                ParameterManage.Instance.IsAttack = false;
                newEnemyHealthUI.SetActive(false);               
                if (PlayerInSight())
                {
                    // �����ҽ�����Ұ��Χ����ʼ׷��
                    TrackPlayer();
                }
                else
                {
                    // �����Ҳ�����Ұ��Χ�ڣ���ָ�Ѳ��
                    ResumePatrol();
                    
                }
            }
        }
        else
        {
            Debug.LogError("δ�ҵ���Ҷ���");
        }
    }

    // ���ù���״̬
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(1f / meleeRate);
        isAttacking = false; // ���ù���״̬
    }
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


    // �� Scene ��ͼ����ʾ������Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        // �ڱ༭���л���Ѳ�߷�Χ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
    // ��ս��������
    private void MeleeAttack(GameObject player)
    {
        /*fsm.SetState(EnemyStateType.Attacking);*/
        // �������Ƿ���� PlayerController ���
        PlayerController playerHealth = player.GetComponent<PlayerController>();

        /*PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;*/

        if (playerHealth != null)
        {
            // ���������˺� �Լ���ֱ�ͻ���Ч��ʵ��
            // �����������ҵķ�������
            Vector3 direction = (player.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);

            ParameterManage.Instance.IsAttack = true;

            if (!ParameterManage.Instance.IsDie)
            {
                //���Լ����˺�
                TakeDamage(PlayerattackDamage, PlayerattackDefense);
                Debug.Log("��ҶԵ��˵��˺�" + PlayerattackDamage);
            }
        }
        // ʹ�����������
        transform.LookAt(player.transform);

        Debug.Log("��ʼ��ս����==============================");
    }

    // Զ�̹�������
    private void RangedAttack(GameObject player)
    {
        //���ŵ����ܻ�����
        /*fsm.SetState(EnemyStateType.Attacking);*/
        // �������Ƿ���� PlayerController ���
        PlayerController playerHealth = player.GetComponent<PlayerController>();
       /* PlayerattackDamage = playerHealth.attack;
        PlayerattackDefense = playerHealth.defense;*/
        if (playerHealth != null)
        {
            // ���������˺�
            Vector3 direction = (player.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(attackDamage, enemyDefend, direction, 0.5f);
            ParameterManage.Instance.IsAttack = true;

            if (!ParameterManage.Instance.IsDie)
            {
                //���Լ����˺�
                TakeDamage(PlayerattackDamage, PlayerattackDefense);
                Debug.Log("��ҶԵ��˵��˺�" + PlayerattackDamage);
            }
        }
        // ʹ�����������
        transform.LookAt(player.transform);

        Debug.Log("��ʼԶս����");
    }
    public void TakeDamage(int damage,int defense)
    {
        //���ŵ����ܻ�����
        //fsm.SetState(EnemyStateType.Hit);
        // ���㾭���������˺�
        int damageTaken = Mathf.Max(0, damage - defense);
        currentHealth -= damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ����Ѫ����0�����Ѫ��֮��
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
        // �����߼�
        Destroy(gameObject);
        //Ѫ��Ԥ����Ҳ��ʧ
        Destroy(newEnemyHealthUI);
        //�����������
        //player.GetComponent<PlayerController>().UseEnemyItem(enemyStats);

    }

    //����Ѳ���߼�

    private void StopPatrol()
    {
        // ֹͣѲ��
        patrolPoints = null;
    }

    private void ResumePatrol()
    {
        // �ָ�Ѳ��
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

    //׷��
    private void TrackPlayer()
    {
        // �����Ҳ�����Ұ��Χ�ڣ��򷵻�Ѳ��
        if (!PlayerInSight())
        {
            // �����Ҳ�����Ұ��Χ�ڣ���ָ�Ѳ��
            ResumePatrol();
            return;
        }

        Debug.Log("��ʼ׷��===========");
        // ����������Ұ��Χ�ڣ�������Ҳ������ƶ�
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.LookAt(player.transform);
        transform.position += direction * trackSpeed * Time.deltaTime;

    }

    // �������Ƿ�����Ұ��Χ��
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
