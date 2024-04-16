using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHitState : StateBase
{
    private Animator animator;
    private FSMControl fsm;
    private float delTime = 5f;
    public EnemyHitState(Animator Ani) 
    {
        this.animator = Ani;
        
    }
    private AnimationClip clip;
    public override void OnEnter()
    {
        Debug.Log("进入敌人受击状态");
        animator.SetBool("Hit", true);
        animator.SetBool("Walk", false);

        // 添加动画事件
        // 获取当前正在播放的动画片段的信息
        /*AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);


        // 检查是否有动画片段正在播放
        if (currentClipInfo.Length > 0)
        {
            // 获取当前播放的动画片段的引用
            clip = currentClipInfo[0].clip;

        }
        else
        {
            Debug.Log("没有动画片段正在播放");
        }
        AddAnimationEvent(clip, 1.0f, "OnHitAnimationEvent");*/
    }

    public override void OnUpdate()
    {
        Debug.Log("执行敌人受击状态");
      
    }

    public override void OnExit()
    {
        Debug.Log("退出敌人受击状态");
        animator.SetBool("Hit", false);
        animator.SetBool("Walk", true);
    }

    // 添加动画事件的辅助方法
    private void AddAnimationEvent(AnimationClip clip, float time, string functionName)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.time = time;

        AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[] { animationEvent });
    }

    // 这是动画事件触发时调用的函数
    private void OnHitAnimationEvent()
    {
        Debug.Log("动画事件触发：敌人受击");
        // 在这里添加动画事件触发后要执行的逻辑
    }
}
