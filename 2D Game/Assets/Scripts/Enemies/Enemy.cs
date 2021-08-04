using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Health health;

    protected void Start()
    {
        health = gameObject.GetComponent<Health>();
    }
}
