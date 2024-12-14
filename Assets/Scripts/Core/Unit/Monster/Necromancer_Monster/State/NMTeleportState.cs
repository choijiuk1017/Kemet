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
        public override void Enter(NecromancerMonsterAI entity)
        {
            Vector2 direction = (entity.necromancerMonster.targetObject.transform.position - entity.transform.position).normalized;

            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            entity.anim.SetTrigger("Teleport");
        }

        public override void Execute(NecromancerMonsterAI entity)
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
            }


        }

        public override void Exit(NecromancerMonsterAI entity)
        {

            entity.anim.SetTrigger("TeleportEnd");
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }
    }


}
