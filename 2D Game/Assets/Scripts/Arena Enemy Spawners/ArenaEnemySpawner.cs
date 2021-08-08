using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemeyWave> waveList;

    private float currentWave = 0;
    private bool waveSpawned = false;

    private void Update()
    {
        if (currentWave == 0 && !waveSpawned)
        {
            waveList[0].SpawnWave();
            waveSpawned = true;
        }
        else if (currentWave == 0 && waveList[0].WaveComplete())
        {
            currentWave += 1;
            waveSpawned = false;
        }
        else if (currentWave == 1 && !waveSpawned)
        {
            waveList[0].SpawnWave();
            waveSpawned = true;
        }
        else if (currentWave == 1 && waveList[0].WaveComplete())
        {
            currentWave += 1;
            waveSpawned = false;
        }
    }
}
