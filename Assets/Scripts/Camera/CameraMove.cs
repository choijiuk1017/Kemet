using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;         // ������ �÷��̾� ������Ʈ�� Transform
    public Vector3 offset;          // ī�޶�� �÷��̾� ������ ������

    public float smoothTime = 0.3f;  // ī�޶� ������ �������ϰ� ����� ���� �ð� ��
    private Vector3 velocity = Vector3.zero;

    public float followThresholdY = 7f; // ���󰡱� ������ y�� ���� ��

    void LateUpdate()
    {
        if (target == null)
            return;

        // �÷��̾��� ��ġ�� �������� ���Ͽ� ī�޶� Ÿ�� ��ġ�� ���
        Vector3 targetPosition = target.position + offset;

        // �÷��̾�� ī�޶��� y��ǥ ���̰� followThresholdY���� ū ��쿡�� ���󰡱�
        if (Mathf.Abs(target.position.y - transform.position.y) > followThresholdY)
        {
            // Lerp �Լ��� ����Ͽ� ���� ī�޶� ��ġ�� Ÿ�� ��ġ ���̸� �������ϰ� ����
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // x�� ��ġ�� �÷��̾��� x�� ��ġ�� �����Ͽ� x�� �������� ����
            smoothPosition.x = target.position.x;

            transform.position = smoothPosition;
        }
        else
        {
            // x�� ��ġ�� �÷��̾��� x�� ��ġ�� �����Ͽ� x�� �������� ����
            Vector3 newPosition = transform.position;
            newPosition.x = target.position.x;
            transform.position = newPosition;
        }
    }
}
