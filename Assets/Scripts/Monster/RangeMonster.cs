using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // ������ �̵� �ӵ�
    public float moveDistance = 5.0f;

    public float attackRange = 2.0f;
    public float chaseRange = 5.0f;
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

    public enum State
    {
        Idle,
        Move,
        Attack
    }
    
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        initialPositionX = transform.position.x;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;  

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            if (distanceToPlayer <= attackRange)
            {
                state = State.Attack;
            }
                state = State.Move;
        }
        else
        {
            state = State.Idle;
        }

        if (state == State.Idle)
        {
            MoveLeftRight();
        }
        else if(state == State.Move)
        {
            ChasePlayer();
        }
        else if(state==State.Attack)
        {
            StartAttack();
        }

       if(velocity.x < 0)
       {
            spriteRenderer.flipX = false;
       }
       else if(velocity.x > 0)
       {
            spriteRenderer.flipX = true;
       }
    }

    void StartAttack()
    {
        // �÷��̾ ���� ���� ���� �ְ�, ���� ��ٿ��� �����ٸ� ������ ����
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            anim.SetTrigger("isAttack");
            lastAttackTime = Time.time;
        }
    }

    void MoveLeftRight()
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
