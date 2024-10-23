using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;


namespace Core.Unit.Monster
{

    public class RushMonster : Monster
    {
        private RushMonsterAI rushMonsterAI;
      
        public RushMonsterAI RushMonsterAI => rushMonsterAI;


        protected override void Init()
        {
            base.Init();
            maxGroggyGauge = 100;

            groggyGauge = 0f;

            maxHealth = 100;
            currentHealth = maxHealth;

            moveSpeed = 2f;

            damage = 30f;

            defense = 5f;
        }

        private void Update()
        {
            // �� ������ �ٴڰ� ���� ����
            DetectGround();
            DetectWall();

            if (isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if (damageFlashTimer >= damageFlashDuration)
                {
                    spriteRenderer.color = originalColor; // ���� ������ ���ƿ�
                    isDamaged = false; // ������ ���� ����
                    damageFlashTimer = 0f;
                }
            }

            if (groggyGauge >= maxGroggyGauge)
            {
                isGroggy = true;
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
            bool parrySuccess = false;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Parry")
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    isDamaged = true; // ������ ���� Ȱ��ȭ
                    groggyGauge += 50;
                    Debug.Log("�÷��̾� �и� ����");
                    parrySuccess = true;
                }
            }

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player" && !parrySuccess) // �и� ���� �� ������ ����
                {
                    collider.GetComponent<Core.Unit.Player.Seth>().TakeDamage(damage);
                    Debug.Log("���� ���� ��");
                }
            }
        }

    }

}

