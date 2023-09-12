using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // ������ �̵� �ӵ�
    public float moveDistance = 5.0f;

    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;

    private Transform player;
    private float lastAttackTime;

    private bool playerDetected = false;

    private bool isMovingRight = true;  // ���Ͱ� ���� ���������� �̵� ������ ����

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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        

        // �÷��̾ ���� ���� ���� �ְ�, ���� ��ٿ��� �����ٸ� ������ ����
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            playerDetected = true;

            StartAttack();
            lastAttackTime = Time.time;

            anim.SetBool("isAttack", true);
        }

        if (playerDetected)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }

        if (isMovingRight)
        {
            spriteRenderer.flipX = true;  // ���������� �̵� ���̸� �������� ����
        }
        else
        {
            spriteRenderer.flipX = false;  // �������� �̵� ���̸� �̹����� �¿�� ����
        }
    }

    void StartAttack()
    {
        anim.SetBool("isAttack", true);
    }

    void Move()
    {
        // ���� �̵��� �Ÿ� ���
        currentDistance = Mathf.Abs(transform.position.x - initialPositionX);

        // ���� �Ÿ�(moveDistance)�� �����ϸ� �̵� ������ �ٲߴϴ�.
        if (currentDistance >= moveDistance)
        {
            isMovingRight = !isMovingRight;
            initialPositionX = transform.position.x;
        }

        // �̵� ���⿡ ���� �̵��մϴ�.
        if (isMovingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        anim.SetBool("isWalking", true);
    }

    void ChasePlayer()
    {
        Vector2 playerDirection = (player.position - transform.position).normalized;
        
        transform.Translate(playerDirection * moveSpeed * Time.deltaTime);
    }
}
