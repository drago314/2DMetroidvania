using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemeyWave : MonoBehaviour
{
    public abstract void SpawnWave();
    public abstract bool WaveComplete();
}
