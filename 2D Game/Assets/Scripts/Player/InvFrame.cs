using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvFrame : MonoBehaviour, IHealthCallback
{
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Health health;

    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        health.SetCallbackListener(this);
    }

    public void OnDeath(Damage damage)
    {}

    public void OnHeal()
    {}

    public void OnHealthChanged(Health health)
    {}

    public void OnHit(Damage damage)
    {
        InvForTime(iFrameDuration);
    }

    public void InvForTime(float duration)
    {
        int numFlashes = (int) (numberOfFlashes / iFrameDuration * duration);
        StartCoroutine(Invulnerability(duration, numFlashes, spriteRend));
    }

    public void Invincible(bool inv)
    {
        if (inv)
            Physics2D.IgnoreLayerCollision(10, 9, true);
        else
            Physics2D.IgnoreLayerCollision(10, 9, false);
    }

    public IEnumerator Invulnerability(float time, int amountOfFlashes, SpriteRenderer spriteRend)
    {
        Physics2D.IgnoreLayerCollision(10, 9, true);
        for (int i = 0; i < amountOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(time / (amountOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(time / (amountOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 9, false);
    }
}
