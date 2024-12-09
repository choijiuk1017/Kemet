using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;

namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDStickState : State<SummonedMonsterAI>
    {
        private Vector2 localStickOffset = Vector2.zero; // �θ� ���� ��ġ ����
        private int damageCount = 0;
        private float damageInterval = 1f;
        private float damageTimer = 0f;

        public override void Enter(SummonedMonsterAI entity)
        {
            // �÷��̾ �θ�� ����
            Transform targetTransform = entity.summonedMonster.targetObject.transform;
            entity.transform.parent = targetTransform;

            // �θ� ���� ������ ���
            localStickOffset = targetTransform.InverseTransformPoint(entity.transform.position);

            // �ִϸ��̼� Ʈ����
            entity.anim.SetTrigger("Stick");

            // ���� ��ġ�� �θ� �������� ����
            entity.transform.localPosition = localStickOffset;

            // �÷��̾� �������� ȸ��
            UpdateFacingDirection(entity);

            damageCount = 0;
            damageTimer = 0f;   
        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if(entity.summonedMonster.targetObject.GetComponent<Core.Unit.Player.Seth>().isSliding)
            {
                // ���� ���� �� �θ���� ���� ����
                entity.transform.parent = null;
                entity.ChangeState(SMDMonsterStateType.Dead);
                return;
            }

            if (entity.transform.parent != null)
            {
                // �θ� ���� ��ġ ���� ����
                entity.transform.localPosition = localStickOffset;

                // �÷��̾� ���� ��ȭ ���� �� ȸ��
                UpdateFacingDirection(entity);

                DealDamageToPlayer(entity);

                if (damageCount == 3)
                {
                    // ���� ���� �� �θ���� ���� ����
                    entity.transform.parent = null;
                    entity.ChangeState(SMDMonsterStateType.Dead);
                }
            }

        }

        public override void Exit(SummonedMonsterAI entity)
        {
            // ���� ���� �� �θ���� ���� ����
            entity.transform.parent = null;
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {

        }

        private void UpdateFacingDirection(SummonedMonsterAI entity)
        {
            Transform playerTransform = entity.summonedMonster.targetObject.transform;

            // �θ�(�÷��̾�)�� ��������Ʈ ���⿡ ���� ���� ���� ����
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