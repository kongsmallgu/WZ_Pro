using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollView; // ����ScrollView���
    public RectTransform content; // ����ScrollView��Content���

    public string itemPrefabPath; // �ӽڵ��Ԥ����·��

    private GameObject itemPrefab; // �ӽڵ��Ԥ����

    public Item item;

    public bool isScrolling = false; // �Ƿ����ڹ���

    void Start()
    {
        // ��Start�����У�������λ�����õ����Ҳ�
        SetScrollToRight();

        //CreateItem(item);
    }

    private void Update()
    {
        // ���û�й����Ҳ�������ק�У��򽫹���λ�����õ����Ҳ�
      /*  if (isScrolling)
        {

            Debug.Log("�����ƶ�===========");
        }*/
        SetScrollToRight();
       
        
    }

    // ������λ�����õ����Ҳ�
    public void SetScrollToRight()
    {

        // ������λ�����õ����Ҳ�
        scrollView.horizontalNormalizedPosition = 1f;   
    }

    public void CreateItem(Item item)
    {
        // ����Ԥ����
        itemPrefab = Resources.Load<GameObject>("potionItem");

        if (itemPrefab != null)
        {
            // ʵ����Ԥ���壬���ø�����Ϊ content
            GameObject newItem = Instantiate(itemPrefab, content);

            // ��ȡԤ�����е� Image ���
            Image itemImage = newItem.transform.GetChild(0).GetComponent<Image>();

            // ���Ԥ�������Ƿ����ͼ��
            if (item.itemIcon != null)
            {
                // ���� Image ����� sprite ����Ϊ��Ʒ��ͼ��
                itemImage.sprite = item.itemIcon;
            }
            else
            {
                Debug.LogWarning("��Ʒͼ��Ϊ�գ�");
            }
        }
        else
        {
            Debug.LogWarning("�Ҳ���Ԥ���壺" + itemPrefab.name);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        /*isScrolling = true;*/
        Debug.Log("�û���ק������====================");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*isScrolling = false;*/
        Debug.Log("�û�ֹͣ��ק������=====================");
    }


}
