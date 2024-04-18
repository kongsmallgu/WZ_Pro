using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public VirtualJoystick virtualJoystick; // ����ҡ��

    private Rigidbody rb;

    private FSMControl fsm;
    private Animator animator;

    private float currentHealth;

    private float maxHealth;
    private float attack;
    private float defense;
    private float moveSpeed; // ��ɫ�ƶ��ٶ�
    private float atkSpeTime = 1f;

    public HealthBar healthBar;

    //public Item potion;
    private Item potion;
    //public PlayerStats playerstats;
    public ScrollViewController scrollViewController;

    public bool IsAttack = false;

    //�����ٶ�
    private float AtkAniSpeed;

    private void Awake()
    {
        //���������ļ�������������
        maxHealth = PlayerAttributesManeger.Instance.maxHealth;
        attack = PlayerAttributesManeger.Instance.attack;
        defense = PlayerAttributesManeger.Instance.defense;
        moveSpeed = PlayerAttributesManeger.Instance.moveSpeed;
        atkSpeTime = PlayerAttributesManeger.Instance.AtkspeedTime;


        AtkAniSpeed = 1/atkSpeTime;
        Debug.Log(AtkAniSpeed+"=========================="+ atkSpeTime);
        //��Ӷ���
        fsm = new FSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //���״̬
        fsm.AddState(StateType.Idle, new IdleState(animator, this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator, AtkAniSpeed));
        fsm.AddState(StateType.Die, new DieState(animator));
        fsm.AddState(StateType.Dizzy, new DizzyState(animator));
        //����״̬
        fsm.SetState(StateType.Idle);

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        //ʵʱ��ȡ ��Ҹ�������
       /* maxHealth = PlayerAttributesManeger.Instance.maxHealth;
        attack = PlayerAttributesManeger.Instance.attack;
        defense = PlayerAttributesManeger.Instance.defense;
        moveSpeed = PlayerAttributesManeger.Instance.moveSpeed;*/

        Debug.Log("�������ֵ"+ currentHealth);
        Debug.Log("��ҹ�����" + attack);
        Debug.Log("��ҷ�����" + defense);
        Debug.Log("����ƶ��ٶ�" + moveSpeed);

        Debug.Log(currentHealth);
        fsm.OnTick();
        // �����ɫ�Ѿ���������ֹͣ����״̬����Ϊ
        if (currentHealth <= 0)
        {
            ParameterManage.Instance.IsAttack = false;
            ParameterManage.Instance.IsDie = true;
            fsm.SetState(StateType.Die);
            return;
        }
        Vector2 inputVector = virtualJoystick.GetInputVector();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            fsm.SetState(StateType.Moving);
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            fsm.SetState(StateType.Idle);
        }

        

        // ����Ƿ��ܹ�����
        if (IsAttack)
        {
            Attack();

        }
    }

    public float attackSpeed = 2f; // �������
    private float lastAttackTime; // ��һ�ι�����ʱ��
    private bool IsAttacking = false;

    public void Attack()
    {
        if (Time.time >= lastAttackTime)
        {
            // ִ�й����߼�
            lastAttackTime = Time.time;
            Vector3 attackTargetPosition = GetAttackTargetPosition();

            Vector3 attackDirection = (attackTargetPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(attackDirection);

            fsm.SetState(StateType.Attacking);

            Debug.Log("��ҿ�ʼ��������������������������������������!");
            StartCoroutine(ResetAttackStatus()); // ���ù���״̬
        }

       
        /*  // ����Ƿ���Թ���
          if (Time.time - lastAttackTime >= 1f / attackSpeed)
          {
              // ִ�й����߼�
              lastAttackTime = Time.time;
              Vector3 attackTargetPosition = GetAttackTargetPosition();

              Vector3 attackDirection = (attackTargetPosition - transform.position).normalized;
              transform.rotation = Quaternion.LookRotation(attackDirection);

              fsm.SetState(StateType.Attacking);

              Debug.Log("��ҿ�ʼ��������������������������������������!");

          }
          else
          {
              Debug.Log("Attack speed too fast!");
          }*/
    }


    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(1f / attackSpeed);
        IsAttacking = false; // ���ù���״̬
    }

    //Ѫ������ ����

    //���ٽ�ɫ��Ѫ��
    public void TakeDamage(int damage , int Defense, Vector3 Direction, float knockbackForce)
    {
        //��ֱ 
        //fsm.SetState(StateType.Dizzy);
        //����
        //Knockback(Direction,knockbackForce);

        // ���㾭���������˺�
        int damageTaken = Mathf.Max(0, damage - Defense);

        // ������ҵ�����ֵ
        currentHealth -= damageTaken;
        healthBar.SetHealth(currentHealth); 
    }
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.5f; // ���˳���ʱ��
    // ����ܵ�����Ч��
    public void Knockback(Vector3 direction ,float knockbackForce)
    {
        if (!isKnockedBack)
        {
            // Ӧ�û�����
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
            isKnockedBack = true;
            StartCoroutine(StopKnockback());
        }
    }

    // ֹͣ����Ч��
    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

    //����Ѫ����Ѫ��
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth); 
    }
    private void Die()
    {
        //ParameterManage.Instance.IsDie = true;
        fsm.SetState(StateType.Die);
        Debug.Log("���Ѿ�ȥ����");
    }
    private Vector3 EnemyAttackObj;
    // ��ȡ����Ŀ���λ��
    private Vector3 GetAttackTargetPosition()
    {

        EnemyAttackObj = ParameterManage.Instance.currentEnemyPosition;
        Debug.Log("������˵�λ����Ϣ" + EnemyAttackObj);
        return EnemyAttackObj;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����봥�����������Ƿ��б�ǩΪ"lotion"
        if (other.gameObject.CompareTag("lotion"))
        {
            //��ȡ��Ʒ��PotionItemShow�ű�
            PotionItemShow potionItemShow = other.gameObject.GetComponent<PotionItemShow>();
            potion = potionItemShow.potionitem;
            /*  ParameterManage.Instance.option = potion;
              ParameterManage.Instance.PacageNum++;*/

            // ������Ч
            MusicManager.instance.PlaySoundEffect(0);            
            Destroy(other.gameObject);

            //ֻ����ӵ�����ϵͳ���� ���ܹ�ʵ��Ч��
            // ����ScrollViewController��CreateItem����
            if (scrollViewController != null)
            {
                scrollViewController.CreateItem(potion); 
            }
            else
            {
                Debug.LogWarning("ScrollViewController ʵ��δ����� GameManager��");
            }

            //Ч��ʵ��
            UseItem(potion);
        }


        if (other.gameObject.CompareTag("Splotion"))
        {
            //��ȡ��Ʒ��PotionItemShow�ű�
            PotionItemShow potionItemShow = other.gameObject.GetComponent<PotionItemShow>();
            potion = potionItemShow.potionitem;
            /*  ParameterManage.Instance.option = potion;
              ParameterManage.Instance.PacageNum++;*/

            // ������Ч
            MusicManager.instance.PlaySoundEffect(0);
            Destroy(other.gameObject);

            //ֻ����ӵ�����ϵͳ���� ���ܹ�ʵ��Ч��
            // ����ScrollViewController��CreateItem����
            if (scrollViewController != null)
            {
                scrollViewController.CreateItem(potion);
            }
            else
            {
                Debug.LogWarning("ScrollViewController ʵ��δ����� GameManager��");
            }

            //Ч��ʵ��
            UseItemSpecial(potion);
        }

        if (other.gameObject.CompareTag("AttackType"))
        {
            ParameterManage.Instance.PlayerWeapon = true;
            // ������Ч
            MusicManager.instance.PlaySoundEffect(0);
          
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("AttackType"))
        {
            ParameterManage.Instance.PlayerWeapon = false;

        }
    }
    // ʹ����Ʒʱ��ǿ�������
    public void UseItem(Item item)
    {
        // �����������ֵ
        maxHealth += item.healthBonus;
        Heal(maxHealth);
        // ������ҹ�����
        attack += item.attackBonus;
        defense += item.defenseBonus;

        Debug.Log("ʹ������Ʒ��" + item.itemName);
        Debug.Log("��ǿ���������ֵ��" + item.healthBonus);
        Debug.Log("��ǿ����ҹ�������" + item.attackBonus);
    }

    //�������
    public void UseItemSpecial(Item item)
    {
        // �����������ֵ
        maxHealth += (item.healthBonus != 0) ? (item.healthBonus * maxHealth) : 0;
        Heal(maxHealth);

        // ������ҹ�����
        attack += (item.attackBonus != 0) ? (item.attackBonus * attack ) : 0;

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

        Heal(maxHealth);
        // ������ҹ�����
        attack += item.AddPlayerAttack;
        defense += item.AddPlayerDefense;

        Debug.Log("��ɱ�˵��ˣ�" + item.EnemyName);
        Debug.Log("��ǿ���������ֵ��" + item.AddPlayerHealth);
        Debug.Log("��ǿ����ҹ�������" + item.AddPlayerAttack);
        Debug.Log("��ǿ����ҷ�������" + item.AddPlayerDefense);
    }

}
