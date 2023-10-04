using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int currentHp = 1;
    public float moveSpeed = 5f;
    public float jumpPower = 10;
    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 3f;

    public bool ishit = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;

    protected Rigidbody2D rigid;
    protected BoxCollider2D boxCollider;
    public GameObject hitBoxCollider;
    public Animator anim;
    public LayerMask layerMask;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

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
                ishit = false;
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
    
}
