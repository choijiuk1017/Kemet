using System.Collections;
using System.Collections.Generic;
using Core.Unit.Monster.FSM;
using Core.Unit.Monster.State.Tawaret;
using UnityEngine;


namespace Core.Unit.Monster
{
    public enum TawaretStateType
    {
        Idle, Pattern1, Pattern2, Pattern3, Dead, Last
    }

    public class TawaretAI : MonoBehaviour
    {
        private MonsterFSM<TawaretAI> fsm;

        private State<TawaretAI>[] states;

        [SerializeField]
        private TawaretStateType prevState;

        [SerializeField]
        private TawaretStateType curState;

        public Tawaret tawaret;

        public Animator anim;

        private void Start()
        {
            anim = this.GetComponent<Animator>();

            tawaret = this.GetComponent<Tawaret>();

            fsm = new MonsterFSM<TawaretAI>();

            states = new State<TawaretAI>[(int)TawaretStateType.Last];

            states[(int)TawaretStateType.Idle] = GetComponent<TIdleState>();
            states[(int)TawaretStateType.Pattern1] = GetComponent<TPattern1State>();
            states[(int)TawaretStateType.Pattern2] = GetComponent<TPattern2State>();
            states[(int)TawaretStateType.Pattern3] = GetComponent<TPattern3State>();
            states[(int)TawaretStateType.Dead] = GetComponent<TDeadState>();

            fsm.Init(this, states[(int)TawaretStateType.Idle]);
        }

        private void Update()
        {
            fsm.StateUpdate();
        }

        public void ChangeState(TawaretStateType newState)
        {
            prevState = curState;
            curState = newState;
            fsm.ChangeState(states[(int)newState]);
        }
    }
}

