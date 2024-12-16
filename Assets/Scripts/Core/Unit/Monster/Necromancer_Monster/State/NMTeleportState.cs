using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;

namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMTeleportState : State<NecromancerMonsterAI>
    {
        private const int MaxTeleportRetries = 5;
        private Vector2 targetPosition;

        private int currenRetry = 0;

        private bool isTeleportComplete = false;

        public override void Enter(NecromancerMonsterAI entity)
        {
            currenRetry = 0;
            isTeleportComplete = false;

            GenerateRandomTargetPosition(entity);

            Vector2 direction = (entity.necromancerMonster.targetObject.transform.position - entity.transform.position).normalized;

            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            
            if (isTeleportComplete)
            {
                if (entity.necromancerMonster.isGroggy)
                {
                    entity.ChangeState(NMMonsterStateType.Groggy);
                    return;
                }

                if (!entity.necromancerMonster.isAlive)
                {
                    entity.ChangeState(NMMonsterStateType.Dead);
                    return;
                }

                float playerDistance = Vector2.Distance(entity.necromancerMonster.targetObject.transform.position, entity.transform.position);

                if (playerDistance <= 15f)
                {
                    //랜덤으로 상태 변환
                    int randomValue = UnityEngine.Random.Range(0, 100);

                    if (randomValue < 80) // 80% 확률로 Attack 상태로 전환
                    {
                        entity.ChangeState(NMMonsterStateType.Attack);
                    }
                    else // 나머지 확률로 Heal 상태로 전환
                    {
                        entity.ChangeState(NMMonsterStateType.Heal);
                    }
                }

                return;
            }

            if(currenRetry < MaxTeleportRetries)
            {
               Teleport(entity);
            }
            else
            {
                entity.transform.position = entity.necromancerMonster.targetObject.transform.position + new Vector3(2, 0);
            }

           


        }

        public override void Exit(NecromancerMonsterAI entity)
        {
            entity.anim.SetTrigger("TeleportEnd");
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }

        private void GenerateRandomTargetPosition(NecromancerMonsterAI entity)
        {
            float randomDistance = UnityEngine.Random.Range(5f, 10f);

            float direction = UnityEngine.Random.value > 0.5f ? 1 : -1;

            targetPosition = new Vector2(entity.transform.position.x + (randomDistance * direction), entity.transform.position.y);
        }

        private void Teleport(NecromancerMonsterAI entity)
        {
            Vector2 ray = new Vector2(targetPosition.x, entity.transform.position.y + 2f);

            RaycastHit2D groundHit = Physics2D.Raycast(ray, Vector2.down, 5f);

            if (groundHit.collider != null)
            {
                entity.anim.SetTrigger("Teleport");
                targetPosition.y = groundHit.point.y;
                entity.transform.position = targetPosition;

                isTeleportComplete = true;
            }
            else
            {
                currenRetry++;

                GenerateRandomTargetPosition(entity);


            }
        }
    }


}
