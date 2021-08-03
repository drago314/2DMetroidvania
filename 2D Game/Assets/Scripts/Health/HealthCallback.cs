using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthCallback
{
    void OnHit(Damage damage);
    void OnDeath(Damage damage);
    void OnHeal();
    void OnHealthChanged(Health health);
}
