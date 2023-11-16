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

    public float distanceToPlayer;

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
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (state == State.patrol || state == State.chase)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        

        if(distanceToPlayer > 5f)
        {
            Patrol();

            if (distanceToPlayer < 5f)
            {
                Chase();
            }
        }

        if (distanceToPlayer < 1f)
        {
            moveSpeed = 0f;
            anim.SetBool("isWalking", false);
        }
    }

    void Patrol()
    {
        state = State.patrol;

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
    }

    void Chase()
    {
        if (distanceToPlayer < 5f)
        {
            state = State.chase;
            

            Vector2 direction = (player.transform.position - transform.position).normalized;

            transform.Translate(direction * moveSpeed * Time.deltaTime);

            if(direction.x < 0f)
            {
                MonsterDirRight = false;
                MonsterFlip();
            }
            else if(direction.x > 0f)
            {
                MonsterDirRight = true;
                MonsterFlip();
            }

            if (distanceToPlayer >= 5f)
            {
                state = State.patrol;
            }
        }

    }

    void Attack()
    {
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
