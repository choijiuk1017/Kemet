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
            // ó�� �÷��̾��� ��ġ�� �������� ���� ����
            Vector2 playerPosition = entity.summonedMonster.targetObject.transform.position;
            Vector2 monsterPosition = entity.transform.position;

            // ������ ���� ���
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
                CheckingHit(entity); // �浹 Ȯ��
                MoveInFixedDirection(entity); // ������ �������� �̵�
            }
            else
            {
                StopAndTransition(entity, SMDMonsterStateType.Stick); // �浹 �� Stick ���·� ��ȯ
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
            // X�� ���⿡ ���� ���� ������ ó��
            if (fixedDirection.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (fixedDirection.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            // �ӵ� ���� (������ �������� �̵�, Y���� 0)
            float xVelocity = fixedDirection.x * entity.summonedMonster.moveSpeed * 1.5f;

            // Rigidbody2D �ӵ� ���� (Y���� ���� ���� ����)
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

