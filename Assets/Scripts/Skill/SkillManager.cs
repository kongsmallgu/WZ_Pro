using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private GameObject weapon; // 武器对象
    public float skillDistanceFromWeapon = 2.0f; // 技能释放距离武器的距离

    // 定义普通攻击和技能攻击的冷却时间                                              
    public float normalAttackCooldown = 0.5f;
    public float skillAttackCooldown = 5f;

    // 记录技能的上次释放时间
    private float lastNormalAttackTime = 0f;
    private float lastSkillAttackTime = 0f;

    private GameObject skill;
    // 创建单例实例
    private static SkillManager _instance;
    public static SkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SkillManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(SkillManager).Name;
                    _instance = obj.AddComponent<SkillManager>();
                }
            }
            return _instance;
        }
    }

    // 定义普通攻击的方法
    public void NormalAttack()
    {
        /* if (Time.time - lastNormalAttackTime >= normalAttackCooldown)
         {
             // 在这里执行普通攻击的逻辑，播放动画、计算伤害等
             Debug.Log("Normal Attack!");

             UseSkill();

             lastNormalAttackTime = Time.time;
         }*/

        UseSkill();
    }

    // 定义技能攻击的方法
    public void SkillAttack()
    {
        if (Time.time - lastSkillAttackTime >= skillAttackCooldown)
        {
            // 在这里执行技能攻击的逻辑，例如创建特效、计算伤害等
            Debug.Log("Skill Attack!");

            UseSkill();

            lastSkillAttackTime = Time.time;
        }
    }

    // 在技能释放时调用的方法
    public void UseSkill()
    {
        // 获取角色的位置和方向
        Vector3 characterPosition = transform.position;
        Quaternion characterRotation = transform.rotation;

        // 计算技能释放的位置
        Vector3 skillDirection = characterRotation * Vector3.right; // 获取角色朝向的右方向
        Quaternion skillRotation = Quaternion.Euler(0, 90, 0) * characterRotation;
        Vector3 skillPosition = characterPosition + new Vector3(1,0,0)/*skillDirection * skillDistanceFromWeapon*/; // 使用技能释放距离计算技能位置

        // 在技能释放的位置释放技能
        InstantiateSkill(skillPosition, skillRotation); // 传入角色旋转而不是技能旋转
    }

    private void InstantiateSkill(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>("PlayerAtkEft/" + "daoguang01");
        if (prefab != null)
        {
            Debug.Log("技能释放============" + position);
            GameObject skillInstance = Instantiate(prefab, position, rotation);
            //几秒后销毁
            Destroy(skillInstance, 3f);
        }
        else
        {
            Debug.LogWarning("Prefab not found!");
        }
    }
}
