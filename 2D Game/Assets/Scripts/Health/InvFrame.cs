using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvFrame : MonoBehaviour
{
    public void InvForTime(float duration)
    {
        StartCoroutine(Invulnerability(duration));
    }

    public void Invincible(bool inv)
    {
        if (inv)
            Physics2D.IgnoreLayerCollision(10, 9, true);
        else
            Physics2D.IgnoreLayerCollision(10, 9, false);
    }

    public IEnumerator Invulnerability(float time)
    {
        Invincible(true);
        yield return new WaitForSeconds(time);
        Invincible(false);
    }
}
