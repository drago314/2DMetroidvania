using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int hitsUntilShadow;
    public Health health;
    public EnemyDamaged enemyDamaged;
    public EnemySpawn enemySpawn;
    public PlayerActions playerActions;

    protected void Start()
    {
        playerActions = FindObjectOfType<PlayerActions>();
        health = gameObject.GetComponent<Health>();
        enemyDamaged = gameObject.GetComponent<EnemyDamaged>();
        enemySpawn = gameObject.GetComponent<EnemySpawn>();
    }

    public bool IsShadow()
    {
        return hitsUntilShadow <= 0;
    }
}
