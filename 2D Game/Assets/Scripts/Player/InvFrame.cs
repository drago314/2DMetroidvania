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

    public void OnDeath()
    {}

    public void OnHeal()
    {}

    public void OnHealthChanged(int currentHealth, int MaxHealth)
    {}

    public void OnHit()
    {
        HaveInvFrame();
    }

    public void HaveInvFrame()
    {
        StartCoroutine(Invulnerability(iFrameDuration, numberOfFlashes, spriteRend));
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
