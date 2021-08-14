using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTowardsPlayerEnemy : Enemy
{
    [SerializeField] private int damage;
    [SerializeField] private EnemyStateManager enemyStateManager;
    
    private new void Start()
    {
        base.Start();
        FlyingTowardsPlayerState testState = (FlyingTowardsPlayerState)enemyStateManager.stateDict["TargetingPlayer"];
        testState.onFlyTowardsPlayer += FlyTowardsPlayer;
    }

    private void Update()
    {
        enemyStateManager.CallState(this, FindObjectOfType<PlayerActions>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(new Damage(damage, this.gameObject, Damage.ENEMY));
        }
    }

    private void FlyTowardsPlayer(object sender, EventArgs e)
    {
        if (!enemyDamaged.isDamaged)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerActions.transform.position, speed * (Time.deltaTime));
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
