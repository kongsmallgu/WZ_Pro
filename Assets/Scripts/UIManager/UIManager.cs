using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������洦��
public class UiLayerManager : MonoBehaviour
{
    public static UiLayerManager Instance;

    private Transform UILayerRoot;

    private void Awake()
    {
        UiLayerManager.Instance = this;

        UILayerRoot = GameObject.Find("GameLayer").transform;
    }
    /// <summary> �������� </summary>
    private ArrayList UiLayerList = new ArrayList();
    /// <summary> ��¼�򿪵Ľ��� ѹջ </summary>
    private ArrayList RecordOpenLayerList = new ArrayList();

    /// <summary>
    /// �򿪽���
    /// <para> strLayerName : �������� </para>
    /// <para> data : �Զ�����������յ�ʱ����Ҫ�Լ�������Ӧ�����ͣ� </para>
    /// </summary>
    public void OpenLayer(string strLayerName, object data = null)
    {
        // ��ǰ�� �򿪵Ľ��� �ȹرյ�
        if (0 < RecordOpenLayerList.Count)
        {
            for (int i = 0; i < RecordOpenLayerList.Count; i++)
            {
                var vChildLayer = (GameObject)RecordOpenLayerList[i];
                vChildLayer.SetActive(false);
            }
        }
        // ͨ������ �ҵ���Ӧ�� ����
        var vGetOpenLayer = GetLayerIndex(strLayerName);
        if (null == vGetOpenLayer)
        {
           /* // ����õ� ab ������Դ
            //var vChildLayer = (GameObject)Instantiate(ResManager.Instance.LoadLayerRes(strLayerName));
            // ���������
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
    /// �رս���
    /// </summary>
    /// <param name="strLayerName"> �������� </param>
    public void CloseLayer(string strLayerName)
    {
        // �ر�һ������
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

      /*  //ת����
        if ("CureLayer" == strLayerName)
        {
            var vLayer = GetLayerIndex(strLayerName);
            if (null != vLayer) RecyclePoolManager.Instance.RecycleChild(vLayer);
            RecordOpenLayerList.Clear();
            UiLayerList.Clear();
            return;
        }
        //��
        if ("OutPutLayer" == strLayerName)
        {
            var vLayer = GetLayerIndex(strLayerName);
            if (null != vLayer) RecyclePoolManager.Instance.RecycleChild(vLayer);
            RecordOpenLayerList.Clear();
            UiLayerList.Clear();
            return;
        }*/

        // ��� ʣ��Ľ������� ����û�ж���ģ��еĻ�����ֱ�Ӵ�
        if (0 < RecordOpenLayerList.Count)
        {
            var vChildLayer = (GameObject)RecordOpenLayerList[RecordOpenLayerList.Count - 1];
            vChildLayer.SetActive(true);
            vChildLayer.GetComponent<UiLayerParent>().Open();
        }
    }

    /// <summary> �ر����н��棬һ������������Ϸ ���߽��� ������ʱ��ʹ�� </summary>
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
    /// �鿴һ���Ƿ��� �������
    /// <para> strLayerName : �������� </para>    
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