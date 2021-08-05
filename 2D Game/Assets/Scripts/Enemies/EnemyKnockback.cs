using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : EnemyDamaged
{
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackForce;

    private Vector2 direction;
    private float knockbackTimer;
    private bool knocked;

    protected  new void Start()
    {
        base.Start();

        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

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

    protected void OnHit(object sender, Health.OnHitEventArg e)
    {
        int damageType = e.damage.damageType;
        if (damageType == Damage.PLAYER_BASIC_ATTACK)
        {
            direction = Vector2.down;
            knockbackTimer = knockbackTime;
            knocked = true;
        }
    }

    protected new void OnDeath(object sender, EventArgs e)
    {
    }
}
