using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysPatroller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distanceToGround;
    [SerializeField] private float distanceToWall;
    [SerializeField] private int damage;
    [SerializeField] private Transform obstacleDetector;

    private float movingRight = 1;

    private void Update()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + movingRight * speed * Time.deltaTime, gameObject.transform.position.y);

        RaycastHit2D groundBelow = Physics2D.Raycast(obstacleDetector.position, Vector2.down, distanceToGround);
        RaycastHit2D wallNear = Physics2D.Raycast(obstacleDetector.position, Vector2.right, distanceToWall);

        if (!groundBelow.collider || (wallNear.collider && wallNear.collider.tag == "Platforms"))
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(damage);
        }
    }
}
