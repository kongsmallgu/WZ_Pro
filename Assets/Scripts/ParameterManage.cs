using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManage : MonoBehaviour
{
    private static ParameterManage instance;

    //�Ƿ����ڹ�������
    public bool IsAttackEnemy = false;
    //�Ƿ񹥻�
    public bool IsAttack = false;
    //�Ƿ�����
    public bool IsDie = false;

    //��һ�õ�ҩƷ����
    public int PacageNum = 0;

    //����ҩƷ����ֵ
    public Item option = null;

    //��ǰ���ڹ����ĵ���λ��
    public Vector3 currentEnemyPosition;

    //��ǰ��ѡ�е�����
    public GameObject currentPlayerWeapon = null;

    //���������Ƿ񱻵���
    public bool PlayerWeapon;

    // ��ȡ����ʵ���ľ�̬����
    public static ParameterManage Instance
    {
        get
        {
            // ���ʵ��Ϊ�գ�����ҳ������Ƿ��Ѵ��ڸ����������������򴴽�һ���µ�ʵ��
            if (instance == null)
            {
                instance = FindObjectOfType<ParameterManage>();

                // ��������в����ڸ�������򴴽�һ���µ� GameObject�������������ӵ�����
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ParameterManage");
                    instance = singletonObject.AddComponent<ParameterManage>();
                }
            }
            return instance;
        }
    }

    // ����Ҫʱ���Ե��ø÷�������ʼ������ʵ��
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ��ֹ�л�����ʱ���ٸö���
        }
        else
        {
            Destroy(gameObject); // �����������ʵ���������ٸö���
        }
    }



}
