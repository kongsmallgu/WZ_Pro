using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public VirtualJoystick virtualJoystick; // 虚拟摇杆

    private Rigidbody rb;

    private FSMControl fsm;
    private Animator animator;

    private float currentHealth;

    private float maxHealth;
    public float attack;
    public float defense;
    private float moveSpeed; // 角色移动速度
    private float atkSpeTime = 1f;

    public HealthBar healthBar;

    //public Item potion;
    private Item potion;
    //public PlayerStats playerstats;
    public ScrollViewController scrollViewController;

    public bool IsAttack = false;

    //攻击速度
    private float AtkAniSpeed;
    private float AtkAniSpeedEnd;

    private StateType CurrentState;

  
    private float lastAttackTime; // 上一次攻击的时间
    private bool IsAttacking = false;

    private void Awake()
    {
        //根据配置文件设置人物属性
/*        maxHealth = PlayerAttributesManeger.Instance.maxHealth;
        attack = PlayerAttributesManeger.Instance.attack;
        defense = PlayerAttributesManeger.Instance.defense;
        moveSpeed = PlayerAttributesManeger.Instance.moveSpeed;
        atkSpeTime = PlayerAttributesManeger.Instance.AtkspeedTime;


        AtkAniSpeed = 1/atkSpeTime;
        Debug.Log(AtkAniSpeed+"=========================="+ atkSpeTime);
        //添加动画
        fsm = new FSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //添加状态
        fsm.AddState(StateType.Idle, new IdleState(animator, this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator, AtkAniSpeed));
        fsm.AddState(StateType.Die, new DieState(animator));
        fsm.AddState(StateType.Dizzy, new DizzyState(animator));
        //设置状态
        fsm.SetState(StateType.Idle);*/

    }

    private void Start()
    {
        //根据配置文件设置人物属性
        maxHealth = PlayerAttributesManeger.Instance.maxHealth;
        attack = PlayerAttributesManeger.Instance.attack;
        defense = PlayerAttributesManeger.Instance.defense;
        moveSpeed = PlayerAttributesManeger.Instance.moveSpeed;
        atkSpeTime = PlayerAttributesManeger.Instance.AtkspeedTime;

        //实时显示血条
        Heal(maxHealth);

        AtkAniSpeed = 4f;
       // AtkAniSpeedEnd = 1/AtkAniSpeed;
        AtkAniSpeedEnd = 1;
        Debug.Log(AtkAniSpeed + "==========================" + atkSpeTime);
        //添加动画
        fsm = new FSMControl();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        //添加状态
        fsm.AddState(StateType.Idle, new IdleState(animator, this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator, AtkAniSpeedEnd));
        fsm.AddState(StateType.Die, new DieState(animator));
        fsm.AddState(StateType.Dizzy, new DizzyState(animator));
        //设置状态
        fsm.SetState(StateType.Idle);

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        //实时获取 玩家各项属性
        maxHealth = PlayerAttributesManeger.Instance.maxHealth;
        attack = PlayerAttributesManeger.Instance.attack;
        defense = PlayerAttributesManeger.Instance.defense;
        moveSpeed = PlayerAttributesManeger.Instance.moveSpeed;

        Debug.Log("玩家生命值"+ currentHealth);
        Debug.Log("玩家攻击力" + attack);
        Debug.Log("玩家防御力" + defense);
        Debug.Log("玩家移动速度" + moveSpeed);
        Debug.Log("玩家攻击速度" + atkSpeTime);

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

       //进入敌人攻击范围 开始攻击 
        if (IsAttack)
        {
            lastAttackTime += Time.deltaTime;

            // 当计时器超过攻击间隔时间时，执行
            // 攻击并重置计时器
            if (lastAttackTime >= atkSpeTime)
            {
                Debug.Log("攻击间隔为=========================" + atkSpeTime);
                enemyTarget = true;
                Attack();

            }
        }      
    }

    private bool enemyTarget;
    public void Attack()
    {
        
        // 执行攻击逻辑
        Vector3 attackTargetPosition = GetAttackTargetPosition();
        Vector3 attackDirection = (attackTargetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(attackDirection);
        fsm.SetState(StateType.Attacking);
        Debug.Log("玩家开始攻击！！！！！！！！！！！！！！！！！!");


        EnemyAttackObj = ParameterManage.Instance.currentEnemyGameObject;
        EnemyController enemyHealth = EnemyAttackObj.GetComponent<EnemyController>();
        enemyHealth.TakeDamage(attack, defense); // 给敌方造成伤害
        Debug.Log("给敌方造成伤害++++++++++++++++++++++++++++++" + attack);

        StartCoroutine(ResetAttackStatus()); // 重置攻击状态
    }
    private IEnumerator ResetAttackStatus()
    {
        yield return new WaitForSeconds(0.9f);
        //fsm.SetState(StateType.Idle);
        lastAttackTime = 0f;
        enemyTarget = false;
    }


    //血条管理 更新

    //减少角色的血量
    public void TakeDamage(float damage , float Defense, Vector3 Direction, float knockbackForce)
    {
        //僵直 
        //fsm.SetState(StateType.Dizzy);
        //击退
        //Knockback(Direction,knockbackForce);

        // 计算经过防御的伤害
        float damageTaken = Mathf.Max(0, damage - Defense);

        // 更新玩家的生命值
        currentHealth -= damageTaken;
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
        Debug.Log("我已经去世了");
    }
    private Vector3 EnemyAttackPos;
    private GameObject EnemyAttackObj;
    // 获取攻击目标的位置
    private Vector3 GetAttackTargetPosition()
    {
        EnemyAttackPos = ParameterManage.Instance.currentEnemyPosition;
        Debug.Log("传入敌人的位置信息" + EnemyAttackPos);
        return EnemyAttackPos;
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
            //UseItem(potion);

            PlayerAttributesManeger.Instance.UseItem(potion);
        }


        if (other.gameObject.CompareTag("Splotion"))
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
            //UseItemSpecial(potion);
            PlayerAttributesManeger.Instance.UseItemSpecial(potion);

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
        defense += item.defenseBonus;
        atkSpeTime -= item.atkSpeed;

        Debug.Log("使用了物品：" + item.itemName);
        Debug.Log("增强了玩家生命值：" + item.healthBonus);
        Debug.Log("增强了玩家攻击力：" + item.attackBonus);
    }

    //特殊道具
    public void UseItemSpecial(Item item)
    {
        // 增加玩家生命值
        maxHealth += (item.healthBonus != 0) ? (item.healthBonus * maxHealth) : 0;
        Heal(maxHealth);

        // 增加玩家攻击力
        attack += (item.attackBonus != 0) ? (item.attackBonus * attack ) : 0;

        defense += (item.defenseBonus != 0) ? (item.defenseBonus * defense) : 0;

        Debug.Log("使用了物品：" + item.itemName);
        Debug.Log("增强了玩家生命值：" + (item.healthBonus * maxHealth - maxHealth)); // 计算实际增加的生命值
        Debug.Log("增强了玩家攻击力：" + (item.attackBonus * attack - attack)); // 计算实际增加的攻击力
        Debug.Log("增强了玩家防御力：" + (item.defenseBonus * defense - defense)); // 计算实际增加的攻击力
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
