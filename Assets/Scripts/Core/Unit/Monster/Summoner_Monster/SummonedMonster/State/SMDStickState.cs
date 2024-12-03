using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;

namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDStickState : State<SummonedMonsterAI>
    {
        private Vector2 stickOffset = Vector2.zero; // 충돌 지점 기준 오프셋 저장

        public override void Enter(SummonedMonsterAI entity)
        {
            // 충돌 지점 계산
            Vector2 collisionPoint = entity.transform.position - entity.summonedMonster.targetObject.transform.position;
            stickOffset = collisionPoint; // 오프셋 저장

            // 애니메이션 트리거
            entity.anim.SetTrigger("Stick");

            // 부모 설정
            entity.transform.parent = entity.summonedMonster.targetObject.transform;

            // 충돌 지점으로 몬스터 위치 고정
            entity.transform.localPosition = stickOffset;

            // 플레이어 방향 바라보기
            LookAtPlayer(entity);
        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if (entity.transform.parent != null)
            {
                // 충돌 지점 유지
                entity.transform.localPosition = stickOffset;

                // 플레이어 방향 바라보기
                LookAtPlayer(entity);
            }
        }

        public override void Exit(SummonedMonsterAI entity)
        {
            // 상태 종료 시 부모 연결 해제
            entity.transform.parent = null;
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {
        }

        private void LookAtPlayer(SummonedMonsterAI entity)
        {
            // 플레이어의 위치를 기준으로 방향 계산
            float playerX = entity.summonedMonster.targetObject.transform.position.x;
            float monsterX = entity.transform.position.x;

            // X축 방향에 따라 몬스터 뒤집기
            if (playerX > monsterX && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(
                    Mathf.Abs(entity.transform.localScale.x),
                    entity.transform.localScale.y,
                    entity.transform.localScale.z
                );
            }
            else if (playerX < monsterX && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(
                    -Mathf.Abs(entity.transform.localScale.x),
                    entity.transform.localScale.y,
                    entity.transform.localScale.z
                );
            }
        }
    }
}