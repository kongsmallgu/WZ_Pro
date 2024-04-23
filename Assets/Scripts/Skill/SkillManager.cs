using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private GameObject weapon; // ��������
    public float skillDistanceFromWeapon = 2.0f; // �����ͷž��������ľ���

    // ������ͨ�����ͼ��ܹ�������ȴʱ��                                              
    public float normalAttackCooldown = 0.5f;
    public float skillAttackCooldown = 5f;

    // ��¼���ܵ��ϴ��ͷ�ʱ��
    private float lastNormalAttackTime = 0f;
    private float lastSkillAttackTime = 0f;

    private GameObject skill;
    // ��������ʵ��
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

    // ������ͨ�����ķ���
    public void NormalAttack()
    {
        /* if (Time.time - lastNormalAttackTime >= normalAttackCooldown)
         {
             // ������ִ����ͨ�������߼������Ŷ����������˺���
             Debug.Log("Normal Attack!");

             UseSkill();

             lastNormalAttackTime = Time.time;
         }*/

        UseSkill();
    }

    // ���弼�ܹ����ķ���
    public void SkillAttack()
    {
        if (Time.time - lastSkillAttackTime >= skillAttackCooldown)
        {
            // ������ִ�м��ܹ������߼������紴����Ч�������˺���
            Debug.Log("Skill Attack!");

            UseSkill();

            lastSkillAttackTime = Time.time;
        }
    }

    // �ڼ����ͷ�ʱ���õķ���
    public void UseSkill()
    {
        // ��ȡ��ɫ��λ�úͷ���
        Vector3 characterPosition = transform.position;
        Quaternion characterRotation = transform.rotation;

        // ���㼼���ͷŵ�λ��
        Vector3 skillDirection = characterRotation * Vector3.right; // ��ȡ��ɫ������ҷ���
        Quaternion skillRotation = Quaternion.Euler(0, 90, 0) * characterRotation;
        Vector3 skillPosition = characterPosition + new Vector3(1,0,0)/*skillDirection * skillDistanceFromWeapon*/; // ʹ�ü����ͷž�����㼼��λ��

        // �ڼ����ͷŵ�λ���ͷż���
        InstantiateSkill(skillPosition, skillRotation); // �����ɫ��ת�����Ǽ�����ת
    }

    private void InstantiateSkill(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>("PlayerAtkEft/" + "daoguang01");
        if (prefab != null)
        {
            Debug.Log("�����ͷ�============" + position);
            GameObject skillInstance = Instantiate(prefab, position, rotation);
            //���������
            Destroy(skillInstance, 3f);
        }
        else
        {
            Debug.LogWarning("Prefab not found!");
        }
    }
}
