using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHitState : StateBase
{
    private Animator animator;
    private FSMControl fsm;
    private float delTime = 5f;
    public EnemyHitState(Animator Ani) 
    {
        this.animator = Ani;
        
    }
    private AnimationClip clip;
    public override void OnEnter()
    {
        Debug.Log("��������ܻ�״̬");
        animator.SetBool("Hit", true);
        animator.SetBool("Walk", false);

        // ��Ӷ����¼�
        // ��ȡ��ǰ���ڲ��ŵĶ���Ƭ�ε���Ϣ
        /*AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);


        // ����Ƿ��ж���Ƭ�����ڲ���
        if (currentClipInfo.Length > 0)
        {
            // ��ȡ��ǰ���ŵĶ���Ƭ�ε�����
            clip = currentClipInfo[0].clip;

        }
        else
        {
            Debug.Log("û�ж���Ƭ�����ڲ���");
        }
        AddAnimationEvent(clip, 1.0f, "OnHitAnimationEvent");*/
    }

    public override void OnUpdate()
    {
        Debug.Log("ִ�е����ܻ�״̬");
      
    }

    public override void OnExit()
    {
        Debug.Log("�˳������ܻ�״̬");
        animator.SetBool("Hit", false);
        animator.SetBool("Walk", true);
    }

    // ��Ӷ����¼��ĸ�������
    private void AddAnimationEvent(AnimationClip clip, float time, string functionName)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.time = time;

        AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[] { animationEvent });
    }

    // ���Ƕ����¼�����ʱ���õĺ���
    private void OnHitAnimationEvent()
    {
        Debug.Log("�����¼������������ܻ�");
        // ��������Ӷ����¼�������Ҫִ�е��߼�
    }
}
