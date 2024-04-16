using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 角色移动速度
    public VirtualJoystick virtualJoystick; // 虚拟摇杆

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
        //添加状态
        fsm.AddState(StateType.Idle, new IdleState(animator, this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator));
        fsm.AddState(StateType.Die, new DieState(animator));
        fsm.AddState(StateType.Dizzy, new DizzyState(animator));
        //设置状态
        fsm.SetState(StateType.Idle);

        //根据配置文件设置人物属性
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
        // 如果角色已经死亡，则停止更新状态和行为
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

        

        // 检查是否能够攻击
        if (IsAttack)
        {
            // 获取攻击目标的位置信息，假设为attackTargetPosition
            Vector3 attackTargetPosition = GetAttackTargetPosition();

            // 计算朝向攻击目标的方向
            Vector3 attackDirection = (attackTargetPosition - transform.position).normalized;

            // 让角色朝向攻击目标
            transform.rotation = Quaternion.LookRotation(attackDirection);

            fsm.SetState(StateType.Attacking);

        }
    }


    //血条管理 更新

    //减少角色的血量
    public void TakeDamage(int damage , int Defense, Vector3 Direction, float knockbackForce)
    {
        //僵直 
        //fsm.SetState(StateType.Dizzy);
        //击退
        Knockback(Direction,knockbackForce);

        // 计算经过防御的伤害
        int damageTaken = Mathf.Max(0, damage - Defense);

        // 更新玩家的生命值
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth); 
    }
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.5f; // 击退持续时间
    // 玩家受到击退效果
    public void Knockback(Vector3 direction ,float knockbackForce)
    {
        if (!isKnockedBack)
        {
            // 应用击退力
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
            isKnockedBack = true;
            StartCoroutine(StopKnockback());
        }
    }

    // 停止击退效果
    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

    //增加血条的血量
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
        Debug.Log("我已经去世了");
    }
    private Vector3 EnemyAttackObj;
    // 获取攻击目标的位置
    private Vector3 GetAttackTargetPosition()
    {
        // 添加获取攻击目标位置的逻辑
        EnemyAttackObj = ParameterManage.Instance.currentEnemyPosition;
        Debug.Log("传入敌人的位置信息" + EnemyAttackObj);
        // 假设在这里返回一个固定的位置，以便演示
        return EnemyAttackObj;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入触发器的物体是否有标签为"lotion"
        if (other.gameObject.CompareTag("lotion"))
        {
            //获取物品的PotionItemShow脚本
            PotionItemShow potionItemShow = other.gameObject.GetComponent<PotionItemShow>();
            potion = potionItemShow.potionitem;
            /*  ParameterManage.Instance.option = potion;
              ParameterManage.Instance.PacageNum++;*/
            // 播放音效
            MusicManager.instance.PlaySoundEffect(0);            
            Destroy(other.gameObject);

            //只有添加到背包系统里面 才能够实现效果
            // 调用ScrollViewController的CreateItem函数
            if (scrollViewController != null)
            {
                scrollViewController.CreateItem(potion); 
            }
            else
            {
                Debug.LogWarning("ScrollViewController 实例未分配给 GameManager！");
            }

            //效果实现
            UseItem(potion);
        }

        if (other.gameObject.CompareTag("AttackType"))
        {
            ParameterManage.Instance.PlayerWeapon = true;
            // 播放音效
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
    // 使用物品时增强玩家属性
    public void UseItem(Item item)
    {
        // 增加玩家生命值
        maxHealth += item.healthBonus;
        Heal(maxHealth);
        // 增加玩家攻击力
        attack += item.attackBonus;

        Debug.Log("使用了物品：" + item.itemName);
        Debug.Log("增强了玩家生命值：" + item.healthBonus);
        Debug.Log("增强了玩家攻击力：" + item.attackBonus);
    }

    //击败敌人增强玩家属性
    public void UseEnemyItem(EnemyStats item)
    {
        // 增加玩家生命值
        maxHealth += item.AddPlayerHealth;
        Heal(maxHealth);
        // 增加玩家攻击力
        attack += item.AddPlayerAttack;
        defense += item.AddPlayerDefense;

        Debug.Log("击杀了敌人：" + item.EnemyName);
        Debug.Log("增强了玩家生命值：" + item.AddPlayerHealth);
        Debug.Log("增强了玩家攻击力：" + item.AddPlayerAttack);
        Debug.Log("增强了玩家防御力：" + item.AddPlayerDefense);
    }

}
