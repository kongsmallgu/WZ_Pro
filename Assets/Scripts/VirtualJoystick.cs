using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour
{
    public RectTransform background; // 摇杆的背景
    public RectTransform handle; // 摇杆的拨杆
    public float handleRange = 1.0f; // 拨杆的移动范围

    private Vector2 inputVector = Vector2.zero; // 用户输入向量


    private void Update()
    {
        // 检测用户输入
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            // 如果点击到了 UI 元素，则不更新摇杆位置
            if (IsPointerOverUIObject())
            {
                return;
            }

            Debug.Log("用户开始触摸");

            Vector2 touchPosition;

            // 检测输入类型
            if (Input.touchCount > 0)
            {
                // 获取触摸点位置
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;
            }
            else
            {
                // 获取鼠标点击位置
                touchPosition = Input.mousePosition;
            }

            // 将触摸点位置转换为摇杆范围内的坐标
            Vector2 handlePosition = background.InverseTransformPoint(touchPosition);
            float clampMagnitude = Mathf.Clamp(handlePosition.magnitude, 0, background.rect.width / 2 * handleRange);
            handlePosition = handlePosition.normalized * clampMagnitude;

            // 更新拨杆位置
            handle.anchoredPosition = handlePosition;

            // 计算用户输入向量
            inputVector = handlePosition / (background.rect.width / 2 * handleRange);
        }
        else
        {
            Debug.Log("用户没有触摸");
            // 如果没有触摸或鼠标点击，则重置输入向量并将拨杆重置到中心位置
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }

    // 检查是否点击到了 UI 元素
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }
}
