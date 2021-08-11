using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTowardsPlayerStateManager : EnemyStateManager
{
    [SerializeField] private float viewRadius;

    public void Awake()
    {
        stateDict.Add("Idle", new IdleState(viewRadius));
        stateDict.Add("TargetingPlayer", new FlyingTowardsPlayerState(viewRadius));
        baseState = stateDict["Idle"];
    }
}
