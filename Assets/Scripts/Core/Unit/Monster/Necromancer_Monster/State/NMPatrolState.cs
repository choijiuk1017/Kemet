using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;

namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMPatrolState : State<NecromancerMonsterAI>
    {
        public bool isMovingRight = true;

        private float moveDistance = 10f;

        private float totalDistance = 0f;

        private float pauseTimer = 0f;

        private float pauseDuration = 1f;

        private Vector2 lastPosition;

        public override void Enter(NecromancerMonsterAI entity)
        {
            totalDistance = 0f;

            pauseTimer = 0f;

            lastPosition = entity.transform.position;
        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            if(entity.necromancerMonster.isGroggy)
            {
                entity.ChangeState(NMMonsterStateType.Groggy);

                return;
            }

            if(!entity.necromancerMonster.isAlive)
            {
                entity.ChangeState(NMMonsterStateType.Dead);

                return;
            }

            float playerDistance = Vector2.Distance(entity.necromancerMonster.targetObject.transform.position, entity.transform.position);

            if(playerDistance <= 15f)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 20) // 20% 확률로 Heal 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Heal);
                }
                else if (randomValue < 50) // 30% 확률로 Teleport 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Teleport);
                }
                else // 나머지 50% 확률로 Attack 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Attack);
                }
                return;
            }
            else
            {
                Patrol(entity);
            }
        }

        public override void Exit(NecromancerMonsterAI entity)
        {
            entity.anim.SetBool("Walk", false);
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }

        private void Patrol(NecromancerMonsterAI entity)
        {
            if(pauseTimer > 0f)
            {
                entity.necromancerMonster.rigid.velocity = Vector2.zero;

                pauseTimer -= Time.deltaTime;

                return;
            }

            if(!entity.necromancerMonster.isGroundAhead || entity.necromancerMonster.isWallAhead)
            {
                entity.necromancerMonster.Flip();

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

            if ((direction.x > 0f && scaleX < 0f) || (direction.x < 0f && scaleX > 0f))
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(scaleX) * Math.Sign(direction.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            entity.necromancerMonster.rigid.velocity = new Vector2(direction.x * entity.necromancerMonster.moveSpeed, entity.necromancerMonster.rigid.velocity.y);

            totalDistance += Vector2.Distance(entity.transform.position, lastPosition);

            lastPosition = entity.transform.position;

            entity.anim.SetBool("Walk", true);
        }
    }

}

