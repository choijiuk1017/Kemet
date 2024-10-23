using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMPatrolState : State<PatrolMonsterAI>
    {


        private const string animationName = "Patrol";

        private bool isMovingRight = true;

        private float moveDistance = 10f;
        private float totalDistance = 0f;
        private float pauseDuration = 1f;
        private float pauseTimer = 0f;

        private Vector2 lastPosition;


        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);

            totalDistance = 0f;
            pauseTimer = 0f;

            lastPosition = entity.transform.position;
        }

        public override void Execute(PatrolMonsterAI entity)
        {

            if (entity.patrolMonster.isGroggy)
            {
                entity.ChangeState(PMMonsterStateType.Groggy);
                return;
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(PMMonsterStateType.Dead);
                return;
            }

            float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

            if (playerDistance <= 7f)
            {
                entity.ChangeState(PMMonsterStateType.Chasing);
            }
            else
            {
                Patrol(entity);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {

        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

        private void Patrol(PatrolMonsterAI entity)
        {
            if (pauseTimer > 0f)
            {
                entity.patrolMonster.rigid.velocity = Vector2.zero;
                pauseTimer -= Time.deltaTime;
                return;
            }

            if (!entity.patrolMonster.isGroundAhead || entity.patrolMonster.isWallAhead)
            {
                entity.patrolMonster.Flip();

                // �̵� ���� ����
                isMovingRight = !isMovingRight;

                // ���� ��ȯ �� �̵��� �Ÿ��� �ٽ� ����
                moveDistance = 20f;
                totalDistance = 0f; // �̵� �Ÿ� �ʱ�ȭ
            }

            // ���� �Ÿ��� �̵��ϸ� ���� ��ȯ
            if (totalDistance >= moveDistance)
            {
                entity.anim.SetBool("Walk", false);
                entity.anim.SetTrigger(animationName);

                // ���� �ð� ����
                pauseTimer = pauseDuration;

                // �̵� ���� ����
                isMovingRight = !isMovingRight;

                // ���� ��ȯ �� �̵��� �Ÿ��� �ٽ� ����
                moveDistance = 20f;
                totalDistance = 0f; // �̵� �Ÿ� �ʱ�ȭ

                return;
            }

            Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;

            // ���� ��ȯ ó�� (�ߺ��� �� ���� ����)
            float scaleX = entity.transform.localScale.x;
            if ((direction.x > 0 && scaleX < 0) || (direction.x < 0 && scaleX > 0))
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(scaleX) * Mathf.Sign(direction.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }

            // ������ �̵�
            entity.patrolMonster.rigid.velocity = new Vector2(direction.x * entity.patrolMonster.moveSpeed, entity.patrolMonster.rigid.velocity.y);

            // totalDistance�� ���� �̵��� �Ÿ� ���� (�ѹ��� ���)
            totalDistance += Vector2.Distance(entity.transform.position, lastPosition);

            // ���� ��ġ�� lastPosition�� �����Ͽ� ���� �����ӿ��� ���� �� �ְ� ��
            lastPosition = entity.transform.position;

            // �ִϸ��̼� ����
            entity.anim.SetBool("Walk", true);
        }

        
    }
}

    
