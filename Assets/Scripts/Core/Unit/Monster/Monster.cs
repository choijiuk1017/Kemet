using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;

namespace Core.Unit.Monster
{
    public class Monster : Unit
    {
        public GameObject targetObject;


        public Rigidbody2D rigid;


        public bool isGroggy = false;

        public float maxGroggyGauge = 100;
        public float groggyGauge;


        //Transform 변수
        public Transform atkPos; //공격 사거리


        //Vector2 변수
        public Vector2 atkBoxSize; //히트 박스

        protected override void Init()
        {
            base.Init();

            rigid = GetComponent<Rigidbody2D>();

            targetObject = GameObject.Find("Seth");
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(atkPos.position, atkBoxSize);
        }
    }
}

