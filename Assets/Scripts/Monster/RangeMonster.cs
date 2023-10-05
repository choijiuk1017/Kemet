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
        
    }

    IEnumerator Patrol()
    {
        Debug.Log("patrol");
        while(true)
        {
            
            if(Mathf.Abs(initialPositionX - transform.position.x) == patrolDistance)
            {
                MonsterDirRight = !MonsterDirRight;

                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            else
            {
                Vector2 moveDirection = MonsterDirRight ? Vector2.right : Vector2.left;

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                MonsterFlip();

                anim.SetBool("isWalking", true);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }


}
