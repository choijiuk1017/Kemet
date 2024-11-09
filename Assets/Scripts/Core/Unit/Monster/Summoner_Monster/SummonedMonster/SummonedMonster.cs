using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit;
using Core.Unit.Monster;


public class SummonedMonster : Monster
{
    private SummonedMonsterAI summonedMonsterAI;

    public SummonedMonsterAI SummonedMonsterAI => summonedMonsterAI;

    protected override void Init()
    {
        base.Init();
        maxGroggyGauge = 100f;

        groggyGauge = 0f;

        maxHealth = 10f;

        currentHealth = maxHealth;

        moveSpeed = 5f;

        damage = 3f;

        defense = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
