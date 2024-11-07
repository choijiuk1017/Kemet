using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;


namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMIdleState : State<SummonerMonsterAI>
    {
        private const float waitTime = 1f;


        public override void Enter(SummonerMonsterAI entity)
        {
            elapsedTime = 0f;
            Debug.Log("대기 상태");
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < waitTime)
            {
                return;
            }

            if(!entity.summonerMonster.isAlive)
            {
                entity.ChangeState(SMMonsterStateType.Dead);
                return;
            }

            if(entity.summonerMonster.isGroggy)
            {
                entity.ChangeState(SMMonsterStateType.Groggy);
            }

            float distance = Vector2.Distance(entity.summonerMonster.targetObject.transform.position, entity.transform.position);

            if(!entity.summonerMonster.isGroundAhead || entity.summonerMonster.isWallAhead)
            {
                if(distance > 15f)
                {
                    entity.ChangeState(SMMonsterStateType.Patrol);
                }
                return;
            }

            if(distance > 15f)
            {
                entity.ChangeState(SMMonsterStateType.Patrol);
            }
            else
            {
                entity.ChangeState(SMMonsterStateType.Summon);
            }



        }

        public override void Exit(SummonerMonsterAI entity)
        {
        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }
    }
}



