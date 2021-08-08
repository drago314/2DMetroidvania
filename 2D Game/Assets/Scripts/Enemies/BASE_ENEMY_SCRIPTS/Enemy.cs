using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int hitsUntilShadow;
    private Health health;
    private EnemyDamaged enemyDamaged;
    private EnemySpawn enemySpawn;

    protected void Start()
    {
        health = gameObject.GetComponent<Health>();
        enemyDamaged = gameObject.GetComponent<EnemyDamaged>();
        enemySpawn = gameObject.GetComponent<EnemySpawn>();
    }

    public bool IsShadow()
    {
        return hitsUntilShadow <= 0;
    }
}
