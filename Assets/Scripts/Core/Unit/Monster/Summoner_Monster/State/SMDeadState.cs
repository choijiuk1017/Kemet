using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;


namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMDeadState : State<SummonerMonsterAI>
    {
        public override void Enter(SummonerMonsterAI entity)
        {
           
            if(entity.summonerMonster.isGroggy)
            {
                entity.anim.SetTrigger("Dead");
            }
            else
            {
                entity.anim.SetTrigger("Groggy");
                entity.anim.SetTrigger("Dead");
            }
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            Destroy(this.gameObject, 2f);
        }

        public override void Exit(SummonerMonsterAI entity)
        {
        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }
    }

}


