using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIFrames : MonoBehaviour, IHealthCallback
{
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Health health;
    [SerializeField] private InvFrame iFrame;

    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        health.SetCallbackListener(this);
    }

    public void OnDeath()
    {

    }

    public void OnHeal()
    {

    }

    public void OnHealthChanged(int currentHealth, int MaxHealth)
    {

    }

    public void OnHit()
    {
        iFrame.HaveInvFrame(iFrameDuration, numberOfFlashes, spriteRend);
    }
}
