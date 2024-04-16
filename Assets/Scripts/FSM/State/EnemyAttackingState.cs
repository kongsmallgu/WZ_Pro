using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : StateBase
{

    private Animator animator;
    private float attackInterval = 0.4f; // 攻击间隔时间，单位为秒
    private float timer = 0f; // 计时器
    private AnimationClip attackAnimation; // 攻击动画
    private Animation animationComponent;
    public AttackingState(Animator Ani)
    {
        this.animator = Ani;
        
    }

    public override void OnEnter()
    {
        Debug.Log("进入攻击状态");
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", true);
        animator.SetBool("Die", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("执行攻击状态");

        //SkillManager.Instance.NormalAttack();
        // 每帧更新计时器
        /* timer += Time.deltaTime;

         // 如果计时器超过攻击间隔时间，则执行攻击状态并重置计时器
         if (timer >= attackInterval)
         {
             Debug.Log("执行攻击状态");

             // 释放攻击技能
             SkillManager.Instance.NormalAttack();

             // 重置计时器
             timer = 0f;
         }*/

    }

    public override void OnExit()
    {
        Debug.Log("退出攻击状态");
        animator.SetBool("Idle", true);
        animator.SetBool("Attacking", false);
        animator.SetBool("Die", false);
    }
}
