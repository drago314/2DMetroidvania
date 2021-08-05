using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private bool isDead;
    private const int MIN_HEALTH = 0;

    public event EventHandler<OnHitEventArg> OnHit;
    public event EventHandler OnDeath;
    public event EventHandler OnHeal;
    public event EventHandler OnHealthChanged;

    public class OnHitEventArg : EventArgs
    {
        public Damage damage;
    }

    private void Awake()
    {
        if (this.currentHealth > this.maxHealth)
        {
            this.currentHealth = maxHealth;
        }
    }

    /// <returns>A boolean of the current deathstate.</returns>
    public bool IsDead()
    {
        return this.isDead;
    }

    /// <returns>A float representing the current health.</returns>
    public float GetHealth()
    {
        return this.currentHealth;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    /// <param name="value">the new max health amount.</param>
    public void SetMaxHealth(int value)
    {
        if (this.currentHealth > value)
        {
            this.maxHealth = value;
            this.currentHealth = value;
        }
    }

    public void Damage(Damage damage)
    {
        this.currentHealth = Mathf.Clamp(currentHealth - damage.damage, MIN_HEALTH, this.maxHealth);
        if (currentHealth <= MIN_HEALTH)
        {
            this.isDead = true;
            this.currentHealth = MIN_HEALTH;

            OnDeath?.Invoke(this, EventArgs.Empty);
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnHit?.Invoke(this, new OnHitEventArg { damage = damage});
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <param name="_heal">the heal amount.</param>
    public void Heal(int heal)
    {
        if (currentHealth != MIN_HEALTH && !isDead)
        {
            this.currentHealth = Mathf.Clamp(currentHealth + heal, MIN_HEALTH, this.maxHealth);
            OnHeal?.Invoke(this, EventArgs.Empty);
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    /// <param name="_heal">the heal amount.</param>
    /// <param name="_revive">Defines if the heal should be able to revive.</param>
    public void Heal(int heal, bool revive)
    {
        this.currentHealth = Mathf.Clamp(currentHealth + heal, MIN_HEALTH, this.maxHealth);
        if (currentHealth != MIN_HEALTH && revive)
        {
            this.isDead = false;
            OnHeal?.Invoke(this, EventArgs.Empty);
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}