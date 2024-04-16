using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour
{
    public RectTransform background; // ҡ�˵ı���
    public RectTransform handle; // ҡ�˵Ĳ���
    public float handleRange = 1.0f; // ���˵��ƶ���Χ

    private Vector2 inputVector = Vector2.zero; // �û���������


    private void Update()
    {
        // ����û�����
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            // ���������� UI Ԫ�أ��򲻸���ҡ��λ��
            if (IsPointerOverUIObject())
            {
                return;
            }

            Debug.Log("�û���ʼ����");

            Vector2 touchPosition;

            // �����������
            if (Input.touchCount > 0)
            {
                // ��ȡ������λ��
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;
            }
            else
            {
                // ��ȡ�����λ��
                touchPosition = Input.mousePosition;
            }

            // ��������λ��ת��Ϊҡ�˷�Χ�ڵ�����
            Vector2 handlePosition = background.InverseTransformPoint(touchPosition);
            float clampMagnitude = Mathf.Clamp(handlePosition.magnitude, 0, background.rect.width / 2 * handleRange);
            handlePosition = handlePosition.normalized * clampMagnitude;

            // ���²���λ��
            handle.anchoredPosition = handlePosition;

            // �����û���������
            inputVector = handlePosition / (background.rect.width / 2 * handleRange);
        }
        else
        {
            Debug.Log("�û�û�д���");
            // ���û�д�������������������������������������õ�����λ��
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }

    // ����Ƿ������� UI Ԫ��
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
