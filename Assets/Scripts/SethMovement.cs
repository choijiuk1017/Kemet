using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;


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
        float moveInput = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveInput * moveSpeed, rigid.velocity.y);

        if(moveInput != 0)
        {
            anim.SetBool("isWalking", true);
            spriteRenderer.flipX = (moveInput < 0);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }


}
