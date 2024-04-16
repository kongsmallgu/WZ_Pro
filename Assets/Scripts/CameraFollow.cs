using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ҫ�����Ŀ������
    public Vector3 offset; // �����Ŀ��֮���ƫ����

    private void LateUpdate()
    {
        if (target != null)
        {
            // ���������Ŀ��λ��
            Vector3 targetPosition = target.position + offset;

            // ƽ�����ƶ������Ŀ��λ��
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
    }
}
