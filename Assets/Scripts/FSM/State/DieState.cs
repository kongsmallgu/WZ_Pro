using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : StateBase
{
    private Animator animator;

    public DieState(Animator Ani)
    {
        this.animator = Ani;        
    }

    public override void OnEnter()
    {
        Debug.Log("½øÈëËÀÍö×´Ì¬");
        animator.SetBool("Die", true);
        animator.SetBool("Attacking", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Moving", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("Ö´ĞĞËÀÍö×´Ì¬");

    }

    public override void OnExit()
    {
        Debug.Log("ÍË³öËÀÍö×´Ì¬");
/*        animator.SetBool("Idle", true);
        animator.SetBool("Die", false);*/
    }
}
