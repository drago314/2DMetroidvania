using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage 
{
    public const int ENVIRONMENT = 0;
    public const int ENEMY = 1;
    public const int PLAYER = 2;

    public int damage;
    public GameObject source;
    public int damageType;

    public Damage(int d, int dt)
    {
        damage = d;
        source = null;
        damageType = dt;
    }

    public Damage(int d, GameObject s, int dt)
    {
        damage = d;
        source = s;
        damageType = dt;
    }
}
