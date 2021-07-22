using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvFrame : MonoBehaviour
{
    public void HaveInvFrame(float time, int amountOfFlashes, SpriteRenderer spriteRend)
    {
        StartCoroutine(Invulnerability(time, amountOfFlashes, spriteRend));
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
