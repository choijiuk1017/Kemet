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
        private bool isSummoning = false;

        public override void Enter(SummonerMonsterAI entity)
        {
            entity.anim.SetBool("Walk", false);

            Vector2 direction = (entity.summonerMonster.targetObject.transform.position - entity.transform.right).normalized;

            if(direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);

            }
            else if(direction.x < 0 && entity.transform.localScale.x > 0)
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

            if(isSummoning)
            {
                return;
            }

            timeSinceLastSummon += Time.deltaTime;

            if(playerDistance <= 15f && timeSinceLastSummon >= summonCooldown)
            {
                entity.anim.SetTrigger("Summon"); //summon()은 애니메이션 이벤트로 처리
                timeSinceLastSummon = 0;
            }
            else if(playerDistance > 15f)
            {
                entity.ChangeState(SMMonsterStateType.Idle);
            }

        }

        public override void Exit(SummonerMonsterAI entity)
        {
            isSummoning = false;

        }

        public override void OnTransition(SummonerMonsterAI entity)
        {

        }

        public void Summon(SummonerMonsterAI entity)
        {
            GameObject monsterPrefab = entity.summonerMonster.summonPrefab;

            Vector3 summonPosition1 = entity.summonerMonster.summonPosition1;
            Vector3 summonPosition2 = entity.summonerMonster.summonPosition2;

            if (monsterPrefab != null)
            {
                GameObject summonedMonster1 = GameObject.Instantiate(monsterPrefab, summonPosition1, Quaternion.identity);
                GameObject summonedMonster2 = GameObject.Instantiate(monsterPrefab, summonPosition2, Quaternion.identity);

                isSummoning = true;
            }
        }

    }

}

