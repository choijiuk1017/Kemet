using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.SummonedMonster;

namespace Core.Unit.Monster.State.SummonedMonster
{
    public class SMDStickState : State<SummonedMonsterAI>
    {
        private Vector2 stickOffset = Vector2.zero; // �浹 ���� ���� ������ ����

        public override void Enter(SummonedMonsterAI entity)
        {
            // �浹 ���� ���
            Vector2 collisionPoint = entity.transform.position - entity.summonedMonster.targetObject.transform.position;
            stickOffset = collisionPoint; // ������ ����

            // �ִϸ��̼� Ʈ����
            entity.anim.SetTrigger("Stick");

            // �θ� ����
            entity.transform.parent = entity.summonedMonster.targetObject.transform;

            // �浹 �������� ���� ��ġ ����
            entity.transform.localPosition = stickOffset;

            // �÷��̾� ���� �ٶ󺸱�
            LookAtPlayer(entity);
        }

        public override void Execute(SummonedMonsterAI entity)
        {
            if (entity.transform.parent != null)
            {
                // �浹 ���� ����
                entity.transform.localPosition = stickOffset;

                // �÷��̾� ���� �ٶ󺸱�
                LookAtPlayer(entity);
            }
        }

        public override void Exit(SummonedMonsterAI entity)
        {
            // ���� ���� �� �θ� ���� ����
            entity.transform.parent = null;
        }

        public override void OnTransition(SummonedMonsterAI entity)
        {
        }

        private void LookAtPlayer(SummonedMonsterAI entity)
        {
            // �÷��̾��� ��ġ�� �������� ���� ���
            float playerX = entity.summonedMonster.targetObject.transform.position.x;
            float monsterX = entity.transform.position.x;

            // X�� ���⿡ ���� ���� ������
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