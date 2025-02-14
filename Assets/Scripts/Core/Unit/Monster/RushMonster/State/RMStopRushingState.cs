using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{

    public class RMStopRushingState : State<RushMonsterAI>
    {


        public override void Enter(RushMonsterAI entity)
        {
            entity.rushMonster.rushCollider.SetActive(false);

            entity.ChangeState(RMMonsterStateType.Groggy);
        }

        public override void Execute(RushMonsterAI entity)
        {
        }

        public override void Exit(RushMonsterAI entity)
        {
        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }
    }


}

