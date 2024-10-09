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

        public float verticalFollowThreshold = 0.2f;

        private Camera mainCamera;
        private float initialCameraX;
        private float initialCameraY;
        private float endCameraX;

        private float cameraWidth;

        private float minY = -5f;



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

            Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.position);
            Vector3 targetCameraPos = mainCamera.transform.position;

            bool shouldFollowX = false;
            bool shouldFollowY = false;

            // X축: 플레이어가 화면의 70% 이상 넘어갔을 때 카메라가 따라감
            if (targetViewportPos.x > followThreshold && mainCamera.transform.position.x < endCameraX)
            {
                targetCameraPos.x = target.position.x;
                shouldFollowX = true;
            }
            else if (targetViewportPos.x < (1 - followThreshold) && mainCamera.transform.position.x > initialCameraX)
            {
                targetCameraPos.x = target.position.x;
                shouldFollowX = true;
            }

            // Y축: 플레이어가 위/아래로 일정 범위를 벗어나면 카메라가 따라감
            if (Mathf.Abs(targetViewportPos.y - 0.5f) > verticalFollowThreshold)
            {
                targetCameraPos.y = target.position.y;
                shouldFollowY = true;
            }

            if (shouldFollowX || shouldFollowY)
            {
                targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);

                // 카메라의 아래쪽 경계를 고려한 Y 좌표 제한
                float cameraHeight = mainCamera.orthographicSize * 2;  // 카메라 높이
                float cameraBottomY = targetCameraPos.y - cameraHeight / 2;  // 카메라의 아래쪽 Y 좌표 계산

                // 카메라의 아래쪽 경계가 -5 이하로 내려가지 않도록 클램프
                if (cameraBottomY < minY)
                {
                    targetCameraPos.y = minY + cameraHeight / 2;
                }

                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
            }

            // X축 경계 제어
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