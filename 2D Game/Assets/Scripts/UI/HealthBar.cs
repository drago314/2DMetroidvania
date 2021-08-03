using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IHealthCallback
{
    [SerializeField] private Health health;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Awake()
    {
        health.SetCallbackListener(this);
    }

    public void OnDeath(Damage damage)
    {
    }

    public void OnHeal()
    {
    }

    public void OnHit(Damage damage)
    {
    }

    public void OnHealthChanged(Health health)
    {
        currentHealthBar.fillAmount = health.GetHealth() / 5f;
        totalHealthBar.fillAmount = health.GetMaxHealth() / 5f; 
    }
}

