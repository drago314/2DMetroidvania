using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStationary : Enemy
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(new Damage(damage, this.gameObject, Damage.ENEMY));
        }
    }
}
