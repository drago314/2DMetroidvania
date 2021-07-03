using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private bool isHit;
    private bool isDead;
    private const int MIN_HEALTH = 0;
    private ArrayList listeners = new ArrayList();

    private void Awake()
    {
        if (this.currentHealth > this.maxHealth)
        {
            this.currentHealth = maxHealth;
        }
    }

    private void Start()
    {
        foreach (IHealthCallback listener in listeners)
        {
            listener.OnHealthChanged(currentHealth, maxHealth);
        }
    }

    public void SetCallbackListener(IHealthCallback listener)
    {
        this.listeners.Add(listener);
    }
    
    public bool IsHit()
    {
        return this.isHit;
    }

    public void SetIsHit(bool isHit)
    {
        this.isHit = isHit;
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

    /// <param name="value">the new max health amount.</param>
    public void SetMaxHealth(int value)
    {
        if (this.currentHealth > value)
        {
            this.maxHealth = value;
            this.currentHealth = value;
        }
    }

    /// <param name="damage">the damage amount.</param>
    public void Damage(int damage)
    {
        this.currentHealth = Mathf.Clamp(currentHealth - damage, MIN_HEALTH, this.maxHealth);
        if (currentHealth <= MIN_HEALTH)
        {
            this.isDead = true;
            this.currentHealth = MIN_HEALTH;
            foreach (IHealthCallback listener in listeners)
            {
                listener.OnDeath();
                listener.OnHealthChanged(currentHealth, maxHealth);
            }
        }
        foreach (IHealthCallback listener in listeners)
        {
            listener.OnHit();
            listener.OnHealthChanged(currentHealth, maxHealth);
        }
    }

    /// <param name="_heal">the heal amount.</param>
    public void Heal(int heal)
    {
        if (currentHealth != MIN_HEALTH && !isDead)
        {
            this.currentHealth = Mathf.Clamp(currentHealth + heal, MIN_HEALTH, this.maxHealth);
            foreach (IHealthCallback listener in listeners)
            {
                listener.OnHeal();
                listener.OnHealthChanged(currentHealth, maxHealth);
            }
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
            foreach (IHealthCallback listener in listeners)
            {
                listener.OnHeal();
                listener.OnHealthChanged(currentHealth, maxHealth);
            }
        }
    }
}