using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss;
using Core.Unit.Boss.State.Tawaret;

namespace Core.Unit.Boss.State.Tawaret
{
    public class TDeadState : State<TawaretAI>
    {
        public override void Enter(TawaretAI entity)
        {

        }

        public override void Execute(TawaretAI entity)
        {
            Destroy(this.gameObject);
        }

        public override void Exit(TawaretAI entity)
        {
        }

        public override void OnTransition(TawaretAI entity)
        {

        }
    }
}
