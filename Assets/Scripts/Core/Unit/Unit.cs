using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unit
{
    public class Unit : MonoBehaviour
    {
        //�ִ� ü��
        public float maxHealth;

        //���� ü��
        public float currentHealth;

        //�̵� �ӵ�
        public float moveSpeed;

        //���ݷ�
        public float damage;

        //����
        public float defense;

        //����ִ� ��
        public bool isAlive = true;

        //���� ��ġ
        public Vector3 position;


        //���� ����
        public float attackRange;

        protected Animator anim;

        protected void Start()
        {
            Init();
        }

        //���� ���� �� ����
        protected virtual void Init()
        {
            anim = GetComponent<Animator>();
        }

        //�̵�
        protected virtual void Move()
        {

        }
        

        //������ �ޱ�
        public virtual void TakeDamage(float damageAmount)
        {
            float defenseFactor = 50f;
            float finalDamage = damageAmount * (1 - defense / (defense + defenseFactor));

            finalDamage = Mathf.Max(finalDamage, 0);

            currentHealth -= finalDamage;

            if(currentHealth <= 0)
            {
                Die();
            }
        }

        //����
        public virtual void Attack()
        {
            
        }

        //����
        protected virtual void Die()
        {
            isAlive = false;
        }

        //ġ��
        public virtual void Heal(float healAmount)
        {
            currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        }

        //����
        public virtual void ApplyBuff()
        {
            
        }

        //�����
        public virtual void ApplyDebuff()
        {

        }



    }
}

