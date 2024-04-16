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
        Debug.Log("�����ƶ�״̬");
        animator.SetBool("Idle",false);
        animator.SetBool("Moving", true);
        animator.SetBool("Die", false);
    }

    public override void OnUpdate()
    {
        Debug.Log("ִ���ƶ�״̬");
    }

    public override void OnExit()
    {
        Debug.Log("�˳��ƶ�״̬");
        animator.SetBool("Idle", true);
        animator.SetBool("Moving", false);
        animator.SetBool("Die", false);
    }
}
