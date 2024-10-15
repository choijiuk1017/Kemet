using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Player
{
    public class Seth : Unit
    {
        Rigidbody2D rigid;


        private int comboCount = 0; //현재 콤보 횟수
        public int maxComboCount = 3; //최대 콤보 횟수
        public float coolTime = 0.5f; //공격 쿨타임
        public float comboTimeWindow = 1.0f; //콤보를 유지하는 시간
        private float curTime; //현재 시간 변수, 공격 후 얼마나 지났는 지 측정 위함
        private float lastAttackTime = 0f; //마지막으로 공격한 시간

        public float jumpForce = 5.0f; //점프 높이


        public float parryTime = 0f; //패링한 시간


        public bool isJump = false; //점프 실행 여부
        public bool isGround = false; //현재 바닥 위에 있는지 확인 여부
        public bool isParry = false; //패링 실행 여부

        //Transform 변수
        public Transform atkPos; //공격 사거리


        //Vector2 변수
        public Vector2 atkBoxSize; //히트 박스

        [SerializeField]
        private BoxCollider2D parryBox;

        public enum State
        {
            Idle,
            Move,
            Attack
        };
        public State state;


        protected override void Init()
        {
            base.Init();

            maxHealth = 100f;

            currentHealth = maxHealth;

            moveSpeed = 5f;

            damage = 10f;

            defense = 10f;

            position = this.transform.position;

            rigid = this.GetComponent<Rigidbody2D>();


            state = State.Idle;
        }

        private void Update()
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
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }


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
            if (Input.GetButtonDown("Jump") && isGround && !Input.GetKeyDown(KeyCode.Z))
            {
                Jump();
            }

            //점프 공격
            if (isJump && Input.GetKeyDown(KeyCode.Z))
            {
                JumpAttack();
            }

            //패링
            if (Input.GetKeyDown(KeyCode.C))
            {
                Parrying();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        protected override void Move()
        {
            // 상하좌우 입력 감지
            float moveX = Input.GetAxisRaw("Horizontal");

            // 이동 벡터 계산
            Vector3 moveDirection = new Vector3(moveX, 0f, 0f).normalized;

            // 플레이어 이동
            rigid.velocity = new Vector2(moveDirection.x * moveSpeed, rigid.velocity.y);

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

            isGround = false;
        }

        //점프 공격
        void JumpAttack()
        {
            anim.SetTrigger("isJumpAttack");

            ////아래쪽으로 급강하
            //rigid.velocity = new Vector2(rigid.velocity.x, -7);

            ////바닥에 닿으면 애니메이션 초기화
            //if (isGround)
            //{
            //    anim.ResetTrigger("isJumpAttack");
            //}
        }

        void DefaultAttack()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                //태그가 몬스터인 오브젝트와 충돌시
                if (collider.tag == "Monster")
                {
                    //몬스터의 TakeDamage 호출, 데미지는 1
                    collider.GetComponent<Core.Unit.Monster.Monster>().TakeDamage(damage); 
                    //그로기 감소 추가 예정

                }
            }

            anim.SetTrigger("isDefaultAttack" + comboCount);

            //공격을 하면 쿨타임 부여
            curTime = coolTime;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }

        //충돌처리
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Stair"))
            {
                anim.SetBool("isJump", false);
                isJump = false;

                isGround = true;
            }
        }

        //패링
        void Parrying()
        {
            parryBox.enabled = true;
            anim.SetTrigger("isParrying");
            Invoke("UnParrying", 0.7f);
        }

        void UnParrying()
        {
            parryBox.enabled = false;
        }
    } 

}


