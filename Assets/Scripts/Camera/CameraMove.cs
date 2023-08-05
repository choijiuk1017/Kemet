using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;         // 추적할 플레이어 오브젝트의 Transform
    public Vector3 offset;          // 카메라와 플레이어 사이의 오프셋

    public float smoothTime = 0.3f;  // 카메라 무빙을 스무스하게 만들기 위한 시간 값
    private Vector3 velocity = Vector3.zero;

    public float followThresholdY = 7f; // 따라가기 시작할 y축 차이 값

    void LateUpdate()
    {
        if (target == null)
            return;

        // 플레이어의 위치에 오프셋을 더하여 카메라 타겟 위치를 계산
        Vector3 targetPosition = target.position + offset;

        // 플레이어와 카메라의 y좌표 차이가 followThresholdY보다 큰 경우에만 따라가기
        if (Mathf.Abs(target.position.y - transform.position.y) > followThresholdY)
        {
            // Lerp 함수를 사용하여 현재 카메라 위치와 타겟 위치 사이를 스무스하게 보간
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // x축 위치를 플레이어의 x축 위치로 고정하여 x축 움직임을 유지
            smoothPosition.x = target.position.x;

            transform.position = smoothPosition;
        }
        else
        {
            // x축 위치를 플레이어의 x축 위치로 고정하여 x축 움직임을 유지
            Vector3 newPosition = transform.position;
            newPosition.x = target.position.x;
            transform.position = newPosition;
        }
    }
}
