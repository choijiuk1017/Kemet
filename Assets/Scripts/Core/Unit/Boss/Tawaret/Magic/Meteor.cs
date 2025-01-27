using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Meteor Settings")]
    public float warningDuration = 1.5f; // ��� ǥ�� �ð�
    public float lifetimeAfterFall = 3f; // ��� ������������ �ð�
    public int damage = 10; // � ������
    public LayerMask playerLayer; // �÷��̾� ���̾�

    [Header("Warning Line Settings")]
    public float warningLineLength = 50f; // ��� ����
    public Color warningLineColor = Color.red; // ��� ����
    private LineRenderer warningLine; // �������� ������ ���

    private Rigidbody2D rb; // ��� Rigidbody2D
    private bool isDealingDamage = false; // �������� ������ ������ ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // �ʱ⿡�� �߷� ��Ȱ��ȭ

        CreateWarningLine(); // ��� ����
        StartCoroutine(WarningAndActivateGravity());
    }

    private void CreateWarningLine()
    {
        // LineRenderer ������Ʈ�� �������� �߰�
        warningLine = gameObject.AddComponent<LineRenderer>();
        warningLine.positionCount = 2; // �� ������ ���� �׸�
        warningLine.startWidth = 1f; // ���� ���� �β�
        warningLine.endWidth = 1f; // ���� �� �β�
        warningLine.startColor = warningLineColor; // ���� ����
        warningLine.endColor = warningLineColor; // �� ����
        warningLine.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���̴� ���
        warningLine.sortingOrder = 10; // ��������Ʈ ���� ǥ�õǵ��� ����

        // ����� ��ġ ����
        Vector3 startPoint = transform.position + Vector3.down * warningLineLength;
        Vector3 endPoint = transform.position;

        warningLine.SetPosition(0, startPoint); // ����� ������
        warningLine.SetPosition(1, endPoint); // ����� ����
    }

    private IEnumerator WarningAndActivateGravity()
    {
        // ��� ǥ�� �ð� ��ٸ���
        yield return new WaitForSeconds(warningDuration);

        // ��� ����
        if (warningLine != null)
        {
            Destroy(warningLine);
        }

        // �߷� Ȱ��ȭ
        rb.gravityScale = 1;

        // ��� �������� 3�� �� ����
        yield return new WaitForSeconds(lifetimeAfterFall);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            // �÷��̾�� �浹 ����
            if (!isDealingDamage)
            {
                isDealingDamage = true;
                StartCoroutine(DealDamage(collision.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            // �÷��̾�� �浹 ����
            isDealingDamage = false;
        }
    }

    private IEnumerator DealDamage(GameObject player)
    {
        while (isDealingDamage)
        {
            // �÷��̾�� ������ ����
            var playerHealth = player.GetComponent<Core.Unit.Player.Seth>(); // �÷��̾� ü�� ���� ��ũ��Ʈ �ʿ�
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(1f); // 1�� �������� ������ ����
        }
    }
}
