using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private float glideGravity;
    [SerializeField] private float glideFallSpeed;
    [SerializeField] private float controlDashTime;
    [SerializeField] private float controlDashSideSpeed;
    [SerializeField] private float controlDashUpSpeed;
    [SerializeField] private ParachuteToggle parachute;
    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;

    private float defaultGravity;
    private Vector2 leftJoystick;
    private bool hasControl;

    private bool isGrounded;
    private bool onLeftWall;
    private bool onRightWall;

    private bool jumpPressed;
    private bool jumpInputUsed;
    private int jumpCounter;

    private bool grabbingWall;
    private bool wasGrabbingWall;
    private float wallJumpTimer;

    private bool glidePressed;
    private bool wasGliding;

    private bool controlDashPressed;
    private bool controlDashing;
    private bool wasControlDashing;
    private bool canControlDash;
    private float controlDashTimer;

    private void Awake()
    {
        //Grab references 
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();

        defaultGravity = body.gravityScale;
    }

    private void Update()
    {
        CheckControl();
        CheckJump();
        CheckControlDash();
        CheckGrounded();
        CheckOnWall();

        //Flipping Character Model
        if (leftJoystick.x > 0.01f)
            transform.localScale = new Vector2(1, 1);
        else if (leftJoystick.x < -0.01f)
            transform.localScale = new Vector2(-1, 1);

        if (hasControl)
        {
            Move();
            Jump();
            WallMovement();
            Glide();
            ControlDash();
        }
    }

    private void Move()
    {
        body.velocity = new Vector2(leftJoystick.x * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        //Regular Jump
        if (jumpCounter < 2 && !jumpInputUsed && !grabbingWall)
        {
            Invoke("AddJump", 0.1f);
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            //anim.SetTrigger("Jump");
            jumpInputUsed = true;
            parachute.Close();
        }
        //Making holding jump jump higher, may need rewrite since I think it effects every time you fall.
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !jumpPressed)
        {
            body.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void AddJump()
    {
        jumpCounter += 1;
    }

    private void WallMovement()
    {
        grabbingWall = (onLeftWall && leftJoystick.x <= 0) || (onRightWall && leftJoystick.x >= 0);
        if (grabbingWall)
        {
            wasGrabbingWall = true;
            body.gravityScale = 0;

            if(leftJoystick.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, climbSpeed);
            }
            else if(leftJoystick.y < 0)
            {
                body.velocity = new Vector2(body.velocity.x, climbSpeed * -1);
            }
            else
            {
                body.velocity = Vector2.zero;
            }

            if (!jumpInputUsed)
            {
                wallJumpTimer = wallJumpTime;
                body.velocity = Vector2.zero;
                body.gravityScale = defaultGravity;
                int jumpDirection;
                if (onLeftWall)
                {
                    jumpDirection = 1;
                }
                else
                {
                    jumpDirection = -1;
                }
                body.velocity = new Vector2(jumpDirection * wallJumpSideForce, wallJumpUpForce);
                grabbingWall = false;
                wasGrabbingWall = false;
                jumpInputUsed = true;
            }
        }
        else if (wasGrabbingWall)
        {
            body.gravityScale = defaultGravity;
            wasGrabbingWall = false;
        }
    }

    private void Glide()
    {
        if (glidePressed && !isGrounded && !onLeftWall && !onRightWall)
        {
            wasGliding = true;
            parachute.Open();
            body.velocity = new Vector2(body.velocity.x, -1 * glideFallSpeed);
            body.gravityScale = glideGravity;
        }
        else if (wasGliding)
        {
            body.gravityScale = defaultGravity;
            wasGliding = false;
            parachute.Close();
        }
    }

    private void ControlDash()
    {
        if (canControlDash && controlDashPressed)
        {
            if (wasControlDashing && controlDashTimer > 0)
            {
                body.velocity = new Vector2(leftJoystick.x * controlDashSideSpeed, leftJoystick.y * controlDashUpSpeed);
            }
            else if (!wasControlDashing)
            {
                body.gravityScale = 0;
                controlDashTimer = controlDashTime;
                wasControlDashing = true;
                body.velocity = new Vector2(leftJoystick.x * controlDashSideSpeed, leftJoystick.y * controlDashUpSpeed);
            }
        }
        else if (wasControlDashing)
        {
            wasControlDashing = false;
            canControlDash = false; 
            body.gravityScale = defaultGravity;
        }
    }

    private void CheckControl()
    {
        hasControl = false;
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if(controlDashTimer > 0)
        {
            ControlDash();
            controlDashTimer -= Time.deltaTime;
        }
        else 
        {
            hasControl = true;
        }
    }

    private void CheckJump()
    {
        if (isGrounded)
        {
            jumpCounter = 0;
        }
        else if (wasGrabbingWall)
        {
            jumpCounter = 1;
        }
    }

    private void CheckControlDash()
    {
        //This is where you would add picking up a collectible to activate control dash
        if (isGrounded || grabbingWall)
            canControlDash = true;
    }

    private void CheckGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
    }

    private void CheckOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onLeftWall = raycastHitLeft.collider != null;
        onRightWall = raycastHitRight.collider != null;
    }

    private void OnMove(InputValue value)
    {
        leftJoystick = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
        if (jumpPressed)
        {
            jumpInputUsed = false;
        }
    }

    private void OnGlide(InputValue value)
    {
        glidePressed = value.isPressed;
    }

    private void OnControlDash(InputValue value)
    {
        controlDashPressed = value.isPressed;
    }

    //displays text for debugging
    private void OnGUI()
    {
        GUI.Label(new Rect(1100, 10, 100, 100), "Jump Input Used: " + jumpInputUsed);
        //GUI.Label(new Rect(1200, 50, 100, 100), "is doing thing: " + isControlDashing);
    }

}
