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
        //StartCoroutine(Thinking());
    }

    void Update()
    {
        if(state == State.patrol)
        {
            Patrol();
        }
        else if(state == State.chase)
        {
            Chase();
        }
        else if (state == State.attack)
        {
            Attack();
        }


    }

    void Patrol()
    {
        if(state == State.patrol)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

            anim.SetBool("isWalking", true);

            Debug.DrawRay(groundDetected.position, Vector2.down * 2f, Color.green);

            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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

            if (distanceToPlayer < 5f && groundInfo.collider)
            {
                moveSpeed = 0f;
                state = State.chase;
            }
        }
    }

    void Chase()
    {
        if(state == State.chase)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < 5f)
            {
                float direction = (player.transform.position.x > transform.position.x) ? 1f : -1f;
                transform.Translate(new Vector2(direction, 0) * moveSpeed * Time.deltaTime);

                if (distanceToPlayer < 2f)
                {
                    moveSpeed = 0f;
                    state = State.attack;
                }
            }
        }
    }

    void Attack()
    {
        if(state == State.attack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < 2f)
            {
                anim.SetTrigger("isAttack");  
            }
        }
    }

    IEnumerator Thinking()
    {
       while(true)
       {
            if (state == State.patrol)
            {
                Patrol();
            }
            else
            {

            
            }
       }

       
        
    }



}
