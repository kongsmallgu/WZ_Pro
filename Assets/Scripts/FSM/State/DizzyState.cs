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
        Debug.Log("���뽩ֱ״̬");
        animator.SetBool("Dizzy", true);
        animator.SetBool("Attacking", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("ִ�н�ֱ״̬");

    }

    public override void OnExit()
    {
        Debug.Log("�˳���ֱ״̬");
        animator.SetBool("Dizzy", false);
        animator.SetBool("Attacking", true);
    }
}
