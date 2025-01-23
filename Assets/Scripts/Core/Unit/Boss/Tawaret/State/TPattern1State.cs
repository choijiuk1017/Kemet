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
        public float rotationSpeed = 50f;
        public float fireRate = 0.2f;
        public int magicCount = 8;
        public float magicSpeed = 5f;

        private float spellTimer;
        private int currentPattern;



        public override void Enter(TawaretAI entity)
        {
            anim.SetTrigger("Pattern1");

            currentPattern = UnityEngine.Random.Range(0,3);
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
            float angleStep = 360f / magicCount;
            float startAngle = currentPattern == 1 ? 45f : 0f;

            for(int i = 0; i < magicCount; i++)
            {
                float angle = startAngle + i * angleStep;

                if(currentPattern == 2)
                {
                    angle += Time.time * rotationSpeed;
                }

                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
                Vector3 spawnPos = (Vector3)spawnPosition.position;

                GameObject magic = GameObject.Instantiate(magicPrefab, spawnPos, Quaternion.identity);
                Rigidbody2D rb = magic.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * magicSpeed;
                }
            }
        }




    }
}


