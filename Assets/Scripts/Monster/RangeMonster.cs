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

    private float minX;
    private float maxX;

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
        minX = transform.position.x - 5f;
        maxX = transform.position.x + 5f;

        state = State.patrol;

        StartCoroutine(Patrol());
    }

    void Update()
    {
        
    }
    void SetX()
    {
        minX = transform.position.x - 5f;
        maxX = transform.position.x + 5f;
    }
    IEnumerator Patrol()
    {
        Debug.Log("patrol");
        while(true)
        {
            Vector2 direction = new Vector2(minX - transform.position.x, transform.localPosition.y).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            MonsterFlip();

            anim.SetBool("isWalking", true);

            if (transform.position.x < maxX)
            {
                anim.SetBool("isWalking", false);
                yield return new WaitForSeconds(2f);
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }
                
        }
    }

}
