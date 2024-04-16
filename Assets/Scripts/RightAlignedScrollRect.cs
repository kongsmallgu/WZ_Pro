using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightAlignedScrollRect : ScrollRect
{
    private bool isDragging = false;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (!isDragging)
        {
            // 将滚动位置设置到最右侧
            horizontalNormalizedPosition = 1f;
        }
    }

    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        isDragging = true;
    }

    public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        isDragging = false;
    }
}
