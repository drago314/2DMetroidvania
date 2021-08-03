using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GetGroundLayer() { return groundLayer; }
    [SerializeField] private LayerMask enemyLayer;
    public LayerMask GetEnemyLayer() { return enemyLayer; }

    public SpriteRenderer spriteRenderer { get; private set; }
    public BoxCollider2D boxCollider { get; private set; }
    public Rigidbody2D body { get; private set; }
    public Animator anim { get; private set; }

    public float defaultGravity { get; private set; }
    public Vector2 leftJoystick { get; private set; }
    public InvFrame iFrame { get; private set; }

    public bool isGrounded { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    public bool facingRight { get; private set; }

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerDamaged playerDamaged;

    private void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();
        playerDamaged = gameObject.GetComponent<PlayerDamaged>();

        //Grab references 
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        body = gameObject.GetComponent<Rigidbody2D>();
        //anim = gameObject.GetComponent<Animator>();
        iFrame = gameObject.GetComponent<InvFrame>();

        defaultGravity = body.gravityScale;
    }

    private void Update()
    {
        CheckGrounded();
        CheckOnWall();

        playerMovement.CheckMovement();

        if (playerMovement.CheckControl() && playerAttack.CheckControl() && playerDamaged.CheckControl())
        {
            //Flipping Character Model
            if (leftJoystick.x > 0.01f)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
            }
            else if (leftJoystick.x < -0.01f)
            {
                spriteRenderer.flipX = true;
                facingRight = false;
            }

            playerMovement.Movement();
            playerAttack.Attack();
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

    //displays text for debugging
    private void OnGUI()
    {
        //GUI.Label(new Rect(1100, 10, 100, 100), "isGrounded: " + isGrounded);
        //GUI.Label(new Rect(1200, 50, 100, 100), "onWall: " + (onLeftWall || onRightWall));
    }

}
