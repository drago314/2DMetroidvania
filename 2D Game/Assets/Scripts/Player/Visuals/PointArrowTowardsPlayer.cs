using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointArrowTowardsPlayer : MonoBehaviour
{
    private bool pointing;
    GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerActions>().gameObject;
    }

    private void Update()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 currentPos = transform.position;
        float pointDirection = Trigonometry.RadianFromPosition(playerPosition.x - currentPos.x, playerPosition.y - currentPos.y);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Trigonometry.RadianToDegree(pointDirection) + 180);
    }

    public void StopPointingToPlayer()
    {
        Destroy(gameObject);
    }
}
