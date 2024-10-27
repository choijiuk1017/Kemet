using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{
    public class RMGroggyState : State<RushMonsterAI>
    {
        private float groggyTime = 3f;
        private float standUpTime = 0f;
        public override void Enter(RushMonsterAI entity)
        {
            entity.anim.SetTrigger("Groggy");
        }

        public override void Execute(RushMonsterAI entity)
        {
            standUpTime += Time.deltaTime;
            if (standUpTime >= groggyTime)
            {
                entity.anim.SetTrigger("StandUp");
                entity.rushMonster.groggyGauge = 0f;
                entity.rushMonster.isGroggy = false;
                entity.rushMonster.isStartRush = false;
                standUpTime = 0f;
                entity.ChangeState(RMMonsterStateType.Idle);
            }

            if (!entity.rushMonster.isAlive)
            {
                entity.ChangeState(RMMonsterStateType.Dead);
            }

        }

        public override void Exit(RushMonsterAI entity)
        {
            entity.rushMonster.rigid.velocity = Vector2.zero;

        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }
    }
}

