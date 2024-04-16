using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : StateBase
{
    private Animator animator;

    public EnemyDieState(Animator Ani)
    {
        this.animator = Ani;        
    }

    public override void OnEnter()
    {
        Debug.Log("��������״̬");
        animator.SetBool("Die", true);
       
    }
  
    public override void OnUpdate()
    {
        Debug.Log("ִ������״̬");

    }

    public override void OnExit()
    {
        Debug.Log("�˳�����״̬");

    }
}
