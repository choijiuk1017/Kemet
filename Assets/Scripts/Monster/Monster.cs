using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int currentHp;
    public int maxHp;

    public int groggyGauge;
    public int maxGroggyGauge;

    public float moveSpeed = 5f;
    public float jumpPower = 10;

    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 3f;

    public bool isHit = false;
    public bool isAttack = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;

    protected Rigidbody2D rigid;
    protected BoxCollider2D boxCollider;
    public GameObject hitBoxCollider;
    public GameObject player;
    public Animator anim;

    //공격 사거리
    public Transform pos;

    //히트 박스
    public Vector2 boxSize;

    public LayerMask layerMask;
    
    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player");

    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;
            if(!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
                hitBoxCollider.SetActive(true);
                isHit = false;
            }
        }
    }

    IEnumerator CalcCoolTime()
    {
        while(true)
        {
            yield return null;
            if(!canAtk)
            {
                atkCoolTimeCalc -= Time.deltaTime;
                if(atkCoolTimeCalc <= 0)
                {
                    atkCoolTimeCalc = atkCoolTime;
                    canAtk = true;
                }
            }
        }
    }

    public bool IsPlayingAnim(string AnimName)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            return true;
        }
        return false;
    }
    
    public void MyAnimSetTrigger(string AnimName)
    {
        if(!IsPlayingAnim(AnimName))
        {
            anim.SetTrigger(AnimName);
        }
    }

    protected void MonsterFlip()
    {
        Vector3 thisScale = transform.localScale;
        if(MonsterDirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
    }

    protected bool IsPlayerDir()
    {
        if(transform.position.x < player.transform.position.x ? MonsterDirRight : !MonsterDirRight)
        {
            return true;
        }
        return false;
    }

    protected void GroundCheck()
    {
        if(Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0 , Vector2.down, 0.05f, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;    
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        isHit = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //if ( collision.transform.CompareTag ( ?? ) )
        //{
        //TakeDamage ( 0 );
        //}
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
