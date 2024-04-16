using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNearAttackingState : StateBase
{

    private Animator animator;
    private float attackInterval = 0.4f; // 攻击间隔时间，单位为秒
    private float timer = 0f; // 计时器
    private AnimationClip attackAnimation; // 攻击动画
    private Animation animationComponent;
    public EnemyNearAttackingState(Animator Ani)
    {
        this.animator = Ani;
        
    }

    public override void OnEnter()
    {
        Debug.Log("进入攻击状态");
        animator.SetBool("FarAtk", false);
        animator.SetBool("NearAtk", true);
        animator.SetBool("Die", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("执行攻击状态");

    }

    public override void OnExit()
    {
        Debug.Log("退出攻击状态");
        animator.SetBool("FarAtk", true);
        animator.SetBool("NearAtk", false);
        animator.SetBool("Die", false);
    }
}
