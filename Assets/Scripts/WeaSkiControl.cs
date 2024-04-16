using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaSkiControl : MonoBehaviour
{
   //public GameObject WeaponObj;

    // 标记是否已经实例化了武器
    private bool weaponInstantiated;

    private void Awake()
    {
      
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void Start()
    {
        InitPrefabWeapons();
    }
    //获取选中的预制体
    private void Update()
    {
      
        if (ParameterManage.Instance.PlayerWeapon)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("显示武器选择界面===============");
           
        }
        else 
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("显示武器选择界面不显示===============");
        }     
    }
    private static GameObject playerWeapon;

    public void InitPrefabWeapons() 
    {
        // 查找玩家对象
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // 检测玩家是否存在
        if (player != null)
        {
            Transform playerTransform = player.transform.GetChild(0).GetChild(1).GetChild(0);
            for (int i = 0; i < playerTransform.childCount; i++)
            {
                Transform child = playerTransform.GetChild(i);
                Debug.Log("子节点名称：" + child.name);
                if (child.name == "Weapon")
                {
                    playerWeapon = child.gameObject;
                    break;
                }
                else
                {
                    Debug.Log("没有找到子节点：Weapon" + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("未找到玩家对象！");
        }

    }

    //切换玩家武器
   public static void SwitchWeapon(GameObject WeaponObj)
    {
        Debug.Log("切换玩家武器" + playerWeapon.name+"=============================");

        int childCount = playerWeapon.transform.childCount;
        for (int i = childCount - 1; i > 0; i--)
        {
            Transform child = playerWeapon.transform.GetChild(i);

            // 销毁除第一个子节点以外的所有子节点
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

                // 设置武器对象的本地旋转为 (-180, 0, 0)
                instantiatedWeapon.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

                // 设置武器对象的本地缩放为 (1, 1, 1)
                instantiatedWeapon.transform.localScale = Vector3.one;

            }
        }
              
    }

}
