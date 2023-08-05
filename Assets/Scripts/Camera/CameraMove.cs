using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform�� ����ŵ�ϴ�.
    public float smoothSpeed = 0.125f; // ī�޶� �̵� �ӵ� (���� �������� �ε巴�� �̵�)

    private Vector3 offset; // ī�޶�� �÷��̾� ������ �Ÿ��� ������ ����

    private void Start()
    {
        // �ʱ� ī�޶�� �÷��̾� ������ �Ÿ��� ����Ͽ� offset�� �����մϴ�.
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // �÷��̾��� ��ġ�� �����Ͽ� ī�޶� �ε巴�� �̵���ŵ�ϴ�.
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
