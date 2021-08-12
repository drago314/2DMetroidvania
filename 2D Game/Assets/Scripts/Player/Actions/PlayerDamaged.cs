using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float screenShakeTime;
    [SerializeField] private float screenShakeIntensity;
    
    private PlayerActions playerActions;
    private Rigidbody2D body;
    private InvFrame iFrame;

    private float knockbackTimer = 0f;

    private void Start()
    {
        playerActions = gameObject.GetComponent<PlayerActions>();
        iFrame = gameObject.GetComponent<InvFrame>();
        body = playerActions.body;

        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
    }

    public bool CheckControl()
    {
        bool hasControl = false;

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            hasControl = true;
        }

        return hasControl;
    }

    private void OnHit(object sender, Health.OnHitEventArg e)
    {
        if (e.damage.damageType == Damage.ENVIRONMENT)
            OnEnvironmentHit(e.damage);
        else if (e.damage.damageType == Damage.ENEMY)
            OnEnemyHit(e.damage);
    }

    private void OnEnvironmentHit(Damage damage)
    {

    }

    private void OnEnemyHit(Damage damage)
    {
        knockbackTimer = knockbackTime;

        Vector2 player = transform.position;
        Vector2 enemy = damage.source.transform.position;
        Vector2 direction = player - enemy;
        direction.Normalize();
        body.velocity = direction * knockbackForce;

        CinemachineEffects.instance.Shake(screenShakeIntensity, screenShakeTime);

        iFrame.InvForTime(invincibilityTime);
    }
}
