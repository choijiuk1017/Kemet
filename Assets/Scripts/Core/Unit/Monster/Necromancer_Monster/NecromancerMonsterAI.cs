using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.NecromancerMonster;
using UnityEngine;

namespace Core.Unit.Monster
{
    public enum NMMonsterStateType
    {
        Idle, Patrol, Attack, Heal, Teleport, Groggy, Dead, Last    
    }

    public class NecromancerMonsterAI : MonoBehaviour
    {
        private MonsterFSM<NecromancerMonsterAI> fsm;

        private State<NecromancerMonsterAI>[] states;


        [SerializeField]
        private NMMonsterStateType prevState;

        [SerializeField]
        private NMMonsterStateType curState;

        public NecromancerMonster necromancerMonster;

        public Animator anim;
        
        void Start()
        {
            anim = this.GetComponent<Animator>();

            necromancerMonster = this.GetComponent<NecromancerMonster>();

            fsm = new MonsterFSM<NecromancerMonsterAI>();

            states = new State<NecromancerMonsterAI>[(int)NMMonsterStateType.Last];

            states[(int)NMMonsterStateType.Idle] = GetComponent<NMIdleState>();
            states[(int)NMMonsterStateType.Patrol] = GetComponent<NMPatrolState>();
            states[(int)NMMonsterStateType.Attack] = GetComponent<NMAttackState>();
            states[(int)NMMonsterStateType.Heal] = GetComponent<NMHealState>();
            states[(int)NMMonsterStateType.Teleport] = GetComponent<NMTeleportState>();
            states[(int)NMMonsterStateType.Groggy] = GetComponent<NMGroggyState>();
            states[(int)NMMonsterStateType.Dead] = GetComponent<NMDeadState>();

            fsm.Init(this, states[(int)NMMonsterStateType.Idle]);

        }

        // Update is called once per frame
        void Update()
        {
            fsm.StateUpdate();
        }

        public void ChangeState(NMMonsterStateType newState)
        {
            prevState = curState;

            curState = newState;

            fsm.ChangeState(states[(int)newState]);
        }


    }


}

