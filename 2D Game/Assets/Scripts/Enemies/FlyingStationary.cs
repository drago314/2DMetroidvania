using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStationary : MonoBehaviour
{
    [SerializeField] private int damage;

    private Rigidbody2D body;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        body.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        body.velocity = Vector2.zero;
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Health>().Damage(damage);
        }
    }
}
