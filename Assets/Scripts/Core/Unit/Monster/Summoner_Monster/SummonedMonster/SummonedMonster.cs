using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;

namespace Core.Unit.Monster
{
    public class SummonedMonster : Monster
    {
        private SummonedMonsterAI summonedMonsterAI;

        public SummonedMonsterAI SummonedMonsterAI => summonedMonsterAI;

        protected override void Init()
        {
            base.Init();
            maxGroggyGauge = 100f;

            groggyGauge = 0f;

            maxHealth = 10f;

            currentHealth = maxHealth;

            moveSpeed = 5f;

            damage = 3f;

            defense = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if (damageFlashTimer >= damageFlashDuration)
                {
                    spriteRenderer.color = originalColor;
                    isDamaged = false;

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

            scale.x *= -1;
            this.transform.localScale = scale;
        }

        public override void TakeDamage(float damageAmount)
        {
            float defenseFactor = 50f;
            float finalDamage = damageAmount * (1 - defense / (defense * defenseFactor));

            finalDamage = Mathf.Max(finalDamage, 0);

            currentHealth -= finalDamage;

            base.TakeDamage(damageAmount);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // ground 또는 stair 태그와 충돌하면 isAlive를 false로 설정
            if (collision.CompareTag("Ground") || collision.CompareTag("Stair"))
            {
                isAlive = false;
            }
        }
    }
}


