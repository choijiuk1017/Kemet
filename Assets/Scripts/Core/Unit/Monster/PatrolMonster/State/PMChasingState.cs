using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMChasingState : State<PatrolMonsterAI>
    {
        // Start is called before the first frame update
        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);
        }

        public override void Execute(PatrolMonsterAI entity)
        {

            if (entity.patrolMonster.isGroggy)
            {
                entity.ChangeState(PMMonsterStateType.Groggy);
                return;
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(PMMonsterStateType.Dead);
                return;
            }

            float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

            if (playerDistance <= 7f)
            {
                Chasing(entity);

                if (!entity.patrolMonster.isGroundAhead || entity.patrolMonster.isWallAhead)
                {
                    StopAndTransition(entity, PMMonsterStateType.Idle);
                    return;
                }

                if (playerDistance <= 2f)
                {
                    StopAndTransition(entity, PMMonsterStateType.Attacking);
                    return;
                }
            }
            else
            {
                entity.ChangeState(PMMonsterStateType.Patrol);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {
            
        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

        private void Chasing(PatrolMonsterAI entity)
        {
            Vector2 direction = (entity.patrolMonster.targetObject.transform.position - entity.transform.position).normalized;

            // 방향에 따른 회전 처리
            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            entity.patrolMonster.rigid.velocity = new Vector2(direction.x * entity.patrolMonster.moveSpeed * 1.5f, entity.patrolMonster.rigid.velocity.y);
        }

        private void StopAndTransition(PatrolMonsterAI entity, PMMonsterStateType newState)
        {
            entity.patrolMonster.rigid.velocity = Vector2.zero;
            entity.anim.SetBool("Walk", false);
            entity.ChangeState(newState);
        }
    }

}