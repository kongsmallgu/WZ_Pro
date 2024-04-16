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
        Debug.Log("½øÈë¿ÕÏÐ×´Ì¬");
        animator.SetBool("Idle", true);

    }

    public override void OnUpdate()
    {
        Debug.Log("Ö´ÐÐ¿ÕÏÐ×´Ì¬");
     
    }

    public override void OnExit()
    {
        Debug.Log("ÍË³ö¿ÕÏÐ×´Ì¬");
        animator.SetBool("Idle", false);
    }
}
