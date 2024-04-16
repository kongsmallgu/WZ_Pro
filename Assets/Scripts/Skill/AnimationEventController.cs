using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    // 在此处定义要触发的事件
    void MyCustomAnimationEvent()
    {
        Debug.Log("Custom animation event triggered!");
        
    }

    public AnimationClip Atkclip;

    private void Start()
    {
        AddCustomAnimationEvent();
    }
    // 传入一个 AnimationClip 参数，为其添加事件
    public void AddCustomAnimationEvent()
    {
        // 创建一个新的动画事件
        AnimationEvent newEvent = new AnimationEvent();

        // 设置事件的时间，这里假设在动画播放 2 秒后触发
        newEvent.time = 0.2f;

        // 设置事件函数的名称，这里假设为 MyCustomAnimationEvent
        newEvent.functionName = "MyCustomAnimationEvent";

        // 将事件添加到动画剪辑中
        Atkclip.AddEvent(newEvent);
    }
}
