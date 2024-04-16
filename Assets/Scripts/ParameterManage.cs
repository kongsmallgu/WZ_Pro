using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManage : MonoBehaviour
{
    private static ParameterManage instance;

    //是否正在攻击敌人
    public bool IsAttackEnemy = false;
    //是否攻击
    public bool IsAttack = false;
    //是否死亡
    public bool IsDie = false;

    //玩家获得的药品数量
    public int PacageNum = 0;

    //传入药品的数值
    public Item option = null;

    //当前正在攻击的敌人位置
    public Vector3 currentEnemyPosition;

    //当前被选中的武器
    public GameObject currentPlayerWeapon = null;

    //武器界面是否被调起
    public bool PlayerWeapon;

    // 获取单例实例的静态方法
    public static ParameterManage Instance
    {
        get
        {
            // 如果实例为空，则查找场景中是否已存在该组件，如果不存在则创建一个新的实例
            if (instance == null)
            {
                instance = FindObjectOfType<ParameterManage>();

                // 如果场景中不存在该组件，则创建一个新的 GameObject，并将该组件添加到上面
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ParameterManage");
                    instance = singletonObject.AddComponent<ParameterManage>();
                }
            }
            return instance;
        }
    }

    // 在需要时可以调用该方法来初始化单例实例
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 防止切换场景时销毁该对象
        }
        else
        {
            Destroy(gameObject); // 如果存在其他实例，则销毁该对象
        }
    }



}
