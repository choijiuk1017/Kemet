using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Unit.Boss;

namespace Core.UI
{
    public class BossHPUI : MonoBehaviour
    {
        public Boss boss;

        public Slider slider;

       void Start()
       {
            slider = GetComponentInChildren<Slider>();
            slider.maxValue = boss.maxHealth;
            slider.value = boss.currentHealth;
       }

        // Update is called once per frame
        void Update()
        {
            slider.value = boss.currentHealth;
        }
    }
}


