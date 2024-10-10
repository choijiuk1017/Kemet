using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unit.Monster.FSM
{
    public class MonsterFSM<T> where T : class
    {
        T entity;
        State<T> curState;
        State<T> prevState;

        public void Init(T entity, State<T> state)
        {
            this.entity = entity;
            curState = state;
            ChangeState(state);
        }

        public void ChangeState(State<T> newState)
        {
            if (newState == null)
            {
                Debug.LogError("New state is null. Cannot change state.");
                return;
            }

            prevState = curState;
            if (curState != null)
                curState.Exit(entity);
            curState = newState;
            curState.Enter(entity);
        }

        public void StateUpdate()
        {
            if(curState != null)
            {
                curState.Execute(entity);
                curState.OnTransition(entity);
            }
        }

        public State<T> GetPrevState()
        {
            return prevState;
        }
    }

}
