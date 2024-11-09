using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;

namespace Core.Unit.Monster
{
    public class SummonerMonster : Monster
    {
        private SummonerMonsterAI summonerMonsterAI;

        public SummonerMonsterAI SummonerMonsterAI => summonerMonsterAI;

        public GameObject summonPrefab;

        public GameObject[] summonPosition;


        public bool isSummoning = false;
        protected override void Init()
        {
            base.Init();

            maxGroggyGauge = 100f;

            groggyGauge = 0f;

            maxHealth = 100f;

            currentHealth = maxHealth;

            moveSpeed = 2f;

            damage = 0f;

            defense = 1f;
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

        public void Summon()
        {
            GameObject monsterPrefab = summonPrefab;

            if (monsterPrefab != null)
            {
               foreach(GameObject position in summonPosition)
               {
                    GameObject summonedMonster = GameObject.Instantiate(monsterPrefab, position.transform.position, Quaternion.identity);

                    summonedMonster.transform.localScale = new Vector3(Mathf.Abs(summonedMonster.transform.localScale.x) * Mathf.Sign(this.transform.localScale.x), summonedMonster.transform.localScale.y, summonedMonster.transform.localScale.z);
               }


                isSummoning = true;
            }
        }
    }
}



