using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : EnemyDamaged
{
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackForce;

    [SerializeField] private float iFrameDuration;

    private InvFrame iFrame;

    private Vector2 direction;
    private float knockbackTimer;
    private bool knocked;

    private new void Start()
    {
        base.Start();
        iFrame = gameObject.GetComponent<InvFrame>();
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

    protected virtual void OnHit(object sender, Health.OnHitEventArg e)
    {
        iFrame.InvForTime(iFrameDuration);

        int damageType = e.damage.damageType;
        if (damageType == Damage.PLAYER_BASIC_ATTACK)
        {
            direction = Vector2.down;
            knockbackTimer = knockbackTime;
            knocked = true;
        }
    }

    protected virtual new void OnDeath(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
