using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.NecromancerMonster;


namespace Core.Unit.Monster.State.NecromancerMonster
{
    public class NMAttackState : State<NecromancerMonsterAI>
    {
        private float attackCooldown = 2f;
        private float timeSinceLastAttack = 0f;
        private bool isAttacking = false;
        private float stateDuration = 3f;
        private float stateElapsedTime = 0f;

        public GameObject magicBallPrefab;
        private Vector2 spawnPos;


        public override void Enter(NecromancerMonsterAI entity)
        {
            spawnPos = entity.necromancerMonster.atkPos.transform.position;
            Vector2 direction = (entity.necromancerMonster.targetObject.transform.position - entity.transform.position).normalized;

            if(direction.x > 0  && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if(direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            
            timeSinceLastAttack = 0f;
            isAttacking = false;


           

            stateElapsedTime = 0f;
        }

        public override void Execute(NecromancerMonsterAI entity)
        {
            stateElapsedTime += Time.deltaTime;

            float playerDistance = Vector2.Distance(entity.necromancerMonster.targetObject.transform.position, entity.transform.position);

            timeSinceLastAttack += Time.deltaTime;

            if (playerDistance <= 15f && timeSinceLastAttack >= attackCooldown)
            {
                entity.anim.SetTrigger("Attack");
                timeSinceLastAttack = 0f;
            }
            else if (playerDistance > 15f)
            {
                entity.ChangeState(NMMonsterStateType.Idle);
            }

            if (entity.necromancerMonster.isGroggy)
            {
                entity.ChangeState(NMMonsterStateType.Groggy);
                return;
            }

            if(!entity.necromancerMonster.isAlive)
            {
                entity.ChangeState(NMMonsterStateType.Dead);
                return;
            }

            if (isAttacking)
            {
                return;
            }

            if(stateElapsedTime >= stateDuration)
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
                return;
            }

        }

        public override void Exit(NecromancerMonsterAI entity)
        {
            isAttacking = false;
            stateElapsedTime = 0f;
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }

        private void Attack()
        {
            isAttacking = true;
            Debug.Log("공격 시작");
        }

        private void SpawnMagic()
        {
            if(magicBallPrefab != null)
            {
                GameObject magicBall = GameObject.Instantiate(magicBallPrefab, spawnPos, Quaternion.identity);  

            }

        }

        public void EndAttack()
        {
            isAttacking = false;
            Debug.Log("공격 끝");
        }
    }

}
   
