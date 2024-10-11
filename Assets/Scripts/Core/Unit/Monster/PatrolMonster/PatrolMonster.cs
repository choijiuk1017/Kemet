using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;

namespace Core.Unit.Monster
{
    public class PatrolMonster : Monster
    {
        private PatrolMonsterAI patrolMonsterAI;

        public Transform groundDetected;
        public Transform wallDetected;

        //읽기 전용으로 AI에 접근 가능
        public PatrolMonsterAI PatrolMonsterAI => patrolMonsterAI;

        public bool isGroundAhead { get; private set; }
        public bool isWallAhead { get; private set; }   


        protected override void Init()
        {
            base.Init();

            
        }
        private void Update()
        {
            // 매 프레임 바닥과 벽을 감지
            DetectGround();
            DetectWall();
        }


        private void DetectGround()
        {
            // groundDetected 위치에서 아래로 레이캐스트 실행
            RaycastHit2D groundHit = Physics2D.Raycast(groundDetected.position, Vector2.down, 1f);
            Debug.DrawRay(groundDetected.position, Vector2.down * 1f, Color.green); // 디버그용 레이

            // 바닥이 감지되면 true, 아니면 false
            isGroundAhead = groundHit.collider != null;
        }

        // 벽 감지
        private void DetectWall()
        {
            // wallDetected 위치에서 앞 방향으로 레이캐스트 실행 (몬스터의 방향에 따라)
            Vector2 rayDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D wallHit = Physics2D.Raycast(wallDetected.position, rayDirection, 0.3f);
            Debug.DrawRay(wallDetected.position, rayDirection * 0.3f, Color.red); // 디버그용 레이

            // 벽이 감지되면 true, 아니면 false
            isWallAhead = wallHit.collider != null && wallHit.collider.tag == "Ground";
        }


        // 데미지 받았을 때 죽는 상태로 전환
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // 죽었을 때 애니메이션 및 파괴 처리
        protected override void Die()
        {
            base.Die();
            
        }

        public void Flip()
        {
            Vector3 scale = this.transform.localScale;
            scale.x *= -1; // X 축 스케일 반전으로 스프라이트를 뒤집음
            this.transform.localScale = scale;
        }

    }
}
