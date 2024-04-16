using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public event Action<string, Vector3> OnSkillUsed;

    private GameObject skill;
    // 创建单例实例
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


    // 在此处定义要触发的事件
    void MyNearEvent()
    {
        Debug.Log("Custom animation event triggered!");
        UseNearSkill("敌人一", this.gameObject.transform.position);
    }

    void MyFarEvent()
    {
        Debug.Log("Custom animation event triggered!");
        UseFarSkill("敌人一", this.gameObject.transform.position);
    }
    public AnimationClip AtkNearclip;
    public AnimationClip AtkFarclip;
    private void Start()
    {
        AddCustomNearAnimationEvent();
        AddCustomFarAnimationEvent();
    }
    // 传入一个 AnimationClip 参数，为其添加事件
    public void AddCustomNearAnimationEvent()
    {
        // 创建一个新的动画事件
        AnimationEvent newEvent = new AnimationEvent();
        newEvent.time = 0.2f;

        newEvent.functionName = "MyNearEvent";
        AtkNearclip.AddEvent(newEvent);
    }

    public void AddCustomFarAnimationEvent()
    {
        // 创建一个新的动画事件
        AnimationEvent newEvent = new AnimationEvent();

        newEvent.time = 0.07f;

        newEvent.functionName = "MyFarEvent";
        AtkFarclip.AddEvent(newEvent);
    }

    //敌人技能释放 远战
    public void UseFarSkill(string skillName, Vector3 skillPos)
    {
        skill = Resources.Load<GameObject>("EnemyAtkEft/" + skillName + "_far");
        Instantiate(skill, skillPos, Quaternion.identity);
        skill.transform.position = skillPos;
        Debug.Log("开启远战攻击特效/////////////////////");
    }

    //外部使用技能 近战
    public void UseNearSkill(string skillName, Vector3 skillPos)
    {
        skill = Resources.Load<GameObject>("EnemyAtkEft/" + skillName + "_near");
        /* GameObject gameObjectskill =  */
        Instantiate(skill, skillPos, Quaternion.identity);
        skill.transform.position = skillPos;
        Debug.Log("开启近战攻击特效///////////////////");
    }
}
