using System;
using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.PatrolMonster;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum PMMonsterStateType
    {
        Idle, Patrol, Chasing, Attacking, Groggy, Dead, Last 
    }
    

    public class PatrolMonsterAI : MonoBehaviour
    {
        private MonsterFSM<PatrolMonsterAI> fsm;

        private State<PatrolMonsterAI>[] states;

        [SerializeField]
        private PMMonsterStateType prevState;

        [SerializeField]
        private PMMonsterStateType curState;

        public PatrolMonster patrolMonster;

        public Animator anim;





        // Start is called before the first frame update
        void Start()
        {
            anim = this.GetComponent<Animator>();
            patrolMonster = this.GetComponent<PatrolMonster>();

            fsm = new MonsterFSM<PatrolMonsterAI>();
            states = new State<PatrolMonsterAI>[(int)PMMonsterStateType.Last];

            states[(int)PMMonsterStateType.Idle] = GetComponent<PMIdleState>();
            states[(int)PMMonsterStateType.Patrol] = GetComponent<PMPatrolState>();
            states[(int)PMMonsterStateType.Chasing] = GetComponent<PMChasingState>();
            states[(int)PMMonsterStateType.Attacking] = GetComponent<PMAttackState>();
            states[(int)PMMonsterStateType.Groggy] = GetComponent<PMGroggyState>();   
            states[(int)PMMonsterStateType.Dead] = GetComponent<PMDeadState>();

            fsm.Init(this, states[(int)PMMonsterStateType.Idle]);
        }


        // Update is called once per frame
        void Update()
        {

            fsm.StateUpdate();

            
        }

        public void ChangeState(PMMonsterStateType newState)
        {

            curState = newState;

            fsm.ChangeState(states[(int)newState]);
        }
    }
}




