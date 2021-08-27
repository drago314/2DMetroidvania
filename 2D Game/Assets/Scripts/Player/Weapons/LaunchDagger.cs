using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDagger : MonoBehaviour
{
    public Vector2 flyDirection { get; private set; }

    public bool flyingTowardsEnemy = true;
    public float flybackTimer = 0;

    [SerializeField] private Rigidbody2D body;

    private void Start()
    {
        body.gravityScale = 0;
        body.velocity = flyDirection;
        flyingTowardsEnemy = true;
    }

    private void Update()
    {
        if (flyingTowardsEnemy)
        {
            body.velocity = flyDirection;
        }
        else if (flybackTimer > 0)
        {
            flybackTimer -= Time.deltaTime;
        }
        else
        {
            Vector2 direction = PlayerActions.player.transform.position - transform.position;
            direction.Normalize();
            body.velocity = direction * PlayerActions.player.playerAttack.daggerFlybackSpeed;
        }
    }

    public void SetFlyDirection(Vector2 flyDirection)
    {
        this.flyDirection = flyDirection;
        if (flyDirection.x >= 0)
        {
            float zRotation = Mathf.Atan2(flyDirection.y, flyDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, zRotation - 45);
        }
        else
        {
            float zRotation = Mathf.Atan2(flyDirection.y, flyDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(180, 0, zRotation - 45);
        }
    }

    public void ResetTimer()
    {
        flybackTimer = PlayerActions.player.playerAttack.timeBeforeFlyback;
        body.velocity = Vector2.zero;
    }

    public void DeleteDagger()
    {
        PlayerActions.player.playerAttack.AddDagger();
        Destroy(gameObject);
    }
}
