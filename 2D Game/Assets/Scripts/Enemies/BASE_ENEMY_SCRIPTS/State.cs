using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IEnemyState
{
    public IEnemyState DoState(Enemy enemy)
    {
        return this;
    }
}   
