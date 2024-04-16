using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    // �ڴ˴�����Ҫ�������¼�
    void MyCustomAnimationEvent()
    {
        Debug.Log("Custom animation event triggered!");
        
    }

    public AnimationClip Atkclip;

    private void Start()
    {
        AddCustomAnimationEvent();
    }
    // ����һ�� AnimationClip ������Ϊ������¼�
    public void AddCustomAnimationEvent()
    {
        // ����һ���µĶ����¼�
        AnimationEvent newEvent = new AnimationEvent();

        // �����¼���ʱ�䣬��������ڶ������� 2 ��󴥷�
        newEvent.time = 0.2f;

        // �����¼����������ƣ��������Ϊ MyCustomAnimationEvent
        newEvent.functionName = "MyCustomAnimationEvent";

        // ���¼���ӵ�����������
        Atkclip.AddEvent(newEvent);
    }
}
