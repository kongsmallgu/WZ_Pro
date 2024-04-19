using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : StateBase
{

    private Animator animator;
    //private float attackInterval = 0.4f; // �������ʱ�䣬��λΪ��
    private float timer = 0f; // ��ʱ��
    private AnimationClip attackAnimation; // ��������
    private Animation animationComponent;

    private float atkSp;
    private float normalatkSp;
    private float attackInterval;
    public AttackingState(Animator Ani,float Atkspeed)
    {
        this.animator = Ani;
        normalatkSp = Ani.speed;
        //��ʼ������ģ��
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips)
        {
            //Debug.LogError(clip.name + "  " + clip.length);

            if (clip.name == "Attack01")
            {
                atkSp = clip.length;
                //����ʱ��
                attackInterval = Atkspeed;
                Debug.Log("�����ٶ�===================="+attackInterval);
                break;
            }
        }

    }

    public override void OnEnter()
    {
        Debug.Log("���빥��״̬");
        animator.SetBool("Idle", false);
        animator.SetBool("Attacking", true);
        animator.SetBool("Die", false);
        animator.SetBool("Moving", false);

        Debug.Log("������������ʱ��Ϊ================="+ atkSp);

        // ����ʱ�޸Ķ����Ĳ����ٶ�
        animator.speed = atkSp / attackInterval;
        //animator.speed = normalatkSp;
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
        animator.SetBool("Moving", false);

        // ����ʱ�޸Ķ����Ĳ����ٶ�
        animator.speed = normalatkSp;
    }
}
