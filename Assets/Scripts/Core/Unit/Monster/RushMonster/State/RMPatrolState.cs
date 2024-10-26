using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.RushMonster;

namespace Core.Unit.Monster.State.RushMonster
{

    public class RMPatrolState : State<RushMonsterAI>
    {

        public bool isMovingRight = true;

        private float moveDistance = 10f;
        private float totalDistance = 0f;
        private float pauseDuration = 1f;
        private float pauseTimer = 0f;

        private Vector2 lastPosition;

        public override void Enter(RushMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);

            totalDistance = 0f; 
            pauseTimer = 0f;

            lastPosition = entity.transform.position;
        }

        public override void Execute(RushMonsterAI entity)
        {
            if(entity.rushMonster.isGroggy)
            {
                entity.ChangeState(RMMonsterStateType.Groggy);

                return;
            }

            if(!entity.rushMonster.isAlive)
            {
                entity.ChangeState(RMMonsterStateType.Dead);

                return;
            }

            float playerDistance = Vector2.Distance(entity.rushMonster.targetObject.transform.position, entity.transform.position);

            if (playerDistance <= 10f)
            {
                entity.ChangeState(RMMonsterStateType.StartRushing);
            }
            else
            {
               Patrol(entity);
            }

        }

        public override void Exit(RushMonsterAI entity)
        {
        }

        public override void OnTransition(RushMonsterAI entity)
        {

        }

        private void Patrol(RushMonsterAI entity)
        {
            if (pauseTimer > 0f)
            {
                entity.rushMonster.rigid.velocity = Vector2.zero;

                pauseTimer -= Time.deltaTime;

                return;

            }

            if (!entity.rushMonster.isGroundAhead || entity.rushMonster.isWallAhead)
            {
                entity.rushMonster.Flip();

                isMovingRight = !isMovingRight;

                moveDistance = 20f;
                totalDistance = 0f;

            }

            if(totalDistance >= moveDistance)
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

            if((direction.x > 0f && scaleX < 0f) || (direction.x < 0f && scaleX > 0f))
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(scaleX) * Math.Sign(direction.x), entity.transform.localScale.y, entity.transform.localScale.z);

            }

            entity.rushMonster.rigid.velocity = new Vector2(direction.x * entity.rushMonster.moveSpeed, entity.rushMonster.rigid.velocity.y);

            totalDistance += Vector2.Distance(entity.transform.position, lastPosition);

            lastPosition = entity.transform.position;

            entity.anim.SetBool("Walk", true);
        }

    }



}
