using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField] protected float iFrameDuration;

    private InvFrame iFrame;

    protected void Start()
    {
        iFrame = gameObject.GetComponent<InvFrame>();

        Health health = gameObject.GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += OnDeath;
    }

    protected void OnHit(object sender, EventArgs e)
    {
        iFrame.InvForTime(iFrameDuration);
    }

    protected void OnDeath(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
