using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;


namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMHealState : State<NecromancerMonsterAI>
    {
        public GameObject healMagicPrefab;

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

            entity.anim.SetTrigger("Heal");

            SpawnHealMagic(entity);
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

                if (randomValue < 50) // 50% 확률로 Attack 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Attack);
                }
                else // 30% 확률로 Teleport 상태로 전환
                {
                    entity.ChangeState(NMMonsterStateType.Teleport);
                }
            }
        }

        public override void Exit(NecromancerMonsterAI entity)
        {
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }

        private void SpawnHealMagic(NecromancerMonsterAI entity)
        {
            if(healMagicPrefab != null)
            {
                Vector2 spawnPosition = new Vector2(entity.transform.position.x, entity.transform.position.y);
                GameObject healMagic = GameObject.Instantiate(healMagicPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}

