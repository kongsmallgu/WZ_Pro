using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollView; // 引用ScrollView组件
    public RectTransform content; // 引用ScrollView的Content组件

    public string itemPrefabPath; // 子节点的预制体路径

    private GameObject itemPrefab; // 子节点的预制体

    public Item item;

    public bool isScrolling = false; // 是否正在滚动

    void Start()
    {
        // 在Start函数中，将滚动位置设置到最右侧
        SetScrollToRight();

        //CreateItem(item);
    }

    private void Update()
    {
        // 如果没有滚动且不是在拖拽中，则将滚动位置设置到最右侧
      /*  if (isScrolling)
        {

            Debug.Log("正常移动===========");
        }*/
        SetScrollToRight();
       
        
    }

    // 将滚动位置设置到最右侧
    public void SetScrollToRight()
    {

        // 将滚动位置设置到最右侧
        scrollView.horizontalNormalizedPosition = 1f;   
    }

    public void CreateItem(Item item)
    {
        // 加载预制体
        itemPrefab = Resources.Load<GameObject>("potionItem");

        if (itemPrefab != null)
        {
            // 实例化预制体，设置父对象为 content
            GameObject newItem = Instantiate(itemPrefab, content);

            // 获取预制体中的 Image 组件
            Image itemImage = newItem.transform.GetChild(0).GetComponent<Image>();

            // 检查预制体中是否存在图标
            if (item.itemIcon != null)
            {
                // 设置 Image 组件的 sprite 属性为物品的图标
                itemImage.sprite = item.itemIcon;
            }
            else
            {
                Debug.LogWarning("物品图标为空！");
            }
        }
        else
        {
            Debug.LogWarning("找不到预制体：" + itemPrefab.name);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        /*isScrolling = true;*/
        Debug.Log("用户推拽滚动条====================");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*isScrolling = false;*/
        Debug.Log("用户停止推拽滚动条=====================");
    }


}
