using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{

    public class PMDeadState : State<PatrolMonsterAI>
    {
        public override void Enter(PatrolMonsterAI entity)
        {

            if (entity.patrolMonster.isGroggy)
            {
                entity.anim.SetTrigger("Dead");
            }
            else
            {
                entity.anim.SetTrigger("Groggy");
                entity.anim.SetTrigger("Dead");
            }
        }

        public override void Execute(PatrolMonsterAI entity)
        {

            Destroy(this.gameObject, 2f);
        }

        public override void Exit(PatrolMonsterAI entity)
        {
            

        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }
    }
}
