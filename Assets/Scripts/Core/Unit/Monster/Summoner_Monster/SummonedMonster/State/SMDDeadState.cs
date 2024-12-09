using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;


namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDDeadState : State<SummonedMonsterAI>
    {
        public override void Enter(SummonedMonsterAI entity)
        {

        }

        public override void Execute(SummonedMonsterAI entity)
        {
            Destroy(this.gameObject);


        }

        public override void Exit(SummonedMonsterAI entity)
        {
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }
    }
}
 
