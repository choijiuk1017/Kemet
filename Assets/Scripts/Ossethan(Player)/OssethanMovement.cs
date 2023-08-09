using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OssethanMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxSpeed;
    private int facingDirection = 1;

    private float curTime;
    public float coolTime = 0.5f;

    public float comboTimeWindow = 1.0f; //�޺��� �����ϴ� �ð�
    public int maxComboCount = 3; //�ִ� �޺� Ƚ��

    private float lastAttackTime = 0f; //���������� ������ �ð�
    private int comboCount = 0;  //���� �޺� Ƚ��


    public float jumpForce = 5.0f;
    public bool isJump = false;
    public bool isGround = false;


    public float slideDuration = 1f; // �����̵� ���� �ð�
    public float slideSpeed = 10f; // �����̵� �ӵ�
    public float slideCooldown = 2f; // �����̵� ��ٿ� �ð�

    private bool isSliding = false;
    private float slideTimer = 0f;
    private float slideCooldownTimer = 0f;

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
        //������ Ű���� ���� ���� �̵��� ����
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //���⿡ ���� �¿� ����
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z) && state != State.Move)
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

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (isJump && Input.GetKeyDown(KeyCode.Z))
        {
            JumpAttack();
        }

        Debug.Log(comboCount);
    }

    void FixedUpdate()
    {
        move();

        Sliding();

        float horizontalInput = Input.GetAxis("Horizontal");

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

    void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);

        anim.SetBool("isJump", true);

        isJump = true;
    }

    void JumpAttack()
    {
        anim.SetTrigger("isJumpAttack");
        rigid.velocity = new Vector2(rigid.velocity.x, -7);
        if (isGround)
        {
            anim.ResetTrigger("isJumpAttack");
        }
    }

    void DefaultAttack()
    {
        //Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        //foreach (Collider2D collider in collider2Ds)
        //{
        //    //�±װ� ������ ������Ʈ�� �浹��
        //    if (collider.tag == "Monster" || collider.tag == "Spawner")
        //    {
        //        collider.GetComponent<Monster>().TakeDamage(1); //�������� ����, HealthPointManager�� TakeDamage�ʹ� �ٸ�
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
        curTime = coolTime;//������ �ϸ� ��Ÿ�� �ο�
    }

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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

    }

    //���� ������ ��Ÿ���� ����� �׸��� �Լ�
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
