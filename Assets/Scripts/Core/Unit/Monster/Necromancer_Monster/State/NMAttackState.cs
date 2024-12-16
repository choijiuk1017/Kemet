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

            if(isAttacking)
            {
                return;
            }


            timeSinceLastAttack += Time.deltaTime;

            if(playerDistance <= 15f && timeSinceLastAttack >= attackCooldown)
            {
                entity.anim.SetTrigger("Attack");
                timeSinceLastAttack = 0f;

                //�������� ���� ��ȯ
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 20) // 20% Ȯ���� Heal ���·� ��ȯ
                {
                    entity.ChangeState(NMMonsterStateType.Heal);
                }
                else if (randomValue < 40) // 20% Ȯ���� Teleport ���·� ��ȯ
                {
                    entity.ChangeState(NMMonsterStateType.Teleport);
                }
                else // ������ 50% Ȯ���� Attack ���·� ��ȯ
                {
                    entity.ChangeState(NMMonsterStateType.Attack);
                }
            }
            else if(playerDistance > 15f)
            {
                entity.ChangeState(NMMonsterStateType.Idle);
            }

        }

        public override void Exit(NecromancerMonsterAI entity)
        {
            isAttacking = false;
        }

        public override void OnTransition(NecromancerMonsterAI entity)
        {

        }

        private void Attack()
        {
            isAttacking = true;
            Debug.Log("���� ����");
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
            Debug.Log("���� ��");
        }
    }

}
   