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

        //읽기 전용으로 AI에 접근 가능
        public PatrolMonsterAI PatrolMonsterAI => patrolMonsterAI;


        protected override void Init()
        {
            base.Init();

            
        }
       

        // 데미지 받았을 때 죽는 상태로 전환
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // 죽었을 때 애니메이션 및 파괴 처리
        protected override void Die()
        {
            base.Die();
            
        }
    }
}
