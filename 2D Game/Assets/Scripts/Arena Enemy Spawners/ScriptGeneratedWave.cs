using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGeneratedWave : EnemeyWave
{
    [SerializeField] private List<GameObject> enemyList;

    private int enemiesAlive = 0;
    private System.Random random = new System.Random();

    public override void SpawnWave()
    {
        SpawnWave(1, 2);
    }

    public void SpawnWave(int amountOfEnemies, float enemySpeed)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            var enemyArray = enemyList.ToArray();
            SpawnEnemy(enemyArray[random.Next(0, enemyArray.Length)], enemySpeed);
        }
    }

    public override bool WaveComplete()
    {
        return enemiesAlive <= 0;
    }

    protected void SpawnEnemy(GameObject enemy, float enemySpeed)
    {
        Vector3 position = new Vector3(random.Next(-11, 9), random.Next(-1, 3), 0);
        GameObject enemySpawned = Instantiate(enemy, position, Quaternion.identity);
        Health enemyHealth = enemySpawned.GetComponent<Health>();
        Enemy enemyEnemy = enemySpawned.GetComponent<Enemy>();
        enemyEnemy.speed = enemySpeed;
        enemyHealth.OnDeath += OnEnemyDeath;
        enemiesAlive += 1;
    }

    private void OnEnemyDeath(object sender, EventArgs e)
    {
        enemiesAlive -= 1;
    }
}
