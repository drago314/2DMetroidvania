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
    [SerializeField] private float wallJumpCherryTime;

    [SerializeField] private float glideGravity;
    [SerializeField] private float glideFallSpeed;
    [SerializeField] private ParachuteToggle parachute;

    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    private PlayerActions playerActions;

    private Rigidbody2D body;
    private Animator anim;
    private LayerMask groundLayer;
    private LayerMask enemyLayer;
    private float defaultGravity;

    private bool jumpPressed;
    private bool jumpInputUsed;
    private int jumpCounter;

    private bool grabbingWall;
    private bool wasGrabbingWall = false;
    private float wallJumpTimer;
    private float wallJumpCherryTimer = 0;

    private bool glidePressed;
    private bool wasGliding;

    private bool dashPressed;
    private bool dashInputUsed;
    private bool canDash;
    private bool dashing;
    private float dashTimer;
    private float dashCooldownTimer = 0f;

    private void Start()
    {
        playerActions = gameObject.GetComponent<PlayerActions>();

        body = playerActions.body;
        anim = playerActions.anim;
        groundLayer = playerActions.GetGroundLayer();
        enemyLayer = playerActions.GetEnemyLayer();
        defaultGravity = playerActions.defaultGravity;
    }   

    public void Movement()
    {
        Move();
        Jump();
        WallMovement();
        Glide();
        Dash();
    }

    public bool CheckControl()
    {
        bool hasControl = false;

        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            hasControl = true;

            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;
        }

        return hasControl;
    }

    public void CheckMovement()
    {
        if (playerActions.isGrounded)
        {
            jumpCounter = 0;
            canDash = true;
        }
        else if (wasGrabbingWall)
        {
            jumpCounter = 1;
            canDash = true;
        }
    }

    public void AirResetMovement()
    {
        jumpCounter = 1;
        canDash = true;
    }

    private void Move()
    {
        body.velocity = new Vector2(playerActions.leftJoystick.x * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        //Regular Jump
        if (jumpCounter < 2 && jumpPressed && !jumpInputUsed && !grabbingWall && wallJumpCherryTimer <= 0)
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
        grabbingWall = (playerActions.onLeftWall && playerActions.leftJoystick.x <= 0) || (playerActions.onRightWall && playerActions.leftJoystick.x >= 0);
        if (grabbingWall)
        {
            wasGrabbingWall = true;
            body.gravityScale = 0;

            if (playerActions.leftJoystick.y > 0)
                body.velocity = new Vector2(body.velocity.x, climbSpeed);
            else if (playerActions.leftJoystick.y < 0)
                body.velocity = new Vector2(body.velocity.x, climbSpeed * -1);
            else
                body.velocity = Vector2.zero;

            if (!jumpInputUsed && jumpPressed)
            {
                int jumpDirection;
                if (playerActions.onLeftWall)
                    jumpDirection = 1;
                else
                    jumpDirection = -1;
                WallJump(jumpDirection);
            }
        }
        else if (wasGrabbingWall)
        {
            body.gravityScale = defaultGravity;
            wallJumpCherryTimer = wallJumpCherryTime;
            wasGrabbingWall = false;
        }

        if (wallJumpCherryTimer > 0)
        {
            if (playerActions.isGrounded)
            {
                wallJumpCherryTimer = 0;
            }
            else if (!jumpInputUsed && jumpPressed)
            {
                int jumpDirection;
                if (playerActions.leftJoystick.x > 0)
                    jumpDirection = 1;
                else
                    jumpDirection = -1;
                WallJump(jumpDirection);
            }
            wallJumpCherryTimer -= Time.deltaTime;
        }
    }

    private void WallJump(int jumpDirection)
    {
        wallJumpTimer = wallJumpTime;
        body.velocity = Vector2.zero;
        body.gravityScale = defaultGravity;
        body.velocity = new Vector2(jumpDirection * wallJumpSideForce, wallJumpUpForce);
        grabbingWall = false;
        wasGrabbingWall = false;
        jumpInputUsed = true;
    }

    private void Glide()
    {
        if (glidePressed && !playerActions.isGrounded && !playerActions.onLeftWall && !playerActions.onRightWall)
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

    private void Dash()
    {
        if (dashing)
        {
            dashCooldownTimer = dashCooldown;
            body.gravityScale = defaultGravity;
            dashing = false;
        }
        if (canDash && dashPressed && !dashInputUsed && dashCooldownTimer <= 0)
        {
            dashTimer = dashTime;
            body.gravityScale = 0;
            dashing = true;
            dashInputUsed = true;
            canDash = false;

            int dashDirection;
            if (playerActions.facingRight)
                dashDirection = 1;
            else
                dashDirection = -1;
            body.velocity = new Vector2(dashDirection * dashForce, 0);
        }
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

    private void OnDash(InputValue value)
    {
        dashPressed = value.isPressed;
        if (dashPressed)
        {
            dashInputUsed = false;
        }
    }

    //displays text for debugging
    private void OnGUI()
    {
        //GUI.Label(new Rect(1100, 10, 100, 100), "isGrounded: " + grabbingWall);
        //GUI.Label(new Rect(1200, 50, 100, 100), "onWall: " + (onLeftWall || onRightWall));
    }
}
