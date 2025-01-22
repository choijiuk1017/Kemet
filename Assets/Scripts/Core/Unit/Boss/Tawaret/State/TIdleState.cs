using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Boss.FSM;
using Core.Unit.Boss;
using Core.Unit.Boss.State.Tawaret;

namespace Core.Unit.Boss.State.Tawaret
{
    public class TIdleState : State<TawaretAI>
    {
        private const float waitTIme = 1f;

        public override void Enter(TawaretAI entity)
        {
            elapsedTime = 0f;
        }

        public override void Execute(TawaretAI entity)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < waitTIme)
            {
                return;
            }

            if(!entity.tawaret.isAlive)
            {
                entity.ChangeState(TawaretStateType.Dead);
                return; 
            }

            if(entity.tawaret.targetObject != null)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);

                if(randomValue < 40)
                {
                    entity.ChangeState(TawaretStateType.Pattern1);
                }
                else if(randomValue < 80)
                {
                    entity.ChangeState(TawaretStateType.Pattern2);
                }
                else 
                { 
                    entity.ChangeState(TawaretStateType.Pattern3); 
                }
                return;
            }
             
        }

        public override void Exit(TawaretAI entity)
        {
        }

        public override void OnTransition(TawaretAI entity)
        {

        }

    }
}

