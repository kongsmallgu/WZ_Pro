using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弹窗界面处理
public class UiLayerManager : MonoBehaviour
{
    public static UiLayerManager Instance;

    private Transform UILayerRoot;

    private void Awake()
    {
        UiLayerManager.Instance = this;

        UILayerRoot = GameObject.Find("GameLayer").transform;
    }
    /// <summary> 界面数组 </summary>
    private ArrayList UiLayerList = new ArrayList();
    /// <summary> 记录打开的界面 压栈 </summary>
    private ArrayList RecordOpenLayerList = new ArrayList();

    /// <summary>
    /// 打开界面
    /// <para> strLayerName : 界面名称 </para>
    /// <para> data : 自定义参数（接收的时候需要自己解析对应的类型） </para>
    /// </summary>
    public void OpenLayer(string strLayerName, object data = null)
    {
        // 当前有 打开的界面 先关闭掉
        if (0 < RecordOpenLayerList.Count)
        {
            for (int i = 0; i < RecordOpenLayerList.Count; i++)
            {
                var vChildLayer = (GameObject)RecordOpenLayerList[i];
                vChildLayer.SetActive(false);
            }
        }
        // 通过名字 找到对应的 界面
        var vGetOpenLayer = GetLayerIndex(strLayerName);
        if (null == vGetOpenLayer)
        {
           /* // 这个用的 ab 加载资源
            //var vChildLayer = (GameObject)Instantiate(ResManager.Instance.LoadLayerRes(strLayerName));
            // 测试用这个
            var vChildLayer = ResManager.Instance.Load(PathMsg.LayerPrefabsFile + strLayerName);
            vChildLayer.transform.name = strLayerName;
            vChildLayer.transform.SetParent(UILayerRoot);
            vChildLayer.transform.localPosition = new Vector2(0, 0);
            vChildLayer.transform.localScale = Vector3.one;
            UiLayerList.Add(vChildLayer);
            RecordOpenLayerList.Add(vChildLayer);

            vChildLayer.SetActive(true);
            vChildLayer.GetComponent<UiLayerParent>().Open(data);*/
        }
        else
        {
            RecordOpenLayerList.Add(vGetOpenLayer);
            vGetOpenLayer.SetActive(true);
            vGetOpenLayer.GetComponent<UiLayerParent>().Open(data);
        }
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="strLayerName"> 界面名称 </param>
    public void CloseLayer(string strLayerName)
    {
        // 关闭一个界面
        int iSaveIndex = 0;
        for (int i = 0; i < RecordOpenLayerList.Count; i++)
        {
            var vChildLayer = (GameObject)RecordOpenLayerList[i];
            if (vChildLayer.name == strLayerName)
            {
                iSaveIndex = i;
                vChildLayer.gameObject.SetActive(false);
                break;
            }
        }
        RecordOpenLayerList.RemoveAt(iSaveIndex);

      /*  //转化室
        if ("CureLayer" == strLayerName)
        {
            var vLayer = GetLayerIndex(strLayerName);
            if (null != vLayer) RecyclePoolManager.Instance.RecycleChild(vLayer);
            RecordOpenLayerList.Clear();
            UiLayerList.Clear();
            return;
        }
        //场
        if ("OutPutLayer" == strLayerName)
        {
            var vLayer = GetLayerIndex(strLayerName);
            if (null != vLayer) RecyclePoolManager.Instance.RecycleChild(vLayer);
            RecordOpenLayerList.Clear();
            UiLayerList.Clear();
            return;
        }*/

        // 检测 剩余的界面容器 还有没有多余的，有的话，就直接打开
        if (0 < RecordOpenLayerList.Count)
        {
            var vChildLayer = (GameObject)RecordOpenLayerList[RecordOpenLayerList.Count - 1];
            vChildLayer.SetActive(true);
            vChildLayer.GetComponent<UiLayerParent>().Open();
        }
    }

    /// <summary> 关闭所有界面，一般用来进入游戏 或者进入 大厅的时候使用 </summary>
    public void CleanAllLayer()
    {
        int iCount = UiLayerList.Count;
        for (int i = 0; i < iCount; i++)
        {
            var vChildLayer = (GameObject)UiLayerList[0];
            vChildLayer.GetComponent<UiLayerParent>().CloseSelf();
            vChildLayer.gameObject.SetActive(false);
            MonoBehaviour.Destroy(vChildLayer);
        }
        RecordOpenLayerList.Clear();
        UiLayerList.Clear();
    }

    /// <summary>
    /// 查看一下是否有 这个界面
    /// <para> strLayerName : 界面名称 </para>    
    /// </summary>
    GameObject GetLayerIndex(string strLayerName)
    {
        for (int i = 0; i < UiLayerList.Count; i++)
        {
            var vChildLayer = (GameObject)UiLayerList[i];
            if (strLayerName == vChildLayer.name)
            {
                if ("CureLayer" == strLayerName)
                {
                    UiLayerList.RemoveAt(i);
                    return vChildLayer;
                }
                if ("OutPutLayer" == strLayerName)
                {
                    UiLayerList.RemoveAt(i);
                    return vChildLayer;
                }
                return vChildLayer;
            }
        }
        return null;
    }
}