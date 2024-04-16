using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // ��ɫ�ƶ��ٶ�
    public VirtualJoystick virtualJoystick; // ����ҡ��

    private Rigidbody rb;

    private FSMControl fsm;
    private Animator animator;

    private int maxHealth;
    public int attack;
    public int defense;
    private int currentHealth;
    public HealthBar healthBar;

    //public Item potion;
    private Item potion;
    public PlayerStats playerstats;
    public ScrollViewController scrollViewController;

    public bool IsAttack = false;

    private void Awake()
    {
        fsm = new FSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //���״̬
        fsm.AddState(StateType.Idle, new IdleState(animator, this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator));
        fsm.AddState(StateType.Die, new DieState(animator));
        fsm.AddState(StateType.Dizzy, new DizzyState(animator));
        //����״̬
        fsm.SetState(StateType.Idle);

        //���������ļ�������������
        maxHealth = playerstats.Health;
        attack = playerstats.Attack;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
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
            // ��ȡ����Ŀ���λ����Ϣ������ΪattackTargetPosition
            Vector3 attackTargetPosition = GetAttackTargetPosition();

            // ���㳯�򹥻�Ŀ��ķ���
            Vector3 attackDirection = (attackTargetPosition - transform.position).normalized;

            // �ý�ɫ���򹥻�Ŀ��
            transform.rotation = Quaternion.LookRotation(attackDirection);

            fsm.SetState(StateType.Attacking);

        }
    }


    //Ѫ������ ����

    //���ٽ�ɫ��Ѫ��
    public void TakeDamage(int damage , int Defense, Vector3 Direction, float knockbackForce)
    {
        //��ֱ 
        //fsm.SetState(StateType.Dizzy);
        //����
        Knockback(Direction,knockbackForce);

        // ���㾭���������˺�
        int damageTaken = Mathf.Max(0, damage - Defense);

        // ������ҵ�����ֵ
        currentHealth -= damage;
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
    public void Heal(int healAmount)
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
        // ��ӻ�ȡ����Ŀ��λ�õ��߼�
        EnemyAttackObj = ParameterManage.Instance.currentEnemyPosition;
        Debug.Log("������˵�λ����Ϣ" + EnemyAttackObj);
        // ���������ﷵ��һ���̶���λ�ã��Ա���ʾ
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

        Debug.Log("ʹ������Ʒ��" + item.itemName);
        Debug.Log("��ǿ���������ֵ��" + item.healthBonus);
        Debug.Log("��ǿ����ҹ�������" + item.attackBonus);
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
