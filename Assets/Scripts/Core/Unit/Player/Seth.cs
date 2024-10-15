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


        public float parryTime = 0f; //�и��� �ð�


        public bool isJump = false; //���� ���� ����
        public bool isGround = false; //���� �ٴ� ���� �ִ��� Ȯ�� ����
        public bool isParry = false; //�и� ���� ����

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
            //������ Ű���� ���� ���� �̵��� ����
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            //���⿡ ���� �¿� ����
            //ȸ���� ��Ű�� ������� �ؾ� �ڽ� ������Ʈ�� �ִ� ��Ʈ �ڽ��� �Բ� ȸ����
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
                //ZŰ�� ������ �̵����� �ʴ� ���¿����� ���� ����
                if (Input.GetKey(KeyCode.Z) && state != State.Move)
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

                    DefaultAttack();

                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            //����
            if (Input.GetButtonDown("Jump") && isGround && !Input.GetKeyDown(KeyCode.Z))
            {
                Jump();
            }

            //���� ����
            if (isJump && Input.GetKeyDown(KeyCode.Z))
            {
                JumpAttack();
            }

            //�и�
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

        //�÷��̾� ���� �Լ�
        void Jump()
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);

            anim.SetBool("isJump", true);

            isJump = true;

            isGround = false;
        }

        //���� ����
        void JumpAttack()
        {
            anim.SetTrigger("isJumpAttack");

            ////�Ʒ������� �ް���
            //rigid.velocity = new Vector2(rigid.velocity.x, -7);

            ////�ٴڿ� ������ �ִϸ��̼� �ʱ�ȭ
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
                //�±װ� ������ ������Ʈ�� �浹��
                if (collider.tag == "Monster")
                {
                    //������ TakeDamage ȣ��, �������� 1
                    collider.GetComponent<Core.Unit.Monster.Monster>().TakeDamage(damage); 
                    //�׷α� ���� �߰� ����

                }
            }

            anim.SetTrigger("isDefaultAttack" + comboCount);

            //������ �ϸ� ��Ÿ�� �ο�
            curTime = coolTime;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }

        //�浹ó��
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Stair"))
            {
                anim.SetBool("isJump", false);
                isJump = false;

                isGround = true;
            }
        }

        //�и�
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


