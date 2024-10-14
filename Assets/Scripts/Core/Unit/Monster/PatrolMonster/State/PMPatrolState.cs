using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMPatrolState : State<PatrolMonsterAI>
    {


        private const string animationName = "Patrol";

        private bool isMovingRight = true;

        private float moveDistance = 10f;
        private float totalDistance = 0f;
        private float pauseDuration = 1f;
        private float pauseTimer = 0f;

        private Vector2 lastPosition;

        private Vector2 startPosition;

        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);
            
            startPosition = entity.transform.position;

            totalDistance = 0f;
            pauseTimer = 0f;

            lastPosition = entity.transform.position;
        }

        public override void Execute(PatrolMonsterAI entity)
        {

            if(!entity.patrolMonster.isGroggy)
            {
                float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

                if (entity.patrolMonster.targetObject != null)
                {
                    if (playerDistance <= 7f)
                    {
                        entity.ChangeState(MonsterStateType.Chasing);
                    }
                    else
                    {
                        Patrol(entity);
                    }
                }

            }
            else
            {
                entity.ChangeState(MonsterStateType.Groggy);
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(MonsterStateType.Dead);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {

        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

        private void Patrol(PatrolMonsterAI entity)
        {
            if (pauseTimer > 0f)
            {
                entity.patrolMonster.rigid.velocity = Vector2.zero;


                pauseTimer -= Time.deltaTime;
                return;
            }

            if (!entity.patrolMonster.isGroundAhead || entity.patrolMonster.isWallAhead)
            {
                entity.patrolMonster.Flip();

                // 이동 방향 반전
                isMovingRight = !isMovingRight;

                // 방향 전환 후 이동할 거리를 다시 설정
                moveDistance = 20f;
                totalDistance = 0f; // 이동 거리 초기화

            }

            // 일정 거리를 이동하면 방향 전환
            if (totalDistance >= moveDistance)
            {
                entity.anim.SetBool("Walk", false);
                entity.anim.SetTrigger(animationName);

                // 일정 시간 멈춤
                pauseTimer = pauseDuration;

                // 이동 방향 반전
                isMovingRight = !isMovingRight;

                // 방향 전환 후 이동할 거리를 다시 설정
                moveDistance = 20f;
                totalDistance = 0f; // 이동 거리 초기화

                return;
            }

            Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;

            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }


            entity.patrolMonster.rigid.velocity = new Vector2(direction.x * entity.patrolMonster.moveSpeed, entity.patrolMonster.rigid.velocity.y);

            totalDistance += Vector2.Distance(entity.transform.position, lastPosition);
            lastPosition = entity.transform.position;

            //entity.anim.SetBool("Walk", true);

            float distanceMoved = Vector2.Distance(entity.transform.position, lastPosition);

            // totalDistance에 실제 이동한 거리 누적
            totalDistance += distanceMoved;

            // 현재 위치를 lastPosition에 저장하여 다음 프레임에서 비교할 수 있게 함
            lastPosition = entity.transform.position;
        }


    }
}

    
