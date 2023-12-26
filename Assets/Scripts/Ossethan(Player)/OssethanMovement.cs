using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OssethanMovement : MonoBehaviour
{
    //int ����
    public int currentHP; //�÷��̾� ���� HP
    public int maxHp = 15; //�÷��̾� �ִ� HP
    public int maxComboCount = 3; //�ִ� �޺� Ƚ��

    private int facingDirection = 1; //�÷��̾ �ٶ󺸰� �ִ� ����
    private int comboCount = 0; //���� �޺� Ƚ��


    //float ����
    public float moveSpeed = 5.0f; //�÷��̾� �ӵ�
    public float maxSpeed; //�÷��̾� �ִ� �ӵ�
    public float coolTime = 0.5f; //���� ��Ÿ��
    public float comboTimeWindow = 1.0f; //�޺��� �����ϴ� �ð�
    public float jumpForce = 5.0f; //���� ����
    public float slideDuration = 1f; //�����̵� ���� �ð�
    public float slideSpeed = 10f; //�����̵� �ӵ�
    public float slideCooldown = 2f; //�����̵� ��ٿ� �ð�
    public float parryTime = 0f; //�и��� �ð�

    private float curTime; //���� �ð� ����, ���� �� �󸶳� ������ �� ���� ����
    private float lastAttackTime = 0f; //���������� ������ �ð�
    private float slideTimer = 0f; //�����̵� ���� �ð� ����  
    private float slideCooldownTimer = 0f; //�����̵� ��Ÿ��


    //bool ����
    public bool isJump = false; //���� ���� ����
    public bool isGround = false; //���� �ٴ� ���� �ִ��� Ȯ�� ����
    public bool isParry = false; //�и� ���� ����

    private bool isSliding = false; //�����̵� Ȯ�� ����


    //Transform ����
    public Transform atkPos; //���� ��Ÿ�


    //Vector2 ����
    public Vector2 atkBoxSize; //��Ʈ �ڽ�


    //���� ������Ʈ ���
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D parryBox;


    //�÷��̾� ���� ����ü
    public enum State
    {
        Idle,
        Move,
        Attack
    };

    public State state;


    // Start is called before the first frame update
    //�ʱ� ����
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
        //������ Ű���� ���� ���� �̵��� ����
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //���⿡ ���� �¿� ����
        //ȸ���� ��Ű�� ������� �ؾ� �ڽ� ������Ʈ�� �ִ� ��Ʈ �ڽ��� �Բ� ȸ����
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //����
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
        if(Input.GetButtonDown("Jump") && isGround)
        {
            Jump();
        }

        //���� ����
        if (isJump && Input.GetKeyDown(KeyCode.Z))
        {
            JumpAttack();
        }

        //�и�
        if(Input.GetKeyDown(KeyCode.C))
        {
            Parrying();
        }

    }

    void FixedUpdate()
    {
        //�̵�
        move();

        //�����̵�
        Sliding();

        float horizontalInput = Input.GetAxis("Horizontal");

        //�÷��̾ �ٶ󺸴� ���� Ȯ��
        if (horizontalInput > 0f)
        {
            facingDirection = 1; // ������
        }
        else if (horizontalInput < 0f)
        {
            facingDirection = -1; // ����
        }
    }

    //�÷��̾� �̵��Լ�
    void move()
    {
        // �����¿� �Է� ����
        float moveX = Input.GetAxisRaw("Horizontal");

        // �̵� ���� ���
        Vector3 moveDirection = new Vector3(moveX, 0f, 0f).normalized;

        // �÷��̾� �̵�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

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

        //�Ʒ������� �ް���
        rigid.velocity = new Vector2(rigid.velocity.x, -7);

        //�ٴڿ� ������ �ִϸ��̼� �ʱ�ȭ
        if (isGround)
        {
            anim.ResetTrigger("isJumpAttack");
        }
    }

    //�⺻ ����
    void DefaultAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            //�±װ� ������ ������Ʈ�� �浹��
            if (collider.tag == "Monster")
            {
                //������ TakeDamage ȣ��, �������� 1
                collider.GetComponent<Monster>().TakeDamage(1); 
                //�׷α� ���� �߰� ����

            }
        }

        anim.SetTrigger("isDefaultAttack" + comboCount);
        state = State.Attack;

        //������ �ϸ� ��Ÿ�� �ο�
        curTime = coolTime;
    }
    
    //�����̵�
    void Sliding()
    {
        if (isSliding)
        {
            // �����̵� ���� �� ó��
            slideTimer += Time.deltaTime;

            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                slideTimer = 0f;
                rigid.velocity = new Vector2(0f, rigid.velocity.y); // �����̵��� ������ �� �ӵ� �ʱ�ȭ
            }
        }
        else
        {
            // �����̵��� �ƴ� �� ó��
            slideCooldownTimer += Time.deltaTime;
            if (slideCooldownTimer >= slideCooldown)
            {
                // �����̵� ��ٿ��� ������ �� �����̵� �ߵ��� Ȯ��
                if (Input.GetKey(KeyCode.Z) && state == State.Move) // ���ϴ� Ű�� ����� �� �ֽ��ϴ�.
                {
                    isSliding = true;

                    anim.SetTrigger("isSlide");
                    state = State.Attack;

                    slideCooldownTimer = 0f;
   
                    rigid.velocity = new Vector2(slideSpeed * facingDirection, rigid.velocity.y); // �����̵� �߿� ���ӵ� ����
                }
            }
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

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }


    //�浹ó��
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJump", false);
            isJump = false;

            isGround = true;
        }
    }

    //���� ������ ��Ÿ���� ����� �׸��� �Լ�
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
    }
}
