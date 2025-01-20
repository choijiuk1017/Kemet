using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;

namespace Core.Unit.Monster
{
    public class Tawaret : Monster
    {
        private TawaretAI tawaretAI;

        public TawaretAI TawaretAI => tawaretAI;

        protected override void Init()
        {
            base.Init();
            maxGroggyGauge = 100f;

            groggyGauge = 0f;

            maxHealth = 100f;

            currentHealth = maxHealth;

            moveSpeed = 0f;

            damage = 10f;

            defense = 5f;
        }

        private void Update()
        {
            if(isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if(damageFlashTimer >= damageFlashDuration)
                {
                    spriteRenderer.color = originalColor;
                    isDamaged = false;

                    damageFlashTimer = 0f;
                }
            }

            if(groggyGauge >= maxGroggyGauge)
            {
                isGroggy = true;
            }
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
        }

    }

}


