using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMAttackState : State<PatrolMonsterAI>
    {
        private float attackCooldown = 2f;
        private float timeSinceLastAttack = 0f;
        private bool isAttacking = false;

        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetTrigger("Attack");
            timeSinceLastAttack = 0f;
            isAttacking = false;
        }

        public override void Execute(PatrolMonsterAI entity)
        {
            if (entity.patrolMonster.isGroggy)
            {
                entity.ChangeState(PMMonsterStateType.Groggy);
                return;  // �̹� ���¸� ���������Ƿ� ���� �ڵ带 ������ �ʿ䰡 �����ϴ�.
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(PMMonsterStateType.Dead);
                return;
            }

            float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

            if (isAttacking)
            {
                return;  // ���� ���̸� �ٸ� �ൿ�� ���� ����
            }

            timeSinceLastAttack += Time.deltaTime;

            if (playerDistance <= 2f && timeSinceLastAttack >= attackCooldown)
            {
                Attack(entity);
                timeSinceLastAttack = 0f;
            }
            else if (playerDistance > 2f)
            {
                entity.ChangeState(PMMonsterStateType.Idle);
            }
        }

        public override void Exit(PatrolMonsterAI entity)
        {
            
            isAttacking = false;
        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

        private void Attack(PatrolMonsterAI entity)
        {
            isAttacking = true;

            entity.patrolMonster.rigid.velocity = Vector2.zero;

            entity.anim.SetTrigger("Attack"); 
        }

        public void EndAttack()
        {
            isAttacking = false;
        }
        

        
    }
}
    
