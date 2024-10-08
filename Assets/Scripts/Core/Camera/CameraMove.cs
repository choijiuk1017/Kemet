using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.CameraSystem
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target;         // ������ �÷��̾� ������Ʈ�� Transform
        public Vector3 offset;          // ī�޶�� �÷��̾� ������ ������

        //Ÿ���� ȭ���� 70%�� �Ѿ�� ����
        public float followThreshold = 0.7f;

        //ī�޶� �ӵ�
        public float followSpeed = 2f;

        //ī�޶� ��������, ������
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
                //ī�޶��� �� ��Ʈ���� ���� �ϴ�(0,0)�� ���� ��ǥ�� ��ȯ
                bottomLeftWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

                bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                cameraWidth = bottomRightWorldPos.x - bottomLeftWorldPos.x;


                //���� ����Ʈ�� ���� ������ ���� ���
                Vector3 offset = startPoint.position - bottomLeftWorldPos;

                //ī�޶� ��ġ ����
                mainCamera.transform.position += new Vector3(offset.x, offset.y, 0);

                initialCameraX = mainCamera.transform.position.x;

                endCameraX = endPoint.position.x;
            }
        }

        private void Update()
        {
            if (mainCamera != null && target != null)
            {
                // Ÿ���� ȭ�� �� ��ġ�� ����Ʈ ��ǥ�� ��� (0~1 ���� ��)
                Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.position);
                
                bottomRightWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));

                // Ÿ���� ȭ���� ������ 70% ������ �Ѿ�� �� ī�޶� ����
                if (targetViewportPos.x > followThreshold && mainCamera.transform.position.x < endCameraX)
                {
                    Vector3 targetCameraPos = mainCamera.transform.position;
                    targetCameraPos.x = target.position.x;
                    targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
                }
                // Ÿ���� ȭ���� ���� 70% ������ �Ѿ�� �� ī�޶� ���� (�ݴ� ����)
                else if (targetViewportPos.x < (1 - followThreshold) && mainCamera.transform.position.x > initialCameraX)
                {
                    Vector3 targetCameraPos = mainCamera.transform.position;
                    targetCameraPos.x = target.position.x;
                    targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, initialCameraX, endCameraX);
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPos, followSpeed * Time.deltaTime);
                }

                // ī�޶� ���� ������ �����ϰų� �� ������ �������� �� ����
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



