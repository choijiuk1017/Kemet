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

    private float lastAttackTime;

    [SerializeField]
    private float distanceToPlayer;


    private Transform player;

    public Transform pos;


    private bool playerDetected = false;

    private bool isMovingRight = true;  // 몬스터가 현재 오른쪽으로 이동 중인지 여부

    private bool isAttacking = false;


    private Vector2 initialPosition;

    public Vector2 boxSize;


    Animator anim;

    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;

    public enum State
    {
        Idle,
        Wander,
        Chase,
        Attack
    };

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        state = State.Wander;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);   

        if(state == State.Wander)
        {
            StartCoroutine(Wander());
        }

        //방향에 따라 좌우 반전
        if (!isMovingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (isMovingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }


    IEnumerator Wander()
    {
        if (isMovingRight)
        {
            // Move right
            while (transform.position.x < initialPosition.x + moveDistance)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            // Move left
            while (transform.position.x > initialPosition.x - moveDistance)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // Change direction
        isMovingRight = !isMovingRight;
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
                                
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
                

                //spriteRenderer.flipX = direction.sqrMagnitude > 0;

                initialPosition = transform.position;
            }
         
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("Attack");
        isAttacking = true;

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;

        anim.ResetTrigger("isAttack");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
