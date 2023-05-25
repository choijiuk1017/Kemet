using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxSpeed;

    private float curTime;
    public float coolTime = 0.5f;

    public Transform pos;
    public Vector2 boxSize;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (Input.GetKey(KeyCode.Z))
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
                anim.SetTrigger("isDefaultAttack");

                curTime = coolTime;//������ �ϸ� ��Ÿ�� �ο�
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

    }

    void FixedUpdate()
    {
        move();
    }

    //�÷��̾� �̵��Լ�
    void move()
    {
        // �����¿� �Է� ����
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // �̵� ���� ���
        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;

        // �÷��̾� �̵�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // �ִϸ��̼� ���
        if (moveDirection.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            // ���� �� �ִϸ��̼� ����
            anim.SetBool("isWalking", false);
        }
    }

    //���� ������ ��Ÿ���� ����� �׸��� �Լ�
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
