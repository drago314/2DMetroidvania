using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int hitsUntilShadow;
    private Health health;

    protected void Start()
    {
        health = gameObject.GetComponent<Health>();
    }

    public bool IsShadow()
    {
        return hitsUntilShadow <= 0;
    }
}
