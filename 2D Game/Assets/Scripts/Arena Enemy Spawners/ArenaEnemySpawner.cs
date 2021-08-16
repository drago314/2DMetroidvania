using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArenaEnemySpawner : MonoBehaviour
{
    public static ArenaEnemySpawner instance;

    [SerializeField] private float enemiesAddedPerWave;
    [SerializeField] private float enemySpeedAddedPerWave;
    [SerializeField] private EnemeyWave[] waveArray;

    public event EventHandler<OnScoreChangeEventHandler> OnScoreChange;
    public int score { get; private set; }

    public class OnScoreChangeEventHandler : EventArgs
    {
        public int score;
    }

    private float currentEnemies = 1;
    private float currentEnemySpeed = 2;
    private bool waveSpawned = false;
    private int currentWave;
    private ScriptGeneratedWave currentGeneratedWave;

    private void Start()
    {
        instance ??= this;
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
                score += 1;
                OnScoreChange?.Invoke(this, new OnScoreChangeEventHandler { score = score });
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
            score += 1;
            OnScoreChange?.Invoke(this, new OnScoreChangeEventHandler { score = score });
        }
    }
}   
