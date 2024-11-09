using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonerMonster;

namespace Core.Unit.Monster.State.SummonerMonster
{
    public class SMSummonState : State<SummonerMonsterAI>
    {
        private float summonCooldown = 2f;
        private float timeSinceLastSummon = 0f;

        public override void Enter(SummonerMonsterAI entity)
        {
            entity.anim.SetBool("Walk", false);

            // ��Ȯ�� ������ ����ϱ� ���� targetObject�� ��ġ�� ��ȯ�� ��ġ�� ���
            Vector2 direction = (entity.summonerMonster.targetObject.transform.position - entity.transform.position).normalized;

            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
        }

        public override void Execute(SummonerMonsterAI entity)
        {
            if (entity.summonerMonster.isGroggy)
            {
                entity.ChangeState(SMMonsterStateType.Groggy);
                return;
            }

            if (!entity.summonerMonster.isAlive)
            {
                entity.ChangeState(SMMonsterStateType.Dead);
                return;
            }

            float playerDistance = Vector2.Distance(entity.summonerMonster.targetObject.transform.position , entity.transform.position);

            if(entity.summonerMonster.isSummoning)
            {
                return;
            }

            timeSinceLastSummon += Time.deltaTime;

            if(playerDistance <= 15f && timeSinceLastSummon >= summonCooldown)
            {
                entity.anim.SetTrigger("Summon"); //summon()�� �ִϸ��̼� �̺�Ʈ�� ó��
                timeSinceLastSummon = 0;
            }
            else if(playerDistance > 15f)
            {
                entity.ChangeState(SMMonsterStateType.Idle);
            }

        }

        public override void Exit(SummonerMonsterAI entity)
        {
            entity.summonerMonster.isSummoning = false;

        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }

       

    }

}

