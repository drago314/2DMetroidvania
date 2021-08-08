using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArenaWave : EnemeyWave
{
    [SerializeField] private GameObject baseEnemy;

    private int enemiesAlive = 0;

    public override void SpawnWave()
    {
        SpawnEnemy(baseEnemy);
    }

    public override bool WaveComplete()
    {
        return enemiesAlive <= 0;
    }

    protected void SpawnEnemy(GameObject enemy)
    {
        GameObject enemySpawned = Instantiate(enemy, Vector3.zero, Quaternion.identity);
        Health enemyHealth = enemySpawned.GetComponent<Health>();
        enemyHealth.OnDeath += OnEnemyDeath;
        enemiesAlive += 1;
    }

    private void OnEnemyDeath(object sender, EventArgs e)
    {
        enemiesAlive -= 1;
    }
}
