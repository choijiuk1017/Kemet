using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;

namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMPatrolState : State<SummonerMonsterAI>
    {
        public bool isMovingRight = true;

        private float moveDistance = 10f;

        private float totalDistance = 0f;

        private float pauseDuration = 1f;

        private float pauseTimer = 0f;

        private Vector2 lastPosition;

        public override void Enter(SummonerMonsterAI entity)
        {
            totalDistance = 0f;

            pauseTimer = 0f;

            lastPosition = entity.transform.position;
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            if (entity.summonerMonster.isGroggy)
            {
                entity.ChangeState(SMMonsterStateType.Groggy);

                return;
            }

            if(!entity.summonerMonster.isAlive)
            {
                entity.ChangeState(SMMonsterStateType.Dead);

                return;
            }

            float playerDistance = Vector2.Distance(entity.summonerMonster.targetObject.transform.position, entity.transform.position);

            if(playerDistance <= 15f)
            {
                entity.ChangeState(SMMonsterStateType.Summon);
            }
            else
            {
                Patrol(entity);
            }



        }

        public override void Exit(SummonerMonsterAI entity)
        {
        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }

        private void Patrol(SummonerMonsterAI entity)
        {
            if (pauseTimer > 0f)
            {
                entity.summonerMonster.rigid.velocity = Vector2.zero;

                pauseTimer -= Time.deltaTime;

                return;

            }

            if (!entity.summonerMonster.isGroundAhead || entity.summonerMonster.isWallAhead)
            {
                entity.summonerMonster.Flip();

                isMovingRight = !isMovingRight;

                moveDistance = 20f;
                totalDistance = 0f;

            }

            if (totalDistance >= moveDistance)
            {
                entity.anim.SetBool("Walk", false);

                pauseTimer = pauseDuration;

                isMovingRight = !isMovingRight;

                moveDistance = 20f;

                totalDistance = 0f;

                return;
            }

            Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;

            float scaleX = entity.transform.localScale.x;

            if ((direction.x > 0f && scaleX < 0f) || (direction.x < 0f && scaleX > 0f))
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(scaleX) * Math.Sign(direction.x), entity.transform.localScale.y, entity.transform.localScale.z);

            }

            entity.summonerMonster.rigid.velocity = new Vector2(direction.x * entity.summonerMonster.moveSpeed, entity.summonerMonster.rigid.velocity.y);

            totalDistance += Vector2.Distance(entity.transform.position, lastPosition);

            lastPosition = entity.transform.position;

            entity.anim.SetBool("Walk", true);
        }
    
    }

}



