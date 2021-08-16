using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class DeathMenu : Menu
{
    [SerializeField] private TMP_Text textShown;
    private Health health;

    public new void Start()
    {
        base.Start();
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        health.OnDeath += OnDeath;
    }

    private void OnDeath(object sender, EventArgs e)
    {
        Open(ArenaEnemySpawner.instance.score);
    }

    public void OnStart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
        base.Close();
    }

    public void Open(int score)
    {
        base.Open();
        Time.timeScale = 0;
        textShown.text = string.Format("You Scored {0} Points!", score);
    }
}
