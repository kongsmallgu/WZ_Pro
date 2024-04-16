using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ����״̬ö��
public enum StateType
{
    Idle,
    Moving,
    Attacking,
    Dizzy,
    Die
}

// ״̬����
public class FSMControl : MonoBehaviour
{
    //��ǰ����״̬
    private StateBase currentState;

    public StateType stateType;

    //��������״̬������
    private Dictionary<StateType, StateBase> allSaveState;

    public FSMControl() 
    {
        allSaveState = new Dictionary<StateType, StateBase>();
    }
    //ÿִ֡��
    public void OnTick() 
    {
        currentState?.OnUpdate();
    }
    //���״̬
    public void AddState(StateType stateType, StateBase stateBase) 
    {
        if (allSaveState.ContainsKey(stateType)) return;
        allSaveState.Add(stateType, stateBase);
    }

    //�л�״̬
    public void SetState(StateType stateType)
    {
        if (currentState == allSaveState[stateType]) return;
        currentState?.OnExit(); //�ж��Ƿ�Ϊ��
        currentState = allSaveState[stateType];
        currentState.OnEnter();
    }
}
