using System.Collections;
using System.Collections.Generic;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss.State.Tawaret;
using UnityEngine;


namespace Core.Unit.Boss
{
    public enum TawaretStateType
    {
        Idle, Pattern1, Pattern2, Pattern3, Dead, Last
    }

    public class TawaretAI : MonoBehaviour
    {
        private BossFSM<TawaretAI> fsm;

        private State<TawaretAI>[] states;

        [SerializeField]
        private TawaretStateType prevState;

        [SerializeField]
        private TawaretStateType curState;

        public Tawaret tawaret;


        private void Start()
        {

            tawaret = this.GetComponent<Tawaret>();

            fsm = new BossFSM<TawaretAI>();

            states = new State<TawaretAI>[(int)TawaretStateType.Last];

            states[(int)TawaretStateType.Idle] = GetComponent<TIdleState>();
            states[(int)TawaretStateType.Pattern1] = GetComponentInChildren<TPattern1State>();
            states[(int)TawaretStateType.Pattern2] = GetComponentInChildren<TPattern2State>();
            states[(int)TawaretStateType.Pattern3] = GetComponentInChildren<TPattern3State>();
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

