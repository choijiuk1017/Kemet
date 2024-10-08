using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.CameraSystem
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target;         // 추적할 플레이어 오브젝트의 Transform
        public Vector3 offset;          // 카메라와 플레이어 사이의 오프셋

        //타겟이 화면의 70%를 넘어가면 따라감
        public float followThreshold = 0.7f;

        //카메라 속도
        public float followSpeed = 2f;

        //카메라 시작지점, 끝지점
        public Transform startPoint;
        public Transform endPoint;

        private Camera mainCamera;
        private float initialCameraX;
        private float endCameraX;

        private Vector3 bottomLeftWorldPos;

        private Vector3 bottomRightWorldPos;

        private float cameraWidth;

        private void Start()
        {
            mainCamera = Camera.main;

            if (mainCamera != null && startPoint != null)
            {
                //카메라의 뷰 포트에서 왼쪽 하단(0,0)을 월드 좌표로 변환
                bottomLeftWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

                bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                cameraWidth = bottomRightWorldPos.x - bottomLeftWorldPos.x;


                //스폰 포인트와 현재 지점의 차이 계산
                Vector3 offset = startPoint.position - bottomLeftWorldPos;

                //카메라 위치 조정
                mainCamera.transform.position += new Vector3(offset.x, offset.y, 0);

                initialCameraX = mainCamera.transform.position.x;

                endCameraX = endPoint.position.x;
            }
        }

        private void Update()
        {
            if (mainCamera != null && target != null)
            {
                // 타겟의 화면 내 위치를 뷰포트 좌표로 계산 (0~1 사이 값)
                Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.position);
                
                bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                // 타겟이 화면의 오른쪽 70% 지점을 넘어갔을 때 카메라가 따라감
                if (targetViewportPos.x > followThreshold && mainCamera.transform.position.x < endCameraX)
                {
                    Vector3 targetCameraPos = mainCamera.transform.position;
                    targetCameraPos.x = target.position.x;
                    targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
                }
                // 타겟이 화면의 왼쪽 70% 지점을 넘어갔을 때 카메라가 따라감 (반대 방향)
                else if (targetViewportPos.x < (1 - followThreshold) && mainCamera.transform.position.x > initialCameraX)
                {
                    Vector3 targetCameraPos = mainCamera.transform.position;
                    targetCameraPos.x = target.position.x;
                    targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
                }

                // 카메라가 시작 지점에 도달하거나 끝 지점에 도달했을 때 멈춤
                if (mainCamera.transform.position.x <= initialCameraX)
                {
                    mainCamera.transform.position = new Vector3(initialCameraX, mainCamera.transform.position.y, mainCamera.transform.position.z);
                }
                else if (mainCamera.transform.position.x + (cameraWidth/2) >= endCameraX)
                {
                    mainCamera.transform.position = new Vector3(endCameraX - (cameraWidth / 2), mainCamera.transform.position.y, mainCamera.transform.position.z);
                }
            }
        }
    }
}



