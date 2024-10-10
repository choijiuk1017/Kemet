using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Monster
{
    public class Monster : Unit
    {
        public GameObject targetObject;


        public Rigidbody2D rigid;

        

        protected override void Init()
        {
            base.Init();

            rigid = GetComponent<Rigidbody2D>();

            targetObject = GameObject.Find("Seth");
        }


        protected override void Die()
        {
            base.Die();

            if(anim != null)
            {
                anim.SetTrigger("Die");
            }

            Destroy(gameObject, 2f);
        }

    }
}

