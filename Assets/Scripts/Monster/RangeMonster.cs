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
        state = State.patrol;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

        Debug.DrawRay(groundDetected.position, Vector2.down * 10f, Color.green);

        if (!groundInfo.collider)
        {
            if (MonsterDirRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                MonsterDirRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                MonsterDirRight = true;
            }
        }

        if(distanceToPlayer < 5f)
        {
            isPatrolling = false;

            state = State.chase;
        }
        else
        {
            isPatrolling = true;
        }
    }

    void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > 2f && distanceToPlayer < 5f)
        {
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if(player.transform.position.x > transform.position.x)
            {
                MonsterDirRight = false;
                MonsterFlip();
            }
            else
            {
                MonsterDirRight = true;
                MonsterFlip();
            }
        }
        else
        {
            moveSpeed = 0f;

            state = State.attack;
        }

        if(distanceToPlayer > 5f)
        {
            isPatrolling = true;

            state = State.patrol;
        }
    }

    void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < 2f)
        {
            anim.SetTrigger("isAttack");
        }
    }

    IEnumerator Thinking()
    {
        yield return new WaitForSeconds(0.1f);

       
    }



}
