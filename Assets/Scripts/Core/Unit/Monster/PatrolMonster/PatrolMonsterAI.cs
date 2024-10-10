using System;
using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.PatrolMonster;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum MonsterStateType
    {
        Idle, Patrol, Chasing, Attacking, Last
    }
    

    public class PatrolMonsterAI : MonoBehaviour
    {
        private MonsterFSM<PatrolMonsterAI> fsm;

        private State<PatrolMonsterAI>[] states;

        [SerializeField]
        private MonsterStateType prevState;

        [SerializeField]
        private MonsterStateType curState;

        public PatrolMonster patrolMonster;

        public Animator anim;





        // Start is called before the first frame update
        void Start()
        {
            anim = this.GetComponent<Animator>();
            patrolMonster = this.GetComponent<PatrolMonster>();

            fsm = new MonsterFSM<PatrolMonsterAI>();
            states = new State<PatrolMonsterAI>[(int)MonsterStateType.Last];

            states[(int)MonsterStateType.Idle] = GetComponent<PMIdleState>();
            states[(int)MonsterStateType.Patrol] = GetComponent<PMPatrolState>();
            states[(int)MonsterStateType.Chasing] = GetComponent<PMChasingState>();

            fsm.Init(this, states[(int)MonsterStateType.Idle]);
        }


        // Update is called once per frame
        void Update()
        {
            fsm.StateUpdate();
        }

        public void ChangeState(MonsterStateType newState)
        {

            curState = newState;

            fsm.ChangeState(states[(int)newState]);
        }
    }
}




