using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysPatroller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distanceToGround;
    [SerializeField] private float distanceToWall;

    [SerializeField] Transform obstacleDetector;

    private Rigidbody2D body;

    private float movingRight = 1;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        body.velocity = new Vector2(movingRight * speed, 0);

        RaycastHit2D groundBelow = Physics2D.Raycast(obstacleDetector.position, Vector2.down, distanceToGround);
        RaycastHit2D wallNear = Physics2D.Raycast(obstacleDetector.position, Vector2.right, distanceToWall);

        if (!groundBelow.collider || (wallNear.collider && wallNear.collider.tag != "Player"))
        {
            if (movingRight == 1)
            {
                movingRight = -1;
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            else
            {
                movingRight = 1;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
