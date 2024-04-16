using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarAttackingState : StateBase
{

    private Animator animator;
    private float attackInterval = 0.4f; // ¹¥»÷¼ä¸ôÊ±¼ä£¬µ¥Î»ÎªÃë
    private float timer = 0f; // ¼ÆÊ±Æ÷
    private AnimationClip attackAnimation; // ¹¥»÷¶¯»­
    private Animation animationComponent;
    public EnemyFarAttackingState(Animator Ani)
    {
        this.animator = Ani;
        
    }

    public override void OnEnter()
    {
        Debug.Log("½øÈë¹¥»÷×´Ì¬");
        animator.SetBool("Walk", false);
        animator.SetBool("FarAtk", true);
        animator.SetBool("NearAtk", false);
        animator.SetBool("Idle", false);
    }
  
    public override void OnUpdate()
    {
        Debug.Log("Ö´ÐÐ¹¥»÷×´Ì¬");

    }

    public override void OnExit()
    {
        Debug.Log("ÍË³ö¹¥»÷×´Ì¬");
        animator.SetBool("Walk", true);
        animator.SetBool("FarAtk", false);
        animator.SetBool("NearAtk", false);
        animator.SetBool("Idle", false);
    }
}
