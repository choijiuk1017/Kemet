using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;

namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMIdleState : State<NecromancerMonsterAI>
    {
        private const float waitTime = 1f;
        public override void Enter(NecromancerMonsterAI entity)
        {
            elapsedTime = 0f;

        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < waitTime)
            {
                return;
            }

            if(!entity.necromancerMonster.isAlive)
            {
                entity.ChangeState(NMMonsterStateType.Dead);
                return;
            }

            if(entity.necromancerMonster.isGroggy)
            {
                entity.ChangeState(NMMonsterStateType.Groggy);
            }

            float distance = Vector2.Distance(entity.necromancerMonster.targetObject.transform.position, entity.transform.position);

            if(!entity.necromancerMonster.isGroundAhead || entity.necromancerMonster.isWallAhead)
            {
                if(distance > 15f)
                {
                   entity.ChangeState(NMMonsterStateType.Patrol);
                }
                return;
                
            }

            if(distance > 15f)
            {
                entity.ChangeState(NMMonsterStateType.Patrol);
            }
            else
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 20) // 20% 확률로 Heal 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Heal);
                }
                else if (randomValue < 50) // 30% 확률로 Teleport 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Teleport);
                }
                else // 나머지 50% 확률로 Attack 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Attack);
                }
                return;
            }
        }

        public override void Exit(NecromancerMonsterAI entity)
        {
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }
    }
}

