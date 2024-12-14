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
                //�������� ���� ��ȯ
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 20) // 20% Ȯ���� Heal ���·� ��ȯ
                {
                    entity.ChangeState(NMMonsterStateType.Heal);
                }
                else if (randomValue < 50) // 30% Ȯ���� Teleport ���·� ��ȯ
                {
                    entity.ChangeState(NMMonsterStateType.Teleport);
                }
                else // ������ 50% Ȯ���� Attack ���·� ��ȯ
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
