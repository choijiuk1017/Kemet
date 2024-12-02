using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;


namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDChasingState : State<SummonedMonsterAI>
    {

        public override void Enter(SummonedMonsterAI entity)
        {

        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if(!entity.summonedMonster.isAlive)
            {
                entity.ChangeState(SMDMonsterStateType.Dead);
                return;
            }

            if(entity.isHit)
            {
                StopAndTransition(entity, SMDMonsterStateType.Stick);
                return;
            }
            else
            {
                Chasing(entity);
            }
            


        }

        public override void Exit(SummonedMonsterAI entity)
        {
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }

        private void Chasing(SummonedMonsterAI entity)
        {
            Vector2 direction = (entity.summonedMonster.targetObject.transform.position - entity.transform.position).normalized;

            if(direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if(direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            entity.summonedMonster.rigid.velocity = new Vector2(direction.x * entity.summonedMonster.moveSpeed * 1.5f, entity.summonedMonster.rigid.velocity.y);
        }

        private void StopAndTransition(SummonedMonsterAI entity, SMDMonsterStateType newState)
        {
            entity.summonedMonster.rigid.velocity = Vector2.zero;
            entity.ChangeState(newState);
        }
    }

}

