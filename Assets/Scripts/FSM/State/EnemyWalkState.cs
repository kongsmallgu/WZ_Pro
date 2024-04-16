using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : StateBase
{
    private Animator animator;
    public EnemyWalkState(Animator Ani)
    {
        this.animator = Ani;
    } 

    public override void OnEnter()
    {
        Debug.Log("进入行走状态");
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

    public override void OnUpdate()
    {
        Debug.Log("执行移动状态");
    }

    public override void OnExit()
    {
        Debug.Log("退出移动状态");
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
    }
}
