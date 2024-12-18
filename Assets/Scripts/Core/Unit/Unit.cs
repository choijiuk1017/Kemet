using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unit
{
    public class Unit : MonoBehaviour
    {
        //최대 체력
        public float maxHealth;

        //현재 체력
        public float currentHealth;

        //이동 속도
        public float moveSpeed;

        //공격력
        public float damage;

        //방어력
        public float defense;

        //살아있는 지
        public bool isAlive = true;

        //현재 위치
        public Vector3 position;


        //공격 범위
        public float attackRange;

        protected Animator anim;

        protected SpriteRenderer spriteRenderer;
        protected Color originalColor;
        protected Color damageColor = Color.red;
        protected bool isDamaged = false;
        protected float damageFlashTimer = 0f;
        public float damageFlashDuration = 0.2f;

        protected void Start()
        {
            Init();
        }

        //게임 시작 시 설정
        protected virtual void Init()
        {
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color; // 원래 색상 저장
        }

        //이동
        protected virtual void Move()
        {

        }
        

        //데미지 받기
        public virtual void TakeDamage(float damageAmount)
        {
            float defenseFactor = 50f;
            float finalDamage = damageAmount * (1 - defense / (defense + defenseFactor));

            finalDamage = Mathf.Max(finalDamage, 0);

            currentHealth -= finalDamage;

            spriteRenderer.color = damageColor; // 빨간색으로 변경
            isDamaged = true; // 데미지 상태 활성화

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        //공격
        public virtual void Attack()
        {
            
        }

        //죽음
        protected virtual void Die()
        {
            isAlive = false;
        }

        //치유
        public virtual void Heal(float healAmount)
        {
            
        }

        //버프
        public virtual void ApplyBuff()
        {
            
        }

        //디버프
        public virtual void ApplyDebuff()
        {

        }



    }
}

