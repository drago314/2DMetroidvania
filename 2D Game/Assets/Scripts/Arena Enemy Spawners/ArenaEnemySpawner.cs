using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    [SerializeField] private float enemiesAddedPerWave;
    [SerializeField] private float enemySpeedAddedPerWave;
    [SerializeField] private EnemeyWave[] waveArray;

    private float currentEnemies = 1;
    private float currentEnemySpeed = 2;
    private bool waveSpawned = false;
    private int currentWave;
    private ScriptGeneratedWave currentGeneratedWave;

    private void Start()
    {
        currentGeneratedWave = gameObject.GetComponent<ScriptGeneratedWave>();
    }

    private void Update()
    {
        if (currentWave >= waveArray.Length && !waveSpawned)
        {
            currentGeneratedWave.SpawnWave((int) currentEnemies, currentEnemySpeed);
            waveSpawned = true;
        }
        else if(currentWave >= waveArray.Length)
        {
            if (currentGeneratedWave.WaveComplete())
            {
                waveSpawned = false;
                currentEnemies += enemiesAddedPerWave;
                currentEnemySpeed += enemySpeedAddedPerWave;
            }
        }
        else if (!waveSpawned)
        {
            waveArray[currentWave].SpawnWave();
            waveSpawned = true;
        }
        else if (waveArray[currentWave].WaveComplete())
        {
            waveSpawned = false;
            currentWave += 1;
        }
    }
}   
