using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Core.Unit;

namespace Core.Unit.Player
{
    public class Seth : Unit
    {
        Rigidbody2D rigid;


        //공격 관련 변수
        private int comboCount = 0; //현재 콤보 횟수
        public int maxComboCount = 3; //최대 콤보 횟수
        public float coolTime = 0.5f; //공격 쿨타임
        public float comboTimeWindow = 1.0f; //콤보를 유지하는 시간
        private float curTime; //현재 시간 변수, 공격 후 얼마나 지났는 지 측정 위함
        private float lastAttackTime = 0f; //마지막으로 공격한 시간
        public Transform atkPos; //공격 사거리

        public Vector2 atkBoxSize; //히트 박스


        //점프 관련 함수
        public float jumpForce = 7.0f; //점프 높이
        public bool isJump = false; //점프 실행 여부

        // 슬라이딩 관련 변수
        public float slideForce = 500f; // 슬라이딩 시 추가할 힘
        public float slideDuration = 0.5f; // 슬라이딩 지속 시간
        public bool isSliding = false; // 슬라이딩 중인지 여부
        private float slideTime = 0f; // 슬라이딩 시간 측정
        public float slopeCheckDistance = 0.5f; // 경사 체크 거리
        public float maxSlopeAngle = 45f; // 캐릭터가 슬라이딩할 수 있는 최대 경사각

        //패링 관련 변수
        public float parryTime = 0f; //패링한 시간
        public bool isParry = false; //패링 실행 여부
        [SerializeField]
        private BoxCollider2D parryBox;

        //타격 판정 변수
        private bool isStunned = false;
        private float stunDuration = 0.5f;
        private float stunTimer = 0f;

        //기타 변수
        public bool isGround = false; //현재 바닥 위에 있는지 확인 여부
        public bool isStair = false;
        public bool isDead = false;


        public Slider playerHPUI;

        private static Seth instance;


        //플레이어의 상태를 나타낼 구조체
        public enum State
        {
            Idle,
            Move,
            Attack
        };
        public State state;

        void Awake()
        {
            // 플레이어 중복 방지
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }


        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(SetSpawnPosition());
        }

