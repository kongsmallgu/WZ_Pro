using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyState : StateBase
{
    private Animator animator;

    public DizzyState(Animator Ani)
    {
        this.animator = Ani;        
    }

    public override void OnEnter()
    {
        Debug.Log("进入僵直状态");
        animator.SetBool("Dizzy", true);
        animator.SetBool("Attacking", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("执行僵直状态");

    }

    public override void OnExit()
    {
        Debug.Log("退出僵直状态");
        animator.SetBool("Dizzy", false);
        animator.SetBool("Attacking", true);
    }
}
