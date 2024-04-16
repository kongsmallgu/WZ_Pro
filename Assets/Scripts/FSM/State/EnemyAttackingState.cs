using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : StateBase
{

    private Animator animator;
    private float attackInterval = 0.4f; // �������ʱ�䣬��λΪ��
    private float timer = 0f; // ��ʱ��
    private AnimationClip attackAnimation; // ��������
    private Animation animationComponent;
    public AttackingState(Animator Ani)
    {
        this.animator = Ani;
        
    }

    public override void OnEnter()
    {
        Debug.Log("���빥��״̬");
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", true);
        animator.SetBool("Die", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("ִ�й���״̬");

        //SkillManager.Instance.NormalAttack();
        // ÿ֡���¼�ʱ��
        /* timer += Time.deltaTime;

         // �����ʱ�������������ʱ�䣬��ִ�й���״̬�����ü�ʱ��
         if (timer >= attackInterval)
         {
             Debug.Log("ִ�й���״̬");

             // �ͷŹ�������
             SkillManager.Instance.NormalAttack();

             // ���ü�ʱ��
             timer = 0f;
         }*/

    }

    public override void OnExit()
    {
        Debug.Log("�˳�����״̬");
        animator.SetBool("Idle", true);
        animator.SetBool("Attacking", false);
        animator.SetBool("Die", false);
    }
}
