using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D Player;
    public float maxRightCord;
    public float maxLeftCord;
    float dirX, moveSpeed = 3f;
    public bool moveRight = true;
    public bool moveLeft = false;
    public bool PlayerIsOnPlatform;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "MovingSurfaceCollider")
        {
            PlayerIsOnPlatform = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= maxRightCord)
        {
            moveRight = false;
        }

        if (transform.position.x >= maxLeftCord)
        {
            moveRight = true;
        }

        if (moveRight == false)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);

            //if (PlayerIsOnPlatform == true)
            //{
            //    Player.AddForce(new Vector2(-moveSpeed, 0) ,ForceMode2D.Impulse);
            //}
        }
        if (moveRight == true)
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);

            //if (PlayerIsOnPlatform == true)
            //{
            //    Player.AddForce(new Vector2(moveSpeed, 0) ,ForceMode2D.Impulse);
            //}
        }
    }
}
