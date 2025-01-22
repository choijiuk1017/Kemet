using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Boss
{
    public class Boss : Unit
    {
        public GameObject targetObject;


        public Rigidbody2D rigid;

        public bool isGroggy = false;

        public float maxGroggyGauge;
        public float groggyGauge;

        protected override void Init()
        {
            base.Init();

            rigid = GetComponent<Rigidbody2D>();

            targetObject = GameObject.Find("Seth");
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);

            groggyGauge += 10;

            spriteRenderer.color = damageColor; // 빨간색으로 변경
            isDamaged = true; // 데미지 상태 활성화

            if (currentHealth <= 0)
            {
                Die();
            }

            if (groggyGauge >= maxGroggyGauge)
            {
                isGroggy = true;
            }

        }

    }
}

