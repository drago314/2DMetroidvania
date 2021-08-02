using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public void Knockback(Vector2 direction)
    {
        transform.position = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
    }
}
