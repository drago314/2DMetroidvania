using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingBehaviour : StateMachineBehaviour
{
    [SerializeField] private float maxRoamLength;
    [SerializeField] private float minRoamLength;
    [SerializeField] private float distanceToWall;

    private Rigidbody2D body;
    private Vector2 randomSpot;
    private Transform obstacleDetector;
    private float speed;
    private int direction = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        body = animator.GetComponent<Rigidbody2D>();
        speed = animator.GetComponent<FlyingTowardsPlayer>().GetSpeed();
        obstacleDetector = animator.GetComponent<FlyingTowardsPlayer>().GetObstacleDetector();

        float xPos = Random.Range(minRoamLength, maxRoamLength);
        randomSpot = new Vector2(xPos + animator.transform.position.x, animator.transform.position.y);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RaycastHit2D wallNear = Physics2D.Raycast(obstacleDetector.position, Vector2.right, distanceToWall);
        if (Vector2.Distance(animator.transform.position, randomSpot) >= 0.2f && !(wallNear && wallNear.collider.tag == "Platforms"))
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, randomSpot, speed * Time.deltaTime);
        else
        {
            float xPos = Random.Range(minRoamLength, maxRoamLength) * -direction;
            randomSpot = new Vector2(xPos + animator.transform.position.x, animator.transform.position.y);
            if (direction == 1)
                animator.transform.eulerAngles = new Vector3(0, -180, 0);
            else
                animator.transform.eulerAngles = new Vector3(0, 0, 0);
            direction = -direction;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
