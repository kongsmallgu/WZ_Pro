using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase
{
    private Animator animator;
    private FSMControl fsm;
    private float delTime = 5f;
    public IdleState(Animator Ani, FSMControl fsm) 
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
       /* if (delTime >= 0) 
        {
            delTime -= Time.deltaTime;
            if (delTime <= 0) 
            {
                fsm.SetState(StateType.Moving);
            }
        }*/
    }

    public override void OnExit()
    {
        Debug.Log("ÍË³ö¿ÕÏÐ×´Ì¬");
        animator.SetBool("Idle", false);
    }
}
