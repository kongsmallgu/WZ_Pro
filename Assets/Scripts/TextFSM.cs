using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFSM : MonoBehaviour
{
    private FSMControl fsm;
    private Animator animator;

    private void Awake()
    {
        fsm = new FSMControl();
        animator = GetComponent<Animator>();
        //Ìí¼Ó×´Ì¬
        fsm.AddState(StateType.Idle,new IdleState(animator,this.fsm));
        fsm.AddState(StateType.Moving, new MovingState(animator));
        fsm.AddState(StateType.Attacking, new AttackingState(animator));
        //ÉèÖÃ×´Ì¬
        fsm.SetState(StateType.Attacking);
    }

    private void Update()
    {
        fsm.OnTick();
    }
}
