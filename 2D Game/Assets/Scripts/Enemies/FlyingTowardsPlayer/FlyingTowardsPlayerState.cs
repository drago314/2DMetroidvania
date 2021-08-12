using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTowardsPlayerState : IEnemyState
{
    public event EventHandler onFlyTowardsPlayer;

    private float viewRadius;

    public FlyingTowardsPlayerState(float viewRadius)
    {
        this.viewRadius = viewRadius;
    }

    public IEnemyState DoState(Enemy enemy, EnemyStateManager stateManager, PlayerActions playerActions)
    {
        onFlyTowardsPlayer?.Invoke(this, EventArgs.Empty);

        float distance = Vector2.Distance(enemy.transform.position, playerActions.transform.position);
        if (distance > viewRadius)
            return stateManager.stateDict["TargetingPlayer"];
        else
            return stateManager.stateDict["Idle"];
    }
}
