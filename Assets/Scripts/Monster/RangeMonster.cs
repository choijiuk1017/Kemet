using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    public float moveSpeed = 2.0f;  // 몬스터의 이동 속도
    public float moveDistance = 5.0f;

    public float attackRange = 2.0f;
    public float chaseRange = 5.0f;
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
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //배회 코루틴 시작
        StartCoroutine(Wander());
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= chaseRange)
        {
            StopCoroutine(Wander());
            StopCoroutine(ChasePlayer());
        }

    }

    IEnumerator Wander()
    {
        while(true)
        {
            float waitTime = Random.Range(1.0f, 3.0f);

            yield return new WaitForSeconds(waitTime);

            isMovingRight = !isMovingRight;

            spriteRenderer.flipX = !isMovingRight;

            Vector2 moveDirection = isMovingRight ? Vector2.right : Vector2.left;
            rigid.velocity = new Vector2(moveDirection.x * moveSpeed, rigid.velocity.y);
        }
    }

    IEnumerator ChasePlayer()
    {
        while(true)
        {
            Vector2 moveDirection = (player.position - transform.position).normalized;

            rigid.velocity = new Vector2(moveDirection.x * moveSpeed, rigid.velocity.y);

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if(distanceToPlayer > chaseRange)
            {
                StopCoroutine(ChasePlayer());
                StartCoroutine(Wander());

                yield break;
            }

            yield return null;
        }
    }
}
