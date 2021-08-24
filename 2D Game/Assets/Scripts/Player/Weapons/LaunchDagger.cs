using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDagger : MonoBehaviour
{
    public Vector2 flyDirection { get; private set; }

    [SerializeField] private Rigidbody2D body;

    private void Start()
    {
        body.gravityScale = 0;
        body.velocity = flyDirection;
    }

    private void Update()
    {
        body.velocity = flyDirection;
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
}
