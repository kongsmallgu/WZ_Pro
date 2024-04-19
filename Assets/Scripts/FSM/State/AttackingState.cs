using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : StateBase
{

    private Animator animator;
    //private float attackInterval = 0.4f; // 攻击间隔时间，单位为秒
    private float timer = 0f; // 计时器
    private AnimationClip attackAnimation; // 攻击动画
    private Animation animationComponent;

    private float atkSp;
    private float normalatkSp;
    private float attackInterval;
    public AttackingState(Animator Ani,float Atkspeed)
    {
        this.animator = Ani;
        normalatkSp = Ani.speed;
        //初始化动作模型
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips)
        {
            //Debug.LogError(clip.name + "  " + clip.length);

            if (clip.name == "Attack01")
            {
                atkSp = clip.length;
                //攻速时间
                attackInterval = Atkspeed;
                Debug.Log("攻击速度===================="+attackInterval);
                break;
            }
        }

    }

    public override void OnEnter()
    {
        Debug.Log("进入攻击状态");
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", true);
        animator.SetBool("Die", false);
        animator.SetBool("Moving", false);

        Debug.Log("攻击动画持续时长为================="+ atkSp);

        // 攻击时修改动画的播放速度
        animator.speed = atkSp / attackInterval;
        //animator.speed = normalatkSp;
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
        animator.SetBool("Moving", false);

        // 攻击时修改动画的播放速度
        animator.speed = normalatkSp;
    }
}
