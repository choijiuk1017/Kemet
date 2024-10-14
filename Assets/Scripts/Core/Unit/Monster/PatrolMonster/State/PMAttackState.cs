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
            if(!entity.patrolMonster.isGroggy)
            {
                float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

                if (isAttacking)
                {
                    entity.patrolMonster.rigid.velocity = Vector2.zero;
                    return;
                }


                timeSinceLastAttack += Time.deltaTime;

                if (entity.patrolMonster.targetObject != null)
                {
                    if (playerDistance <= 2f)
                    {
                        if (timeSinceLastAttack >= attackCooldown)
                        {
                            Attack(entity);
                            timeSinceLastAttack = 0f;
                        }
                        entity.patrolMonster.rigid.velocity = Vector2.zero;
                    }
                    else if (playerDistance > 2f && isAttacking == false)
                    {
                        entity.patrolMonster.rigid.velocity = Vector2.zero;
                        entity.ChangeState(MonsterStateType.Idle);
                    }
                }

            }
            else
            {
                entity.ChangeState(MonsterStateType.Groggy);
            }


            if(!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(MonsterStateType.Dead);
            }
        }

        public override void Exit(PatrolMonsterAI entity)
        {
            entity.patrolMonster.rigid.velocity = Vector2.zero;
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
            Debug.Log(isAttacking);
        }
        
    }
}
    
