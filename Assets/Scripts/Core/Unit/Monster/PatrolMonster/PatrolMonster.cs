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

        //�б� �������� AI�� ���� ����
        public PatrolMonsterAI PatrolMonsterAI => patrolMonsterAI;

        public bool isGroundAhead { get; private set; }
        public bool isWallAhead { get; private set; }


        protected override void Init()
        {
            base.Init();
            maxGroggyGauge = 100;
            maxHealth = 100;
            currentHealth = maxHealth;

            moveSpeed = 3f;

            damage = 20f;
            defense = 10f;

            attackRange = 3f;
            
        }
        private void Update()
        {
            // �� ������ �ٴڰ� ���� ����
            DetectGround();
            DetectWall();

            if (groggyGauge >= maxGroggyGauge)
            {
                isGroggy = true;
            }
        }


        private void DetectGround()
        {
            // groundDetected ��ġ���� �Ʒ��� ����ĳ��Ʈ ����
            RaycastHit2D groundHit = Physics2D.Raycast(groundDetected.position, Vector2.down, 1f);
            Debug.DrawRay(groundDetected.position, Vector2.down * 1f, Color.green); // ����׿� ����

            // �ٴ��� �����Ǹ� true, �ƴϸ� false
            isGroundAhead = groundHit.collider != null;
        }

        // �� ����
        private void DetectWall()
        {
            // wallDetected ��ġ���� �� �������� ����ĳ��Ʈ ���� (������ ���⿡ ����)
            Vector2 rayDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D wallHit = Physics2D.Raycast(wallDetected.position, rayDirection, 0.3f);
            Debug.DrawRay(wallDetected.position, rayDirection * 0.3f, Color.red); // ����׿� ����

            // ���� �����Ǹ� true, �ƴϸ� false
            isWallAhead = wallHit.collider != null && wallHit.collider.tag == "Ground";
        }


        // ������ �޾��� �� �״� ���·� ��ȯ
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);

            groggyGauge += 10;

            if (currentHealth <= 0)
            {
                Die();
            }
            
        }



        public void Flip()
        {
            Vector3 scale = this.transform.localScale;
            scale.x *= -1; // X �� ������ �������� ��������Ʈ�� ������
            this.transform.localScale = scale;
        }

        public void HitBoxOn()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player")
                {
                    collider.GetComponent<Core.Unit.Player.Seth>().TakeDamage(damage);
                    Debug.Log("���� ���� ��");
                }
            }
        }


        

    }
}
