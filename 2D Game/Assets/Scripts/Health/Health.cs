using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
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
            listener.OnHealthChanged(this);
        }
    }

    public void SetCallbackListener(IHealthCallback listener)
    {
        this.listeners.Add(listener);
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
            foreach (IHealthCallback listener in listeners)
            {
                listener.OnDeath(damage);
                listener.OnHealthChanged(this);
            }
        }
        else
        {
            foreach (IHealthCallback listener in listeners)
            {
                listener.OnHit(damage);
                listener.OnHealthChanged(this);
            }
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
                listener.OnHealthChanged(this);
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
                listener.OnHealthChanged(this);
            }
        }
    }
}