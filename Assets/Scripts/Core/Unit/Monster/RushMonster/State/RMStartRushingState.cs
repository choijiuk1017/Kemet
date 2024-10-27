using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{
    public class RMStartRushingState : State<RushMonsterAI>
    {
        public override void Enter(RushMonsterAI entity)
        {
            Vector2 direction = (entity.rushMonster.targetObject.transform.position - entity.transform.position).normalized;

            if(direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x),entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if(direction.x < 0 && entity.transform.localScale.x >0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            entity.anim.SetTrigger("StartRushing");
        }

        public override void Execute(RushMonsterAI entity)
        {
            if (entity.rushMonster.isGroggy)
            {
                entity.ChangeState(RMMonsterStateType.Groggy);
                return;
            }

            if(!entity.rushMonster.isAlive)
            {
                entity.ChangeState(RMMonsterStateType.Dead);
                return;
            }


            entity.rushMonster.isStartRush = true;
            entity.ChangeState(RMMonsterStateType.Idle);

        }

        public override void Exit(RushMonsterAI entity)
        {
        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }
    }

}



