using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 要跟随的目标物体
    public Vector3 offset; // 相机与目标之间的偏移量
    public float followSpeed = 5f; // 跟随速度

    private void LateUpdate()
    {
        if (target != null)
        {
            // 计算相机的目标位置
            Vector3 targetPosition = target.position + offset;

            // 使用平滑的插值方式移动相机到目标位置
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
