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

        if (distanceToPlayer < 5f)
        {
            Chase();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

        anim.SetBool("isWalking", true);

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
        
        }
        else if (distanceToPlayer >= 5f)
        {
            state = State.patrol;
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
