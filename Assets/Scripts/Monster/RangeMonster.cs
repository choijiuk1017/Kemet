using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // 몬스터의 이동 속도
    public float moveDistance = 5.0f;

    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;

    private Transform player;
    private float lastAttackTime;

    private bool playerDetected = false;

    private bool isMovingRight = true;  // 몬스터가 현재 오른쪽으로 이동 중인지 여부

    private float initialPositionX;

    private float currentDistance;

    Animator anim;

    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        initialPositionX = transform.position.x;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 공격 범위 내에 있고, 공격 쿨다운이 지났다면 공격을 실행
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            playerDetected = true;

            StartAttack();
            lastAttackTime = Time.time;
        }
        else
        {
            if (playerDetected)
            {
                Move();
            }
            else
            {
                StopMoving();
            }
        }

        if (isMovingRight)
        {
            spriteRenderer.flipX = true;  // 오른쪽으로 이동 중이면 반전하지 않음
        }
        else
        {
            spriteRenderer.flipX = false;  // 왼쪽으로 이동 중이면 이미지를 좌우로 반전
        }
    }

    void StartAttack()
    {
        anim.SetTrigger("isAttack");
    }

    void Move()
    {
        // 현재 이동한 거리 계산
        currentDistance = Mathf.Abs(transform.position.x - initialPositionX);

        // 일정 거리(moveDistance)에 도달하면 이동 방향을 바꿉니다.
        if (currentDistance >= moveDistance)
        {
            isMovingRight = !isMovingRight;
            initialPositionX = transform.position.x;
        }

        // 이동 방향에 따라 이동합니다.
        if (isMovingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }

    void StopMoving()
    {
        rigid.velocity = Vector3.zero;
    }
}
