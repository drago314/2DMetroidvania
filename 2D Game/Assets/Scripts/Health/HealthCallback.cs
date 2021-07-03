using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthCallback
{
    void OnHit();
    void OnDeath();
    void OnHeal();
    void OnHealthChanged(int currentHealth, int MaxHealth);
}
