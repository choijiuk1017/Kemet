using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{
    public class RMDeadState : State<RushMonsterAI>
    {
        public override void Enter(RushMonsterAI entity)
        {
            if(entity.rushMonster.isGroggy)
            {
                entity.anim.SetTrigger("Dead");
            }
            else
            {
                entity.anim.SetTrigger("Groggy");
                entity.anim.SetTrigger("Dead");
            }
        }

        public override void Execute(RushMonsterAI entity)
        {
            Destroy(this.gameObject, 2f);


        }

        public override void Exit(RushMonsterAI entity)
        {

        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }
    }
}


