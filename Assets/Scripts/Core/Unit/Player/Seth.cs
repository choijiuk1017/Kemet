using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Player
{
    public class Seth : Unit
    {
        Rigidbody2D rigid;


        private int comboCount = 0; //���� �޺� Ƚ��
        public int maxComboCount = 3; //�ִ� �޺� Ƚ��
        public float coolTime = 0.5f; //���� ��Ÿ��
        public float comboTimeWindow = 1.0f; //�޺��� �����ϴ� �ð�
        private float curTime; //���� �ð� ����, ���� �� �󸶳� ������ �� ���� ����
        private float lastAttackTime = 0f; //���������� ������ �ð�

        public float jumpForce = 5.0f; //���� ����

        // �����̵� ���� ����
        public float slideForce = 500f; // �����̵� �� �߰��� ��
        public float slideDuration = 0.5f; // �����̵� ���� �ð�
        private bool isSliding = false; // �����̵� ������ ����
        private float slideTime = 0f; // �����̵� �ð� ����
        public float slopeCheckDistance = 0.5f; // ��� üũ �Ÿ�
        public float maxSlopeAngle = 45f; // ĳ���Ͱ� �����̵��� �� �ִ� �ִ� ��簢


        public float parryTime = 0f; //�и��� �ð�


        public bool isJump = false; //���� ���� ����
        public bool isGround = false; //���� �ٴ� ���� �ִ��� Ȯ�� ����
        public bool isParry = false; //�и� ���� ����
        public bool isStair = false;

        //Transform ����
        public Transform atkPos; //���� ��Ÿ�


        //Vector2 ����
        public Vector2 atkBoxSize; //��Ʈ �ڽ�

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

            if (isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if (damageFlashTimer >= damageFlashDuration)
                {
                    spriteRenderer.color = originalColor; // ���� ������ ���ƿ�
                    isDamaged = false; // ������ ���� ����
                    damageFlashTimer = 0f;
                }
            }


            //������ Ű���� ���� ���� �̵��� ����
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            //���⿡ ���� �¿� ����
            //ȸ���� ��Ű�� ������� �ؾ� �ڽ� ������Ʈ�� �ִ� ��Ʈ �ڽ��� �Բ� ȸ����
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isSliding && !Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isSliding && !Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && isGround && !isSliding && !isStair)
            {
                StartSlide();
            }

            // �����̵� ���� ���, �����̵� �ð� ó��
            if (isSliding)
            {
                slideTime += Time.deltaTime;
                if (slideTime >= slideDuration)
                {
                    EndSlide();
                }
            }


            if (curTime <= 0)
            {
                //ZŰ�� ������ �̵����� �ʴ� ���¿����� ���� ����
                if (Input.GetKey(KeyCode.Z) && state != State.Move && !isJump && !isSliding)
                {
                    //�޺� Ȯ��
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

                    //������ �ϸ� ��Ÿ�� �ο�
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

            //����
            if (Input.GetButtonDown("Jump") && isGround && !Input.GetKeyDown(KeyCode.Z) && !isSliding)
            {
                Jump();
            }

            

            //�и�
            if (Input.GetKeyDown(KeyCode.C) && !isSliding)
            {
                anim.SetTrigger("isParrying");
            }
        }

        private void FixedUpdate()
        {
            // �Ϲ� �̵� ó�� (�����̵� ���� �ƴ� ����)
            if (!isSliding)
            {
                Move();
            }
            else
            {
                SlopeCheck();
            }
        }

        protected override void Move()
        {
            // �����¿� �Է� ����
            float moveX = Input.GetAxisRaw("Horizontal");

            // �̵� ���� ���
            Vector3 moveDirection = new Vector3(moveX, 0f, 0f).normalized;

            // �÷��̾� �̵�
            rigid.velocity = new Vector2(moveDirection.x * moveSpeed, rigid.velocity.y);

            // �ִϸ��̼� ���
            if (moveDirection.magnitude > 0 && !isJump)
            {
                anim.SetBool("isWalking", true);
                state = State.Move;
            }
            else
            {
                // ���� �� �ִϸ��̼� ����
                anim.SetBool("isWalking", false);

                state = State.Idle;

            }
        }

        // �����̵� ����
        void StartSlide()
        {
            isSliding = true;
            slideTime = 0f;
            anim.SetTrigger("isSliding"); // �����̵� �ִϸ��̼� Ʈ����

            // �÷��̾ ȸ���� ���¿� ���� �����̵� ���� ���� (eulerAngles.y)
            float slideDirection = transform.eulerAngles.y == 0 ? -1 : 1; // y�� 0�̸� ������, 180�̸� ����

            // �����̵� �� ����
            rigid.AddForce(new Vector2(slideDirection * slideForce, 0f), ForceMode2D.Impulse);

        }

        // �����̵� ����
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


        //�÷��̾� ���� �Լ�
        void Jump()
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);

            anim.SetTrigger("isJump");

            isJump = true;

            isGround = false;
        }

        void DefaultAttack()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                //�±װ� ������ ������Ʈ�� �浹��
                if (collider.tag == "Monster")
                {
                    //������ TakeDamage ȣ��, �������� 1
                    collider.GetComponent<Core.Unit.Monster.Monster>().TakeDamage(damage); 


                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }

        //�浹ó��
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

        //�и�
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


