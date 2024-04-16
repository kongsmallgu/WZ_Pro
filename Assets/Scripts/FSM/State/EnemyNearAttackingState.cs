using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNearAttackingState : StateBase
{

    private Animator animator;
    private float attackInterval = 0.4f; // �������ʱ�䣬��λΪ��
    private float timer = 0f; // ��ʱ��
    private AnimationClip attackAnimation; // ��������
    private Animation animationComponent;
    public EnemyNearAttackingState(Animator Ani)
    {
        this.animator = Ani;
        
    }

    public override void OnEnter()
    {
        Debug.Log("���빥��״̬");
        animator.SetBool("FarAtk", false);
        animator.SetBool("NearAtk", true);
        animator.SetBool("Die", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("ִ�й���״̬");

    }

    public override void OnExit()
    {
        Debug.Log("�˳�����״̬");
        animator.SetBool("FarAtk", true);
        animator.SetBool("NearAtk", false);
        animator.SetBool("Die", false);
    }
}
