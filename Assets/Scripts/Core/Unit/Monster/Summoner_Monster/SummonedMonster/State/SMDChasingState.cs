using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;


namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDChasingState : State<SummonedMonsterAI>
    {
        public bool isHit = false;


        public override void Enter(SummonedMonsterAI entity)
        {

        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if(!entity.summonedMonster.isAlive)
            {
                entity.ChangeState(SMDMonsterStateType.Dead);
                return;
            }

            if(!isHit)
            {
                CheckingHit(entity);
                Chasing(entity);

            }
            if(isHit)
            {
                StopAndTransition(entity, SMDMonsterStateType.Stick);
            }
            


        }

        public override void Exit(SummonedMonsterAI entity)
        {
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }

        private void Chasing(SummonedMonsterAI entity)
        {
            // 플레이어와 몬스터 위치
            Vector2 playerPosition = entity.summonedMonster.targetObject.transform.position;
            Vector2 monsterPosition = entity.transform.position;

            // X축 방향 계산 및 이동
            Vector2 direction = (playerPosition - monsterPosition).normalized;

            // X축 방향에 따라 몬스터 뒤집기 처리
            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            // X축 속도 설정
            float xVelocity = direction.x * entity.summonedMonster.moveSpeed * 1.5f;

            // Y축 속도: 몬스터가 플레이어보다 높을 때만 내려오도록 설정
            float yVelocity = entity.summonedMonster.rigid.velocity.y; // 기본 Y축 속도 유지
            float headHeightOffset = 0.5f; // 플레이어 머리 높이 오프셋

            if (monsterPosition.y > playerPosition.y + headHeightOffset)
            {
                yVelocity = -entity.summonedMonster.moveSpeed; // 플레이어 머리 높이까지 내려오는 속도
            }

            // Rigidbody2D 속도 설정
            entity.summonedMonster.rigid.velocity = new Vector2(xVelocity, yVelocity);
        }

        private void StopAndTransition(SummonedMonsterAI entity, SMDMonsterStateType newState)
        {
            entity.summonedMonster.rigid.velocity = Vector2.zero;
            entity.ChangeState(newState);
        }

        private void CheckingHit(SummonedMonsterAI entity)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(entity.summonedMonster.atkPos.position, entity.summonedMonster.atkBoxSize, 0);

            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.tag == "Player")
                {
                    isHit = true;
                }
            }
        }
    }

}

