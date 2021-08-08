using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private bool spawnOnSceneLoad;
    [SerializeField] private float spawnIFrameTime;

    void Start()
    {
        if (!spawnOnSceneLoad)
            gameObject.GetComponent<InvFrame>().InvForTime(spawnIFrameTime);
    }
}
