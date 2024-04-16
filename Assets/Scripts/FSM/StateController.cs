using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 定义状态枚举
public enum StateType
{
    Idle,
    Moving,
    Attacking,
    Dizzy,
    Die
}

// 状态机类
public class FSMControl : MonoBehaviour
{
    //当前运行状态
    private StateBase currentState;

    public StateType stateType;

    //保存所有状态的容器
    private Dictionary<StateType, StateBase> allSaveState;

    public FSMControl() 
    {
        allSaveState = new Dictionary<StateType, StateBase>();
    }
    //每帧执行
    public void OnTick() 
    {
        currentState?.OnUpdate();
    }
    //添加状态
    public void AddState(StateType stateType, StateBase stateBase) 
    {
        if (allSaveState.ContainsKey(stateType)) return;
        allSaveState.Add(stateType, stateBase);
    }

    //切换状态
    public void SetState(StateType stateType)
    {
        if (currentState == allSaveState[stateType]) return;
        currentState?.OnExit(); //判断是否为空
        currentState = allSaveState[stateType];
        currentState.OnEnter();
    }
}
