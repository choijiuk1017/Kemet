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


        }

        public override void Execute(PatrolMonsterAI entity)
        { 
        }

        public override void Exit(PatrolMonsterAI entity)
        {
        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }
    }

}