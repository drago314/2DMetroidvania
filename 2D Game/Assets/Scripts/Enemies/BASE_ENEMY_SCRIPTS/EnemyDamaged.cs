using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField] protected float iFrameDuration;
    [SerializeField] protected Animator animator;

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
        animator.SetTrigger("OnDamage");
        iFrame.InvForTime(iFrameDuration);
    }

    protected void OnDeath(object sender, EventArgs e)
    {
        animator.SetTrigger("OnDeath");
        Destroy(gameObject);
    }
}
