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

    [SerializeField]
    private float distanceToPlayer;
    private Transform player;
    private float lastAttackTime;

    private bool playerDetected = false;

    private bool isMovingRight = true;  // 몬스터가 현재 오른쪽으로 이동 중인지 여부

    private bool isStopped = false;

    private Vector2 initialPosition;


    Animator anim;

    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        
        //배회 코루틴 시작
        StartCoroutine(Wander());

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange && distanceToPlayer > attackRange)
        {
            StopCoroutine(Wander());
            StartCoroutine(ChasePlayer());
        }

    }


    IEnumerator Wander()
    {
        while(true)
        {
            Debug.Log("Wander");

            Vector2 target = isMovingRight ? initialPosition + new Vector2(moveDistance, 0f) : initialPosition;

            spriteRenderer.flipX = isMovingRight;
            while (Mathf.Abs(transform.position.x - target.x) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
                yield return null;
            }

            isMovingRight = !isMovingRight;

            anim.SetBool("isWalking", false);
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
            {
                StopCoroutine(Wander());
                StartCoroutine(ChasePlayer());
            }
            yield return new WaitForSeconds(2f);

        }
    }

    IEnumerator ChasePlayer()
    {
        while(true)
        {
            Debug.Log("ChasePlayer");
            if (player != null)
            {
                // 플레이어가 오른쪽에 있으면 몬스터는 오른쪽으로 이동하고, 그 반대면 왼쪽으로 이동
                Vector2 direction = new Vector2((player.position.x - transform.position.x), 0).normalized;
                if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
                {                   
                    transform.Translate(direction * moveSpeed * Time.deltaTime);
                    anim.SetBool("isWalking", true);
                }

                spriteRenderer.flipX = direction.sqrMagnitude > 0;

                if (distanceToPlayer <= attackRange)
                {
                    Debug.Log("StopChasePlayer");
                    StopCoroutine(ChasePlayer());
                    anim.SetBool("isWalking", false);
                    rigid.velocity = Vector2.zero;
                    rigid.angularVelocity = 0f;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

}
