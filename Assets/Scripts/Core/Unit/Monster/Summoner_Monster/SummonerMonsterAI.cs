using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.SummonerMonster;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum SMMonsterStateType
    {
        Idle, Patrol, Summon, Groggy, Dead, Last
    }


    public class SummonerMonsterAI : MonoBehaviour
    {
        private MonsterFSM<SummonerMonsterAI> fsm;

        private State<SummonerMonsterAI>[] states;

        [SerializeField]
        private SMMonsterStateType prevState;

        [SerializeField]
        private SMMonsterStateType curState;

        public SummonerMonster summonerMonster;

        public Animator anim;

        private void Start()
        {
            anim = this.GetComponent<Animator>();

            summonerMonster = this.GetComponent<SummonerMonster>();

            fsm = new MonsterFSM<SummonerMonsterAI>();


            states = new State<SummonerMonsterAI>[(int)SMMonsterStateType.Last];

            states[(int)SMMonsterStateType.Idle] = GetComponent<SMIdleState>();
            states[(int)SMMonsterStateType.Patrol] = GetComponent<SMPatrolState>();
            states[(int)SMMonsterStateType.Summon] = GetComponent<SMSummonState>();
            states[(int)(SMMonsterStateType.Groggy)] = GetComponent<SMGroggyState>();
            states[(int)SMMonsterStateType.Dead] = GetComponent<SMDeadState>();

            fsm.Init(this, states[(int)SMMonsterStateType.Idle]);
            
        }

        private void Update()
        {
            fsm.StateUpdate();
        }

        public void ChangeState(SMMonsterStateType newState)
        {
            prevState = curState;

            curState = newState;

            fsm.ChangeState(states[(int)newState]);
        }

    }


}


