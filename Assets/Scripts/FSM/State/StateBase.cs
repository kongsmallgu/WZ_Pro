using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    /// <summary>
    /// �״ν���
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// �������ڵ�ǰ״̬
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// �˳���ǰ״̬
    /// </summary>
    public abstract void OnExit();
}
