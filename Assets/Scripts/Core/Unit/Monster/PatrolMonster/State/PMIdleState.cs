using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMIdleState : State<PatrolMonsterAI>
    {
        private const float waitTime = 1f;


        public override void Enter(PatrolMonsterAI entity)
        {
            elapsedTime = 0f;
        }

        public override void Execute(PatrolMonsterAI entity)
        {
            elapsedTime += Time.deltaTime;


            // 대기 시간이 지나기 전까지는 아무 행동도 하지 않음
            if (elapsedTime < waitTime)
            {
                return;
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(PMMonsterStateType.Dead);
                return;
            }

            if (entity.patrolMonster.isGroggy)
            {
                entity.ChangeState(PMMonsterStateType.Groggy);
                return;
            }

            float distance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

            if (!entity.patrolMonster.isGroundAhead || entity.patrolMonster.isWallAhead)
            {
                if (distance > 7f)
                {
                    entity.ChangeState(PMMonsterStateType.Patrol);
                }
                return; // 벽이나 낭떠러지가 있을 경우 더 이상의 상태 변경을 방지
            }

            // 몬스터의 거리에 따른 상태 변경
            if (distance > 7f)
            {
                entity.ChangeState(PMMonsterStateType.Patrol);
            }
            else if (distance <= 7f && distance > 2f)
            {
                entity.ChangeState(PMMonsterStateType.Chasing);
            }
            else
            {
                entity.ChangeState(PMMonsterStateType.Attacking);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {
        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

    }

}

