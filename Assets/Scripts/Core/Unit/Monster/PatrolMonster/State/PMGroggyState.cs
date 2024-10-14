using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMGroggyState : State<PatrolMonsterAI>
    {
        private float groggyTime = 3f;
        private float standUpTime = 0f;
        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetTrigger("Groggy");

        }

        public override void Execute(PatrolMonsterAI entity)
        {   
            standUpTime += Time.deltaTime;
            if(standUpTime >= groggyTime)
            {
                entity.anim.SetTrigger("StandUp");
                entity.patrolMonster.groggyGauge = 0f;
                entity.patrolMonster.isGroggy = false;
                standUpTime = 0f;
                entity.ChangeState(MonsterStateType.Idle);
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(MonsterStateType.Dead);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {
            entity.patrolMonster.rigid.velocity = Vector2.zero;

        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }
    }
}
