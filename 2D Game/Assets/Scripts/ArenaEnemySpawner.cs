using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemy;

    private bool hasSpawned;

    private void Update()
    {
        if (!hasSpawned)
            Instantiate(basicEnemy, Vector3.zero, Quaternion.identity);
        hasSpawned = true;  
    }
}
