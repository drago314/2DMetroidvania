using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        health.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(object sender, EventArgs e)
    {
        currentHealthBar.fillAmount = health.GetHealth() / 5f;
        totalHealthBar.fillAmount = health.GetMaxHealth() / 5f; 
    }
}

