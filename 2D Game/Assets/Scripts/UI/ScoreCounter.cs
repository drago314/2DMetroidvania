using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreCounter;
    private ArenaEnemySpawner aES;

    void Start()
    {
        aES = ArenaEnemySpawner.instance;
        aES.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(object sender, ArenaEnemySpawner.OnScoreChangeEventHandler e)
    {
        scoreCounter.text = string.Format("Score: {0}", e.score);
    }
}
