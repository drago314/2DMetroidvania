using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackForce;

    private Vector2 direction;
    private float knockbackTimer;
    private bool knocked;

    private void Update()
    {
        if (knocked)
        {
            float xPos = transform.position.x + (direction.x * knockbackTimer * Time.deltaTime * knockbackForce);
            float yPos = transform.position.y + (direction.y * knockbackTimer * Time.deltaTime * knockbackForce);
            transform.position = new Vector2(xPos, yPos);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
                knocked = false;
        }
    }

    public void Knockback(Vector2 d)
    {
        direction = d;
        knockbackTimer = knockbackTime;
        knocked = true;
    }
}
