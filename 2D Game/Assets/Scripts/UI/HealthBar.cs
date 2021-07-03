using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IHealthCallback
{
    [SerializeField] private Health health;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        health.SetCallbackListener(this);
    }

    public void OnDeath()
    {
    }

    public void OnHeal()
    {
    }

    public void OnHit()
    {
    }

    public void OnHealthChanged(int currentHealth, int MaxHealth)
    {
        currentHealthBar.fillAmount = currentHealth / 5f;
        totalHealthBar.fillAmount = MaxHealth / 5f; 
    }
}

