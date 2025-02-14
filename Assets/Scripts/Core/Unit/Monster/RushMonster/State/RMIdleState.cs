using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;


namespace Core.Unit.Monster.State.RushMonster
{

    public class RMIdleState : State<RushMonsterAI>
    {
        private const float waitTime = 1f;

        

        public override void Enter(RushMonsterAI entity)
        {
            elapsedTime = 0f;

        }

        public override void Execute(RushMonsterAI entity)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < waitTime)
            {
                return;

            }
            float distance = Vector2.Distance(entity.rushMonster.targetObject.transform.position, entity.transform.position);


            if (!entity.rushMonster.isGroundAhead || entity.rushMonster.isWallAhead)
            {
                entity.anim.SetBool("Walk", false);
                if (distance > 7f)
                {
                    entity.ChangeState(RMMonsterStateType.Patrol);
                }

                return; //벽이나 낭떠러지가 있을 경우 더 이상의 상태 변환을 방지
            }
            
            if (entity.rushMonster.isGroundAhead || !entity.rushMonster.isWallAhead && distance > 10f)
            {
                entity.ChangeState(RMMonsterStateType.Patrol);
            }
            else if (entity.rushMonster.isGroundAhead || !entity.rushMonster.isWallAhead && distance <= 10f && !entity.rushMonster.isGroggy)
            {
                entity.ChangeState(RMMonsterStateType.StartRushing);
            }

            if (!entity.rushMonster.isAlive)
            {
                entity.ChangeState(RMMonsterStateType.Dead);
                return;
            }

            if(entity.rushMonster.isGroggy)
            {
                entity.ChangeState(RMMonsterStateType.Groggy);
                return;
            }

           
            

            

        }



        public override void Exit(RushMonsterAI entity)
        {
        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }
    }

}

