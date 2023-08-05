using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxSpeed;

    private float curTime;
    public float coolTime = 0.5f;

    public float comboTimeWindow = 1.0f; //콤보를 유지하는 시간
    public int maxComboCount = 3; //최대 콤보 횟수

    private float lastAttackTime = 0f; //마지막으로 공격한 시간
    private int comboCount = 0;  //현재 콤보 횟수


    public Transform pos;
    public Vector2 boxSize;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public enum State
    {
        Idle,
        Move,
        Attack
    };


    public State state;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (curTime <= 0 && state != State.Move)
        {
            if (Input.GetKey(KeyCode.Z))
            {
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

        Debug.Log(comboCount);
    }

    void FixedUpdate()
    {
        move();
    }

    //플레이어 이동함수
    void move()
    {
        // 상하좌우 입력 감지
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 이동 벡터 계산
        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;

        // 플레이어 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // 애니메이션 재생
        if (moveDirection.magnitude > 0)
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
    void DefaultAttack()
    {
        //Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        //foreach (Collider2D collider in collider2Ds)
        //{
        //    //태그가 몬스터인 오브젝트와 충돌시
        //    if (collider.tag == "Monster" || collider.tag == "Spawner")
        //    {
        //        collider.GetComponent<Monster>().TakeDamage(1); //데미지를 입음, HealthPointManager의 TakeDamage와는 다름
        //    }
        //    if (collider.tag == "Dust")
        //    {
        //        collider.GetComponent<Dust>().TakeDamage(1);
        //    }
        //    if (collider.tag == "Door")
        //    {
        //        SceneManager.LoadScene("Puzzle");
        //    }
        //}
        anim.SetTrigger("isDefaultAttack" + comboCount);
        state = State.Attack;
        curTime = coolTime;//공격을 하면 쿨타임 부여
    }

    //공격 범위를 나타내는 기즈모 그리는 함수
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
