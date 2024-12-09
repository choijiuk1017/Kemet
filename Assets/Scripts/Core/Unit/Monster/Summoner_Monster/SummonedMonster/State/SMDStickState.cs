using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;

namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDStickState : State<SummonedMonsterAI>
    {
        private Vector2 localStickOffset = Vector2.zero; // 부모 기준 위치 저장
        private int damageCount = 0;
        private float damageInterval = 1f;
        private float damageTimer = 0f;

        public override void Enter(SummonedMonsterAI entity)
        {
            // 플레이어를 부모로 설정
            Transform targetTransform = entity.summonedMonster.targetObject.transform;
            entity.transform.parent = targetTransform;

            // 부모 기준 오프셋 계산
            localStickOffset = targetTransform.InverseTransformPoint(entity.transform.position);

            // 애니메이션 트리거
            entity.anim.SetTrigger("Stick");

            // 몬스터 위치를 부모 기준으로 고정
            entity.transform.localPosition = localStickOffset;

            // 플레이어 방향으로 회전
            UpdateFacingDirection(entity);

            damageCount = 0;
            damageTimer = 0f;   
        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if(entity.summonedMonster.targetObject.GetComponent<Core.Unit.Player.Seth>().isSliding)
            {
                // 상태 종료 시 부모와의 연결 해제
                entity.transform.parent = null;
                entity.ChangeState(SMDMonsterStateType.Dead);
                return;
            }

            if (entity.transform.parent != null)
            {
                // 부모 기준 위치 고정 유지
                entity.transform.localPosition = localStickOffset;

                // 플레이어 방향 변화 감지 후 회전
                UpdateFacingDirection(entity);

                DealDamageToPlayer(entity);

                if (damageCount == 3)
                {
                    // 상태 종료 시 부모와의 연결 해제
                    entity.transform.parent = null;
                    entity.ChangeState(SMDMonsterStateType.Dead);
                }
            }

        }

        public override void Exit(SummonedMonsterAI entity)
        {
            // 상태 종료 시 부모와의 연결 해제
            entity.transform.parent = null;
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }

        private void UpdateFacingDirection(SummonedMonsterAI entity)
        {
            Transform playerTransform = entity.summonedMonster.targetObject.transform;

            // 부모(플레이어)의 스프라이트 방향에 따라 몬스터 방향 설정
            float playerDirection = Mathf.Sign(playerTransform.localScale.x);
            entity.transform.localScale = new Vector3(
                Mathf.Abs(entity.transform.localScale.x) * -playerDirection,
                entity.transform.localScale.y,
                entity.transform.localScale.z
            );
        }

        private void DealDamageToPlayer(SummonedMonsterAI entity)
        {
            if(damageCount < 3)
            {
                damageTimer += Time.deltaTime;

                if(damageTimer >= damageInterval)
                {
                    entity.summonedMonster.targetObject.GetComponent<Core.Unit.Player.Seth>().TakeDamage(entity.summonedMonster.damage);

                    damageCount++;
                    damageTimer = 0f;
                }
            }
        }
    }
}