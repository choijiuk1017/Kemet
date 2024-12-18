using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;

namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMDeadState : State<NecromancerMonsterAI>
    {
        public override void Enter(NecromancerMonsterAI entity)
        {
            if(entity.necromancerMonster.isGroggy)
            {
                entity.anim.SetTrigger("Dead");
            }
            else
            {
                entity.anim.SetTrigger("Groggy");
                entity.anim.SetTrigger("Dead");
            }
        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            Destroy(this.gameObject, 2f);
        }

        public override void Exit(NecromancerMonsterAI entity)
        {
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }
    }
}
