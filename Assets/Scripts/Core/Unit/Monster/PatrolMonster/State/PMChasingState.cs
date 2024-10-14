using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster;
using Core.Unit.Monster.State.PatrolMonster;

namespace Core.Unit.Monster.State.PatrolMonster
{
    public class PMChasingState : State<PatrolMonsterAI>
    {
        // Start is called before the first frame update
        public override void Enter(PatrolMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);

           
        }

        public override void Execute(PatrolMonsterAI entity)
        {
            if (!entity.patrolMonster.isGroggy)
            {
                float playerDistance = Vector2.Distance(entity.patrolMonster.targetObject.transform.position, entity.transform.position);

                if (entity.patrolMonster.targetObject != null)
                {
                    if (playerDistance <= 7f)
                    {
                        Chasing(entity);

                        if (playerDistance <= 2f)
                        {
                            entity.patrolMonster.rigid.velocity = Vector2.zero;
                            entity.anim.SetBool("Walk", false);
                            entity.ChangeState(MonsterStateType.Attacking);
                        }
                    }
                    else
                    {
                        entity.ChangeState(MonsterStateType.Patrol);
                    }
                }
            }
            else
            {
                entity.ChangeState(MonsterStateType.Groggy);
            }

            if (!entity.patrolMonster.isAlive)
            {
                entity.ChangeState(MonsterStateType.Dead);
            }

        }

        public override void Exit(PatrolMonsterAI entity)
        {
            
        }

        public override void OnTransition(PatrolMonsterAI entity)
        {

        }

        private void Chasing(PatrolMonsterAI entity)
        {
            entity.anim.SetBool("Walk", true);

            Vector2 direction = (entity.patrolMonster.targetObject.transform.position - entity.transform.position).normalized;

            if (direction.x > 0 && entity.transform.localScale.x < 0)
            {
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }
            else if (direction.x < 0 && entity.transform.localScale.x > 0)
            {
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            }


            entity.patrolMonster.rigid.velocity = new Vector2(direction.x * entity.patrolMonster.moveSpeed * 1.5f, entity.patrolMonster.rigid.velocity.y) ;

        }
    }

}