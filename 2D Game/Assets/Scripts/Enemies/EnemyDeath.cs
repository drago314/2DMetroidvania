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

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void OnHeal()
    {
    }

    public void OnHealthChanged(int currentHealth, int MaxHealth)
    {
    }

    public void OnHit()
    {
    }
}
