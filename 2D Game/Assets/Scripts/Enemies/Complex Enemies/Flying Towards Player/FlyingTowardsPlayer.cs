using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTowardsPlayer : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    public float GetSpeed()
    {
        return speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(damage);
        }
    }
}
