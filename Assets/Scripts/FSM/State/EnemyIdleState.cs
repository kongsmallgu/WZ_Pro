using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : StateBase
{
    private Animator animator;
    private EnemyFSMControl fsm;
    private float delTime = 5f;
    public EnemyIdleState(Animator Ani, EnemyFSMControl fsm) 
    {
        this.animator = Ani;
        this.fsm = fsm;
    }

    public override void OnEnter()
    {
        Debug.Log("�������״̬");
        animator.SetBool("Idle", true);

    }

    public override void OnUpdate()
    {
        Debug.Log("ִ�п���״̬");
     
    }

    public override void OnExit()
    {
        Debug.Log("�˳�����״̬");
        animator.SetBool("Idle", false);
    }
}
