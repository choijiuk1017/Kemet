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

            // 정확한 방향을 계산하기 위해 targetObject의 위치와 소환사 위치를 사용
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

            float playerDistance = Vector2.Distance(entity.summonerMonster.targetObject.transform.position, entity.transform.position);

            timeSinceLastSummon += Time.deltaTime;

            // 소환 중이 아닐 때만 소환 가능
            if (!entity.summonerMonster.isSummoning && timeSinceLastSummon >= summonCooldown && playerDistance <= 15f)
            {
                // 소환 직전에 플레이어를 바라보도록 방향 갱신
                Vector2 direction = (entity.summonerMonster.targetObject.transform.position - entity.transform.position).normalized;

                if (direction.x > 0 && entity.transform.localScale.x < 0)
                {
                    entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
                }
                else if (direction.x < 0 && entity.transform.localScale.x > 0)
                {
                    entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
                }


                entity.anim.SetTrigger("Summon"); // Summon 애니메이션 실행
                entity.summonerMonster.isSummoning = true; // 소환 중 상태 설정
                timeSinceLastSummon = 0;
            }
            else if (playerDistance > 15f)
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

