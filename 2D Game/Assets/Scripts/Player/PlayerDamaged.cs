using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour, IHealthCallback
{
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackForce;
    
    private PlayerActions playerActions;
    private Rigidbody2D body;

    private float knockbackTimer = 0f;

    private void Start()
    {
        playerActions = gameObject.GetComponent<PlayerActions>();

        body = playerActions.body;

        gameObject.GetComponent<Health>().SetCallbackListener(this);
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

    public void OnDeath(Damage damage)
    {
    }

    public void OnHeal()
    {
    }

    public void OnHealthChanged(Health health)
    {
    }

    public void OnHit(Damage damage)
    {
        if (damage.damageType == Damage.ENVIRONMENT)
            OnEnvironmentHit(damage);
        else if (damage.damageType == Damage.ENEMY)
            OnEnemyHit(damage);
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

        CinemachineEffects.instance.Punch();
    }
}
