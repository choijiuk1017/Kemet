using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.SummonedMonster;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum SMDMonsterStateType
    {
        Chasing, Stick, Dead, Last    
    }

    public class SummonedMonsterAI : MonoBehaviour
    {
        private MonsterFSM<SummonedMonsterAI> fsm;

        private State<SummonedMonsterAI>[] states;

        [SerializeField]
        private SMDMonsterStateType prevState;

        [SerializeField]
        private SMDMonsterStateType curState;

        public SummonedMonster summonedMonster;

        public Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();

            summonedMonster = this.GetComponent<SummonedMonster>();

            fsm = new MonsterFSM<SummonedMonsterAI>();

            states = new State<SummonedMonsterAI>[(int)SMDMonsterStateType.Last];
            /*
            states[(int)SMDMonsterStateType.Chasing] = new State<SMDChasingState>();
            states[(int)SMDMonsterStateType.Chasing] = new State<SMDStickState>();
            states[(int)SMDMonsterStateType.Chasing] = new State<SMDDeadState>();
            */
            fsm.Init(this, states[(int)SMDMonsterStateType.Chasing]);
        }
    }

}
