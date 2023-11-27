using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public Transform[] wallCheck;

    //몬스터 기본 상태 설정
    private void Awake()
    {
        base.Awake();
        maxHp = 30;
        moveSpeed = 2f;
        jumpPower = 15f;
    }

    //지면을 체크하는 위치 변수
    public Transform groundDetected;

    //레이캐스트의 길이 변수
    public float distance;

    //정찰 모드 확인 변수
    public bool isPatrolling;

    //공격 중인지 확인하는 변수
    public bool isAttackCoroutine = false;

    //상태 구조체
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
        //처음 상태를 정찰 상태로 설정
        state = State.patrol;
        isPatrolling = true;

        currentHp = maxHp;
    }

    void Update()
    {
        //정찰 중이거나 추적 중이면 걷는 애니메이션 출력
        //아니면 멈춤
        if (state == State.patrol || state == State.chase)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //정찰 모드일 때는 정찰, 아니면 추적
        if (isPatrolling == true)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
     
    }


    //정찰 함수
    void Patrol()
    {
        if(isPatrolling)
        {
            //정찰을 멈출 때 속도를 0으로 설정하기 때문에 다시 정찰을 하게되면 멈추는 상황 발생
            //따라서 속도 초기화 필요
            moveSpeed = 2f;
            state = State.patrol;

            //플레이어 거리 측정
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            //지면 확인 위치와 플레이어 위치를 비교하여 현재 바라보고 있는 방향 설정
            Vector2 patrolDirection = transform.position.x - groundDetected.position.x > 0 ? Vector2.left: Vector2.right;

            //이동
            transform.Translate(patrolDirection * moveSpeed * Time.deltaTime);

            //지면 확인을 위한 레이캐스트
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetected.position, Vector2.down, distance);

            Debug.DrawRay(groundDetected.position, Vector2.down * 10f, Color.green);

            //지면이 확인되지 않을 경우
            if (!groundInfo.collider)
            {
                //반대 방향으로 이동
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

            //플레이어와 어느정도 가까워 지면 추적 상태로 변경
            if (distanceToPlayer < 5f)
            {
                isPatrolling = false;

                state = State.chase;
            }
        }
    }

    //추적 함수
    void Chase()
    {
        //플레이어와 거리 측정
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //플레이어가 공격 사거리에는 들어오지 않았으나 추적 범위에 있다면
        if (distanceToPlayer > 2f && distanceToPlayer < 5f)
        {
            //속도 초기화
            moveSpeed = 2f;

            //목표 지점 설정, 플레이어의 위치
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            
            //이동
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //플레이어의 위치에 따라 몬스터가 바라보는 방향 설정
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
            //공격 사거리에 들어왔다면 추적을 멈춤
            moveSpeed = 0f;

            //공격 실행
            state = State.attack;

            //공격 여부 확인 변수가 참이 아닐때만 공격하도록 설정
            if(!isAttackCoroutine)
            {
                StartCoroutine(Thinking());
            }
           
        }

        //공격 사거리보다 멀어졌다면 다시 추적 시작
        if(distanceToPlayer >= 2f)
        {
            isAttackCoroutine = false;
            StopAllCoroutines();

            isPatrolling = false;

            state = State.chase;

            //추적 사거리보다도 멀어졌다면 다시 정찰 시작
            if(distanceToPlayer >= 5f)
            {
                isPatrolling = true;

                state = State.patrol;
            }
        }
    }

    //공격 함수
    void Attack()
    {
        anim.SetTrigger("isAttack");

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            //태그가 몬스터인 오브젝트와 충돌시
            if (collider.tag == "Player")
            {
                collider.GetComponent<OssethanMovement>().TakeDamage(5);
            }
        }
    }

    //공격 여부 코루틴
    IEnumerator Thinking()
    {
        //공격 여부 참으로 설정
        isAttackCoroutine = true;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        moveSpeed = 0;

        //2초마다 한 번씩 공격하도록 설정
        if (distanceToPlayer < 2f && state == State.attack)
        {
            Attack();
            yield return new WaitForSeconds(2f);

            //공격을 끝내면 공격 여부 초기화
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
