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
        //StartCoroutine(Thinking());
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (state == State.patrol)
        {
            Patrol();
        }

    }

    void Patrol()
    {
        if(state == State.patrol)
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
    }

    void Chase()
    {
        if(state == State.chase)
        {
            if (distanceToPlayer < 5f)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;

                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            else if(distanceToPlayer >= 5f)
            {
                state = State.patrol;
            }
        }
    }

    void Attack()
    {
        if(state == State.attack)
        {

            if (distanceToPlayer < 2f)
            {
                anim.SetTrigger("isAttack");  
            }
        }
    }

    IEnumerator Thinking()
    {
        yield return new WaitForSeconds(0.1f);

       
    }



}
