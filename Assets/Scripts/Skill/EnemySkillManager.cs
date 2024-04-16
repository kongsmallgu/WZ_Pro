using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public event Action<string, Vector3> OnSkillUsed;

    private GameObject skill;
    // ��������ʵ��
    private static EnemySkillManager _instance;
    public static EnemySkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemySkillManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(EnemySkillManager).Name;
                    _instance = obj.AddComponent<EnemySkillManager>();
                }
            }
            return _instance;
        }
    }


    // �ڴ˴�����Ҫ�������¼�
    void MyNearEvent()
    {
        Debug.Log("Custom animation event triggered!");
        UseNearSkill("����һ", this.gameObject.transform.position);
    }

    void MyFarEvent()
    {
        Debug.Log("Custom animation event triggered!");
        UseFarSkill("����һ", this.gameObject.transform.position);
    }
    public AnimationClip AtkNearclip;
    public AnimationClip AtkFarclip;
    private void Start()
    {
        AddCustomNearAnimationEvent();
        AddCustomFarAnimationEvent();
    }
    // ����һ�� AnimationClip ������Ϊ������¼�
    public void AddCustomNearAnimationEvent()
    {
        // ����һ���µĶ����¼�
        AnimationEvent newEvent = new AnimationEvent();
        newEvent.time = 0.2f;

        newEvent.functionName = "MyNearEvent";
        AtkNearclip.AddEvent(newEvent);
    }

    public void AddCustomFarAnimationEvent()
    {
        // ����һ���µĶ����¼�
        AnimationEvent newEvent = new AnimationEvent();

        newEvent.time = 0.07f;

        newEvent.functionName = "MyFarEvent";
        AtkFarclip.AddEvent(newEvent);
    }

    //���˼����ͷ� Զս
    public void UseFarSkill(string skillName, Vector3 skillPos)
    {
        skill = Resources.Load<GameObject>("EnemyAtkEft/" + skillName + "_far");
        Instantiate(skill, skillPos, Quaternion.identity);
        skill.transform.position = skillPos;
        Debug.Log("����Զս������Ч/////////////////////");
    }

    //�ⲿʹ�ü��� ��ս
    public void UseNearSkill(string skillName, Vector3 skillPos)
    {
        skill = Resources.Load<GameObject>("EnemyAtkEft/" + skillName + "_near");
        /* GameObject gameObjectskill =  */
        Instantiate(skill, skillPos, Quaternion.identity);
        skill.transform.position = skillPos;
        Debug.Log("������ս������Ч///////////////////");
    }
}
