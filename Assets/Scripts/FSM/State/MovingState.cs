using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : StateBase
{
    private Animator animator;
    public MovingState(Animator Ani)
    {
        this.animator = Ani;
    }
   

    public override void OnEnter()
    {
        Debug.Log("进入移动状态");
        animator.SetBool("Idle",false);
        animator.SetBool("Moving", true);
        animator.SetBool("Die", false);
    }

    public override void OnUpdate()
    {
        Debug.Log("执行移动状态");
    }

    public override void OnExit()
    {
        Debug.Log("退出移动状态");
        animator.SetBool("Idle", true);
        animator.SetBool("Moving", false);
        animator.SetBool("Die", false);
    }
}
