using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CameraSystem
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target;         // ������ �÷��̾� ������Ʈ�� Transform
        public Vector3 offset;          // ī�޶�� �÷��̾� ������ ������

        // Ÿ���� ȭ���� 70%�� �Ѿ�� ����
        public float followThreshold = 0.5f;

        // ī�޶� �ӵ�
        public float followSpeed = 2f;

        // ī�޶� ��������, ������
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
                // ī�޶��� ����Ʈ���� ���� �ϴ�(0,0)�� ������ �ϴ�(1,0)�� ���� ��ǥ�� ��ȯ
                Vector3 bottomLeftWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
                Vector3 bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                // ī�޶��� �ʺ� ���
                cameraWidth = bottomRightWorldPos.x - bottomLeftWorldPos.x;

                // ���� ����Ʈ�� ���� ������ ���� ��� �� ī�޶� ��ġ ����
                Vector3 offset = startPoint.position - bottomLeftWorldPos;
                mainCamera.transform.position += new Vector3(offset.x, offset.y, 0);

                // �ʱ� ī�޶�� �� ī�޶� ��ġ �� ����
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

            // X��: �÷��̾ ȭ���� 70% �̻� �Ѿ�� �� ī�޶� ����
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

            // Y��: �÷��̾ ��/�Ʒ��� ���� ������ ����� ī�޶� ����
            if (Mathf.Abs(targetViewportPos.y - 0.5f) > verticalFollowThreshold)
            {
                targetCameraPos.y = target.position.y;
                shouldFollowY = true;
            }

            if (shouldFollowX || shouldFollowY)
            {
                targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);

                // ī�޶��� �Ʒ��� ��踦 ����� Y ��ǥ ����
                float cameraHeight = mainCamera.orthographicSize * 2;  // ī�޶� ����
                float cameraBottomY = targetCameraPos.y - cameraHeight / 2;  // ī�޶��� �Ʒ��� Y ��ǥ ���

                // ī�޶��� �Ʒ��� ��谡 -5 ���Ϸ� �������� �ʵ��� Ŭ����
                if (cameraBottomY < minY)
                {
                    targetCameraPos.y = minY + cameraHeight / 2;
                }

                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
            }

            // X�� ��� ����
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