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

        //�б� �������� AI�� ���� ����
        public PatrolMonsterAI PatrolMonsterAI => patrolMonsterAI;


        protected override void Init()
        {
            base.Init();

            
        }
       

        // ������ �޾��� �� �״� ���·� ��ȯ
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // �׾��� �� �ִϸ��̼� �� �ı� ó��
        protected override void Die()
        {
            base.Die();
            
        }
    }
}
