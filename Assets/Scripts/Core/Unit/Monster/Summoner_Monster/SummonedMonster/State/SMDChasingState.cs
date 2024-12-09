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
        private Vector2 fixedDirection;

        public override void Enter(SummonedMonsterAI entity)
        {
            // 처음 플레이어의 위치를 기준으로 방향 설정
            Vector2 playerPosition = entity.summonedMonster.targetObject.transform.position;
            Vector2 monsterPosition = entity.transform.position;

            // 고정된 방향 계산
            fixedDirection = (playerPosition - monsterPosition).normalized;
        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if (!entity.summonedMonster.isAlive)
            {
                StopAndTransition(entity, SMDMonsterStateType.Dead);
                return;
            }

            if (!isHit)
            {
                CheckingHit(entity); // 충돌 확인
                MoveInFixedDirection(entity); // 고정된 방향으로 이동
            }
            else
            {
                StopAndTransition(entity, SMDMonsterStateType.Stick); // 충돌 시 Stick 상태로 전환
            }


        }

        public override void Exit(SummonedMonsterAI entity)
        {
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }

        private void MoveInFixedDirection(SummonedMonsterAI entity)
        {
            // X축 방향에 따라 몬스터 뒤집기 처리
            if (fixedDirection.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (fixedDirection.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            // 속도 설정 (고정된 방향으로 이동, Y축은 0)
            float xVelocity = fixedDirection.x * entity.summonedMonster.moveSpeed * 1.5f;

            // Rigidbody2D 속도 설정 (Y축은 기존 값을 유지)
            entity.summonedMonster.rigid.velocity = new Vector2(xVelocity, entity.summonedMonster.rigid.velocity.y);
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

