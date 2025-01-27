using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss;
using Core.Unit.Boss.State.Tawaret;

namespace Core.Unit.Boss.State.Tawaret
{
    public class TPattern1State : State<TawaretAI>
    {
        public Animator anim;
        public Animator bodyAnim;
        public Animator rArmAnim;

        private bool isAttacking = false;
        private float stateDuration = 3f;
        private float stateElapsedTime = 0f;

        public GameObject magicPrefab;
        public Transform spawnPosition;  
        private int magicCount = 10;
        private float magicSpeed = 5f;

        private float spellTimer;



        public override void Enter(TawaretAI entity)
        {
            if(entity.tawaret.currentHealth < entity.tawaret.maxHealth/2)
            {
                magicCount = 20;
            }
            anim.SetTrigger("Pattern1");
        }

        public override void Execute(TawaretAI entity)
        {
            stateElapsedTime += Time.deltaTime;

            if (!entity.tawaret.isAlive)
            {
                entity.ChangeState(TawaretStateType.Dead);
                return;
            }

            if (stateElapsedTime >= stateDuration)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if (randomValue < 15)
                {
                    entity.ChangeState(TawaretStateType.Pattern1);
                }
                else if (randomValue < 60)
                {
                    entity.ChangeState(TawaretStateType.Pattern2);
                }
                else
                {
                    entity.ChangeState(TawaretStateType.Pattern3);
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


        //애니메이션 이벤트
        private void StartAttack()
        {
            isAttacking = true;
            bodyAnim.enabled = false;
            rArmAnim.enabled = false;
            Debug.Log("공격 시작");
        }

        private void EndAttack()
        {
            isAttacking = false;
            bodyAnim.enabled = true;
            rArmAnim.enabled = true;
            Debug.Log("공격 끝");
        }

        private void FireMagic()
        {
            // 탄막의 각도 설정
            float angleStep = 360f / magicCount; // 탄막 사이의 간격 각도
            float startAngle = 0f; // 패턴 1에서는 시작 각도 조정

            for (int i = 0; i < magicCount; i++)
            {
                // 기본 각도 계산
                float angle = startAngle + i * angleStep;

                // 방향 계산
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

                // 탄막 생성
                GameObject magic = GameObject.Instantiate(magicPrefab, spawnPosition.position, Quaternion.identity);

                // Null 확인 및 속도 적용
                if (magic != null)
                {
                    Rigidbody2D rb = magic.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = direction * magicSpeed;
                    }
                }
            }
        }




    }
}


