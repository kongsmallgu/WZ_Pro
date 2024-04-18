using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ҫ�����Ŀ������
    public Vector3 offset; // �����Ŀ��֮���ƫ����
    public float followSpeed = 5f; // �����ٶ�

    private void LateUpdate()
    {
        if (target != null)
        {
            // ���������Ŀ��λ��
            Vector3 targetPosition = target.position + offset;

            // ʹ��ƽ���Ĳ�ֵ��ʽ�ƶ������Ŀ��λ��
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
