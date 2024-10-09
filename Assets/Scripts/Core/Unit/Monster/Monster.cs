using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Monster
{
    public class Monster : Unit
    {
        public Unit target;

        public float detactionRange;

        private Rigidbody2D rigid;

        public enum State {Idle, Moving, Patrol, Attacking, Dead}
        public State currentState = State.Idle;

        protected override void Init()
        {
            base.Init();

            rigid = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isAlive)
            {
                
            }
        }

        protected virtual void AIBehavior()
        {
            if (target != null)
            {
                float diatanceToTarget = Vector2.Distance(transform.position, target.position);

            }

        }

        protected override void Die()
        {
            base.Die();

            if(anim != null)
            {
                anim.SetTrigger("Die");
            }

            Destroy(gameObject, 2f);
        }

    }
}

