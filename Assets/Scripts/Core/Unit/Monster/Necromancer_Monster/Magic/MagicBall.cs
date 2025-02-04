using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Unit.Player;

namespace Core.Effect
{
    public class MagicBall : MonoBehaviour
    {
        private bool isHit = false;


        private Vector2 fixedDirection;

        private GameObject player;

        private float speed = 5f;

        private Rigidbody2D rigid;

        private int damage = 10;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Core.Unit.Player.Seth>().gameObject;

            rigid = this.GetComponent<Rigidbody2D>();   

            fixedDirection = (player.transform.position - transform.position).normalized;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isHit)
            {
                MoveInFixedDirection();
            }
        }

        private void MoveInFixedDirection()
        {
            if(fixedDirection.x > 0 && transform.localPosition.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(fixedDirection.x < 0 && transform.localPosition.x >0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            float xVelocity = fixedDirection.x * speed * 1.5f;

            rigid.velocity = new Vector2(xVelocity, rigid.velocity.y);
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isHit = true;
                collision.gameObject.GetComponent<Core.Unit.Player.Seth>().TakeDamage(damage);
                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Parry"))
            {
                Destroy(gameObject);
            }
        }
    }
}


