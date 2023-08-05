using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform을 가리킵니다.
    public float smoothSpeed = 0.125f; // 카메라 이동 속도 (값이 작을수록 부드럽게 이동)

    private Vector3 offset; // 카메라와 플레이어 사이의 거리를 저장할 변수

    private void Start()
    {
        // 초기 카메라와 플레이어 사이의 거리를 계산하여 offset에 저장합니다.
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // 플레이어의 위치를 추적하여 카메라를 부드럽게 이동시킵니다.
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
