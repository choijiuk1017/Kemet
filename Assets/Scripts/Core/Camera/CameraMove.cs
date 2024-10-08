using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CameraSystem
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target;         // 추적할 플레이어 오브젝트의 Transform
        public Vector3 offset;          // 카메라와 플레이어 사이의 오프셋

        // 타겟이 화면의 70%를 넘어가면 따라감
        public float followThreshold = 0.5f;

        // 카메라 속도
        public float followSpeed = 2f;

        // 카메라 시작지점, 끝지점
        public Transform startPoint;
        public Transform endPoint;

        private Camera mainCamera;
        private float initialCameraX;
        private float initialCameraY;
        private float endCameraX;

        private float cameraWidth;

        private void Start()
        {
            mainCamera = Camera.main;

            if (mainCamera != null && startPoint != null)
            {
                // 카메라의 뷰포트에서 왼쪽 하단(0,0)과 오른쪽 하단(1,0)을 월드 좌표로 변환
                Vector3 bottomLeftWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                Vector3 bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                // 카메라의 너비 계산
                cameraWidth = bottomRightWorldPos.x - bottomLeftWorldPos.x;

                // 스폰 포인트와 현재 지점의 차이 계산 후 카메라 위치 조정
                Vector3 offset = startPoint.position - bottomLeftWorldPos;
                mainCamera.transform.position += new Vector3(offset.x, offset.y, 0);

                // 초기 카메라와 끝 카메라 위치 값 저장
                initialCameraX = mainCamera.transform.position.x;
                initialCameraY = mainCamera.transform.position.y;
                endCameraX = endPoint.position.x;
            }
        }

        private void LateUpdate()
        {
            if (mainCamera == null || target == null) return;

            // 타겟의 화면 내 위치를 뷰포트 좌표로 계산 (0~1 사이 값)
            Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.position);
            Vector3 targetCameraPos = mainCamera.transform.position;

            bool shouldFollow = false;

            // 타겟이 화면의 오른쪽 70% 지점을 넘어갔을 때 카메라가 따라감
            if (targetViewportPos.x > followThreshold && mainCamera.transform.position.x < endCameraX)
            {
                targetCameraPos.x = target.position.x;
                shouldFollow = true;
            }
            // 타겟이 화면의 왼쪽 70% 지점을 넘어갔을 때 카메라가 따라감 (반대 방향)
            else if (targetViewportPos.x < (1 - followThreshold) && mainCamera.transform.position.x > initialCameraX)
            {
                targetCameraPos.x = target.position.x;
                shouldFollow = true;
            }

            if (shouldFollow)
            {
                // 카메라 이동 위치를 클램프하여 경계를 벗어나지 않게 설정
                targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
            }

            // 카메라가 시작 지점이나 끝 지점을 넘어가지 않도록 제어
            if (mainCamera.transform.position.x <= initialCameraX)
            {
                mainCamera.transform.position = new Vector3(initialCameraX, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            else if (mainCamera.transform.position.x + (cameraWidth / 2) >= endCameraX)
            {
                mainCamera.transform.position = new Vector3(endCameraX - (cameraWidth / 2), mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
        }
    }
}