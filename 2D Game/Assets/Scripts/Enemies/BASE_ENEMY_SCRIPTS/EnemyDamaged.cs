using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    protected void Start()
    {
        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

    protected virtual void OnHit(object sender, EventArgs e)
    {
    }

    protected virtual void OnDeath(object sender, EventArgs e)
    {
    }
}
