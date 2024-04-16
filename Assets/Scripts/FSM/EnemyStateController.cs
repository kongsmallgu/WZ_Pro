using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ����״̬ö��
public enum EnemyStateType
{
    Idle,
    Walk,
    FarAttacking,
    NearAttacking,
    Hit,
    Die   
}

// ״̬����
public class EnemyFSMControl : MonoBehaviour
{
    //��ǰ����״̬
    private StateBase currentState;

    public EnemyStateType stateType;

    //��������״̬������
    private Dictionary<EnemyStateType, StateBase> allSaveState;

    public EnemyFSMControl() 
    {
        allSaveState = new Dictionary<EnemyStateType, StateBase>();
    }
    //ÿִ֡��
    public void OnTick() 
    {
        currentState?.OnUpdate();
    }
    //���״̬
    public void AddState(EnemyStateType stateType, StateBase stateBase) 
    {
        if (allSaveState.ContainsKey(stateType)) return;
        allSaveState.Add(stateType, stateBase);
    }

    //�л�״̬
    public void SetState(EnemyStateType stateType)
    {
        if (currentState == allSaveState[stateType]) return;
        currentState?.OnExit(); //�ж��Ƿ�Ϊ��
        currentState = allSaveState[stateType];
        currentState.OnEnter();
    }
}
