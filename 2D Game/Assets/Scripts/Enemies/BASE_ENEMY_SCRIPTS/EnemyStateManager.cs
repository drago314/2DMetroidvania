using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] protected IEnemyState baseState;
    protected IEnemyState currentState = null;

    public Dictionary<string, IEnemyState> stateDict = new Dictionary<string, IEnemyState>();

    public void OnEnable()
    {
        currentState = baseState;
    }

    public void CallState(Enemy enemy, PlayerActions playerActions)
    {
        if (currentState != null)
            currentState = currentState.DoState(enemy, this, playerActions);
        else
            currentState = baseState.DoState(enemy, this, playerActions);
    }
}
