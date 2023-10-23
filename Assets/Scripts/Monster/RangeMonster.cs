using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public Transform[] wallCheck;

    private void Awake()
    {
        base.Awake();
        moveSpeed = 5f;
        jumpPower = 15f;
    }

    public float patrolDistance = 5.0f;

    private float initialPositionX;

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
        initialPositionX = transform.position.x;

        state = State.patrol;

        StartCoroutine(Patrol());
    }

    void Update()
    {
        MonsterFlip();

    }

    IEnumerator Patrol()
    {
        Debug.Log("patrol");
        
        while (true)
        {
            float nextmove = Random.Range(-1, 2);

            if(nextmove < 0)
            {
                MonsterDirRight = false;
            }
            else
            {
                MonsterDirRight = true;
            }

            rigid.velocity = new Vector2(nextmove , rigid.velocity.y);

            if (rigid.velocity == Vector2.zero)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true); 
            }
            
            yield return new WaitForSeconds(5.0f);
            

        }
    }


}
