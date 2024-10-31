using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{
    public class RMRushingState : State<RushMonsterAI>
    {
        
        private float lastDamageTime = 0f;
        private float damageInterval = 0.5f;

        private Vector2 rushDirection; // 돌진 방향을 저장할 변수
        private bool isRushing; // 돌진 중인지 체크하는 변수

        private int hitCount = 0;
        private int maxHitCount = 3;
        public override void Enter(RushMonsterAI entity)
        {
            entity.anim.SetTrigger("Rushing");
            
        }

        public override void Execute(RushMonsterAI entity)
        {

            if(!entity.rushMonster.isAlive)
            {
                entity.ChangeState(RMMonsterStateType.Dead);
                return;
            }

            if(entity.rushMonster.isStartRush)
            {
                Rushing(entity);

                if (Time.time >= lastDamageTime + damageInterval && hitCount < maxHitCount)
                {
                    entity.rushMonster.RushHitBox();
                    lastDamageTime = Time.time;
                    hitCount++;
                }
                else if(hitCount == maxHitCount)
                {
                    StopAndTransition(entity, RMMonsterStateType.StopRushing);
                    
                }
                
                if(!entity.rushMonster.isGroundAhead || entity.rushMonster.isWallAhead)
                {

                    StopAndTransition(entity, RMMonsterStateType.StopRushing);
                }

                if (entity.rushMonster.isGroggy)
                {
                    StopAndTransition(entity, RMMonsterStateType.StopRushing);

                }
            }



        }

        public override void Exit(RushMonsterAI entity)
        {
            hitCount = 0;
        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }


        private void Rushing(RushMonsterAI entity)
        {
            if (!isRushing) // 돌진이 시작될 때만 방향을 설정
            {
                Vector2 targetPosition = entity.rushMonster.targetObject.transform.position;
                rushDirection = (targetPosition - (Vector2)entity.transform.position).normalized;

                entity.rushMonster.rushCollider.SetActive(true);
                isRushing = true;
            }

            if (rushDirection.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (rushDirection.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            // 고정된 방향으로 계속 이동
            entity.rushMonster.rigid.velocity = new Vector2(rushDirection.x * entity.rushMonster.moveSpeed * 5f, entity.rushMonster.rigid.velocity.y);
        }

        private void StopAndTransition(RushMonsterAI entity, RMMonsterStateType newState)
        {
            rushDirection = Vector2.zero;
            isRushing = false;
            entity.rushMonster.rushCollider.SetActive(false);
            entity.rushMonster.rigid.velocity = Vector2.zero;
            entity.ChangeState(newState);
        }

    }

}


