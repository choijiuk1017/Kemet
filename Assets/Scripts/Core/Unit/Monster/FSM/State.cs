using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unit.Monster.FSM
{
    public abstract class State<T> : MonoBehaviour
    {

        protected float elapsedTime;

        protected virtual void Awake()
        {

        }

        public abstract void Enter(T entity);
        public abstract void Execute(T entity);
        public abstract void Exit(T entity);
        public abstract void OnTransition(T entity);
    }
}
