using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMagic : MonoBehaviour
{
    public float healAmount = 20f;

    public float duration = 6f;

    // 이미 회복한 몬스터를 추적
    private HashSet<GameObject> healedMonsters =new HashSet<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Monster"))
        {
            GameObject monster = other.gameObject;

            if(!healedMonsters.Contains(monster))
            {
                HealMonster(monster);
                healedMonsters.Add(monster);
            }
        }
    }

    private void HealMonster(GameObject monster)
    {
        monster.GetComponent<Core.Unit.Monster.Monster>().Heal(healAmount);
    }
}
