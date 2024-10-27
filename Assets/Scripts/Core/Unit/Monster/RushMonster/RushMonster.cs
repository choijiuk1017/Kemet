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

        public bool isStartRush = false;


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

        public void RushHitBox()
        {
            bool parrySuccess = false;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Parry")
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;

                    isDamaged = true;
                    groggyGauge += 100f;
                    Debug.Log("�÷��̾� �и� ����");

                    parrySuccess = true;

                }
            }

            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Player" && !parrySuccess)
                {
                    collider.GetComponent<Core.Unit.Player.Seth>().TakeDamage(damage);
                    Debug.Log("���� ����");
                }
            }

        }


        public override void TakeDamage(float damageAmount)
        {
            float defenseFactor = 50f;
            float finalDamage = damageAmount * (1 - defense / (defense + defenseFactor));

            finalDamage = Mathf.Max(finalDamage, 0);

            currentHealth -= finalDamage;

            spriteRenderer.color = damageColor; // ���������� ����
            isDamaged = true; // ������ ���� Ȱ��ȭ

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

