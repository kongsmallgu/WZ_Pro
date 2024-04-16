using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 定义状态枚举
public enum EnemyStateType
{
    Idle,
    Walk,
    FarAttacking,
    NearAttacking,
    Hit,
    Die   
}

// 状态机类
public class EnemyFSMControl : MonoBehaviour
{
    //当前运行状态
    private StateBase currentState;

    public EnemyStateType stateType;

    //保存所有状态的容器
    private Dictionary<EnemyStateType, StateBase> allSaveState;

    public EnemyFSMControl() 
    {
        allSaveState = new Dictionary<EnemyStateType, StateBase>();
    }
    //每帧执行
    public void OnTick() 
    {
        currentState?.OnUpdate();
    }
    //添加状态
    public void AddState(EnemyStateType stateType, StateBase stateBase) 
    {
        if (allSaveState.ContainsKey(stateType)) return;
        allSaveState.Add(stateType, stateBase);
    }

    //切换状态
    public void SetState(EnemyStateType stateType)
    {
        if (currentState == allSaveState[stateType]) return;
        currentState?.OnExit(); //判断是否为空
        currentState = allSaveState[stateType];
        currentState.OnEnter();
    }
}
