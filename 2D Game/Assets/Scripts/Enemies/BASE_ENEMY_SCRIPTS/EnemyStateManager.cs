using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : Enemy
{
    protected IEnemyState currentState;

    protected State baseState;

    protected void OnEnable()
    {
        currentState = baseState;
    }

    protected void Update()
    {
        currentState = currentState.DoState(this);
    }
}
