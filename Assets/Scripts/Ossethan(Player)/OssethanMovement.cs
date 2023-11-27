using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OssethanMovement : MonoBehaviour
{
    //플레이어 현재 HP
    public int currentHP;

    //플레이어 최대 HP
    public int maxHp = 15;

    //플레이어 속도
    public float moveSpeed = 5.0f;

    //플레이어 최대 속도
    public float maxSpeed;

    //플레이어가 바라보고 있는 방향
    private int facingDirection = 1;

    //현재 시간 변수, 공격 후 얼마나 지났는 지 측정 위함
    private float curTime;

    //공격 쿨타임
    public float coolTime = 0.5f;

    //콤보를 유지하는 시간
    public float comboTimeWindow = 1.0f;

    //최대 콤보 횟수
    public int maxComboCount = 3;

    //마지막으로 공격한 시간
    private float lastAttackTime = 0f;

    //현재 콤보 횟수
    private int comboCount = 0;  

    //점프 높이
    public float jumpForce = 5.0f;

    //점프 실행 여부
    public bool isJump = false;

    //현재 바닥 위에 있는지 확인 여부
    public bool isGround = false;

    //슬라이딩 지속 시간
    public float slideDuration = 1f;

    //슬라이딩 속도
    public float slideSpeed = 10f;

    //슬라이딩 쿨다운 시간
    public float slideCooldown = 2f; 

    //슬라이딩 확인 여부
    private bool isSliding = false;

    //슬라이딩 직후 시간 측정 
    private float slideTimer = 0f;

    //슬라이딩 쿨타임
    private float slideCooldownTimer = 0f;

    //공격 사거리
    public Transform pos;
    
    //히트 박스
    public Vector2 boxSize;

    //기초 컴포넌트 요소
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    //플레이어 상태 구조체
    public enum State
    {
        Idle,
        Move,
        Attack
    };

    public State state;

    // Start is called before the first frame update
    //초기 설정
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHP = maxHp;

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //유저가 키에서 손을 떼면 이동을 멈춤
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //방향에 따라 좌우 반전
        //회전을 시키는 방식으로 해야 자식 오브젝트에 있는 히트 박스도 함께 회전함
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //공격
        if (curTime <= 0)
        {
            //Z키를 누르고 이동하지 않는 상태에서만 공격 가능
            if (Input.GetKey(KeyCode.Z) && state != State.Move)
            {
                //콤보 확인
                if (Time.time - lastAttackTime < comboTimeWindow)
                {
                    lastAttackTime = Time.time;

                    comboCount++;

                    if (comboCount > maxComboCount)
                    {
                        comboCount = 1;
                    }
                }
                else
                {
                    comboCount = 1;
                    lastAttackTime = Time.time;
                }

                DefaultAttack();
                
            }
        }
        else 
        {
            curTime -= Time.deltaTime;
        }

        //점프
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //점프 공격
        if (isJump && Input.GetKeyDown(KeyCode.Z))
        {
            JumpAttack();
        }

        //패링
        if(Input.GetKeyDown(KeyCode.C))
        {
            Parrying();
        }

    }

    void FixedUpdate()
    {
        //이동
        move();

        //슬라이딩
        Sliding();

        float horizontalInput = Input.GetAxis("Horizontal");

        //플레이어가 바라보는 방향 확인
        if (horizontalInput > 0f)
        {
            facingDirection = 1; // 오른쪽
        }
        else if (horizontalInput < 0f)
        {
            facingDirection = -1; // 왼쪽
        }
    }

    //플레이어 이동함수
    void move()
    {
        // 상하좌우 입력 감지
        float moveX = Input.GetAxisRaw("Horizontal");

        // 이동 벡터 계산
        Vector3 moveDirection = new Vector3(moveX, 0f, 0f).normalized;

        // 플레이어 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 애니메이션 재생
        if (moveDirection.magnitude > 0 && !isJump)
        {
            anim.SetBool("isWalking", true);

            state = State.Move;
        }
        else
        {
            // 정지 시 애니메이션 정지
            anim.SetBool("isWalking", false);

            state = State.Idle;
        }

     
    }

    //플레이어 점프 함수
    void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);

        anim.SetBool("isJump", true);

        isJump = true;
    }

    //점프 공격
    void JumpAttack()
    {
        anim.SetTrigger("isJumpAttack");

        //아래쪽으로 급강하
        rigid.velocity = new Vector2(rigid.velocity.x, -7);

        //바닥에 닿으면 애니메이션 초기화
        if (isGround)
        {
            anim.ResetTrigger("isJumpAttack");
        }
    }

    //기본 공격
    void DefaultAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            //태그가 몬스터인 오브젝트와 충돌시
            if (collider.tag == "Monster")
            {
                collider.GetComponent<Monster>().TakeDamage(1); 
            }
        }

        anim.SetTrigger("isDefaultAttack" + comboCount);
        state = State.Attack;

        //공격을 하면 쿨타임 부여
        curTime = coolTime;
    }
    
    //슬라이딩
    void Sliding()
    {
        if (isSliding)
        {
            // 슬라이딩 중일 때 처리
            slideTimer += Time.deltaTime;

            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                slideTimer = 0f;
                rigid.velocity = new Vector2(0f, rigid.velocity.y); // 슬라이딩이 끝났을 때 속도 초기화
            }
        }
        else
        {
            // 슬라이딩이 아닐 때 처리
            slideCooldownTimer += Time.deltaTime;
            if (slideCooldownTimer >= slideCooldown)
            {
                // 슬라이딩 쿨다운이 끝났을 때 슬라이딩 발동을 확인
                if (Input.GetKey(KeyCode.Z) && state == State.Move) // 원하는 키를 사용할 수 있습니다.
                {
                    isSliding = true;

                    anim.SetTrigger("isSlide");
                    state = State.Attack;

                    slideCooldownTimer = 0f;
   
                    rigid.velocity = new Vector2(slideSpeed * facingDirection, rigid.velocity.y); // 슬라이딩 중에 가속도 적용
                }
            }
        }
    }

    //패링
    void Parrying()
    {
        anim.SetTrigger("isParrying");  
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }


    //충돌처리
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    //공격 범위를 나타내는 기즈모 그리는 함수
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
