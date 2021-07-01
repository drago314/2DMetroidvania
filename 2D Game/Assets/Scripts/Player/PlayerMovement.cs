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
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private Animator anim;
    private float defaultGravity;
    private Vector2 leftJoystick;
    private bool jumpPressed;
    private bool jumpInputUsed;
    private bool isGrounded;
    private bool onLeftWall;
    private bool onRightWall;
    private int jumpCounter;

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
        CheckJump();

        //Flipping Character Model
        if (leftJoystick.x > 0.01f)
            transform.localScale = new Vector2(1, 1);
        else if (leftJoystick.x < -0.01f)
            transform.localScale = new Vector2(-1, 1);

        Move();
        Jump();
    }

    private void Move()
    {
        body.velocity = new Vector2(leftJoystick.x * moveSpeed, body.velocity.y);
    }

    private void Jump()
    {
        //Regular Jump
        if (jumpCounter < 2 && !jumpInputUsed)
        {
            Invoke("AddJump", 0.1f);
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            //anim.SetTrigger("Jump");
            jumpInputUsed = true;
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

    private void CheckJump()
    {
        if (isGrounded)
        {
            jumpCounter = 0;
        }
    }

    private void AddJump()
    {
        jumpCounter += 1;
    }

    private void CheckOnWall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        onLeftWall = raycastHitLeft.collider != null;
        onRightWall = raycastHitRight.collider != null;
        //isSliding = !isGrounded && ((onLeftWall && horizontalInput < 0) || (onRightWall && horizontalInput > 0));
    }

    private void CheckGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.05f, groundLayer);
        isGrounded = raycastHit.collider != null;
    }

    private void OnMove(InputValue value)
    {
        Debug.Log("sorta working");
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

    //displays text for debugging
    private void OnGUI()
    {
        //GUI.Label(new Rect(1200, 10, 100, 100), "Can controldash: " + body.velocity);
        //GUI.Label(new Rect(1200, 50, 100, 100), "is doing thing: " + isControlDashing);
    }
}
