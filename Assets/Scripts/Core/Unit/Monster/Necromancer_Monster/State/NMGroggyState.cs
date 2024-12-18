using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;


namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMGroggyState : State<NecromancerMonsterAI>
    {
        private float groggyTime = 3f;
        private float standUpTime = 0f;

        public override void Enter(NecromancerMonsterAI entity)
        {
            entity.anim.SetTrigger("Groggy");
        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            standUpTime += Time.deltaTime;
            if(standUpTime >= groggyTime)
            {
                entity.anim.SetTrigger("StandUp");

                entity.necromancerMonster.groggyGauge = 0f;
                entity.necromancerMonster.isGroggy = false;
                standUpTime = 0f;

                entity.ChangeState(NMMonsterStateType.Idle);
            }

            if(!entity.necromancerMonster.isAlive)
            {
                entity.ChangeState(NMMonsterStateType.Dead);
            }


        }

        public override void Exit(NecromancerMonsterAI entity)
        {
            entity.necromancerMonster.rigid.velocity = Vector2.zero;
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }
    }

}
