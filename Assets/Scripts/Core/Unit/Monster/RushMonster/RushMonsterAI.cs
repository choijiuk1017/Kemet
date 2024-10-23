using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.RushMonster;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum RMMonsterStateType
    {
        Idle, Patrol, StartRushing, Rushing, StopRushing, Groggy, Dead, Last
    }

    public class RushMonsterAI : MonoBehaviour
    {

        private MonsterFSM<RushMonsterAI> fsm;

        private State<RushMonsterAI>[] states;

        [SerializeField]
        private RMMonsterStateType prevState;

        [SerializeField]
        private RMMonsterStateType curState;

        public RushMonster rushMonster;

        public Animator anim;


        // Start is called before the first frame update
        void Start()
        {
            anim = this.GetComponent<Animator>();

            rushMonster = this.GetComponent<RushMonster>();

            fsm = new MonsterFSM<RushMonsterAI>();

            states = new State<RushMonsterAI>[(int)RMMonsterStateType.Last];

            states[(int)RMMonsterStateType.Idle] = GetComponent<RMIdleState>();
            states[(int)RMMonsterStateType.Patrol] = GetComponent<RMPatrolState>();
            states[(int)RMMonsterStateType.StartRushing] = GetComponent<RMStartRushingState>();
            states[(int)RMMonsterStateType.Rushing] = GetComponent<RMRushingState>();
            states[(int)RMMonsterStateType.StopRushing] = GetComponent<RMStopRushingState>();
            states[(int)RMMonsterStateType.Groggy] = GetComponent<RMGroggyState>();
            states[(int)RMMonsterStateType.Dead] = GetComponent<RMDeadState>();

            fsm.Init(this, states[(int)RMMonsterStateType.Idle]);
        }

        // Update is called once per frame
        void Update()
        {
            fsm.StateUpdate();
        }

        public void ChangeState(RMMonsterStateType newState)
        {
            curState = newState;

            fsm.ChangeState(states[(int)newState]);  
        }
    }


}

