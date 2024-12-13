using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;

namespace Core.Unit.Monster
{
    public class NecromancerMonster : Monster
    {
        private NecromancerMonsterAI necromancerMonsterAI;

        public NecromancerMonsterAI NecromancerMonsterAI => necromancerMonsterAI;

        protected override void Init()
        {
            base.Init();
            maxGroggyGauge = 100f;

            groggyGauge = 0f;

            maxHealth = 100f;

            currentHealth = maxHealth;

            moveSpeed = 3f;

            damage = 5f;

            defense = 2f;
        }

        private void Update()
        {
            DetectGround();
            DetectWall();

            if(isDamaged)
            {
                damageFlashTimer += Time.deltaTime;

                if (damageFlashTimer >= damageFlashDuration)
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

        public void Flip()
        {
            Vector3 Scale = this.transform.localScale;

            Scale.x *= -1;

            this.transform.localScale = Scale;
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
        }
    }



}


