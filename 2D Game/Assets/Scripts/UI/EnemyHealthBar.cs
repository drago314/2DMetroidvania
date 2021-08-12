using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(
    typeof(Health)
)]
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private float damageSpeed;
    [SerializeField] private Health health;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;

    private float lerpTimer;

    private void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.OnHit += TakeDamage;
        health.OnDeath += TakeDamage;
    }

    private void Update()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health.GetHealth() / health.GetMaxHealth();
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / damageSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
    }

    // Update is called once per frame
    private void TakeDamage(object source, EventArgs e)
    {
        lerpTimer = 0;
    }
}
