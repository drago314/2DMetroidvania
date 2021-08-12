using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    IEnemyState DoState(Enemy enemy, EnemyStateManager enemyStateManager, PlayerActions player);
}
