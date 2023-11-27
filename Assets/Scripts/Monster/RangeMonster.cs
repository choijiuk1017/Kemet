using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public Transform[] wallCheck;

    //���� �⺻ ���� ����
    private void Awake()
    {
        base.Awake();
        maxHp = 30;
        moveSpeed = 2f;
        jumpPower = 15f;
    }

    //������ üũ�ϴ� ��ġ ����
    public Transform groundDetected;

    //����ĳ��Ʈ�� ���� ����
    public float distance;

    //���� ��� Ȯ�� ����
    public bool isPatrolling;

    //���� ������ Ȯ���ϴ� ����
    public bool isAttackCoroutine = false;

    //���� ����ü
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
        //ó�� ���¸� ���� ���·� ����
        state = State.patrol;
        isPatrolling = true;

        currentHp = maxHp;
    }

    void Update()
    {
        //���� ���̰ų� ���� ���̸� �ȴ� �ִϸ��̼� ���
        //�ƴϸ� ����
        if (state == State.patrol || state == State.chase)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //���� ����� ���� ����, �ƴϸ� ����
        if (isPatrolling == true)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
     
    }


    //���� �Լ�
    void Patrol()
    {
        if(isPatrolling)
        {
            //������ ���� �� �ӵ��� 0���� �����ϱ� ������ �ٽ� ������ �ϰԵǸ� ���ߴ� ��Ȳ �߻�
            //���� �ӵ� �ʱ�ȭ �ʿ�
            moveSpeed = 2f;
            state = State.patrol;

            //�÷��̾� �Ÿ� ����
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            //���� Ȯ�� ��ġ�� �÷��̾� ��ġ�� ���Ͽ� ���� �ٶ󺸰� �ִ� ���� ����
            Vector2 patrolDirection = transform.position.x - groundDetected.position.x > 0 ? Vector2.left: Vector2.right;

            //�̵�
            transform.Translate(patrolDirection * moveSpeed * Time.deltaTime);

            //���� Ȯ���� ���� ����ĳ��Ʈ
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

            Debug.DrawRay(groundDetected.position, Vector2.down * 10f, Color.green);

            //������ Ȯ�ε��� ���� ���
            if (!groundInfo.collider)
            {
                //�ݴ� �������� �̵�
                if (MonsterDirRight)
                {
                    MonsterFlip();
                    MonsterDirRight = false;
                }
                else
                {
                    MonsterFlip();
                    MonsterDirRight = true;
                }
            }

            //�÷��̾�� ������� ����� ���� ���� ���·� ����
            if (distanceToPlayer < 5f)
            {
                isPatrolling = false;

                state = State.chase;
            }
        }
    }

    //���� �Լ�
    void Chase()
    {
        //�÷��̾�� �Ÿ� ����
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //�÷��̾ ���� ��Ÿ����� ������ �ʾ����� ���� ������ �ִٸ�
        if (distanceToPlayer > 2f && distanceToPlayer < 5f)
        {
            //�ӵ� �ʱ�ȭ
            moveSpeed = 2f;

            //��ǥ ���� ����, �÷��̾��� ��ġ
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            
            //�̵�
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //�÷��̾��� ��ġ�� ���� ���Ͱ� �ٶ󺸴� ���� ����
            if(player.transform.position.x > transform.position.x)
            {
                MonsterDirRight = true;
                MonsterFlip();
            }
            else
            {
                MonsterDirRight = false;
                MonsterFlip();
            }
        }
        else
        {
            //���� ��Ÿ��� ���Դٸ� ������ ����
            moveSpeed = 0f;

            //���� ����
            state = State.attack;

            //���� ���� Ȯ�� ������ ���� �ƴҶ��� �����ϵ��� ����
            if(!isAttackCoroutine)
            {
                StartCoroutine(Thinking());
            }
           
        }

        //���� ��Ÿ����� �־����ٸ� �ٽ� ���� ����
        if(distanceToPlayer >= 2f)
        {
            isAttackCoroutine = false;
            StopAllCoroutines();

            isPatrolling = false;

            state = State.chase;

            //���� ��Ÿ����ٵ� �־����ٸ� �ٽ� ���� ����
            if(distanceToPlayer >= 5f)
            {
                isPatrolling = true;

                state = State.patrol;
            }
        }
    }

    //���� �Լ�
    void Attack()
    {
        anim.SetTrigger("isAttack");

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            //�±װ� ������ ������Ʈ�� �浹��
            if (collider.tag == "Player")
            {
                collider.GetComponent<OssethanMovement>().TakeDamage(5);
            }
        }
    }

    //���� ���� �ڷ�ƾ
    IEnumerator Thinking()
    {
        //���� ���� ������ ����
        isAttackCoroutine = true;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        moveSpeed = 0;

        //2�ʸ��� �� ���� �����ϵ��� ����
        if (distanceToPlayer < 2f && state == State.attack)
        {
            Attack();
            yield return new WaitForSeconds(2f);

            //������ ������ ���� ���� �ʱ�ȭ
            isAttackCoroutine = false;

        }
        else
        {
            if (distanceToPlayer > 2f)
            {
                isAttackCoroutine = false;
                state = State.chase;

                if (distanceToPlayer > 5f)
                {
                    state = State.patrol;
                }
            }

        }
    }



}
