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
        private float distance;

        private float waitTime = 1f;
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
                return; // 대기 시간 동안 아무것도 하지 않음
            }

            if (!entity.patrolMonster.isGroggy)
            {
                distance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

                if (distance > 7f)
                {
                    entity.ChangeState(MonsterStateType.Patrol);
                }
                else if (distance <= 7f && distance > 2f)
                {
                    entity.ChangeState(MonsterStateType.Chasing);
                }
                else
                {
                    entity.ChangeState(MonsterStateType.Attacking);
                }

            }
            else
            {
                entity.ChangeState(MonsterStateType.Groggy);
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(MonsterStateType.Dead);
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