        IEnumerator SetSpawnPosition()
        {
            yield return new WaitForSeconds(0.01f); // 씬이 완전히 로드될 때까지 대기

            anim.SetBool("isDead", false);
            isDead = false;

            currentHealth = maxHealth;

            if (playerHPUI != null)
            {
                playerHPUI.maxValue = maxHealth;
                playerHPUI.value = currentHealth;
            }

            GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log("? 플레이어가 'PlayerSpawn' 위치로 이동: " + spawnPoint.transform.position);
            }
            else
            {
                Debug.LogError("? 'PlayerSpawn' 태그를 가진 오브젝트가 씬에서 발견되지 않았습니다.");
            }
        }

        protected override void Init()
        {
            base.Init();

            maxHealth = 100f;

            currentHealth = maxHealth;

            if(playerHPUI != null)
            {
                playerHPUI.maxValue = maxHealth;
                playerHPUI.value = currentHealth;
            }

            moveSpeed = 5f;

            damage = 10f;

            defense = 10f;

            position = this.transform.position;

            SceneManager.sceneLoaded += OnSceneLoaded;


            rigid = this.GetComponent<Rigidbody2D>();
           

            state = State.Idle;
        }

        private void Update()
        {

            

            if (isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if (damageFlashTimer >= damageFlashDuration)
                {
                    spriteRenderer.color = originalColor; // 원래 색으로 돌아옴
                    isDamaged = false; // 데미지 상태 해제
                    damageFlashTimer = 0f;
                }
            }

            if (isDead) return;

            if (isStunned)
            {
                stunTimer += Time.deltaTime;
                if(stunTimer >= stunDuration)
                {
                    isStunned = false;
                    stunTimer = 0f; 
                }
                return;
            }

            
            

            //유저가 키에서 손을 떼면 이동을 멈춤
            if (Input.GetButtonUp("Horizontal") && !isDead)
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            //방향에 따라 좌우 반전
            //회전을 시키는 방식으로 해야 자식 오브젝트에 있는 히트 박스도 함께 회전함
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isSliding && !Input.GetKeyDown(KeyCode.LeftArrow) && !isDead)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isSliding && !Input.GetKeyDown(KeyCode.RightArrow) && !isDead)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && isGround && !isSliding && !isStair && !isDead)
            {
                StartSlide();
            }

            // 슬라이딩 중인 경우, 슬라이드 시간 처리
            if (isSliding)
            {
                slideTime += Time.deltaTime;
                if (slideTime >= slideDuration)
                {
                    EndSlide();
                }
            }


            if (curTime <= 0 && !isDead)
            {
                //Z키를 누르고 이동하지 않는 상태에서만 공격 가능
                if (Input.GetKey(KeyCode.Z) && state != State.Move && !isJump && !isSliding)
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

                    anim.SetTrigger("isDefaultAttack" + comboCount);

                    //공격을 하면 쿨타임 부여
                    curTime = coolTime;

                }
                
                if (isJump && Input.GetKeyDown(KeyCode.Z))
                {
                    anim.SetTrigger("isJumpAttack");

                    curTime = coolTime;
                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            //점프
            if (Input.GetButtonDown("Jump") && isGround && !Input.GetKeyDown(KeyCode.Z) && !isSliding && !isDead)
            {
                Jump();
            }

            

            //패링
            if (Input.GetKeyDown(KeyCode.C) && !isSliding && !isDead)
            {
                anim.SetTrigger("isParrying");
            }

        }

        private void FixedUpdate()
        {

            if (!isStunned && !isDead)
            {
                // 일반 이동 처리 (슬라이딩 중이 아닐 때만)
                if (!isSliding)
                {
                    Move();
                }
                else
                {
                    SlopeCheck();
                }
            }
            else
            {
                rigid.velocity = Vector2.zero;
            }
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

        // 슬라이드 시작
        void StartSlide()
        {
            isSliding = true;
            slideTime = 0f;
            anim.SetTrigger("isSliding"); // 슬라이딩 애니메이션 트리거

            // 플레이어가 회전된 상태에 맞춰 슬라이딩 방향 결정 (eulerAngles.y)
            float slideDirection = transform.eulerAngles.y == 0 ? -1 : 1; // y가 0이면 오른쪽, 180이면 왼쪽

            // 슬라이드 힘 적용
            rigid.AddForce(new Vector2(slideDirection * slideForce, 0f), ForceMode2D.Impulse);

        }

        // 슬라이드 종료
        void EndSlide()
        {
            isSliding = false;
            anim.ResetTrigger("isSliding");
            state = State.Idle;
        }

        void SlopeCheck()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle <= maxSlopeAngle)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, -slideForce * Time.fixedDeltaTime);
                }
            }
        }


        //플레이어 점프 함수
        void Jump()
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            anim.ResetTrigger("isLand");
            anim.SetTrigger("isJump");

            isJump = true;

            isGround = false;
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
                }
                else if(collider.tag == "Boss")
                {
                    collider.GetComponentInParent<Core.Unit.Boss.Boss>().TakeDamage(damage);
                }
            }
        }

        public override void TakeDamage(float damageAmount)
        {

            if (isDead) return;
            base.TakeDamage(damageAmount);

            UpdateHealthUI();

            isStunned = true;
            anim.SetTrigger("TakeDamage");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }

        //충돌처리
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                if (isJump && !isGround)
                {
                    anim.SetTrigger("isLand");
                }
                
                isJump = false;
                isStair = false;
                isGround = true;
               
            }
            else if(col.gameObject.CompareTag("Stair"))
            {
                if (isJump && !isGround)
                {
                    anim.SetTrigger("isLand");
                }

                isJump = false;
                isStair = true;

                isGround = true;

            }
        }

        private void UpdateHealthUI()
        {
            if (playerHPUI != null)
            {
                playerHPUI.value = currentHealth;
            }
        }

        protected override void Die()
        {
            isDead = true;
            anim.SetBool("isDead", true); // 죽는 애니메이션 실행
            rigid.velocity = Vector2.zero; // 이동 중지
        }

        //패링
        void Parrying()
        {
            parryBox.enabled = true;
                     
            Invoke("UnParrying", 0.3f);
        }

        void UnParrying()
        {
            parryBox.enabled = false;
        }
    } 

}


