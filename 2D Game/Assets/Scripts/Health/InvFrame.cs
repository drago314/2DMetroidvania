using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvFrame : MonoBehaviour
{
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;

    private SpriteRenderer spriteRend;

    private void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
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
        Invincible(true);
        for (int i = 0; i < amountOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(time / (amountOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(time / (amountOfFlashes * 2));
        }
        Invincible(false);
    }
}
