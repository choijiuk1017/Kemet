using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Meteor Settings")]
    public float warningDuration = 1.5f; // 경고선 표시 시간
    public float lifetimeAfterFall = 3f; // 운석이 사라지기까지의 시간
    public int damage = 10; // 운석 데미지
    public LayerMask playerLayer; // 플레이어 레이어

    [Header("Warning Line Settings")]
    public float warningLineLength = 50f; // 경고선 길이
    public Color warningLineColor = Color.red; // 경고선 색상
    private LineRenderer warningLine; // 동적으로 생성된 경고선

    private Rigidbody2D rb; // 운석의 Rigidbody2D
    private bool isDealingDamage = false; // 데미지를 입히는 중인지 여부

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 초기에는 중력 비활성화

        CreateWarningLine(); // 경고선 생성
        StartCoroutine(WarningAndActivateGravity());
    }

    private void CreateWarningLine()
    {
        // LineRenderer 컴포넌트를 동적으로 추가
        warningLine = gameObject.AddComponent<LineRenderer>();
        warningLine.positionCount = 2; // 두 점으로 선을 그림
        warningLine.startWidth = 1f; // 선의 시작 두께
        warningLine.endWidth = 1f; // 선의 끝 두께
        warningLine.startColor = warningLineColor; // 시작 색상
        warningLine.endColor = warningLineColor; // 끝 색상
        warningLine.material = new Material(Shader.Find("Sprites/Default")); // 기본 셰이더 사용
        warningLine.sortingOrder = 10; // 스프라이트 위에 표시되도록 정렬

        // 경고선의 위치 설정
        Vector3 startPoint = transform.position + Vector3.down * warningLineLength;
        Vector3 endPoint = transform.position;

        warningLine.SetPosition(0, startPoint); // 경고선의 시작점
        warningLine.SetPosition(1, endPoint); // 경고선의 끝점
    }

    private IEnumerator WarningAndActivateGravity()
    {
        // 경고선 표시 시간 기다리기
        yield return new WaitForSeconds(warningDuration);

        // 경고선 제거
        if (warningLine != null)
        {
            Destroy(warningLine);
        }

        // 중력 활성화
        rb.gravityScale = 1;

        // 운석이 떨어지고 3초 후 삭제
        yield return new WaitForSeconds(lifetimeAfterFall);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            // 플레이어와 충돌 시작
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
            // 플레이어와 충돌 종료
            isDealingDamage = false;
        }
    }

    private IEnumerator DealDamage(GameObject player)
    {
        while (isDealingDamage)
        {
            // 플레이어에게 데미지 적용
            var playerHealth = player.GetComponent<Core.Unit.Player.Seth>(); // 플레이어 체력 관리 스크립트 필요
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(1f); // 1초 간격으로 데미지 적용
        }
    }
}
