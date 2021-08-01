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

    [SerializeField] private float animeDashForce;
    [SerializeField] private float animeDashRange;
    [SerializeField] private float animeDashInvincibilityTime;
    [SerializeField] private float animeDashTime;
    [SerializeField] private float animeDashCooldown;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;

    private float defaultGravity;
    private Vector2 leftJoystick;

    private InvFrame iFrame;

    private bool isGrounded;
    private bool onLeftWall;
    private bool onRightWall;

    private bool hasControl;
    private bool facingRight;

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

    private bool animeDashPressed;
    private bool animeDashInputUsed;
    private bool animeDashing = false;
    private bool reachedTarget;
    private float animeDashTimer = 0f;
    private float animeDashCooldownTimer = 0f;
    private Vector2 animeDashTarget;
    private Vector2 animeDashDirection;

    private void Awake()
    {
        //Grab references 
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        iFrame = gameObject.GetComponent<InvFrame>();

        defaultGravity = body.gravityScale;
    }

    private void Update()
    {
        CheckGrounded();
        CheckOnWall();
        CheckControl();
        CheckAbilities();

        //Flipping Character Model
        if (leftJoystick.x > 0.01f)
        {
            transform.localScale = new Vector2(1, 1);
            facingRight = true;
        }
        else if (leftJoystick.x < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
            facingRight = false;
        }

        if (hasControl)
        {
            Move();
            Jump();
            WallMovement();
            Glide();
            Dash();
            AnimeDash();
        }
    }

    private void Move()
    {
        body.velocity = new Vector2(leftJoystick.x * moveSpeed, body.velocity.y);
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
        grabbingWall = (onLeftWall && leftJoystick.x <= 0) || (onRightWall && leftJoystick.x >= 0);
        if (grabbingWall)
        {
            wasGrabbingWall = true;
            body.gravityScale = 0;

            if(leftJoystick.y > 0)
                body.velocity = new Vector2(body.velocity.x, climbSpeed);
            else if(leftJoystick.y < 0)
                body.velocity = new Vector2(body.velocity.x, climbSpeed * -1);
            else
                body.velocity = Vector2.zero;

            if (!jumpInputUsed && jumpPressed)
            {
                int jumpDirection;
                if (onLeftWall)
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
            if (isGrounded)
            {
                wallJumpCherryTimer = 0;    
            }
            else if (!jumpInputUsed && jumpPressed)
            {
                int jumpDirection;
                if (leftJoystick.x > 0)
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
            if (facingRight)
                dashDirection = 1;
            else
                dashDirection = -1;
            body.velocity = new Vector2(dashDirection * dashForce, 0);
        }
    }

    private void AnimeDash()
    {
        if (animeDashing && !reachedTarget && Vector2.Distance(body.position, animeDashTarget) < 0.2f)
        {
            Invoke("RemoveInvincibility", animeDashInvincibilityTime);
            reachedTarget = true;
            animeDashCooldownTimer = animeDashCooldown;
            animeDashTimer = animeDashTime;
            body.velocity = animeDashDirection;
        }
        else if (animeDashing && reachedTarget && animeDashTimer <= 0)
        {
            body.gravityScale = defaultGravity;
            body.velocity = Vector2.zero;
            animeDashing = false;
        }
        else if (animeDashPressed && !animeDashInputUsed && animeDashCooldownTimer <= 0)
        {
            animeDashInputUsed = true;
            Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(body.position, animeDashRange, enemyLayer);
            if (possibleTargets.Length > 0)
            {
                iFrame.Invincible(true);

                body.gravityScale = 0;
                animeDashing = true;
                reachedTarget = false;

                animeDashTarget = possibleTargets[0].transform.position;
                DashToPosition(animeDashTarget);
            }
        }
    }

    private void DashToPosition(Vector2 target)
    {
        Vector2 current = body.position;
        float slope = (target.y - current.y) / (target.x - current.x);
        float radian = Mathf.Atan(Mathf.Abs(slope));
        radian = CheckRadian(target.x - current.x, target.y - current.y, radian);
        float xForce = Mathf.Cos(radian);
        float yForce = Mathf.Sin(radian);
        animeDashDirection = new Vector2(xForce * animeDashForce, yForce * animeDashForce);
        body.velocity = animeDashDirection;
    }

    private float CheckRadian(float x, float y, float radianPrime)
    {
        //don't yell at me my trig is dum and bad and idk the good way to do it
        if (y < 0 && x < 0)
            return radianPrime + Mathf.PI;
        else if (y < 0 && x != 0)
            return 2 * Mathf.PI - radianPrime;
        else if (x < 0 && y != 0)
            return Mathf.PI - radianPrime;
        else if (x > 0 && y > 0)
            return radianPrime;
        else if (y == 0 && x > 0)
            return 0;
        else if (y == 0 && x < 0)
            return Mathf.PI;
        else if (y > 0)
            return Mathf.PI / 2f;
        else if (y < 0)
            return 3 * Mathf.PI / 2f;
        else
            return 0;
    }

    private void RemoveInvincibility()
    {
        iFrame.Invincible(false);
    }

    private void CheckControl()
    {
        hasControl = false;

        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else if (animeDashing)
        {
            AnimeDash();
            if (animeDashTimer > 0)
                animeDashTimer -= Time.deltaTime;
        }
        else
        {
            hasControl = true;

            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;

            if (animeDashCooldownTimer > 0)
                animeDashCooldownTimer -= Time.deltaTime;
        }
    }

    private void CheckAbilities()
    {
        if (isGrounded)
        {
            jumpCounter = 0;
            canDash = true;
        }
        else if (wasGrabbingWall)
        {
            jumpCounter = 1;
            canDash = true;
        }
        else if (animeDashing)
        {
            jumpCounter = 1;
            canDash = true;
        }
    }
    private void CheckGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
    }

    private void CheckOnWall()
    {
        Vector2 center = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y + 0.5f);
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(center, boxCollider.bounds.size, 0, Vector2.right, 0.05f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(center, boxCollider.bounds.size, 0, Vector2.left, 0.05f, groundLayer);
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

    private void OnDash(InputValue value)
    {
        dashPressed = value.isPressed;
        if (dashPressed)
        {
            dashInputUsed = false;
        }
    }

    private void OnAnimeDash(InputValue value)
    {
        animeDashPressed = value.isPressed;
        if (animeDashPressed)
        {
            animeDashInputUsed = false;
        }
    }

    //displays text for debugging
    private void OnGUI()
    {
        GUI.Label(new Rect(1100, 10, 100, 100), "anime Dashing: " + animeDashing);
        //GUI.Label(new Rect(1200, 50, 100, 100), "onWall: " + (onLeftWall || onRightWall));
    }

}
