using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IWet
{
    [SerializeField] private float amountOfJumps;
    [SerializeField] private bool hasWallJump;
    [SerializeField] private bool hasGlide;
    [SerializeField] private bool hasControlDash;
    [SerializeField] private bool hasDash;

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
    [SerializeField] private float controlDashSmoothing;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashStartup;

    [SerializeField] private float swimUpSpeed;
    [SerializeField] private float swimSideSpeed;
    [SerializeField] private float swimJumpForce;

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
    private bool facingRight;

    private bool jumpPressed;
    private bool jumpInputUsed;
    private int jumpCounter;

    private bool grabbingWall;
    private bool wasGrabbingWall;
    private float wallJumpTimer;

    private bool glidePressed;
    private bool wasGliding;

    private bool controlDashPressed;
    private bool wasControlDashing;
    private bool canControlDash;
    private float controlDashTimer;
    private float lastRadian;

    private bool dashPressed;
    private bool wasDashing;
    private bool canDash;
    private bool dashStarted;
    private float dashTimer;
    private float dashStartTimer;

    private bool inWater;
    private bool wasSwimming;
    private float waterLevel;

    private void Awake()
    {
        //Grab references 
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();

        defaultGravity = body.gravityScale;
        facingRight = true;
        dashStartTimer = dashStartup;
    }

    private void Update()
    {
        CheckControl();
        CheckJump();
        if (hasControlDash)
            CheckControlDash();
        if (hasDash)
            CheckDash();
        CheckGrounded();
        CheckOnWall();

        //Flipping Character Model
        if (leftJoystick.x > 0.01f)
        {
            transform.localScale = new Vector2(1, 1);
            facingR();
        }
        else if (leftJoystick.x < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
            facingL();
        }

        if (hasControl)
        {
            Move();
            Jump();
            if (hasWallJump)
                WallMovement();
            if (hasGlide)
                Glide();
            if (hasControlDash)
                ControlDash();
            if (hasDash)
                Dash();
        }
        else if (inWater)
        {
            Swim();
        }
    }

    private void Move()
    {
        body.velocity = new Vector2(leftJoystick.x * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        //Regular Jump
        if (jumpCounter < amountOfJumps && !jumpInputUsed && !grabbingWall)
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

            if (leftJoystick.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, climbSpeed);
            }
            else if (leftJoystick.y < 0)
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
        if (canControlDash && controlDashPressed && !wasControlDashing)
        {
            body.gravityScale = 0;
            controlDashTimer = controlDashTime;
            wasControlDashing = true;
            body.velocity = new Vector2(leftJoystick.x * controlDashSideSpeed, leftJoystick.y * controlDashUpSpeed);
            float currentRadianPrime = Mathf.Atan2(Mathf.Abs(leftJoystick.y), Mathf.Abs(leftJoystick.x));
            lastRadian = CheckRadian(leftJoystick.x, leftJoystick.y, currentRadianPrime);
        }
        else if (wasControlDashing && controlDashTimer > 0)
        {
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(leftJoystick.x, 2) + Mathf.Pow(leftJoystick.y, 2));
            if (hypotenuse == 0)
            {
                //this code happens when there is no joystick input
                //change for better slowdown or no slowdown or float down while no input or smth
                body.velocity = Vector2.zero;
            }
            else
            {
                float currentRadianPrime = Mathf.Atan2(Mathf.Abs(leftJoystick.y), Mathf.Abs(leftJoystick.x));
                float currentRadian = CheckRadian(leftJoystick.x, leftJoystick.y, currentRadianPrime);
                float movementRadian = Mathf.MoveTowardsAngle(lastRadian * 180 / Mathf.PI, currentRadian * 180 / Mathf.PI, controlDashSmoothing);
                movementRadian *= Mathf.PI / 180;
                //sin of 0 = 0, cos of 0 = 1
                float xForce = Mathf.Cos(movementRadian) * hypotenuse;
                float yForce = Mathf.Sin(movementRadian) * hypotenuse;

                body.velocity = new Vector2(xForce * controlDashSideSpeed, yForce * controlDashUpSpeed);
                lastRadian = movementRadian;
            }
        }
        else if (wasControlDashing)
        {
            wasControlDashing = false;
            canControlDash = false;
            body.gravityScale = defaultGravity;
            controlDashTimer = 0;
        }
    }

    private float CheckRadian(float x, float y, float joystickRadianPrime)
    {
        //don't yell at me my trig is dum and bad and idk the good way to do it
        if (leftJoystick.y < 0 && leftJoystick.x < 0)
            return joystickRadianPrime + Mathf.PI;
        else if (leftJoystick.y < 0 && leftJoystick.x != 0)
            return 2 * Mathf.PI - joystickRadianPrime;
        else if (leftJoystick.x < 0 && leftJoystick.y != 0)
            return Mathf.PI - joystickRadianPrime;
        else if (leftJoystick.x > 0 && leftJoystick.y > 0)
            return joystickRadianPrime;
        else if (leftJoystick.y == 0 && leftJoystick.x > 0)
            return 0;
        else if (leftJoystick.y == 0 && leftJoystick.x < 0)
            return Mathf.PI;
        else if (leftJoystick.y > 0)
            return Mathf.PI / 2f;
        else if (leftJoystick.y < 0)
            return 3 * Mathf.PI / 2f;
        else
            return 0;
    }

    private void Dash()
    {
        if (canDash && dashPressed && !wasDashing && (dashTimer > dashCooldown || dashTimer == 0) && dashStartTimer >= 0)
        {
            dashStartTimer = dashStartup;
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
            dashStarted = true;
        }
        if (canDash && (dashPressed || dashStarted) && !wasDashing && (dashTimer > dashCooldown || dashTimer == 0) && dashStartTimer <= 0)
        {
            dashTimer = dashTime;
            wasDashing = true;
            dashStartTimer = 0;
            dashStarted = false;
            if (facingRight)
                body.velocity = new Vector2(dashSpeed, 0);
            else if (!facingRight)
                body.velocity = new Vector2(dashSpeed * -1, 0);
        }
        else if (dashTimer <= dashCooldown && dashTimer > 0)
        {
            body.gravityScale = defaultGravity;
        }
        else if (wasDashing && dashTimer <= 0)
        {
            wasDashing = false;
            canDash = false;
            dashTimer = 0;
        }
    }

    private void Swim()
    {
        if (!wasSwimming)
        {
            wasSwimming = true;
            waterLevel = body.position.y;
            body.gravityScale = 0;
        }
        else
        {
            if (body.position.y >= waterLevel)
            {
                if (!jumpInputUsed && jumpPressed)
                {
                    body.velocity = new Vector2(leftJoystick.x * swimSideSpeed, swimJumpForce);
                    jumpInputUsed = true;
                    jumpCounter = 1;
                    canDash = true;
                    canControlDash = true;
                    inWater = false;
                }
                else
                {
                    float yForce = Mathf.Clamp(leftJoystick.y * swimUpSpeed, -Mathf.Infinity, 0);
                    body.velocity = new Vector2(leftJoystick.x * swimSideSpeed, yForce);
                }
            }
            else
            {
                body.velocity = new Vector2(leftJoystick.x * swimSideSpeed, leftJoystick.y * swimUpSpeed);
            }
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
        else if(dashTimer > dashCooldown)
        {
            dashTimer -= Time.deltaTime;
        }
        else if(dashStartTimer > 0)
        {
            dashStartTimer -= Time.deltaTime;
        }
        else if (!inWater)
        {
            hasControl = true;
            if (wasSwimming)
            {
                wasSwimming = false;
                body.gravityScale = defaultGravity;
            }
            if (dashTimer > 0)
                dashTimer -= Time.deltaTime;
        }
    }

    private void CheckJump()
    {
        if (isGrounded)
        {
            jumpCounter = 0;
        }
        else if (wasGrabbingWall || wasControlDashing)
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

    private void CheckDash()
    {
        if (isGrounded || grabbingWall)
            canDash = true;
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

    public void OnEnterLiquid(Liquid liquid)
    {
        if (liquid.GetType() == typeof(Water))
            inWater = true;
    }

    public void OnExitLiquid(Liquid liquid)
    {
        if (liquid.GetType() == typeof(Water))
            inWater = false;
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

    private void OnDash(InputValue value)
    {
        dashPressed = value.isPressed;
    }

    private void facingR()
    {
        facingRight = true;
    }

    private void facingL()
    {
        facingRight = false;
    }

    //displays text for debugging
    private void OnGUI()
    {
        GUI.Label(new Rect(1100, 10, 100, 100), "bodypostition: " + body.position.y);
        GUI.Label(new Rect(1200, 50, 100, 100), "waterlevel: " + waterLevel);
    }
}
