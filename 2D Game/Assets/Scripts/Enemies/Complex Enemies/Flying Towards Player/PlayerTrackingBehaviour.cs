using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackingBehaviour : StateMachineBehaviour
{
    private float viewRadius;
    private float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        viewRadius = animator.GetComponent<FlyingTowardsPlayer>().GetViewRadius();
        speed = animator.GetComponent<FlyingTowardsPlayer>().GetSpeed();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector2.Distance(animator.transform.position, FindObjectOfType<PlayerMovement>().transform.position);
        if (distance > viewRadius)
        {
            animator.SetBool("PlayerInSight", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
