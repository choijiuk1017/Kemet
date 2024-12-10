using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;

namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMGroggyState : State<SummonerMonsterAI>
    {
        private float groggyTime = 3f;
        private float standUpTime = 0f;
        public override void Enter(SummonerMonsterAI entity)
        {
            if(entity.summonerMonster.isSummoning)
            {
                entity.summonerMonster.isSummoning = false;
            }

            entity.anim.SetTrigger("Groggy");
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            standUpTime += Time.deltaTime;

            if(standUpTime >= groggyTime)
            {
                entity.anim.SetTrigger("StandUp");
                entity.summonerMonster.groggyGauge = 0f;
                entity.summonerMonster.isGroggy = false;

                standUpTime = 0f;
                entity.ChangeState(SMMonsterStateType.Idle);
            }

            if(!entity.summonerMonster.isAlive)
            {
                entity.ChangeState(SMMonsterStateType.Dead);
            }
        }

        public override void Exit(SummonerMonsterAI entity)
        {
            entity.summonerMonster.rigid.velocity = Vector2.zero;

        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }
    }
}



