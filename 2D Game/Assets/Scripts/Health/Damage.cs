using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage 
{
    public const int ENVIRONMENT = 0;
    public const int ENEMY = 1;
    public const int PLAYER_BASIC_ATTACK = 2;
    public const int PLAYER_ANIME_DASH = 3;
    public const int PLAYER_DAGGER_ATTACK = 4;

    public int damage;
    public GameObject source;
    public int damageType;
    public Vector2 knockbackDirection;

    /// <param name="damage">the amout of damage taken.</param>
    /// <param name="damageType">the type of damage, found in const variables in damage class.</param>
    public Damage(int damage, int damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    /// <param name="damage">the amout of damage taken.</param>
    /// <param name="source">the source gameObject of the damage.</param>
    /// <param name="damageType">the type of damage, found in const variables in damage class.</param>
    public Damage(int damage, GameObject source, int damageType)
    {
        this.damage = damage;
        this.source = source;
        this.damageType = damageType;
    }

    /// <param name="damage">the amout of damage taken.</param>
    /// <param name="knockbackDirection">the direction the enemy should take knockback.</param>
    /// <param name="damageType">the type of damage, found in const variables in damage class.</param>
    public Damage(int damage, int damageType, Vector2 knockbackDirection)
    {
        this.damage = damage;
        this.knockbackDirection = knockbackDirection;
        this.damageType = damageType;
    }
}
