using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss;
using Core.Unit.Boss.State.Tawaret;

namespace Core.Unit.Boss.State.Tawaret
{
    public class TPattern2State : State<TawaretAI>
    {
        public Animator anim;
        public Animator bodyAnim;
        public Animator rArmAnim;

        private bool isAttacking = false;
        private float stateDuration = 3f;
        private float stateElapsedTime = 0f;

        //Transform ����
        public Transform atkPos; //���� ��Ÿ�


        //Vector2 ����
        public Vector2 atkBoxSize; //��Ʈ �ڽ�

        public override void Enter(TawaretAI entity)
        {
            anim.SetTrigger("Pattern2");
        }

        public override void Execute(TawaretAI entity)
        {
            stateElapsedTime += Time.deltaTime;

            if(!entity.tawaret.isAlive)
            {
                entity.ChangeState(TawaretStateType.Dead);
                return;
            }

            if(isAttacking)
            {
                return;
            }

            if(stateElapsedTime >= stateDuration)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if(randomValue < 15)
                {
                    entity.ChangeState(TawaretStateType.Pattern2);
                }
                else if(randomValue < 60)
                {
                    entity.ChangeState(TawaretStateType.Pattern1);
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

        private void StartAttack()
        {
            isAttacking = true;
            bodyAnim.enabled = false;
            rArmAnim.enabled = false;
            Debug.Log("���� ����");
        }

        private void HitBoxOn()
        {
            bool parrySuccess = false;

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(atkPos.position, atkBoxSize, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Parry")
                {
                    Debug.Log("�÷��̾� �и� ����");
                    parrySuccess = true;
                }
            }

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player" && !parrySuccess) // �и� ���� �� ������ ����
                {
                    collider.GetComponent<Core.Unit.Player.Seth>().TakeDamage(20);
                    Debug.Log("���� ���� ��");
                }
            }
        }

        private void EndAttack()
        {
            isAttacking = false;
            bodyAnim.enabled = true;
            rArmAnim.enabled = true;
            Debug.Log("���� ��");
        }
    

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }
    }
}


