using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss;
using Core.Unit.Boss.State.Tawaret;

namespace Core.Unit.Boss.State.Tawaret
{
    public class TPattern3State : State<TawaretAI>
    {
        public Animator anim;
        public Animator bodyAnim;
        public Animator lArmAnim;

        private bool isAttacking = false;
        private float stateDuration = 3f;
        private float stateElapsedTime = 0f;
        private TawaretAI currentEntity;

        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject meteorPrefab;

        public override void Enter(TawaretAI entity)
        {
            anim.SetTrigger("Pattern3");
            currentEntity = entity;
        }

        public override void Execute(TawaretAI entity)
        {
            stateElapsedTime += Time.deltaTime;

            if (!entity.tawaret.isAlive)
            {
                entity.ChangeState(TawaretStateType.Dead);
                return;
            }

            if (isAttacking)
            {
                return;
            }

            if (stateElapsedTime >= stateDuration)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 15)
                {
                    entity.ChangeState(TawaretStateType.Pattern3);
                }
                else if (randomValue < 60)
                {
                    entity.ChangeState(TawaretStateType.Pattern1);
                }
                else
                {
                    entity.ChangeState(TawaretStateType.Pattern2);
                }
                return;
            }
        }

        public override void Exit(TawaretAI entity)
        {
            isAttacking = false;
            stateElapsedTime = 0f;
        }

        public override void OnTransition(TawaretAI entity)
        {

        }

        private void StartAttack()
        {
            isAttacking = true;
            bodyAnim.enabled = false;
            lArmAnim.enabled = false;
            Debug.Log("공격 시작");
        }


        private void EndAttack()
        {
            isAttacking = false;
            bodyAnim.enabled = true;
            lArmAnim.enabled = true;
            Debug.Log("공격 끝");
        }

        private void SpawnMeteor()
        {
            int spawnCount = (currentEntity.tawaret.currentHealth <= currentEntity.tawaret.maxHealth * 0.5f) ? 2 : 1;
            List<int> usedIndices = new List<int>();

            for (int i = 0; i < spawnCount; i++)
            {
                int randomIndex;

                // 중복되지 않는 스폰 포인트 선택
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                } while (usedIndices.Contains(randomIndex));

                usedIndices.Add(randomIndex);

                Transform spawnPoint = spawnPoints[randomIndex];
                GameObject meteor = GameObject.Instantiate(meteorPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}



