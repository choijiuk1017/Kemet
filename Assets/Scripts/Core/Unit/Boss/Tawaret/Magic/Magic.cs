using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{

    private GameObject player;

    private float speed = 5f;

    private Rigidbody2D rigid;

    private int damage =10;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Core.Unit.Player.Seth>().gameObject;

        rigid = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 10f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Core.Unit.Player.Seth>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Parry"))
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
