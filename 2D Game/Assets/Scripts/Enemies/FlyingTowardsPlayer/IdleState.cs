using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private float viewRadius;

    public IdleState(float viewRadius)
    {
        this.viewRadius = viewRadius;
    }

    public IEnemyState DoState(Enemy enemy, EnemyStateManager stateManager, PlayerActions playerActions)
    {
        float distance = Vector2.Distance(enemy.transform.position, playerActions.transform.position);
        if (distance < viewRadius)
            return stateManager.stateDict["TargetingPlayer"];
        else
            return stateManager.stateDict["Idle"];
    }
}
