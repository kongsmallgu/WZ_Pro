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
        Debug.Log("��������״̬");
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

    public override void OnUpdate()
    {
        Debug.Log("ִ���ƶ�״̬");
    }

    public override void OnExit()
    {
        Debug.Log("�˳��ƶ�״̬");
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
    }
}
