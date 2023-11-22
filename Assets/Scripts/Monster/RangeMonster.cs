using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public Transform[] wallCheck;

    private void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        jumpPower = 15f;
    }

    public Transform groundDetected;

    public float distance;

    public bool isPatrolling;

    public enum State
    {
        idle,
        patrol,
        chase,
        attack,
    };

    public State state;
    void Start()
    {
        state = State.patrol;

        isPatrolling = true;
    }

    void Update()
    {
        if (state == State.patrol || state == State.chase)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }


        if (isPatrolling == true)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
     
    }

    void Patrol()
    {
        if(isPatrolling)
        {
            moveSpeed = 2f;
            state = State.patrol;

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            Vector2 patrolDirection = transform.position.x - groundDetected.position.x > 0 ? Vector2.left: Vector2.right;

            transform.Translate(patrolDirection * moveSpeed * Time.deltaTime);

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

            Debug.DrawRay(groundDetected.position, Vector2.down * 10f, Color.green);

            if (!groundInfo.collider)
            {
                if (MonsterDirRight)
                {
                    MonsterFlip();
                    MonsterDirRight = false;
                }
                else
                {
                    MonsterFlip();
                    MonsterDirRight = true;
                }
            }

            if (distanceToPlayer < 5f)
            {
                isPatrolling = false;

                state = State.chase;
            }
        }
    }

    void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > 2f && distanceToPlayer < 5f)
        {
            moveSpeed = 2f;
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if(player.transform.position.x > transform.position.x)
            {
                MonsterDirRight = true;
                MonsterFlip();
            }
            else
            {
                MonsterDirRight = false;
                MonsterFlip();
            }
        }
        else
        {
            moveSpeed = 0f;

            state = State.attack;

            StartCoroutine(Thinking());
        }

        if(distanceToPlayer >= 2f)
        {
            StopAllCoroutines();

            isPatrolling = false;

            state = State.chase;

            if(distanceToPlayer >= 5f)
            {
                isPatrolling = true;

                state = State.patrol;
            }
        }
    }

    void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        moveSpeed = 0;
        if (distanceToPlayer < 2f && state == State.attack)
        {
            anim.SetTrigger("isAttack");
        }
        else
        {
            if(distanceToPlayer > 2f)
            {
                state = State.chase;

                if (distanceToPlayer > 5f)
                {
                    state = State.patrol;
                }
            }
            
        }
        
    }

    IEnumerator Thinking()
    {
        

        if(state == State.attack)
        {
            Attack();

            yield return new WaitForSeconds(2f);
        }

        

    }



}
