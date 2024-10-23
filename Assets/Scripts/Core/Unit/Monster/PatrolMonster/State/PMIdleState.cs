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


            // ��� �ð��� ������ �������� �ƹ� �ൿ�� ���� ����
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
                return; // ���̳� ���������� ���� ��� �� �̻��� ���� ������ ����
            }

            // ������ �Ÿ��� ���� ���� ����
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

