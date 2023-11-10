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

    State state;
    void Start()
    {

        state = State.patrol;

    }

    void Update()
    {

        
    }

    void Patrol()
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
        
        if(distanceToPlayer < 5)
        {
            moveSpeed = 0f;
            state = State.chase;
        }
    }

    void Chase()
    {
        
    }

    




}
