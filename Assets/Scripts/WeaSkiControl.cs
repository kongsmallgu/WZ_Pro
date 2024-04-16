using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaSkiControl : MonoBehaviour
{
   //public GameObject WeaponObj;

    // ����Ƿ��Ѿ�ʵ����������
    private bool weaponInstantiated;

    private void Awake()
    {
      
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void Start()
    {
        InitPrefabWeapons();
    }
    //��ȡѡ�е�Ԥ����
    private void Update()
    {
      
        if (ParameterManage.Instance.PlayerWeapon)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("��ʾ����ѡ�����===============");
           
        }
        else 
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("��ʾ����ѡ����治��ʾ===============");
        }     
    }
    private static GameObject playerWeapon;

    public void InitPrefabWeapons() 
    {
        // ������Ҷ���
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // �������Ƿ����
        if (player != null)
        {
            Transform playerTransform = player.transform.GetChild(0).GetChild(1).GetChild(0);
            for (int i = 0; i < playerTransform.childCount; i++)
            {
                Transform child = playerTransform.GetChild(i);
                Debug.Log("�ӽڵ����ƣ�" + child.name);
                if (child.name == "Weapon")
                {
                    playerWeapon = child.gameObject;
                    break;
                }
                else
                {
                    Debug.Log("û���ҵ��ӽڵ㣺Weapon" + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("δ�ҵ���Ҷ���");
        }

    }

    //�л��������
   public static void SwitchWeapon(GameObject WeaponObj)
    {
        Debug.Log("�л��������" + playerWeapon.name+"=============================");

        int childCount = playerWeapon.transform.childCount;
        for (int i = childCount - 1; i > 0; i--)
        {
            Transform child = playerWeapon.transform.GetChild(i);

            // ���ٳ���һ���ӽڵ�����������ӽڵ�
            Destroy(child.gameObject);
        }

        //WeaponObj = ParameterManage.Instance.currentPlayerWeapon;

        if (WeaponObj != null)
        {
            Debug.Log(WeaponObj.name + "===================================");
            GameObject weaponResource = Resources.Load<GameObject>("Weapons/" + WeaponObj.name);
            if (weaponResource != null)
            {
                GameObject instantiatedWeapon = Instantiate(WeaponObj, playerWeapon.transform.position, Quaternion.identity, playerWeapon.transform);

                // ������������ı�����תΪ (-180, 0, 0)
                instantiatedWeapon.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

                // ������������ı�������Ϊ (1, 1, 1)
                instantiatedWeapon.transform.localScale = Vector3.one;

            }
        }
              
    }

}
