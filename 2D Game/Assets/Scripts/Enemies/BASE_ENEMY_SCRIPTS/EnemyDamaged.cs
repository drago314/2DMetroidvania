using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField] protected float iFrameDuration;
    [SerializeField] protected Animator animator;

    public bool isDamaged = false;

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
        isDamaged = true;
        animator.SetTrigger("OnDamage");
        iFrame.InvForTime(iFrameDuration);
        StartCoroutine(NotDamaged());
    }

    private IEnumerator NotDamaged()
    {
        yield return new WaitForSeconds(iFrameDuration);
        isDamaged = false;
    }

    protected void OnDeath(object sender, EventArgs e)
    {
        animator.SetTrigger("OnDeath");
        Destroy(gameObject);
    }
}
