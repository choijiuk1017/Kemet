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

        public bool isHit = false;

        private void Start()
        {
            anim = GetComponent<Animator>();

            summonedMonster = this.GetComponent<SummonedMonster>();

            fsm = new MonsterFSM<SummonedMonsterAI>();

            states = new State<SummonedMonsterAI>[(int)SMDMonsterStateType.Last];
            
            states[(int)SMDMonsterStateType.Chasing] = GetComponent<SMDChasingState>();
            states[(int)SMDMonsterStateType.Stick] = GetComponent<SMDStickState>();
            states[(int)SMDMonsterStateType.Dead] = GetComponent<SMDDeadState>();
            
            fsm.Init(this, states[(int)SMDMonsterStateType.Chasing]);
        }

        private void Update()
        {
            fsm.StateUpdate();            
        }

        public void ChangeState(SMDMonsterStateType newState)
        {
            prevState = curState;

            curState = newState;

            fsm.ChangeState(states[(int)newState]); 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject == summonedMonster.targetObject)
            {
                isHit = true;
            }
        }
    }

}
