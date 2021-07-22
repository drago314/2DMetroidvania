using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingBehaviour : StateMachineBehaviour
{
    [SerializeField] private float maxRoamLength;
    [SerializeField] private float minRoamLength;

    private Rigidbody2D body;
    private Vector2 randomSpot;
    private float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        body = animator.GetComponent<Rigidbody2D>();
        speed = animator.GetComponent<FlyingTowardsPlayer>().GetSpeed();

        float xPos = Random.Range(-maxRoamLength, maxRoamLength);
        if (Mathf.Abs(xPos) < minRoamLength)
            xPos = minRoamLength * Mathf.Sign(xPos);
        randomSpot = new Vector2(xPos + animator.transform.position.x, animator.transform.position.y);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, randomSpot) >= 0.2f)
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, randomSpot, speed * Time.deltaTime);
        else
        {
            float xPos = Random.Range(-maxRoamLength, maxRoamLength);
            if (Mathf.Abs(xPos) < minRoamLength)
                xPos = minRoamLength * Mathf.Sign(xPos);
            randomSpot = new Vector2(xPos + animator.transform.position.x, animator.transform.position.y);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
