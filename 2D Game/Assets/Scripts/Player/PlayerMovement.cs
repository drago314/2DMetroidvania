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
    [SerializeField] private ParachuteToggle parachute;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private float defaultGravity;
    private Vector2 leftJoystick;
    private bool hasControl;
    private bool jumpPressed;
    private bool jumpInputUsed;
    private bool isGrounded;
    private bool onLeftWall;
    private bool onRightWall;
    private int jumpCounter;
    private bool grabbingWall;
    private bool wasGrabbingWall;
    private float wallJumpTimer;
    private bool glidePressed;
    private bool wasGliding;

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
        CheckGrounded();
        CheckOnWall();
        CheckControl();
        CheckJump();

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

    private void CheckControl()
    {
        if (wallJumpTimer > 0)
        {
            hasControl = false;
            wallJumpTimer -= Time.deltaTime;
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

    private void CheckOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onLeftWall = raycastHitLeft.collider != null;
        onRightWall = raycastHitRight.collider != null;
    }

    private void CheckGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
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

    //displays text for debugging
    private void OnGUI()
    {
        GUI.Label(new Rect(1100, 10, 100, 100), "Jump Input Used: " + jumpInputUsed);
        //GUI.Label(new Rect(1200, 50, 100, 100), "is doing thing: " + isControlDashing);
    }

}
