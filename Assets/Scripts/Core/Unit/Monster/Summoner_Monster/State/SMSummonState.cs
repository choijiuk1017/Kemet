using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;

namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMSummonState : State<SummonerMonsterAI>
    {
        public override void Enter(SummonerMonsterAI entity)
        {
            Vector2 direction = (entity.summonerMonster.targetObject.transform.position - entity.transform.right).normalized;

            if(direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);

            }
            else if(direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            if (entity.summonerMonster.isGroggy)
            {
                entity.ChangeState(SMMonsterStateType.Groggy);
                return;
            }

            if (!entity.summonerMonster.isAlive)
            {
                entity.ChangeState(SMMonsterStateType.Dead);
                return;
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

