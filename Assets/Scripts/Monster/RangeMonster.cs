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
        initialPositionX = transform.position.x;

        state = State.patrol;

        StartCoroutine(Patrol());
    }

    void Update()
    {
        MonsterFlip();
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

        if(groundInfo.collider == false)
        {

        }
    }

    IEnumerator Patrol()
    {
        Debug.Log("patrol");
        
        while (true)
        {
            
            
            yield return new WaitForSeconds(2.0f);
            

        }
    }




}
