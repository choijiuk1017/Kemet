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


        public bool isGroggy = false;

        public float maxGroggyGauge;
        public float groggyGauge;

        public Transform groundDetected;
        public Transform wallDetected;


        public bool isGroundAhead { get; private set; }
        public bool isWallAhead { get; private set; }

        //Transform 변수
        public Transform atkPos; //공격 사거리


        //Vector2 변수
        public Vector2 atkBoxSize; //히트 박스

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

            if(groggyGauge >= maxGroggyGauge)
            {
                isGroggy = true;
            }

        }

        public override void Heal(float healAmount)
        {
            if(currentHealth == maxHealth)
            {
                return;
            }
            else
            {
                currentHealth += healAmount;
            }                
        }

        public virtual void DetectGround()
        {
            RaycastHit2D groundHit = Physics2D.Raycast(groundDetected.position, Vector2.down, 1f);

            Debug.DrawRay(groundDetected.position, Vector2.down * 1f, Color.green);

            isGroundAhead = groundHit.collider != null;
        }


        public virtual void DetectWall()
        {
            // wallDetected 위치에서 앞 방향으로 레이캐스트 실행 (몬스터의 방향에 따라)
            Vector2 rayDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D wallHit = Physics2D.Raycast(wallDetected.position, rayDirection, 0.3f);
            Debug.DrawRay(wallDetected.position, rayDirection * 0.3f, Color.red); // 디버그용 레이

            // 벽이 감지되면 true, 아니면 false
            isWallAhead = wallHit.collider != null && wallHit.collider.tag == "Ground";
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }
    }
}

