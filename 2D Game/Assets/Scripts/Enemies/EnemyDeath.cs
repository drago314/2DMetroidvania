using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour, IHealthCallback
{
    [SerializeField] private Health health;

    private void Awake()
    {
        health.SetCallbackListener(this);
    }

    public void OnDeath(Damage damage)
    {
        Destroy(gameObject);
    }

    public void OnHeal()
    {
    }

    public void OnHealthChanged(Health health)
    {
    }

    public void OnHit(Damage damage)
    {
    }
}
